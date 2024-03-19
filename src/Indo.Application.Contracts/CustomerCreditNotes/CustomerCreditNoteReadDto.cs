using Indo.NumberSequences;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CustomerCreditNotes
{
    public class CustomerCreditNoteReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string PaymentNote { get; set; }
        public CustomerCreditNoteStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime CreditNoteDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CurrencyName { get; set; }
        public float Total { get; set; }
        public Guid CustomerInvoiceId { get; set; }
        public string CustomerInvoiceNumber { get; set; }
    }
}
