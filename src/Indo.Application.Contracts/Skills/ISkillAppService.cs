using Indo.Departments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Indo.Skills
{
    public interface ISkillAppService : IApplicationService
    {
        Task<SkillReadDto> GetAsync(Guid id);

        Task<List<SkillReadDto>> GetListAsync();

        Task<SkillReadDto> CreateAsync(SkillCreateDto input);

        Task UpdateAsync(Guid id, SkillUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
