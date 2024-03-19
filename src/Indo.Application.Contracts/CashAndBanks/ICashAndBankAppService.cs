using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.CashAndBanks
{
    public interface ICashAndBankAppService : IApplicationService
    {
        Task<CashAndBankReadDto> GetAsync(Guid id);

        Task<List<CashAndBankReadDto>> GetListAsync();

        Task<CashAndBankReadDto> CreateAsync(CashAndBankCreateDto input);

        Task UpdateAsync(Guid id, CashAndBankUpdateDto input);

        Task DeleteAsync(Guid id);
        Task<List<CashAndBankReportDto>> GetReportAsync();
    }
}
