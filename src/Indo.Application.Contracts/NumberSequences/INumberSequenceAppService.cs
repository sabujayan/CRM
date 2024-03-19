using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.NumberSequences
{
    public interface INumberSequenceAppService : IApplicationService
    {
        Task<NumberSequenceReadDto> GetAsync(Guid id);

        Task<string> GetNextNumberAsync(NumberSequenceModules module);

        Task<List<NumberSequenceReadDto>> GetListAsync();

        Task<NumberSequenceReadDto> CreateAsync(NumberSequenceCreateDto input);

        Task UpdateAsync(Guid id, NumberSequenceUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
