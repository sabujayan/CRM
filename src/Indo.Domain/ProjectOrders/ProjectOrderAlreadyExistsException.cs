using Volo.Abp;

namespace Indo.ProjectOrders
{
    public class ProjectOrderAlreadyExistsException : BusinessException
    {
        public ProjectOrderAlreadyExistsException(string number)
            : base("ProjectOrderAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
