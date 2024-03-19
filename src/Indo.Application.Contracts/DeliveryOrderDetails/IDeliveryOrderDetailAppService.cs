using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.DeliveryOrderDetails
{
    public interface IDeliveryOrderDetailAppService : IApplicationService
    {
        Task<DeliveryOrderDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<DeliveryOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<DeliveryOrderDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<DeliveryOrderDetailReadDto>> GetListByDeliveryOrderAsync(Guid deliveryOrderId);

        Task<DeliveryOrderDetailReadDto> CreateAsync(DeliveryOrderDetailCreateDto input);

        Task UpdateAsync(Guid id, DeliveryOrderDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<DeliveryOrderLookupDto>> GetDeliveryOrderLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductByDeliveryOrderLookupAsync(Guid id);
    }
}
