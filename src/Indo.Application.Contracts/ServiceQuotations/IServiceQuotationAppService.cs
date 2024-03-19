using Indo.ServiceOrders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.ServiceQuotations
{
    public interface IServiceQuotationAppService : IApplicationService
    {
        Task<ServiceQuotationReadDto> GetAsync(Guid id);

        Task<float> GetSummarySubTotalAsync(Guid id);

        Task<string> GetSummarySubTotalInStringAsync(Guid id);

        Task<float> GetSummaryDiscAmtAsync(Guid id);

        Task<string> GetSummaryDiscAmtInStringAsync(Guid id);

        Task<float> GetSummaryBeforeTaxAsync(Guid id);

        Task<string> GetSummaryBeforeTaxInStringAsync(Guid id);

        Task<float> GetSummaryTaxAmountAsync(Guid id);

        Task<string> GetSummaryTaxAmountInStringAsync(Guid id);

        Task<float> GetSummaryTotalAsync(Guid id);

        Task<string> GetSummaryTotalInStringAsync(Guid id);

        Task<QuotationCountDto> GetCountQuotationAsync();

        Task<List<ServiceQuotationReadDto>> GetListAsync();

        Task<List<ServiceQuotationReadDto>> GetListWithTotalAsync();

        Task<List<ServiceQuotationReadDto>> GetListWithTotalByCustomerAsync(Guid customerId);

        Task<ServiceQuotationReadDto> CreateAsync(ServiceQuotationCreateDto input);

        Task UpdateAsync(Guid id, ServiceQuotationUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();
        Task<ListResultDto<SalesExecutiveLookupDto>> GetSalesExecutiveLookupAsync();

        Task<ServiceQuotationReadDto> ConfirmAsync(Guid serviceQuotationId);

        Task<ServiceQuotationReadDto> CancelAsync(Guid serviceQuotationId);

        Task<ServiceOrderReadDto> ConvertToOrderAsync(Guid serviceQuotationId);
    }
}
