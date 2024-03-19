using Indo.CustomerCreditNotes;
using Indo.CustomerPayments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.CustomerInvoices
{
    public interface ICustomerInvoiceAppService : IApplicationService
    {
        Task<CustomerInvoiceReadDto> GetAsync(Guid id);

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

        Task<CustomerInvoiceCountDto> GetCountOrderAsync();

        Task<List<CustomerInvoiceReadDto>> GetListAsync();

        Task<List<CustomerInvoiceReadDto>> GetListWithTotalAsync();

        Task<List<CustomerInvoiceReadDto>> GetListWithTotalByCustomerAsync(Guid customerId);

        Task<List<CustomerInvoiceReadDto>> GetListWithTotalByServiceOrderAsync(Guid serviceOrderId);

        Task<List<CustomerInvoiceReadDto>> GetListWithTotalByProjectOrderAsync(Guid projectOrderId);

        Task<List<CustomerInvoiceReadDto>> GetListWithTotalBySalesOrderAsync(Guid salesOrderId);

        Task<CustomerInvoiceReadDto> CreateAsync(CustomerInvoiceCreateDto input);

        Task UpdateAsync(Guid id, CustomerInvoiceUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();

        Task<CustomerInvoiceReadDto> ConfirmAsync(Guid customerInvoiceId);

        Task<CustomerInvoiceReadDto> CancelAsync(Guid customerInvoiceId);

        Task<CustomerCreditNoteReadDto> GenerateCreditNoteAsync(Guid customerInvoiceId);

        Task<CustomerPaymentReadDto> GeneratePaymentAsync(Guid customerInvoiceId);
    }
}
