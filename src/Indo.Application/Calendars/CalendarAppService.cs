using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Calendars
{
    public class CalendarAppService : IndoAppService, ICalendarAppService
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly CalendarManager _calendarManager;
        public CalendarAppService(
            ICalendarRepository calendarRepository,
            CalendarManager calendarManager
            )
        {
            _calendarRepository = calendarRepository;
            _calendarManager = calendarManager;
        }
        public async Task<CalendarReadDto> GetAsync(Guid id)
        {
            var obj = await _calendarRepository.GetAsync(id);
            return ObjectMapper.Map<Calendar, CalendarReadDto>(obj);
        }
        public async Task<List<CalendarReadDto>> GetListAsync()
        {
            var queryable = await _calendarRepository.GetQueryableAsync();
            var query = from calendar in queryable
                        select new { calendar };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Calendar, CalendarReadDto>(x.calendar);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<CalendarReadDto> CreateAsync(CalendarCreateDto input)
        {
            var obj = await _calendarManager.CreateAsync(
                input.Name,
                input.StartTime,
                input.EndTime
            );

            obj.Description = input.Description;
            obj.IsDone = input.IsDone;
            obj.Location = input.Location;

            await _calendarRepository.InsertAsync(obj);

            return ObjectMapper.Map<Calendar, CalendarReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, CalendarUpdateDto input)
        {
            var obj = await _calendarRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _calendarManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;
            obj.StartTime = input.StartTime;
            obj.EndTime = input.EndTime;
            obj.IsDone = input.IsDone;
            obj.Location = input.Location;

            await _calendarRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _calendarRepository.DeleteAsync(id);
        }
    }
}
