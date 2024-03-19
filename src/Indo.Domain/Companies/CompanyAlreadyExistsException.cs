using Volo.Abp;

namespace Indo.Companies
{
    public class CompanyAlreadyExistsException : BusinessException
    {
        public CompanyAlreadyExistsException(string name)
            : base("CompanyAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
