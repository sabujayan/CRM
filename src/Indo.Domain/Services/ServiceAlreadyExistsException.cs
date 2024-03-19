using Volo.Abp;

namespace Indo.Services
{
    public class ServiceAlreadyExistsException : BusinessException
    {
        public ServiceAlreadyExistsException(string name)
            : base("ServiceAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
