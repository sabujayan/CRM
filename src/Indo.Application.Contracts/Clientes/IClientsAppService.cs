using Indo.Projectes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Clientes
{
    public interface IClientsAppService : IApplicationService
    {
        Task<ClientsReadDto> GetAsync(Guid id);
        Task<List<ClientsReadDto>> GetListAsync();
        Task<ClientsReadDto> CreateAsync(ClientsCreateDto input);
        Task UpdateAsync(Guid id, ClientsUpdateDto input);
        Task DeleteAsync(Guid id);
        Task<ListResultDto<ProjectsLookupDto>> GetProjectLookupAsync();
        Task<PagedResultDto<ClientsReadDto>> GetClientList(GetClientInfoListDto input);
        Task<ClientRegisterDto> ClientRegister(ClientRegisterDto input);
    }
}
