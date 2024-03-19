using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.ServiceOrderDetails
{
    public interface IServiceOrderDetailAppService : IApplicationService
    {
        Task<ServiceOrderDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<ServiceOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<ServiceOrderDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<ServiceOrderDetailReadDto>> GetListByServiceOrderAsync(Guid serviceOrderId);

        Task<ServiceOrderDetailReadDto> CreateAsync(ServiceOrderDetailCreateDto input);

        Task UpdateAsync(Guid id, ServiceOrderDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<ServiceOrderLookupDto>> GetServiceOrderLookupAsync();

        Task<ListResultDto<ServiceLookupDto>> GetServiceLookupAsync();
    }
}
