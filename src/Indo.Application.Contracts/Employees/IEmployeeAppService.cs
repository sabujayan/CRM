using Indo.Clientes;
using Indo.Projectes;
using Indo.Skills;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Employees
{
    public interface IEmployeeAppService : IApplicationService
    {
        Task<EmployeeReadDto> GetAsync(Guid id);

         Task<PagedResultDto<EmployeeReadDto>> GetListAsync(GetEmployeeInfoListDto input);

        Task<List<EmployeeReadDto>> GetBuyerListAsync();

        Task<List<EmployeeReadDto>> GetSalesListAsync();

        Task<EmployeeReadDto> CreateAsync(EmployeeCreateDto input);

        Task UpdateAsync(Guid id, EmployeeUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<DepartmentLookupDto>> GetDepartmentLookupAsync();
        Task<ListResultDto<SkillLookupDto>> GetSkillLookupAsync();
        Task<ListResultDto<ClientsLookupDto>> GetClientLookupAsync();
        Task<ListResultDto<ProjectsLookupDto>> GetProjectLookupAsync();
        Task<PagedResultDto<EmployeeReadDto>> GetEmployeeList(GetEmployeeInfoListDto input);
        Task<List<string>> GetClientProjectMapping(Guid id);
    }
}
