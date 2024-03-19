using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Currencies
{
    public interface ICurrencyAppService : IApplicationService
    {
        Task<CurrencyReadDto> GetAsync(Guid id);

        Task<List<CurrencyReadDto>> GetListAsync();

        Task<CurrencyReadDto> CreateAsync(CurrencyCreateDto input);

        Task UpdateAsync(Guid id, CurrencyUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
