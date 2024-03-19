using Volo.Abp;

namespace Indo.Warehouses
{
    public class WarehouseAlreadyExistsException : BusinessException
    {
        public WarehouseAlreadyExistsException(string name)
            : base("WarehouseAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
