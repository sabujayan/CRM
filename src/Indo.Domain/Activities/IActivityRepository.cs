using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Activities
{
    public interface IActivityRepository : IRepository<Activity, Guid>
    {
    }
}
