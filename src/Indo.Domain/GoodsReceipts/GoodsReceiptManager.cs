using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Movements;
using Indo.NumberSequences;
using Indo.DeliveryOrderDetails;
using Indo.DeliveryOrders;
using Indo.GoodsReceiptDetails;
using Indo.Stocks;
using Indo.Warehouses;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using System.Collections.Generic;
using Indo.TransferOrders;

namespace Indo.GoodsReceipts
{
    public class GoodsReceiptManager : DomainService
    {
        private readonly IGoodsReceiptRepository _goodsReceiptRepository;
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly IDeliveryOrderDetailRepository _deliveryOrderDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly IGoodsReceiptDetailRepository _goodsReceiptDetailRepository;
        private readonly GoodsReceiptDetailManager _goodsReceiptDetailManager;
        private readonly IMovementRepository _movementRepository;
        private readonly MovementManager _movementManager;
        private readonly WarehouseManager _warehouseManager;
        private readonly CompanyManager _companyManager;
        private readonly IStockRepository _stockRepository;
        private readonly StockManager _stockManager;

        public GoodsReceiptManager(
            IGoodsReceiptRepository goodsReceiptRepository,
            IDeliveryOrderRepository deliveryOrderRepository,
            ITransferOrderRepository transferOrderRepository,
            IDeliveryOrderDetailRepository deliveryOrderDetailRepository,
            NumberSequenceManager numberSequenceManager,
            IGoodsReceiptDetailRepository goodsReceiptDetailRepository,
            GoodsReceiptDetailManager goodsReceiptDetailManager,
            IMovementRepository movementRepository,
            MovementManager movementManager,
            WarehouseManager warehouseManager,
            CompanyManager companyManager,
            IStockRepository stockRepository,
            StockManager stockManager
            )
        {
            _goodsReceiptRepository = goodsReceiptRepository;
            _deliveryOrderRepository = deliveryOrderRepository;
            _deliveryOrderDetailRepository = deliveryOrderDetailRepository;
            _numberSequenceManager = numberSequenceManager;
            _goodsReceiptDetailRepository = goodsReceiptDetailRepository;
            _goodsReceiptDetailManager = goodsReceiptDetailManager;
            _movementRepository = movementRepository;
            _movementManager = movementManager;
            _warehouseManager = warehouseManager;
            _companyManager = companyManager;
            _stockRepository = stockRepository;
            _stockManager = stockManager;
            _transferOrderRepository = transferOrderRepository;
        }
        public async Task<GoodsReceipt> CreateAsync(
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

            var existing = await _goodsReceiptRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new GoodsReceiptAlreadyExistsException(number);
            }

            return new GoodsReceipt(
                GuidGenerator.Create(),
                number,
                transferOrderId,
                fromWarehouseId,
                toWarehouseId,
                orderDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] GoodsReceipt goodsReceipt,
           [NotNull] string newName)
        {
            Check.NotNull(goodsReceipt, nameof(goodsReceipt));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _goodsReceiptRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != goodsReceipt.Id)
            {
                throw new GoodsReceiptAlreadyExistsException(newName);
            }

            goodsReceipt.ChangeName(newName);
        }

        public async Task<GoodsReceipt> GenerateGoodsReceiptFromDeliveryAsync(Guid deliveryOrderId)
        {
            var order = _deliveryOrderRepository.Where(x => x.Id.Equals(deliveryOrderId)).FirstOrDefault();
            var transfer = _transferOrderRepository.Where(x => x.Id.Equals(order.TransferOrderId)).FirstOrDefault();
            var lines = _deliveryOrderDetailRepository.Where(x => x.DeliveryOrderId.Equals(deliveryOrderId)).ToList();

            var receipt = await CreateAsync(
                await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.GoodsReceipt),
                order.Id,
                transfer.FromWarehouseId,
                transfer.ToWarehouseId,
                order.OrderDate
                );
            receipt.Status = GoodsReceiptStatus.Draft;
            var result = await _goodsReceiptRepository.InsertAsync(receipt, autoSave: true);
            var details = new List<GoodsReceiptDetail>();
            foreach (var item in lines)
            {
                var line = await _goodsReceiptDetailManager.CreateAsync(
                    result.Id,
                    item.ProductId,
                    item.Quantity
                    );
                details.Add(line);
            }
            await _goodsReceiptDetailRepository.InsertManyAsync(details, autoSave: true);
            return result;
        }

        public async Task ConfirmGoodsReceiptAsync(Guid goodsReceiptId)
        {
            var receipt = _goodsReceiptRepository.Where(x => x.Id.Equals(goodsReceiptId)).FirstOrDefault();
            if (receipt.Status == GoodsReceiptStatus.Draft)
            {
                receipt.Status = GoodsReceiptStatus.Confirm;
                var defaultCompany = await _companyManager.GetDefaultCompanyAsync();
                var lines = _goodsReceiptDetailRepository.Where(x => x.GoodsReceiptId.Equals(receipt.Id)).ToList();
                var intransitWarehouse = await _warehouseManager.GetDefaultWarehouseIntransitAsync();
                foreach (var item in lines)
                {
                    var movement = await _movementManager.CreateAsync(
                        await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Movement),
                        intransitWarehouse.Id,
                        receipt.ToWarehouseId,
                        receipt.OrderDate,
                        receipt.Number,
                        NumberSequenceModules.GoodsReceipt,
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
                await _goodsReceiptRepository.UpdateAsync(receipt, autoSave: true);
            }
        }
    }
}
