using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.ImportantDates
{
    public interface IImportantDateRepository : IRepository<ImportantDate, Guid>
    {
    }
}
