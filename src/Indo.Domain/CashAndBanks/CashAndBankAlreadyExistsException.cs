using Volo.Abp;

namespace Indo.CashAndBanks
{
    public class CashAndBankAlreadyExistsException : BusinessException
    {
        public CashAndBankAlreadyExistsException(string name)
            : base("CashAndBankAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
