using Volo.Abp;

namespace Indo.DeliveryOrders
{
    public class DeliveryOrderAlreadyExistsException : BusinessException
    {
        public DeliveryOrderAlreadyExistsException(string number)
            : base("DeliveryOrderAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
