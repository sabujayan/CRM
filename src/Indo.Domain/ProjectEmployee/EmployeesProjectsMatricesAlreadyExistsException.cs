using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.ProjectEmployee
{
    public class EmployeesProjectsMatricesAlreadyExistsException : BusinessException
    {
        public EmployeesProjectsMatricesAlreadyExistsException(string name)
         : base("EmployeesProjectsAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
