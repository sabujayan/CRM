using Volo.Abp;

namespace Indo.Departments
{
    public class DepartmentAlreadyExistsException : BusinessException
    {
        public DepartmentAlreadyExistsException(string name)
            : base("DepartmentAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
