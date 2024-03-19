using Volo.Abp;

namespace Indo.VendorPayments
{
    public class VendorPaymentAlreadyExistsException : BusinessException
    {
        public VendorPaymentAlreadyExistsException(string number)
            : base("VendorPaymentAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
