using Volo.Abp;

namespace Indo.TransferOrders
{
    public class TransferOrderAlreadyExistsException : BusinessException
    {
        public TransferOrderAlreadyExistsException(string number)
            : base("TransferOrderAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
