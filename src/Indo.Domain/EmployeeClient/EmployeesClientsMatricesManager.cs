using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.EmployeeClient
{
    public class EmployeesClientsMatricesManager: DomainService
    {
        private readonly IEmployeesClientsMatricesRepository _employeesClientsMatricesRepository;
        public EmployeesClientsMatricesManager(IEmployeesClientsMatricesRepository employeesClientsMatricesRepository)
        {
            _employeesClientsMatricesRepository = employeesClientsMatricesRepository;
        }
        public async Task<EmployeesClientsMatrices> CreateAsync(
           [NotNull] Guid EmployeeId,
           [NotNull] Guid ClientId
           )
        {
            Check.NotNull(EmployeeId, nameof(EmployeeId));
            Check.NotNull(ClientId, nameof(ClientId));
           
            return new EmployeesClientsMatrices(
                GuidGenerator.Create(),
                EmployeeId,
                ClientId
            );
        }
    }
}
