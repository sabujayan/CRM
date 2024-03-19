using Volo.Abp;

namespace Indo.Customers
{
    public class CustomerAlreadyExistsException : BusinessException
    {
        public CustomerAlreadyExistsException(string name)
            : base("CustomerAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
