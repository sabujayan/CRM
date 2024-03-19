using Volo.Abp;

namespace Indo.ServiceOrders
{
    public class ServiceOrderAlreadyExistsException : BusinessException
    {
        public ServiceOrderAlreadyExistsException(string number)
            : base("ServiceOrderAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
