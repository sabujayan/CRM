using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Companies
{
    public interface ICompanyRepository : IRepository<Company, Guid>
    {
    }
}
