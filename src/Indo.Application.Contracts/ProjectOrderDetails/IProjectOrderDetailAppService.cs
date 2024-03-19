using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.ProjectOrderDetails
{
    public interface IProjectOrderDetailAppService : IApplicationService
    {
        Task<ProjectOrderDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<ProjectOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<ProjectOrderDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<ProjectOrderDetailReadDto>> GetListByProjectOrderAsync(Guid projectOrderId);


        Task<ProjectOrderDetailReadDto> CreateAsync(ProjectOrderDetailCreateDto input);

        Task UpdateAsync(Guid id, ProjectOrderDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<ProjectOrderLookupDto>> GetProjectOrderLookupAsync();
    }
}
