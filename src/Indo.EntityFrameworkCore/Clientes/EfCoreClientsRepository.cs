using Indo.EntityFrameworkCore;
using Indo.Projectes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.Clientes
{
    public class EfCoreClientsRepository
    : EfCoreRepository<IndoDbContext, Clients, Guid>,
            IClientsRepository
    {
        public EfCoreClientsRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<List<Clients>> GetAllProjectsAsync(int SkipCount, int MaxResultCount, string? Sorting, string? SortingColumn, string Filter = null, string nameFilter = null, string emailFilter = null, string phoneNoFilter = null, string countryFilter = null, string stateFilter = null, string cityFilter = null, string zipFilter = null, string? clientProjectsFilter = null)
        {
            throw new NotImplementedException();
        }

        Task<Clients> IClientsRepository.FindByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
