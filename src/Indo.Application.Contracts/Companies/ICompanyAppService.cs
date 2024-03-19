using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Companies
{
    public interface ICompanyAppService : IApplicationService
    {
        Task<CompanyReadDto> GetAsync(Guid id);
        Task<CompanyReadDto> GetDefaultCompanyAsync();

        Task<List<CompanyReadDto>> GetListAsync();

        Task<CompanyReadDto> CreateAsync(CompanyCreateDto input);

        Task UpdateAsync(Guid id, CompanyUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CurrencyLookupDto>> GetCurrencyLookupAsync();

        Task<ListResultDto<WarehouseLookupDto>> GetWarehouseLookupAsync();
    }
}
