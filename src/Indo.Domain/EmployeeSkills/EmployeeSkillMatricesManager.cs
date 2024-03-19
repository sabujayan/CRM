using Indo.Customers;
using Indo.EmployeeClient;
using Indo.Employees;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.EmployeeSkills
{
    public class EmployeeSkillMatricesManager:DomainService
    {
        private readonly IEmployeeSkillMatricesRepository _employeeSkillMatricesRepository;
        public EmployeeSkillMatricesManager(IEmployeeSkillMatricesRepository employeeSkillMatricesRepository)
        {
            _employeeSkillMatricesRepository = employeeSkillMatricesRepository;
        }
        public async Task<EmployeeSkillMatrices> CreateAsync(
         [NotNull] Guid EmployeeId,
           [NotNull] Guid SkillId
            )
        {
            Check.NotNull(EmployeeId, nameof(EmployeeId));
            Check.NotNull(SkillId, nameof(SkillId));
            return new EmployeeSkillMatrices(
                GuidGenerator.Create(),
                EmployeeId,
                SkillId
            );
        }
    }
}
