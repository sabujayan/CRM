using Volo.Abp;

namespace Indo.CustomerCreditNotes
{
    public class CustomerCreditNoteAlreadyExistsException : BusinessException
    {
        public CustomerCreditNoteAlreadyExistsException(string number)
            : base("CustomerCreditNoteAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
