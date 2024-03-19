using Indo.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Indo.EmployeeSkills
{
    public interface IEmployeeSkillMatricesRepository : IRepository<EmployeeSkillMatrices, Guid>
    {
    }
}
