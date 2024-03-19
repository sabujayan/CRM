using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.Products;
using Indo.PurchaseOrderDetails;
using Indo.PurchaseReceipts;
using Indo.VendorBillDetails;
using Indo.VendorBills;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.PurchaseOrders
{
    public class PurchaseOrderManager : DomainService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        private readonly IPurchaseReceiptRepository _purchaseReceiptRepository;
        private readonly PurchaseReceiptManager _purchaseReceiptManager;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly IProductRepository _productRepository;
        private readonly IVendorBillRepository _vendorBillRepository;
        private readonly VendorBillManager _vendorBillManager;
        private readonly IVendorBillDetailRepository _vendorBillDetailRepository;
        private readonly VendorBillDetailManager _vendorBillDetailManager;

        public PurchaseOrderManager(
            IPurchaseOrderRepository purchaseOrderRepository,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
            IPurchaseReceiptRepository purchaseReceiptRepository,
            PurchaseReceiptManager purchaseReceiptManager,
            NumberSequenceManager numberSequenceManager,
            IProductRepository productRepository,
            IVendorBillRepository vendorBillRepository,
            VendorBillManager vendorBillManager,
            IVendorBillDetailRepository vendorBillDetailRepository,
            VendorBillDetailManager vendorBillDetailManager
            )
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
            _purchaseReceiptRepository = purchaseReceiptRepository;
            _purchaseReceiptManager = purchaseReceiptManager;
            _numberSequenceManager = numberSequenceManager;
            _productRepository = productRepository;
            _vendorBillRepository = vendorBillRepository;
            _vendorBillManager = vendorBillManager;
            _vendorBillDetailRepository = vendorBillDetailRepository;
            _vendorBillDetailManager = vendorBillDetailManager;
        }
        public async Task<PurchaseOrder> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid vendorId,
            [NotNull] Guid buyerId,
            [NotNull] DateTime orderDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(vendorId, nameof(vendorId));
            Check.NotNull<Guid>(buyerId, nameof(buyerId));
            Check.NotNull<DateTime>(orderDate, nameof(orderDate));

            var existing = await _purchaseOrderRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new PurchaseOrderAlreadyExistsException(number);
            }

            return new PurchaseOrder(
                GuidGenerator.Create(),
                number,
                vendorId,
                buyerId,
                orderDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] PurchaseOrder purchaseOrder,
           [NotNull] string newName)
        {
            Check.NotNull(purchaseOrder, nameof(purchaseOrder));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _purchaseOrderRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != purchaseOrder.Id)
            {
                throw new PurchaseOrderAlreadyExistsException(newName);
            }

            purchaseOrder.ChangeName(newName);
        }



        public async Task<PurchaseOrder> ConfirmAsync(Guid purchaseOrderId)
        {
            var obj = await _purchaseOrderRepository.GetAsync(purchaseOrderId);
            if (obj.Status == PurchaseOrderStatus.Draft)
            {
                obj.Status = PurchaseOrderStatus.Confirm;
                return await _purchaseOrderRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<PurchaseOrder> CancelAsync(Guid purchaseOrderId)
        {
            var obj = await _purchaseOrderRepository.GetAsync(purchaseOrderId);
            if (obj.Status == PurchaseOrderStatus.Confirm)
            {
                obj.Status = PurchaseOrderStatus.Cancelled;
                var query = await _purchaseReceiptRepository.GetQueryableAsync();
                var lists = await AsyncExecuter.ToListAsync(query.Where(x => x.Status == PurchaseReceiptStatus.Confirm && x.PurchaseOrderId.Equals(obj.Id)));
                foreach (var item in lists)
                {
                    var returned = await _purchaseReceiptManager.GeneratePurchaseReceiptReturnFromReceiptAsync(item.Id);
                    await _purchaseReceiptManager.ConfirmPurchaseReceiptReturnAsync(returned.Id);
                }
                return await _purchaseOrderRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }


        public async Task<VendorBill> GenerateBill(Guid purchaseOrderId)
        {
            try
            {
                var order = await _purchaseOrderRepository.GetAsync(purchaseOrderId);
                if (order.Status == PurchaseOrderStatus.Confirm)
                {

                    var invoice = await _vendorBillManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Bill),
                            order.VendorId,
                            DateTime.Now,
                            DateTime.Now.AddDays(14)
                        );

                    invoice.SourceDocument = order.Number;
                    invoice.SourceDocumentId = order.Id;
                    invoice.SourceDocumentModule = NumberSequenceModules.PurchaseOrder;

                    var result = await _vendorBillRepository.InsertAsync(invoice, true);

                    var queryable = await _purchaseOrderDetailRepository.GetQueryableAsync();
                    var query = from salesOrderDetail in queryable
                                join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                                where salesOrderDetail.PurchaseOrderId == order.Id
                                select new { salesOrderDetail, product };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var details = new List<VendorBillDetail>();
                    foreach (var item in queryResult)
                    {
                        var detail = await _vendorBillDetailManager.CreateAsync(
                                result.Id,
                                item.product.Name,
                                item.product.UomId,
                                item.salesOrderDetail.Price,
                                item.salesOrderDetail.TaxRate,
                                item.salesOrderDetail.Quantity,
                                item.salesOrderDetail.DiscAmt
                            );
                        details.Add(detail);

                    }
                    await _vendorBillDetailRepository.InsertManyAsync(details, true);

                    return invoice;
                }
                else
                {
                    throw new UserFriendlyException("Only Confirm can be processed!");
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Error: " + ex.Message);
            }
        }


    }
}
