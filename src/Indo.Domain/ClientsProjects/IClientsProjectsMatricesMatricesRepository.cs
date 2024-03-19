using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Indo.ClientsProjects
{
    public interface IClientsProjectsMatricesRepository : IRepository<ClientsProjectsMatrices, Guid>
    {
    }
}
