using Indo.CustomerPayments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.CustomerCreditNotes
{
    public interface ICustomerCreditNoteAppService : IApplicationService
    {
        Task<CustomerCreditNoteReadDto> GetAsync(Guid id);

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

        Task<CreditNoteCountDto> GetCountOrderAsync();

        Task<List<CustomerCreditNoteReadDto>> GetListAsync();

        Task<List<CustomerCreditNoteReadDto>> GetListWithTotalAsync();

        Task<List<CustomerCreditNoteReadDto>> GetListWithTotalByCustomerAsync(Guid customerId);

        Task<List<CustomerCreditNoteReadDto>> GetListWithTotalByInvoiceAsync(Guid customerInvoiceId);

        Task<CustomerCreditNoteReadDto> CreateAsync(CustomerCreditNoteCreateDto input);

        Task UpdateAsync(Guid id, CustomerCreditNoteUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();

        Task<ListResultDto<CustomerInvoiceLookupDto>> GetCustomerInvoiceLookupAsync();

        Task<CustomerCreditNoteReadDto> ConfirmAsync(Guid customerCreditNoteId);

        Task<CustomerCreditNoteReadDto> CancelAsync(Guid customerCreditNoteId);

        Task<CustomerPaymentReadDto> GeneratePaymentAsync(Guid customerCreditNoteId);
    }
}
