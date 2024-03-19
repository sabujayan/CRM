using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Departments
{
    public interface IDepartmentAppService : IApplicationService
    {
        Task<DepartmentReadDto> GetAsync(Guid id);

        Task<List<DepartmentReadDto>> GetListAsync();

        Task<DepartmentReadDto> CreateAsync(DepartmentCreateDto input);

        Task UpdateAsync(Guid id, DepartmentUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
