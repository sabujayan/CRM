using Volo.Abp;

namespace Indo.Activities
{
    public class ActivityAlreadyExistsException : BusinessException
    {
        public ActivityAlreadyExistsException(string name)
            : base("ActivityAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
