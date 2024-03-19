using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.SalesQuotationDetails
{
    public interface ISalesQuotationDetailAppService : IApplicationService
    {
        Task<SalesQuotationDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<SalesQuotationDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<SalesQuotationDetailReadDto>> GetListDetailAsync();

        Task<List<SalesQuotationDetailReadDto>> GetListHighPerformerAsync();

        Task<List<SalesQuotationDetailReadDto>> GetListLowPerformerAsync();

        Task<PagedResultDto<SalesQuotationDetailReadDto>> GetListBySalesQuotationAsync(Guid salesOrderId);

        Task<SalesQuotationDetailReadDto> CreateAsync(SalesQuotationDetailCreateDto input);

        Task UpdateAsync(Guid id, SalesQuotationDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<SalesQuotationLookupDto>> GetSalesQuotationLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync();
    }
}
