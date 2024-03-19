using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.PurchaseReceipts
{
    public interface IPurchaseReceiptAppService : IApplicationService
    {
        Task<PurchaseReceiptReadDto> GetAsync(Guid id);

        Task<List<PurchaseReceiptReadDto>> GetListAsync();

        Task<List<PurchaseReceiptReadDto>> GetListByPurchaseOrderAsync(Guid purchaseOrderId);

        Task<PurchaseReceiptReadDto> CreateAsync(PurchaseReceiptCreateDto input);

        Task UpdateAsync(Guid id, PurchaseReceiptUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<PurchaseOrderLookupDto>> GetPurchaseOrderLookupAsync();

        Task ConfirmAsync(Guid purchaseReceiptId);

        Task ReturnAsync(Guid purchaseReceiptId);
    }
}
