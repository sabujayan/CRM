using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.StockAdjustments
{
    public interface IStockAdjustmentAppService : IApplicationService
    {
        Task<StockAdjustmentReadDto> GetAsync(Guid id);

        Task<List<StockAdjustmentReadDto>> GetListAsync();

        Task<StockAdjustmentReadDto> CreateAsync(StockAdjustmentCreateDto input);

        Task UpdateAsync(Guid id, StockAdjustmentUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<WarehouseLookupDto>> GetWarehouseLookupAsync();

        Task<StockAdjustmentReadDto> ConfirmAsync(Guid stockAdjustmentId);

        Task<StockAdjustmentReadDto> ReturnAsync(Guid stockAdjustmentId);
    }
}
