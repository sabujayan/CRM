using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.StockAdjustmentDetails
{
    public interface IStockAdjustmentDetailAppService : IApplicationService
    {
        Task<StockAdjustmentDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<StockAdjustmentDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<StockAdjustmentDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<StockAdjustmentDetailReadDto>> GetListByStockAdjustmentAsync(Guid stockAdjustmentId);

        Task<StockAdjustmentDetailReadDto> CreateAsync(StockAdjustmentDetailCreateDto input);

        Task UpdateAsync(Guid id, StockAdjustmentDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<StockAdjustmentLookupDto>> GetStockAdjustmentLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync();
    }
}
