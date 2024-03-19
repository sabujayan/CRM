using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.PurchaseOrderDetails
{
    public interface IPurchaseOrderDetailAppService : IApplicationService
    {
        Task<PurchaseOrderDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<PurchaseOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        
        Task<List<PurchaseOrderDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<PurchaseOrderDetailReadDto>> GetListByPurchaseOrderAsync(Guid purchaseOrderId);

        Task<PurchaseOrderDetailReadDto> CreateAsync(PurchaseOrderDetailCreateDto input);

        Task UpdateAsync(Guid id, PurchaseOrderDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<PurchaseOrderLookupDto>> GetPurchaseOrderLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync();
    }
}
