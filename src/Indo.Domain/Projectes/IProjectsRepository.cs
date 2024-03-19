using Indo.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Indo.Projectes
{
    public interface IProjectsRepository : IRepository<Projects, Guid>
    {
        Task<Projects> FindByNameAsync(string name);

        Task<List<Projects>> GetAllProjectsAsync(int SkipCount, int MaxResultCount, string? sorting, string? Filter = null, string? nameFilter = null, string? startdate = null, string? enddate = null, string? estimateFilter = null, string? technologyFilter = null);


    



    }
}
