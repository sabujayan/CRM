using Volo.Abp;

namespace Indo.GoodsReceipts
{
    public class GoodsReceiptAlreadyExistsException : BusinessException
    {
        public GoodsReceiptAlreadyExistsException(string number)
            : base("GoodsReceiptAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
