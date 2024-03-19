using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.ServiceOrderDetails;
using Indo.ServiceOrders;
using Indo.ServiceQuotationDetails;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ServiceQuotations
{
    public class ServiceQuotationManager : DomainService
    {
        private readonly IServiceQuotationRepository _serviceQuotationRepository;
        private readonly IServiceQuotationDetailRepository _serviceQuotationDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly ServiceOrderManager _serviceOrderManager;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly ServiceOrderDetailManager _serviceOrderDetailManager;
        private readonly IServiceOrderDetailRepository _serviceOrderDetailRepository;

        public ServiceQuotationManager(
            IServiceQuotationRepository serviceQuotationRepository,
            IServiceQuotationDetailRepository serviceQuotationDetailRepository,
            NumberSequenceManager numberSequenceManager,
            ServiceOrderManager serviceOrderManager,
            IServiceOrderRepository serviceOrderRepository,
            ServiceOrderDetailManager serviceOrderDetailManager,
            IServiceOrderDetailRepository serviceOrderDetailRepository
            )
        {
            _serviceQuotationRepository = serviceQuotationRepository;
            _numberSequenceManager = numberSequenceManager;
            _serviceOrderManager = serviceOrderManager;
            _serviceOrderRepository = serviceOrderRepository;
            _serviceOrderDetailManager = serviceOrderDetailManager;
            _serviceOrderDetailRepository = serviceOrderDetailRepository;
            _serviceQuotationDetailRepository = serviceQuotationDetailRepository;
        }
        public async Task<ServiceQuotation> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid customerId,
            [NotNull] Guid salesExecutiveId,
            [NotNull] DateTime quotationDate,
            [NotNull] DateTime quotationValidUntilDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(customerId, nameof(customerId));
            Check.NotNull<Guid>(salesExecutiveId, nameof(salesExecutiveId));
            Check.NotNull<DateTime>(quotationDate, nameof(quotationDate));
            Check.NotNull<DateTime>(quotationValidUntilDate, nameof(quotationValidUntilDate));

            var existing = await _serviceQuotationRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new ServiceQuotationAlreadyExistsException(number);
            }

            return new ServiceQuotation(
                GuidGenerator.Create(),
                number,
                customerId,
                salesExecutiveId,
                quotationDate,
                quotationValidUntilDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] ServiceQuotation serviceQuotation,
           [NotNull] string newName)
        {
            Check.NotNull(serviceQuotation, nameof(serviceQuotation));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _serviceQuotationRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != serviceQuotation.Id)
            {
                throw new ServiceQuotationAlreadyExistsException(newName);
            }

            serviceQuotation.ChangeName(newName);
        }

        public async Task<ServiceQuotation> ConfirmAsync(Guid serviceQuotationId)
        {
            var obj = await _serviceQuotationRepository.GetAsync(serviceQuotationId);
            if (obj.Status == ServiceQuotationStatus.Draft)
            {
                obj.Status = ServiceQuotationStatus.Confirm;
                return await _serviceQuotationRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<ServiceQuotation> CancelAsync(Guid serviceQuotationId)
        {
            var obj = await _serviceQuotationRepository.GetAsync(serviceQuotationId);
            if (obj.Status == ServiceQuotationStatus.Confirm)
            {
                obj.Status = ServiceQuotationStatus.Cancelled;
                return await _serviceQuotationRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }

        public async Task<ServiceOrder> ConvertToOrder(Guid serviceQuotationId)
        {
            try
            {
                var quotation = await _serviceQuotationRepository.GetAsync(serviceQuotationId);
                if (quotation.Status == ServiceQuotationStatus.Confirm)
                {

                    var order = await _serviceOrderManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.ServiceOrder),
                            quotation.CustomerId,
                            quotation.SalesExecutiveId,
                            quotation.QuotationDate
                        );

                    order.SourceDocument = quotation.Number;
                    order.SourceDocumentId = quotation.Id;
                    var result = await _serviceOrderRepository.InsertAsync(order);

                    var query = await _serviceQuotationDetailRepository.GetQueryableAsync();
                    var quotationItems = await AsyncExecuter.ToListAsync(query.Where(x => x.ServiceQuotationId == quotation.Id));
                    var details = new List<ServiceOrderDetail>();
                    foreach (var item in quotationItems)
                    {
                        var detail = await _serviceOrderDetailManager.CreateAsync(
                                result.Id,
                                item.ServiceId,
                                item.Quantity,
                                item.DiscAmt
                            );
                        details.Add(detail);

                    }
                    await _serviceOrderDetailRepository.InsertManyAsync(details);

                    return order;
                }
                else
                {
                    throw new UserFriendlyException("Only Confirm can be converted!");
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Error: " + ex.Message);
            }
        }
    }
}
