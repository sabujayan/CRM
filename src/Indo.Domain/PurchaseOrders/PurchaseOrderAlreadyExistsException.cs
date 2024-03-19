using Volo.Abp;

namespace Indo.PurchaseOrders
{
    public class PurchaseOrderAlreadyExistsException : BusinessException
    {
        public PurchaseOrderAlreadyExistsException(string number)
            : base("PurchaseOrderAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
