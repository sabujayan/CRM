using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ServiceQuotations
{
    public class ServiceQuotation : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public ServiceQuotationStatus Status { get; set; }
        public DateTime QuotationDate { get; set; }
        public DateTime QuotationValidUntilDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SalesExecutiveId { get; set; }
        public ServiceQuotationPipeline Pipeline { get; set; }

        private ServiceQuotation() { }
        internal ServiceQuotation(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid customerId,
            [NotNull] Guid salesExecutiveId,
            [NotNull] DateTime quotationDate,
            [NotNull] DateTime quotationValidUntilDate
            ) 
            : base(id)
        {
            SetName(number);
            CustomerId = customerId;
            SalesExecutiveId = salesExecutiveId;
            QuotationDate = quotationDate;
            QuotationValidUntilDate = quotationValidUntilDate;
            Status = ServiceQuotationStatus.Draft;
            Pipeline = ServiceQuotationPipeline.New;
        }        
        internal ServiceQuotation ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: ServiceQuotationConsts.MaxNumberLength
                );
        }
    }
}
