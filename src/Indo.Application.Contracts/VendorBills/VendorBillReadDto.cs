using Indo.NumberSequences;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorBills
{
    public class VendorBillReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string TermCondition { get; set; }
        public string PaymentNote { get; set; }
        public VendorBillStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime BillDueDate { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public string CurrencyName { get; set; }
        public float Total { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }
        public NumberSequenceModules SourceDocumentModule { get; set; }
        public string SourceDocumentModuleString { get; set; }
        public string SourceDocumentPath { get; set; }
    }
}
