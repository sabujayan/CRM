using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Calendars
{
    public interface ICalendarAppService : IApplicationService
    {
        Task<CalendarReadDto> GetAsync(Guid id);

        Task<List<CalendarReadDto>> GetListAsync();

        Task<CalendarReadDto> CreateAsync(CalendarCreateDto input);

        Task UpdateAsync(Guid id, CalendarUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
