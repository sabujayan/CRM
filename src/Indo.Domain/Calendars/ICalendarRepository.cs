using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Calendars
{
    public interface ICalendarRepository : IRepository<Calendar, Guid>
    {
    }
}
