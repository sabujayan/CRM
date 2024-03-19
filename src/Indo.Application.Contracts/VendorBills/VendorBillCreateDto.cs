using Indo.NumberSequences;
using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.VendorBills
{
    public class VendorBillCreateDto
    {

        [Required]
        [StringLength(VendorBillConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }
        public string TermCondition { get; set; }
        public string PaymentNote { get; set; }

        [Required]
        public DateTime BillDate { get; set; }

        [Required]
        public DateTime BillDueDate { get; set; }

        [Required]
        public Guid VendorId { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }
        public NumberSequenceModules SourceDocumentModule { get; set; }
    }
}
