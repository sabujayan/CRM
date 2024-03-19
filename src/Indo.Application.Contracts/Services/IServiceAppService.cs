using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Services
{
    public interface IServiceAppService : IApplicationService
    {
        Task<ServiceReadDto> GetAsync(Guid id);

        Task<List<ServiceReadDto>> GetListAsync();

        Task<ServiceReadDto> CreateAsync(ServiceCreateDto input);

        Task UpdateAsync(Guid id, ServiceUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<UomLookupDto>> GetUomLookupAsync();
    }
}
