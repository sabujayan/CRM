using Indo.CustomerPayments;
using Indo.NumberSequences;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorPayments
{
    public class VendorPaymentReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid CashAndBankId { get; set; }
        public string CashAndBankName { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public float Amount { get; set; }
        public float Debit { get; set; }
        public float Credit { get; set; }
        public float Balance { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }
        public NumberSequenceModules SourceDocumentModule { get; set; }
        public string SourceDocumentModuleString { get; set; }
        public string CurrencyName { get; set; }
        public string SourceDocumentPath { get; set; }
        public CustomerPaymentStatus Status { get; set; }
        public string StatusString { get; set; }
        public string Period { get; set; }
    }
}
