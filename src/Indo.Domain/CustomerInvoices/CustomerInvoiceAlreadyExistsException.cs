using Volo.Abp;

namespace Indo.CustomerInvoices
{
    public class CustomerInvoiceAlreadyExistsException : BusinessException
    {
        public CustomerInvoiceAlreadyExistsException(string number)
            : base("CustomerInvoiceAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
