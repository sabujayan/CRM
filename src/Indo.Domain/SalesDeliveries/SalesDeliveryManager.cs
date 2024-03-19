using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Movements;
using Indo.NumberSequences;
using Indo.SalesOrderDetails;
using Indo.SalesOrders;
using Indo.SalesDeliveryDetails;
using Indo.Stocks;
using Indo.Warehouses;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using System.Collections.Generic;

namespace Indo.SalesDeliveries
{
    public class SalesDeliveryManager : DomainService
    {
        private readonly ISalesDeliveryRepository _salesDeliveryRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly ISalesDeliveryDetailRepository _salesDeliveryDetailRepository;
        private readonly SalesDeliveryDetailManager _salesDeliveryDetailManager;
        private readonly IMovementRepository _movementRepository;
        private readonly MovementManager _movementManager;
        private readonly WarehouseManager _warehouseManager;
        private readonly CompanyManager _companyManager;
        private readonly IStockRepository _stockRepository;
        private readonly StockManager _stockManager;

        public SalesDeliveryManager(
            ISalesDeliveryRepository salesDeliveryRepository,
            ISalesOrderRepository salesOrderRepository,
            ISalesOrderDetailRepository salesOrderDetailRepository,
            NumberSequenceManager numberSequenceManager,
            ISalesDeliveryDetailRepository salesDeliveryDetailRepository,
            SalesDeliveryDetailManager salesDeliveryDetailManager,
            IMovementRepository movementRepository,
            MovementManager movementManager,
            WarehouseManager warehouseManager,
            CompanyManager companyManager,
            IStockRepository stockRepository,
            StockManager stockManager
            )
        {
            _salesDeliveryRepository = salesDeliveryRepository;
            _salesOrderRepository = salesOrderRepository;
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _numberSequenceManager = numberSequenceManager;
            _salesDeliveryDetailRepository = salesDeliveryDetailRepository;
            _salesDeliveryDetailManager = salesDeliveryDetailManager;
            _movementRepository = movementRepository;
            _movementManager = movementManager;
            _warehouseManager = warehouseManager;
            _companyManager = companyManager;
            _stockRepository = stockRepository;
            _stockManager = stockManager;
        }
        public async Task<SalesDelivery> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid salesOrderId,
            [NotNull] DateTime deliveryDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(salesOrderId, nameof(salesOrderId));
            Check.NotNull<DateTime>(deliveryDate, nameof(deliveryDate));

            var existing = await _salesDeliveryRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new SalesDeliveryAlreadyExistsException(number);
            }

