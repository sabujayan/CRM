using Volo.Abp;

namespace Indo.VendorDebitNotes
{
    public class VendorDebitNoteAlreadyExistsException : BusinessException
    {
        public VendorDebitNoteAlreadyExistsException(string number)
            : base("VendorDebitNoteAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
