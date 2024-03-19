using Volo.Abp;

namespace Indo.Tasks
{
    public class TaskAlreadyExistsException : BusinessException
    {
        public TaskAlreadyExistsException(string name)
            : base("TaskAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
