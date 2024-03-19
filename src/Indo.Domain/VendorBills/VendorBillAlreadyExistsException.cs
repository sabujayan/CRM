using Volo.Abp;

namespace Indo.VendorBills
{
    public class VendorBillAlreadyExistsException : BusinessException
    {
        public VendorBillAlreadyExistsException(string number)
            : base("VendorBillAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
