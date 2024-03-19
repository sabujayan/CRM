using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.SalesDeliveryDetails
{
    public interface ISalesDeliveryDetailAppService : IApplicationService
    {
        Task<SalesDeliveryDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<SalesDeliveryDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<SalesDeliveryDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<SalesDeliveryDetailReadDto>> GetListBySalesDeliveryAsync(Guid salesDeliveryId);

        Task<SalesDeliveryDetailReadDto> CreateAsync(SalesDeliveryDetailCreateDto input);

        Task UpdateAsync(Guid id, SalesDeliveryDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<SalesDeliveryLookupDto>> GetSalesDeliveryLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductBySalesDeliveryLookupAsync(Guid id);
    }
}
