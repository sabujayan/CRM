using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.DeliveryOrders
{
    public interface IDeliveryOrderAppService : IApplicationService
    {
        Task<DeliveryOrderReadDto> GetAsync(Guid id);

        Task<List<DeliveryOrderReadDto>> GetListAsync();

        Task<List<DeliveryOrderReadDto>> GetListByTransferOrderAsync(Guid transferOrderId);

        Task<DeliveryOrderReadDto> CreateAsync(DeliveryOrderCreateDto input);

        Task UpdateAsync(Guid id, DeliveryOrderUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<WarehouseLookupDto>> GetWarehouseLookupAsync();

        Task<ListResultDto<TransferOrderLookupDto>> GetTransferOrderLookupAsync();
    }
}
