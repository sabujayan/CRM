using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Bookings
{
    public interface IBookingAppService : IApplicationService
    {
        Task<BookingReadDto> GetAsync(Guid id);

        Task<List<BookingReadDto>> GetListAsync();

        Task<BookingReadDto> CreateAsync(BookingCreateDto input);

        Task UpdateAsync(Guid id, BookingUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<ResourceLookupDto>> GetResourceLookupAsync();
    }
}
