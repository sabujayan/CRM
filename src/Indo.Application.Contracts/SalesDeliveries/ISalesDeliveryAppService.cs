using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.SalesDeliveries
{
    public interface ISalesDeliveryAppService : IApplicationService
    {
        Task<SalesDeliveryReadDto> GetAsync(Guid id);

        Task<List<SalesDeliveryReadDto>> GetListAsync();

        Task<List<SalesDeliveryReadDto>> GetListBySalesOrderAsync(Guid salesOrderId);

        Task<SalesDeliveryReadDto> CreateAsync(SalesDeliveryCreateDto input);

        Task UpdateAsync(Guid id, SalesDeliveryUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<SalesOrderLookupDto>> GetSalesOrderLookupAsync();
        Task ConfirmAsync(Guid salesDeliveryId);

        Task ReturnAsync(Guid salesDeliveryId);
    }
}
