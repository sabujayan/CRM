using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Movements;
using Indo.NumberSequences;
using Indo.TransferOrderDetails;
using Indo.TransferOrders;
using Indo.DeliveryOrderDetails;
using Indo.Stocks;
using Indo.Warehouses;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using System.Collections.Generic;

namespace Indo.DeliveryOrders
{
    public class DeliveryOrderManager : DomainService
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly ITransferOrderDetailRepository _transferOrderDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly IDeliveryOrderDetailRepository _deliveryOrderDetailRepository;
        private readonly DeliveryOrderDetailManager _deliveryOrderDetailManager;
        private readonly IMovementRepository _movementRepository;
        private readonly MovementManager _movementManager;
        private readonly WarehouseManager _warehouseManager;
        private readonly CompanyManager _companyManager;
        private readonly IStockRepository _stockRepository;
        private readonly StockManager _stockManager;

        public DeliveryOrderManager(
            IDeliveryOrderRepository deliveryOrderRepository,
            ITransferOrderRepository transferOrderRepository,
            ITransferOrderDetailRepository transferOrderDetailRepository,
            NumberSequenceManager numberSequenceManager,
            IDeliveryOrderDetailRepository deliveryOrderDetailRepository,
            DeliveryOrderDetailManager deliveryOrderDetailManager,
            IMovementRepository movementRepository,
            MovementManager movementManager,
            WarehouseManager warehouseManager,
            CompanyManager companyManager,
            IStockRepository stockRepository,
            StockManager stockManager
            )
        {
            _deliveryOrderRepository = deliveryOrderRepository;
            _transferOrderRepository = transferOrderRepository;
            _transferOrderDetailRepository = transferOrderDetailRepository;
            _numberSequenceManager = numberSequenceManager;
            _deliveryOrderDetailRepository = deliveryOrderDetailRepository;
            _deliveryOrderDetailManager = deliveryOrderDetailManager;
            _movementRepository = movementRepository;
            _movementManager = movementManager;
            _warehouseManager = warehouseManager;
            _companyManager = companyManager;
            _stockRepository = stockRepository;
            _stockManager = stockManager;
        }
        public async Task<DeliveryOrder> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid transferOrderId,
            [NotNull] Guid fromWarehouseId,
            [NotNull] Guid toWarehouseId,
            [NotNull] DateTime orderDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(transferOrderId, nameof(transferOrderId));
            Check.NotNull<Guid>(fromWarehouseId, nameof(fromWarehouseId));
            Check.NotNull<Guid>(toWarehouseId, nameof(toWarehouseId));
            Check.NotNull<DateTime>(orderDate, nameof(orderDate));

            var existing = await _deliveryOrderRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new DeliveryOrderAlreadyExistsException(number);
            }
            
            return new DeliveryOrder(
                GuidGenerator.Create(),
                number,
                transferOrderId,
                fromWarehouseId,
                toWarehouseId,
                orderDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] DeliveryOrder deliveryOrder,
           [NotNull] string newName)
        {
            Check.NotNull(deliveryOrder, nameof(deliveryOrder));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _deliveryOrderRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != deliveryOrder.Id)
            {
                throw new DeliveryOrderAlreadyExistsException(newName);
            }

            deliveryOrder.ChangeName(newName);
        }

        public async Task<DeliveryOrder> GenerateDeliveryOrderFromTransferAsync(Guid transferOrderId)
        {
            var order = _transferOrderRepository.Where(x => x.Id.Equals(transferOrderId)).FirstOrDefault();
            var lines = _transferOrderDetailRepository.Where(x => x.TransferOrderId.Equals(transferOrderId)).ToList();

            var delivery = await CreateAsync(
                await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.DeliveryOrder),
                order.Id,
                order.FromWarehouseId,
                order.ToWarehouseId,
                order.OrderDate
                );
            delivery.Status = DeliveryOrderStatus.Draft;
            var result = await _deliveryOrderRepository.InsertAsync(delivery, autoSave: true);
            var details = new List<DeliveryOrderDetail>();
            foreach (var item in lines)
            {
                var line = await _deliveryOrderDetailManager.CreateAsync(
                    result.Id,
                    item.ProductId,
                    item.Quantity
                    );
                details.Add(line);
            }
            await _deliveryOrderDetailRepository.InsertManyAsync(details, autoSave: true);
            return result;
        }

        public async Task ConfirmDeliveryOrderAsync(Guid deliveryOrderId)
        {
            var delivery = _deliveryOrderRepository.Where(x => x.Id.Equals(deliveryOrderId)).FirstOrDefault();
            if (delivery.Status == DeliveryOrderStatus.Draft)
            {
                delivery.Status = DeliveryOrderStatus.Confirm;
                var defaultCompany = await _companyManager.GetDefaultCompanyAsync();
                var lines = _deliveryOrderDetailRepository.Where(x => x.DeliveryOrderId.Equals(delivery.Id)).ToList();
                var intranstiWarehouse = await _warehouseManager.GetDefaultWarehouseIntransitAsync();
                foreach (var item in lines)
                {
                    var movement = await _movementManager.CreateAsync(
                        await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Movement),
                        delivery.FromWarehouseId,
                        intranstiWarehouse.Id,
                        delivery.OrderDate,
                        delivery.Number,
                        NumberSequenceModules.DeliveryOrder,
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
                await _deliveryOrderRepository.UpdateAsync(delivery, autoSave: true);
            }
        }




    }
}
