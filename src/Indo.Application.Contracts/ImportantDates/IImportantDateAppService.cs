using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.ImportantDates
{
    public interface IImportantDateAppService : IApplicationService
    {
        Task<ImportantDateReadDto> GetAsync(Guid id);

        Task<List<ImportantDateReadDto>> GetListAsync();

        Task<List<ImportantDateReadDto>> GetListByCustomerAsync(Guid customerId);

        Task<ImportantDateReadDto> CreateAsync(ImportantDateCreateDto input);

        Task UpdateAsync(Guid id, ImportantDateUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();
    }
}
