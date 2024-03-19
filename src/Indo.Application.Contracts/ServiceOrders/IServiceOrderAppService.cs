using Indo.CustomerInvoices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.ServiceOrders
{
    public interface IServiceOrderAppService : IApplicationService
    {
        Task<ServiceOrderReadDto> GetAsync(Guid id);

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

        Task<OrderCountDto> GetCountOrderAsync();

        Task<List<ServiceOrderReadDto>> GetListAsync();

        Task<List<ServiceOrderReadDto>> GetListWithTotalAsync();

        Task<List<ServiceOrderReadDto>> GetListWithTotalByCustomerAsync(Guid customerId);

        Task<List<ServiceOrderReadDto>> GetListWithTotalByQuotationAsync(Guid serviceQuotationId);

        Task<ServiceOrderReadDto> CreateAsync(ServiceOrderCreateDto input);

        Task UpdateAsync(Guid id, ServiceOrderUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();
        Task<ListResultDto<SalesExecutiveLookupDto>> GetSalesExecutiveLookupAsync();

        Task<ServiceOrderReadDto> ConfirmAsync(Guid serviceOrderId);

        Task<ServiceOrderReadDto> CancelAsync(Guid serviceOrderId);

        Task<CustomerInvoiceReadDto> GenerateInvoiceAsync(Guid serviceOrderId);
    }
}
