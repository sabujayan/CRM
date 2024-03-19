using Volo.Abp;

namespace Indo.Products
{
    public class ProductAlreadyExistsException : BusinessException
    {
        public ProductAlreadyExistsException(string name)
            : base("ProductAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
