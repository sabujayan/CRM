using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Notes
{
    public interface INoteAppService : IApplicationService
    {
        Task<NoteReadDto> GetAsync(Guid id);

        Task<List<NoteReadDto>> GetListAsync();

        Task<List<NoteReadDto>> GetListByCustomerAsync(Guid customerId);

        Task<NoteReadDto> CreateAsync(NoteCreateDto input);

        Task UpdateAsync(Guid id, NoteUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();
    }
}
