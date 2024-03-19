using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.GoodsReceiptDetails
{
    public interface IGoodsReceiptDetailAppService : IApplicationService
    {
        Task<GoodsReceiptDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<GoodsReceiptDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<GoodsReceiptDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<GoodsReceiptDetailReadDto>> GetListByGoodsReceiptAsync(Guid goodsReceiptId);

        Task<GoodsReceiptDetailReadDto> CreateAsync(GoodsReceiptDetailCreateDto input);

        Task UpdateAsync(Guid id, GoodsReceiptDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<GoodsReceiptLookupDto>> GetGoodsReceiptLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductByGoodsReceiptLookupAsync(Guid id);
    }
}
