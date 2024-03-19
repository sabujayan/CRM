using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.EmployeeSkills
{
    public class EmployeeSkillMatricesAlreadyExistsException : BusinessException
    {
        public EmployeeSkillMatricesAlreadyExistsException(string employeeNumber)
           : base("EmployeeAlreadyExists")
        {
            WithData("employeeNumber", employeeNumber);
        }
    }
}
