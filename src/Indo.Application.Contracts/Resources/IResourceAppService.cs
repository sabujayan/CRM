using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Resources
{
    public interface IResourceAppService : IApplicationService
    {
        Task<ResourceReadDto> GetAsync(Guid id);

        Task<List<ResourceReadDto>> GetListAsync();

        Task<ResourceReadDto> CreateAsync(ResourceCreateDto input);

        Task UpdateAsync(Guid id, ResourceUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
