using Volo.Abp;

namespace Indo.ImportantDates
{
    public class ImportantDateAlreadyExistsException : BusinessException
    {
        public ImportantDateAlreadyExistsException(string name)
            : base("ImportantDateAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