            return new SalesDelivery(
                GuidGenerator.Create(),
                number,
                salesOrderId,
                deliveryDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] SalesDelivery salesDelivery,
           [NotNull] string newName)
        {
            Check.NotNull(salesDelivery, nameof(salesDelivery));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _salesDeliveryRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != salesDelivery.Id)
            {
                throw new SalesDeliveryAlreadyExistsException(newName);
            }

            salesDelivery.ChangeName(newName);
        }

        public async Task<SalesDelivery> GenerateSalesDeliveryFromSalesAsync(Guid salesOrderId)
        {
            var order = await _salesOrderRepository.GetAsync(salesOrderId);
            var query = await _salesOrderDetailRepository.GetQueryableAsync();
            var lines = await AsyncExecuter.ToListAsync(query.Where(x => x.SalesOrderId.Equals(salesOrderId)));

            var delivery = await CreateAsync(
                await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.SalesDelivery),
                order.Id,
                order.OrderDate
                );
            delivery.Status = SalesDeliveryStatus.Draft;
            var result = await _salesDeliveryRepository.InsertAsync(delivery, autoSave: true);
            var details = new List<SalesDeliveryDetail>();
            foreach (var item in lines)
            {
                var line = await _salesDeliveryDetailManager.CreateAsync(
                    result.Id,
                    item.ProductId,
                    item.Quantity
                    );
                details.Add(line);
            }
            await _salesDeliveryDetailRepository.InsertManyAsync(details, autoSave: true);
            return result;
        }

        public async Task<SalesDelivery> GenerateSalesDeliveryReturnFromDeliveryAsync(Guid salesDeliveryId)
        {
            var origin = await _salesDeliveryRepository.GetAsync(salesDeliveryId);
            var query = await _salesDeliveryDetailRepository.GetQueryableAsync();
            var lines = await AsyncExecuter.ToListAsync(query.Where(x => x.SalesDeliveryId.Equals(origin.Id)));

            var delivery = await CreateAsync(
                origin.Number + "-RN",
                origin.SalesOrderId,
                DateTime.Now
                );
            delivery.Status = SalesDeliveryStatus.Draft;
            var result = await _salesDeliveryRepository.InsertAsync(delivery, autoSave: true);
            var details = new List<SalesDeliveryDetail>();
            foreach (var item in lines)
            {
                var line = await _salesDeliveryDetailManager.CreateAsync(
                    result.Id,
                    item.ProductId,
                    item.Quantity
                    );
                details.Add(line);
            }
            await _salesDeliveryDetailRepository.InsertManyAsync(details, autoSave: true);
            return result;
        }

        public async Task ConfirmSalesDeliveryAsync(Guid salesDeliveryId)
        {
            var delivery = _salesDeliveryRepository.Where(x => x.Id.Equals(salesDeliveryId)).FirstOrDefault();
            if (delivery.Status == SalesDeliveryStatus.Draft)
            {
                delivery.Status = SalesDeliveryStatus.Confirm;
                var defaultCompany = await _companyManager.GetDefaultCompanyAsync();
                var defaultWH = defaultCompany.DefaultWarehouseId;
                var customerWH = await _warehouseManager.GetDefaultWarehouseCustomerAsync();
                var lines = _salesDeliveryDetailRepository.Where(x => x.SalesDeliveryId.Equals(delivery.Id)).ToList();
                foreach (var item in lines)
                {
                    var movement = await _movementManager.CreateAsync(
                        await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Movement),
                        defaultWH,
                        customerWH.Id,
                        delivery.DeliveryDate,
                        delivery.Number,
                        NumberSequenceModules.SalesDelivery,
                        item.ProductId,
                        item.Quantity
                        );
                    var result = await _movementRepository.InsertAsync(movement, autoSave: true);

                    //stock outlet
                    var outlet = await _stockManager.CreateAsync(
                        result.Id,
                        movement.FromWarehouseId,
                        movement.MovementDate,
                        movement.Number,
                        StockFlow.Outlet,
                        movement.ProductId,
                        movement.Qty * -1
                        );
                    await _stockRepository.InsertAsync(outlet, autoSave: true);

                    //stock inlet
                    var inlet = await _stockManager.CreateAsync(
                        result.Id,
                        movement.ToWarehouseId,
                        movement.MovementDate,
                        movement.Number,
                        StockFlow.Inlet,
                        movement.ProductId,
                        movement.Qty * 1
                        );
                    await _stockRepository.InsertAsync(inlet, autoSave: true);
                }
                await _salesDeliveryRepository.UpdateAsync(delivery, autoSave: true);
            }
        }


        public async Task ConfirmSalesDeliveryReturnAsync(Guid salesDeliveryId)
        {
            var delivery = await _salesDeliveryRepository.GetAsync(salesDeliveryId);
            if (delivery.Status == SalesDeliveryStatus.Draft)
            {
                delivery.Status = SalesDeliveryStatus.Confirm;
                var defaultCompany = await _companyManager.GetDefaultCompanyAsync();
                var defaultWH = defaultCompany.DefaultWarehouseId;
                var customerWH = await _warehouseManager.GetDefaultWarehouseCustomerAsync();
                var lines = _salesDeliveryDetailRepository.Where(x => x.SalesDeliveryId.Equals(delivery.Id)).ToList();
                foreach (var item in lines)
                {
                    var movement = await _movementManager.CreateAsync(
                        await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Movement),
                        customerWH.Id,
                        defaultWH,
                        delivery.DeliveryDate,
                        delivery.Number,
                        NumberSequenceModules.SalesDelivery,
                        item.ProductId,
                        item.Quantity
                        );
                    var result = await _movementRepository.InsertAsync(movement, autoSave: true);

                    //stock outlet
                    var outlet = await _stockManager.CreateAsync(
                        result.Id,
                        movement.FromWarehouseId,
                        movement.MovementDate,
                        movement.Number,
                        StockFlow.Outlet,
                        movement.ProductId,
                        movement.Qty * -1
                        );
                    await _stockRepository.InsertAsync(outlet, autoSave: true);

                    //stock inlet
                    var inlet = await _stockManager.CreateAsync(
                        result.Id,
                        movement.ToWarehouseId,
                        movement.MovementDate,
                        movement.Number,
                        StockFlow.Inlet,
                        movement.ProductId,
                        movement.Qty * 1
                        );
                    await _stockRepository.InsertAsync(inlet, autoSave: true);
                }
                await _salesDeliveryRepository.UpdateAsync(delivery, autoSave: true);
            }
        }



    }
}
