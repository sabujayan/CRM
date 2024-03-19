using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.SalesOrderDetails;
using Indo.SalesOrders;
using Indo.SalesQuotationDetails;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.SalesQuotations
{
    public class SalesQuotationManager : DomainService
    {
        private readonly ISalesQuotationRepository _salesQuotationRepository;
        private readonly ISalesQuotationDetailRepository _salesQuotationDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly SalesOrderManager _salesOrderManager;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly SalesOrderDetailManager _salesOrderDetailManager;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;

        public SalesQuotationManager(
            ISalesQuotationRepository serviceQuotationRepository,
            ISalesQuotationDetailRepository salesQuotationDetailRepository,
            NumberSequenceManager numberSequenceManager,
            SalesOrderManager salesOrderManager,
            ISalesOrderRepository salesOrderRepository,
            SalesOrderDetailManager salesOrderDetailManager,
            ISalesOrderDetailRepository salesOrderDetailRepository
            )
        {
            _salesQuotationRepository = serviceQuotationRepository;
            _numberSequenceManager = numberSequenceManager;
            _salesOrderManager = salesOrderManager;
            _salesOrderRepository = salesOrderRepository;
            _salesOrderDetailManager = salesOrderDetailManager;
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _salesQuotationDetailRepository = salesQuotationDetailRepository;
        }
        public async Task<SalesQuotation> CreateAsync(
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

            var existing = await _salesQuotationRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new SalesQuotationAlreadyExistsException(number);
            }

            return new SalesQuotation(
                GuidGenerator.Create(),
                number,
                customerId,
                salesExecutiveId,
                quotationDate,
                quotationValidUntilDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] SalesQuotation serviceQuotation,
           [NotNull] string newName)
        {
            Check.NotNull(serviceQuotation, nameof(serviceQuotation));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _salesQuotationRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != serviceQuotation.Id)
            {
                throw new SalesQuotationAlreadyExistsException(newName);
            }

            serviceQuotation.ChangeName(newName);
        }

        public async Task<SalesQuotation> ConfirmAsync(Guid serviceQuotationId)
        {
            var obj = await _salesQuotationRepository.GetAsync(serviceQuotationId);
            if (obj.Status == SalesQuotationStatus.Draft)
            {
                obj.Status = SalesQuotationStatus.Confirm;
                return await _salesQuotationRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<SalesQuotation> CancelAsync(Guid serviceQuotationId)
        {
            var obj = await _salesQuotationRepository.GetAsync(serviceQuotationId);
            if (obj.Status == SalesQuotationStatus.Confirm)
            {
                obj.Status = SalesQuotationStatus.Cancelled;
                return await _salesQuotationRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }

        public async Task<SalesOrder> ConvertToOrder(Guid salesQuotationId)
        {
            try
            {
                var quotation = await _salesQuotationRepository.GetAsync(salesQuotationId);
                if (quotation.Status == SalesQuotationStatus.Confirm)
                {

                    var order = await _salesOrderManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.SalesOrder),
                            quotation.CustomerId,
                            quotation.SalesExecutiveId,
                            quotation.QuotationDate
                        );

                    order.SourceDocument = quotation.Number;
                    order.SourceDocumentId = quotation.Id;
                    var result = await _salesOrderRepository.InsertAsync(order);

                    var query = await _salesQuotationDetailRepository.GetQueryableAsync();
                    var quotationItems = await AsyncExecuter.ToListAsync(query.Where(x => x.SalesQuotationId == quotation.Id));
                    var details = new List<SalesOrderDetail>();
                    foreach (var item in quotationItems)
                    {
                        var detail = await _salesOrderDetailManager.CreateAsync(
                                result.Id,
                                item.ProductId,
                                item.Quantity,
                                item.DiscAmt
                            );
                        details.Add(detail);

                    }
                    await _salesOrderDetailRepository.InsertManyAsync(details);

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
