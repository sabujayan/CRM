using Indo.NumberSequences;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorDebitNotes
{
    public class VendorDebitNoteReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string PaymentNote { get; set; }
        public VendorDebitNoteStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime DebitNoteDate { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public string CurrencyName { get; set; }
        public float Total { get; set; }
        public Guid VendorBillId { get; set; }
        public string VendorBillNumber { get; set; }
    }
}
