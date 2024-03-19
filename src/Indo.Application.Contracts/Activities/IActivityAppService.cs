using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Activities
{
    public interface IActivityAppService : IApplicationService
    {
        Task<ActivityReadDto> GetAsync(Guid id);

        Task<List<ActivityReadDto>> GetListAsync();

        Task<ActivityReadDto> CreateAsync(ActivityCreateDto input);

        Task UpdateAsync(Guid id, ActivityUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
