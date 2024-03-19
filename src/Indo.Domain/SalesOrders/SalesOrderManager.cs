using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerInvoiceDetails;
using Indo.CustomerInvoices;
using Indo.NumberSequences;
using Indo.Products;
using Indo.SalesDeliveries;
using Indo.SalesOrderDetails;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.SalesOrders
{
    public class SalesOrderManager : DomainService
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly ISalesDeliveryRepository _salesDeliveryRepository;
        private readonly SalesDeliveryManager _salesDeliveryManager;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerInvoiceRepository _customerInvoiceRepository;
        private readonly CustomerInvoiceManager _customerInvoiceManager;
        private readonly ICustomerInvoiceDetailRepository _customerInvoiceDetailRepository;
        private readonly CustomerInvoiceDetailManager _customerInvoiceDetailManager;

        public SalesOrderManager(
            ISalesOrderRepository salesOrderRepository,
            ISalesOrderDetailRepository salesOrderDetailRepository,
            ISalesDeliveryRepository salesDeliveryRepository,
            SalesDeliveryManager salesDeliveryManager,
            NumberSequenceManager numberSequenceManager,
            IProductRepository productRepository,
            ICustomerInvoiceRepository customerInvoiceRepository,
            CustomerInvoiceManager customerInvoiceManager,
            ICustomerInvoiceDetailRepository customerInvoiceDetailRepository,
            CustomerInvoiceDetailManager customerInvoiceDetailManager
            )
        {
            _salesOrderRepository = salesOrderRepository;
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _salesDeliveryRepository = salesDeliveryRepository;
            _salesDeliveryManager = salesDeliveryManager;
            _numberSequenceManager = numberSequenceManager;
            _productRepository = productRepository;
            _customerInvoiceRepository = customerInvoiceRepository;
            _customerInvoiceManager = customerInvoiceManager;
            _customerInvoiceDetailRepository = customerInvoiceDetailRepository;
            _customerInvoiceDetailManager = customerInvoiceDetailManager;
        }
        public async Task<SalesOrder> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid customerId,
            [NotNull] Guid salesExecutiveId,
            [NotNull] DateTime orderDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(customerId, nameof(customerId));
            Check.NotNull<Guid>(salesExecutiveId, nameof(salesExecutiveId));
            Check.NotNull<DateTime>(orderDate, nameof(orderDate));

            var existing = await _salesOrderRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new SalesOrderAlreadyExistsException(number);
            }

            return new SalesOrder(
                GuidGenerator.Create(),
                number,
                customerId,
                salesExecutiveId,
                orderDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] SalesOrder salesOrder,
           [NotNull] string newName)
        {
            Check.NotNull(salesOrder, nameof(salesOrder));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _salesOrderRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != salesOrder.Id)
            {
                throw new SalesOrderAlreadyExistsException(newName);
            }

            salesOrder.ChangeName(newName);
        }
        public async Task<SalesOrder> ConfirmAsync(Guid salesOrderId)
        {
            var obj = await _salesOrderRepository.GetAsync(salesOrderId);
            if (obj.Status == SalesOrderStatus.Draft)
            {
                obj.Status = SalesOrderStatus.Confirm;
                return await _salesOrderRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<SalesOrder> CancelAsync(Guid salesOrderId)
        {
            var obj = await _salesOrderRepository.GetAsync(salesOrderId);
            if (obj.Status == SalesOrderStatus.Confirm)
            {
                obj.Status = SalesOrderStatus.Cancelled;
                var query = await _salesDeliveryRepository.GetQueryableAsync();
                var lists = await AsyncExecuter.ToListAsync(query.Where(x => x.Status == SalesDeliveryStatus.Confirm && x.SalesOrderId.Equals(obj.Id)));
                foreach (var item in lists)
                {
                    var returned = await _salesDeliveryManager.GenerateSalesDeliveryReturnFromDeliveryAsync(item.Id);
                    await _salesDeliveryManager.ConfirmSalesDeliveryReturnAsync(returned.Id);
                }
                return await _salesOrderRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }

        public async Task<CustomerInvoice> GenerateInvoice(Guid serviceOrderId)
        {
            try
            {
                var order = await _salesOrderRepository.GetAsync(serviceOrderId);
                if (order.Status == SalesOrderStatus.Confirm)
                {

                    var invoice = await _customerInvoiceManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Invoice),
                            order.CustomerId,
                            DateTime.Now,
                            DateTime.Now.AddDays(14)
                        );

                    invoice.SourceDocument = order.Number;
                    invoice.SourceDocumentId = order.Id;
                    invoice.SourceDocumentModule = NumberSequenceModules.SalesOrder;

                    var result = await _customerInvoiceRepository.InsertAsync(invoice, true);

                    var queryable = await _salesOrderDetailRepository.GetQueryableAsync();
                    var query = from salesOrderDetail in queryable
                                join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                                where salesOrderDetail.SalesOrderId == order.Id
                                select new { salesOrderDetail, product };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var details = new List<CustomerInvoiceDetail>();
                    foreach (var item in queryResult)
                    {
                        var detail = await _customerInvoiceDetailManager.CreateAsync(
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
                    await _customerInvoiceDetailRepository.InsertManyAsync(details, true);

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
