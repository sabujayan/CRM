using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.LeadRatings
{
    public interface ILeadRatingAppService : IApplicationService
    {
        Task<LeadRatingReadDto> GetAsync(Guid id);

        Task<List<LeadRatingReadDto>> GetListAsync();

        Task<LeadRatingReadDto> CreateAsync(LeadRatingCreateDto input);

        Task UpdateAsync(Guid id, LeadRatingUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
