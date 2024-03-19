using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ClientsProjects
{
    public class ClientsProjectsMatricesManager : DomainService
    {
        private readonly IClientsProjectsMatricesRepository _clientsProjectsMatricesMatricesRepository;
        public ClientsProjectsMatricesManager(IClientsProjectsMatricesRepository clientsProjectsMatricesMatricesRepository)
        {
            _clientsProjectsMatricesMatricesRepository = clientsProjectsMatricesMatricesRepository;
        }
        public async Task<ClientsProjectsMatrices> CreateAsync(
          [NotNull] Guid ClientId,
          [NotNull] Guid ProjectId
          )
        {
            Check.NotNull(ClientId, nameof(ClientId));
            Check.NotNull(ProjectId, nameof(ProjectId));

            return new ClientsProjectsMatrices(
                GuidGenerator.Create(),
                ClientId,
                ProjectId
            );
        }
    }
}
