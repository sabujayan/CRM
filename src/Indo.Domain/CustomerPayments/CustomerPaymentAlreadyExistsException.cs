using Volo.Abp;

namespace Indo.CustomerPayments
{
    public class CustomerPaymentAlreadyExistsException : BusinessException
    {
        public CustomerPaymentAlreadyExistsException(string number)
            : base("CustomerPaymentAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
