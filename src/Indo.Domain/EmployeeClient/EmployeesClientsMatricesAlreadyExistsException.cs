using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.EmployeeClient
{
    public class EmployeesClientsMatricesAlreadyExistsException : BusinessException
    {
        public EmployeesClientsMatricesAlreadyExistsException(string employeeNumber)
         : base("EmployeesClientsMatricesAlreadyExists")
        {
            WithData("employeeNumber", employeeNumber);
        }
    }
}
