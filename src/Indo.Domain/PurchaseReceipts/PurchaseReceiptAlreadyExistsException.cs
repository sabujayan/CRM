using Volo.Abp;

namespace Indo.PurchaseReceipts
{
    public class PurchaseReceiptAlreadyExistsException : BusinessException
    {
        public PurchaseReceiptAlreadyExistsException(string number)
            : base("PurchaseReceiptAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
