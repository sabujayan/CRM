using Indo.NumberSequences;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CustomerInvoices
{
    public class CustomerInvoiceReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string TermCondition { get; set; }
        public string PaymentNote { get; set; }
        public CustomerInvoiceStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CurrencyName { get; set; }
        public float Total { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }
        public NumberSequenceModules SourceDocumentModule { get; set; }
        public string SourceDocumentModuleString { get; set; }
        public string SourceDocumentPath { get; set; }
    }
}
