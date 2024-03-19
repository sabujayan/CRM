using Volo.Abp;

namespace Indo.Vendors
{
    public class VendorAlreadyExistsException : BusinessException
    {
        public VendorAlreadyExistsException(string name)
            : base("VendorAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
