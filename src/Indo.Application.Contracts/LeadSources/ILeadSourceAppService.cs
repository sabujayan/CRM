using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.LeadSources
{
    public interface ILeadSourceAppService : IApplicationService
    {
        Task<LeadSourceReadDto> GetAsync(Guid id);

        Task<List<LeadSourceReadDto>> GetListAsync();

        Task<LeadSourceReadDto> CreateAsync(LeadSourceCreateDto input);

        Task UpdateAsync(Guid id, LeadSourceUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
