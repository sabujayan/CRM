using Indo.DeliveryOrders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.TransferOrders
{
    public interface ITransferOrderAppService : IApplicationService
    {
        Task<TransferOrderReadDto> GetAsync(Guid id);

        Task<List<TransferOrderReadDto>> GetListAsync();

        Task<TransferOrderReadDto> CreateAsync(TransferOrderCreateDto input);

        Task UpdateAsync(Guid id, TransferOrderUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<WarehouseLookupDto>> GetWarehouseLookupAsync();

        Task<TransferOrderReadDto> ConfirmAsync(Guid transferOrderId);

        Task<TransferOrderReadDto> ReturnAsync(Guid transferOrderId);
    }
}
