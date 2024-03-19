using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.LeadSources
{
    public interface ILeadSourceRepository : IRepository<LeadSource, Guid>
    {
    }
}
