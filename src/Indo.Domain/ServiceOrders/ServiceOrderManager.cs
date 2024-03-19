using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerInvoiceDetails;
using Indo.CustomerInvoices;
using Indo.NumberSequences;
using Indo.ServiceOrderDetails;
using Indo.Services;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ServiceOrders
{
    public class ServiceOrderManager : DomainService
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IServiceOrderDetailRepository _serviceOrderDetailRepository;
        private readonly ICustomerInvoiceRepository _customerInvoiceRepository;
        private readonly CustomerInvoiceManager _customerInvoiceManager;
        private readonly ICustomerInvoiceDetailRepository _customerInvoiceDetailRepository;
        private readonly CustomerInvoiceDetailManager _customerInvoiceDetailManager;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly IServiceRepository _serviceRepository;

        public ServiceOrderManager(
            IServiceOrderRepository serviceOrderRepository,
            IServiceOrderDetailRepository serviceOrderDetailRepository,
            ICustomerInvoiceRepository customerInvoiceRepository,
            CustomerInvoiceManager customerInvoiceManager,
            ICustomerInvoiceDetailRepository customerInvoiceDetailRepository,
            CustomerInvoiceDetailManager customerInvoiceDetailManager,
            NumberSequenceManager numberSequenceManager,
            IServiceRepository serviceRepository
            )
        {
            _serviceOrderRepository = serviceOrderRepository;
            _serviceOrderDetailRepository = serviceOrderDetailRepository;
            _customerInvoiceRepository = customerInvoiceRepository;
            _customerInvoiceManager = customerInvoiceManager;
            _customerInvoiceDetailRepository = customerInvoiceDetailRepository;
            _customerInvoiceDetailManager = customerInvoiceDetailManager;
            _numberSequenceManager = numberSequenceManager;
            _serviceRepository = serviceRepository;
        }
        public async Task<ServiceOrder> CreateAsync(
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

            var existing = await _serviceOrderRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new ServiceOrderAlreadyExistsException(number);
            }

            return new ServiceOrder(
                GuidGenerator.Create(),
                number,
                customerId,
                salesExecutiveId,
                orderDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] ServiceOrder serviceOrder,
           [NotNull] string newName)
        {
            Check.NotNull(serviceOrder, nameof(serviceOrder));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _serviceOrderRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != serviceOrder.Id)
            {
                throw new ServiceOrderAlreadyExistsException(newName);
            }

            serviceOrder.ChangeName(newName);
        }

        public async Task<ServiceOrder> ConfirmAsync(Guid serviceOrderId)
        {
            var obj = await _serviceOrderRepository.GetAsync(serviceOrderId);
            if (obj.Status == ServiceOrderStatus.Draft)
            {
                obj.Status = ServiceOrderStatus.Confirm;
                return await _serviceOrderRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<ServiceOrder> CancelAsync(Guid serviceOrderId)
        {
            var obj = await _serviceOrderRepository.GetAsync(serviceOrderId);
            if (obj.Status == ServiceOrderStatus.Confirm)
            {
                obj.Status = ServiceOrderStatus.Cancelled;
                return await _serviceOrderRepository.UpdateAsync(obj, true);
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
                var order = await _serviceOrderRepository.GetAsync(serviceOrderId);
                if (order.Status == ServiceOrderStatus.Confirm)
                {

                    var invoice = await _customerInvoiceManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Invoice),
                            order.CustomerId,
                            DateTime.Now,
                            DateTime.Now.AddDays(14)
                        );

                    invoice.SourceDocument = order.Number;
                    invoice.SourceDocumentId = order.Id;
                    invoice.SourceDocumentModule = NumberSequenceModules.ServiceOrder;

                    var result = await _customerInvoiceRepository.InsertAsync(invoice, true);

                    var queryable = await _serviceOrderDetailRepository.GetQueryableAsync();
                    var query = from serviceOrderDetail in queryable
                                join service in _serviceRepository on serviceOrderDetail.ServiceId equals service.Id
                                where serviceOrderDetail.ServiceOrderId == order.Id
                                select new { serviceOrderDetail, service };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var details = new List<CustomerInvoiceDetail>();
                    foreach (var item in queryResult)
                    {
                        var detail = await _customerInvoiceDetailManager.CreateAsync(
                                result.Id,
                                item.service.Name,
                                item.service.UomId,
                                item.serviceOrderDetail.Price,
                                item.serviceOrderDetail.TaxRate,
                                item.serviceOrderDetail.Quantity,
                                item.serviceOrderDetail.DiscAmt
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
