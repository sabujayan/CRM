using Volo.Abp;

namespace Indo.Currencies
{
    public class CurrencyAlreadyExistsException : BusinessException
    {
        public CurrencyAlreadyExistsException(string name)
            : base("CurrencyAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
