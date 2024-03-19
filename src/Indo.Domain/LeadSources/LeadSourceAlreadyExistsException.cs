using Volo.Abp;

namespace Indo.LeadSources
{
    public class LeadSourceAlreadyExistsException : BusinessException
    {
        public LeadSourceAlreadyExistsException(string name)
            : base("LeadSourceAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
