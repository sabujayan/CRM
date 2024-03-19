using Indo.SalesOrders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.SalesQuotations
{
    public interface ISalesQuotationAppService : IApplicationService
    {
        Task<SalesQuotationReadDto> GetAsync(Guid id);

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

        Task<List<SalesQuotationReadDto>> GetListAsync();

        Task<List<SalesQuotationReadDto>> GetListWithTotalAsync();

        Task<List<SalesQuotationReadDto>> GetListWithTotalByCustomerAsync(Guid customerId);

        Task<SalesQuotationReadDto> CreateAsync(SalesQuotationCreateDto input);

        Task UpdateAsync(Guid id, SalesQuotationUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();
        Task<ListResultDto<SalesExecutiveLookupDto>> GetSalesExecutiveLookupAsync();

        Task<SalesQuotationReadDto> ConfirmAsync(Guid salesQuotationId);

        Task<SalesQuotationReadDto> CancelAsync(Guid salesQuotationId);

        Task<SalesOrderReadDto> ConvertToOrderAsync(Guid salesQuotationId);
    }
}
