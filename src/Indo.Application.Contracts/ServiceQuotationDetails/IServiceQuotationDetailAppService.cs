using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.ServiceQuotationDetails
{
    public interface IServiceQuotationDetailAppService : IApplicationService
    {
        Task<ServiceQuotationDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<ServiceQuotationDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<ServiceQuotationDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<ServiceQuotationDetailReadDto>> GetListByServiceQuotationAsync(Guid serviceQuotationId);

        Task<ServiceQuotationDetailReadDto> CreateAsync(ServiceQuotationDetailCreateDto input);

        Task UpdateAsync(Guid id, ServiceQuotationDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<ServiceQuotationLookupDto>> GetServiceQuotationLookupAsync();

        Task<ListResultDto<ServiceLookupDto>> GetServiceLookupAsync();
    }
}
