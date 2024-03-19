using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ProjectEmployee
{
    public class EmployeesProjectsMatricesManager: DomainService
    {
        private readonly IEmployeesProjectsMatricesRepository _employeesProjectsMatricesRepository;
        public EmployeesProjectsMatricesManager(IEmployeesProjectsMatricesRepository employeesProjectsMatricesRepository)
        {
            _employeesProjectsMatricesRepository = employeesProjectsMatricesRepository;
        }
        public async Task<EmployeesProjectsMatrices> CreateAsync(
             [NotNull] Guid clientsid,
             [NotNull] Guid projectsid
            )
        {
            Check.NotNull(clientsid, nameof(clientsid));
            Check.NotNull(projectsid, nameof(projectsid));

            return new EmployeesProjectsMatrices(
                GuidGenerator.Create(),
                clientsid,
                projectsid
            );
        }
    }
}
