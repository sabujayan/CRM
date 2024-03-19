using Volo.Abp;

namespace Indo.Resources
{
    public class ResourceAlreadyExistsException : BusinessException
    {
        public ResourceAlreadyExistsException(string name)
            : base("ResourceAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
