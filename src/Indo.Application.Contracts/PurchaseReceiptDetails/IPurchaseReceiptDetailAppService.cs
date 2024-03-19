using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.PurchaseReceiptDetails
{
    public interface IPurchaseReceiptDetailAppService : IApplicationService
    {
        Task<PurchaseReceiptDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<PurchaseReceiptDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<PurchaseReceiptDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<PurchaseReceiptDetailReadDto>> GetListByPurchaseReceiptAsync(Guid purchaseReceiptId);

        Task<PurchaseReceiptDetailReadDto> CreateAsync(PurchaseReceiptDetailCreateDto input);

        Task UpdateAsync(Guid id, PurchaseReceiptDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<PurchaseReceiptLookupDto>> GetPurchaseReceiptLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupByPurchaseReceiptAsync(Guid id);
    }
}
