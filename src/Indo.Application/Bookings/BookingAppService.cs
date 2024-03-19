using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Resources;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Bookings
{
    public class BookingAppService : IndoAppService, IBookingAppService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly BookingManager _bookingManager;
        private readonly IResourceRepository _resourceRepository;
        public BookingAppService(
            IBookingRepository bookingRepository,
            BookingManager bookingManager,
            IResourceRepository resourceRepository
            )
        {
            _bookingRepository = bookingRepository;
            _bookingManager = bookingManager;
            _resourceRepository = resourceRepository;
        }
        public async Task<BookingReadDto> GetAsync(Guid id)
        {
            var obj = await _bookingRepository.GetAsync(id);
            return ObjectMapper.Map<Booking, BookingReadDto>(obj);
        }
        public async Task<List<BookingReadDto>> GetListAsync()
        {
            var queryable = await _bookingRepository.GetQueryableAsync();
            var query = from booking in queryable
                        join resource in _resourceRepository on booking.ResourceId equals resource.Id
                        select new { booking, resource };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Booking, BookingReadDto>(x.booking);
                dto.ResourceName = x.resource.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<ListResultDto<ResourceLookupDto>> GetResourceLookupAsync()
        {
            var list = await _resourceRepository.GetListAsync();
            return new ListResultDto<ResourceLookupDto>(
                ObjectMapper.Map<List<Resource>, List<ResourceLookupDto>>(list)
            );
        }
        public async Task<BookingReadDto> CreateAsync(BookingCreateDto input)
        {
            var obj = await _bookingManager.CreateAsync(
                input.Name,
                input.StartTime,
                input.EndTime,
                input.ResourceId
            );

            obj.Description = input.Description;
            obj.IsDone = input.IsDone;
            obj.Location = input.Location;

            await _bookingRepository.InsertAsync(obj);

            return ObjectMapper.Map<Booking, BookingReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, BookingUpdateDto input)
        {
            var obj = await _bookingRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _bookingManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;
            obj.StartTime = input.StartTime;
            obj.EndTime = input.EndTime;
            obj.IsDone = input.IsDone;
            obj.ResourceId = input.ResourceId;
            obj.Location = input.Location;

            await _bookingRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _bookingRepository.DeleteAsync(id);
        }
    }
}
