using Volo.Abp;

namespace Indo.Employees
{
    public class EmployeeAlreadyExistsException : BusinessException
    {
        public EmployeeAlreadyExistsException(string employeeNumber)
            : base("EmployeeAlreadyExists")
        {
            WithData("employeeNumber", employeeNumber);
        }
    }
}
