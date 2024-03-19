using Indo.CustomerInvoices;
using Indo.SalesDeliveries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.SalesOrders
{
    public interface ISalesOrderAppService : IApplicationService
    {
        Task<SalesOrderReadDto> GetAsync(Guid id);

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

        Task<float> GetTotalQtyAsync(Guid id);

        Task<float> GetTotalCOGSAsync();

        Task<float> GetTotalSalesAsync();

        Task<float> GetTotalQtyByYearMonthAsync(int year, int month);


        Task<OrderCountDto> GetCountOrderAsync();

        Task<List<SalesOrderReadDto>> GetListAsync();

        Task<List<SalesOrderReadDto>> GetListWithTotalAsync();

        Task<List<SalesOrderReadDto>> GetListWithTotalByCustomerAsync(Guid customerId);

        Task<List<SalesOrderReadDto>> GetListWithTotalByQuotationAsync(Guid salesQuotationId);

        Task<SalesOrderReadDto> CreateAsync(SalesOrderCreateDto input);

        Task UpdateAsync(Guid id, SalesOrderUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();
        Task<ListResultDto<SalesExecutiveLookupDto>> GetSalesExecutiveLookupAsync();

        Task<SalesOrderReadDto> ConfirmAsync(Guid salesOrderId);

        Task<SalesOrderReadDto> CancelAsync(Guid salesOrderId);

        Task<SalesDeliveryReadDto> GenerateConfirmDeliveryAsync(Guid salesOrderId);

        Task<SalesDeliveryReadDto> GenerateDraftDeliveryAsync(Guid salesOrderId);

        Task<CustomerInvoiceReadDto> GenerateInvoiceAsync(Guid salesOrderId);
    }
}
