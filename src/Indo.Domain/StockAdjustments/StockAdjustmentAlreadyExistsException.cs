using Volo.Abp;

namespace Indo.StockAdjustments
{
    public class StockAdjustmentAlreadyExistsException : BusinessException
    {
        public StockAdjustmentAlreadyExistsException(string number)
            : base("StockAdjustmentAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
