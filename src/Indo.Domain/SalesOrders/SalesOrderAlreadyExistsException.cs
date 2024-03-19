using Volo.Abp;

namespace Indo.SalesOrders
{
    public class SalesOrderAlreadyExistsException : BusinessException
    {
        public SalesOrderAlreadyExistsException(string number)
            : base("SalesOrderAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
