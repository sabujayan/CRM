using Volo.Abp;

namespace Indo.ServiceQuotations
{
    public class ServiceQuotationAlreadyExistsException : BusinessException
    {
        public ServiceQuotationAlreadyExistsException(string number)
            : base("ServiceQuotationAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
