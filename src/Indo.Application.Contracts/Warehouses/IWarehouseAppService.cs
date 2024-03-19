using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Warehouses
{
    public interface IWarehouseAppService : IApplicationService
    {
        Task<WarehouseReadDto> GetAsync(Guid id);

        Task<List<WarehouseReadDto>> GetListAsync();

        Task<WarehouseReadDto> CreateAsync(WarehouseCreateDto input);

        Task UpdateAsync(Guid id, WarehouseUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
