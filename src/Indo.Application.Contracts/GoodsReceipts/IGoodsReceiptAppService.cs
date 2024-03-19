using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.GoodsReceipts
{
    public interface IGoodsReceiptAppService : IApplicationService
    {
        Task<GoodsReceiptReadDto> GetAsync(Guid id);

        Task<List<GoodsReceiptReadDto>> GetListAsync();

        Task<List<GoodsReceiptReadDto>> GetListByTransferOrderAsync(Guid transferOrderId);

        Task<GoodsReceiptReadDto> CreateAsync(GoodsReceiptCreateDto input);

        Task UpdateAsync(Guid id, GoodsReceiptUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<WarehouseLookupDto>> GetWarehouseLookupAsync();

        Task<ListResultDto<DeliveryOrderLookupDto>> GetDeliveryOrderLookupAsync();
    }
}
