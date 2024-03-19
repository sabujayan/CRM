using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Uoms
{
    public interface IUomAppService : IApplicationService
    {
        Task<UomReadDto> GetAsync(Guid id);

        Task<List<UomReadDto>> GetListAsync();

        Task<UomReadDto> CreateAsync(UomCreateDto input);

        Task UpdateAsync(Guid id, UomUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
