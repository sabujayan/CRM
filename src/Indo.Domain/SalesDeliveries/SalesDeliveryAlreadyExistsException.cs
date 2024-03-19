using Volo.Abp;

namespace Indo.SalesDeliveries
{
    public class SalesDeliveryAlreadyExistsException : BusinessException
    {
        public SalesDeliveryAlreadyExistsException(string number)
            : base("SalesDeliveryAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
