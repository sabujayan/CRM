using Volo.Abp;

namespace Indo.Uoms
{
    public class UomAlreadyExistsException : BusinessException
    {
        public UomAlreadyExistsException(string name)
            : base("UomAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
