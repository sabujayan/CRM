using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Movements
{
    public interface IMovementAppService : IApplicationService
    {
        Task<MovementReadDto> GetAsync(Guid id);

        Task<List<MovementReadDto>> GetListAsync();

        Task<MovementReadDto> CreateAsync(MovementCreateDto input);

        Task UpdateAsync(Guid id, MovementUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<WarehouseLookupDto>> GetWarehouseLookupAsync();
    }
}
