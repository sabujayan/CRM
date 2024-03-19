using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Calendars
{
    public class CalendarManager : DomainService
    {
        private readonly ICalendarRepository _calendarRepository;

        public CalendarManager(ICalendarRepository calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }
        public async Task<Calendar> CreateAsync(
            [NotNull] string name,
            [NotNull] DateTime startTime,
            [NotNull] DateTime endTime
            )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(startTime, nameof(startTime));
            Check.NotNull(endTime, nameof(endTime));

            var existing = await _calendarRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new CalendarAlreadyExistsException(name);
            }

            return new Calendar(
                GuidGenerator.Create(),
                name,
                startTime,
                endTime
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Calendar calendar,
           [NotNull] string newName)
        {
            Check.NotNull(calendar, nameof(calendar));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _calendarRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != calendar.Id)
            {
                throw new CalendarAlreadyExistsException(newName);
            }

            calendar.ChangeName(newName);
        }
    }
}
