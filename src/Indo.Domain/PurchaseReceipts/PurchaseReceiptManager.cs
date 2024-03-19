using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Movements;
using Indo.NumberSequences;
using Indo.PurchaseOrderDetails;
using Indo.PurchaseOrders;
using Indo.PurchaseReceiptDetails;
using Indo.Stocks;
using Indo.Warehouses;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.PurchaseReceipts
{
    public class PurchaseReceiptManager : DomainService
    {
        private readonly IPurchaseReceiptRepository _purchaseReceiptRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly IPurchaseReceiptDetailRepository _purchaseReceiptDetailRepository;
        private readonly PurchaseReceiptDetailManager _purchaseReceiptDetailManager;
        private readonly IMovementRepository _movementRepository;
        private readonly MovementManager _movementManager;
        private readonly WarehouseManager _warehouseManager;
        private readonly CompanyManager _companyManager;
        private readonly IStockRepository _stockRepository;
        private readonly StockManager _stockManager;

        public PurchaseReceiptManager(
            IPurchaseReceiptRepository purchaseReceiptRepository,
            IPurchaseOrderRepository purchaseOrderRepository,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
            NumberSequenceManager numberSequenceManager,
            IPurchaseReceiptDetailRepository purchaseReceiptDetailRepository,
            PurchaseReceiptDetailManager purchaseReceiptDetailManager,
            IMovementRepository movementRepository,
            MovementManager movementManager,
            WarehouseManager warehouseManager,
            CompanyManager companyManager,
            IStockRepository stockRepository,
            StockManager stockManager
            )
        {
            _purchaseReceiptRepository = purchaseReceiptRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
            _numberSequenceManager = numberSequenceManager;
            _purchaseReceiptDetailRepository = purchaseReceiptDetailRepository;
            _purchaseReceiptDetailManager = purchaseReceiptDetailManager;
            _movementRepository = movementRepository;
            _movementManager = movementManager;
            _warehouseManager = warehouseManager;
            _companyManager = companyManager;
            _stockRepository = stockRepository;
            _stockManager = stockManager;
        }
        public async Task<PurchaseReceipt> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid purchaseOrderId,
            [NotNull] DateTime receiptDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(purchaseOrderId, nameof(purchaseOrderId));
            Check.NotNull<DateTime>(receiptDate, nameof(receiptDate));

            var existing = await _purchaseReceiptRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new PurchaseReceiptAlreadyExistsException(number);
            }

            return new PurchaseReceipt(
                GuidGenerator.Create(),
                number,
                purchaseOrderId,
                receiptDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] PurchaseReceipt purchaseReceipt,
           [NotNull] string newName)
        {
            Check.NotNull(purchaseReceipt, nameof(purchaseReceipt));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _purchaseReceiptRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != purchaseReceipt.Id)
            {
                throw new PurchaseReceiptAlreadyExistsException(newName);
            }

            purchaseReceipt.ChangeName(newName);
        }

        public async Task<PurchaseReceipt> GeneratePurchaseReceiptFromPurchaseAsync(Guid purchaseOrderId)
        {
            var order = await _purchaseOrderRepository.GetAsync(purchaseOrderId);
            var query = await _purchaseOrderDetailRepository.GetQueryableAsync();
            var lines = await AsyncExecuter.ToListAsync(query.Where(x => x.PurchaseOrderId.Equals(purchaseOrderId)));

            var receipt = await CreateAsync(
                await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.PurchaseReceipt),
                order.Id,
                order.OrderDate
                );
            receipt.Status = PurchaseReceiptStatus.Draft;
            var result = await _purchaseReceiptRepository.InsertAsync(receipt, autoSave: true);
            var details = new List<PurchaseReceiptDetail>();
            foreach (var item in lines)
            {
                var line = await _purchaseReceiptDetailManager.CreateAsync(
                    result.Id,
                    item.ProductId,
                    item.Quantity
                    );
                details.Add(line);
            }
            await _purchaseReceiptDetailRepository.InsertManyAsync(details, autoSave: true);
            return result;
        }

        public async Task<PurchaseReceipt> GeneratePurchaseReceiptReturnFromReceiptAsync(Guid purchaseReceiptId)
        {
            var origin = await _purchaseReceiptRepository.GetAsync(purchaseReceiptId);
            var query = await _purchaseReceiptDetailRepository.GetQueryableAsync();
            var lines = await AsyncExecuter.ToListAsync(query.Where(x => x.PurchaseReceiptId.Equals(origin.Id)));

            var receipt = await CreateAsync(
                origin.Number + "-RN",
                origin.PurchaseOrderId,
                DateTime.Now
                );
            receipt.Status = PurchaseReceiptStatus.Draft;
            var result = await _purchaseReceiptRepository.InsertAsync(receipt, autoSave: true);
            var details = new List<PurchaseReceiptDetail>();
            foreach (var item in lines)
            {
                var line = await _purchaseReceiptDetailManager.CreateAsync(
                    result.Id,
                    item.ProductId,
                    item.Quantity
                    );
                details.Add(line);
            }
            await _purchaseReceiptDetailRepository.InsertManyAsync(details, autoSave: true);
            return result;
        }

        public async Task ConfirmPurchaseReceiptAsync(Guid purchaseReceiptId)
        {
            var receipt = await _purchaseReceiptRepository.GetAsync(purchaseReceiptId);
            if (receipt.Status == PurchaseReceiptStatus.Draft)
            {
                receipt.Status = PurchaseReceiptStatus.Confirm;
                var defaultCompany = await _companyManager.GetDefaultCompanyAsync();
                var defaultWH = defaultCompany.DefaultWarehouseId;
                var vendorWH = await _warehouseManager.GetDefaultWarehouseVendorAsync();
                var lines = _purchaseReceiptDetailRepository.Where(x => x.PurchaseReceiptId.Equals(receipt.Id)).ToList();
                foreach (var item in lines)
                {
                    var movement = await _movementManager.CreateAsync(
                        await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Movement),
                        vendorWH.Id,
                        defaultWH,
                        receipt.ReceiptDate,
                        receipt.Number,
                        NumberSequenceModules.PurchaseReceipt,
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
                await _purchaseReceiptRepository.UpdateAsync(receipt, autoSave: true);
            }
        }

        public async Task ConfirmPurchaseReceiptReturnAsync(Guid purchaseReceiptId)
        {
            var receipt = await _purchaseReceiptRepository.GetAsync(purchaseReceiptId);
            if (receipt.Status == PurchaseReceiptStatus.Draft)
            {
                receipt.Status = PurchaseReceiptStatus.Confirm;
                var defaultCompany = await _companyManager.GetDefaultCompanyAsync();
                var defaultWH = defaultCompany.DefaultWarehouseId;
                var vendorWH = await _warehouseManager.GetDefaultWarehouseVendorAsync();
                var lines = _purchaseReceiptDetailRepository.Where(x => x.PurchaseReceiptId.Equals(receipt.Id)).ToList();
                foreach (var item in lines)
                {
                    var movement = await _movementManager.CreateAsync(
                        await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Movement),
                        defaultWH,
                        vendorWH.Id,
                        receipt.ReceiptDate,
                        receipt.Number,
                        NumberSequenceModules.PurchaseReceipt,
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
                await _purchaseReceiptRepository.UpdateAsync(receipt, autoSave: true);
            }
        }


    }
}
