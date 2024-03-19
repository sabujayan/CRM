using Indo.Clientes;
using Indo.Technologies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Projectes
{
    public interface IProjectsAppService : IApplicationService
    {
        Task<ProjectsReadDto> GetAsync(Guid id);
        Task<List<ProjectsReadDto>> GetListAsync();
        Task<ProjectsReadDto> CreateAsync(ProjectsCreateDto input);
        Task UpdateAsync(Guid id, ProjectsUpdateDto input);
        Task DeleteAsync(Guid id);
        Task<ListResultDto<ClientsLookupDto>> GetClientLookupAsync();
        Task<PagedResultDto<ProjectsReadDto>> GetProjectList(GetProjectInfoListDto input);
        Task<ListResultDto<TechnologyLookupDto>> GetTechnologyLookupAsync();
       


    }
}
