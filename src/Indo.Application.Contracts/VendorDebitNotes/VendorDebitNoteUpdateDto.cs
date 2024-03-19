using Indo.NumberSequences;
using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.VendorDebitNotes
{
    public class VendorDebitNoteUpdateDto
    {

        [Required]
        [StringLength(VendorDebitNoteConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }
        public string PaymentNote { get; set; }
        public VendorDebitNoteStatus Status { get; set; }

        [Required]
        public DateTime DebitNoteDate { get; set; }

        [Required]
        public Guid VendorId { get; set; }

        [Required]
        public Guid VendorBillId { get; set; }
    }
}
