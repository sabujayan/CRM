using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Technologies
{
    public interface ITechnologyAppService : IApplicationService
    {
        Task<TechnologyReadDto> GetAsync(Guid id);
        Task<List<TechnologyReadDto>> GetListAsync();
        Task<TechnologyReadDto> CreateAsync(TechnologyCreateDto input);
        Task UpdateAsync(Guid id, TechnologyUpdateDto input);
        Task DeleteAsync(Guid id);
        Task<ListResultDto<TechnologyLookupDto>> GetTechnologyLookupAsync();
        Task<PagedResultDto<TechnologyReadDto>> GetTechnologyList(GetTechnologyInfoListDto input);
    }
}
