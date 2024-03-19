using Volo.Abp;

namespace Indo.SalesQuotations
{
    public class SalesQuotationAlreadyExistsException : BusinessException
    {
        public SalesQuotationAlreadyExistsException(string number)
            : base("SalesQuotationAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
