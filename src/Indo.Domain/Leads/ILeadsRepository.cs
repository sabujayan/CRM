using Indo.Clientes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Indo.Leads
{
    public interface ILeadsRepository : IRepository<LeadsInfo, Guid>
    {
    }
}
