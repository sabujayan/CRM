using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Stocks
{
    public interface IStockAppService : IApplicationService
    {
        Task<StockReadDto> GetAsync(Guid id);

        Task<List<StockReadDto>> GetListAsync();

        Task<StockReadDto> CreateAsync(StockCreateDto input);

        Task UpdateAsync(Guid id, StockUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<float> GetTotalValuationAsync();

        Task<float> GetSummaryValuationByYearMonthAsync(int year, int month);

        Task<List<MonthlyValuationDto>> GetListMonthlyValuation(int monthsCount);
    }
}
