using Indo.VendorPayments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.VendorDebitNotes
{
    public interface IVendorDebitNoteAppService : IApplicationService
    {
        Task<VendorDebitNoteReadDto> GetAsync(Guid id);

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

        Task<DebitNoteCountDto> GetCountOrderAsync();

        Task<List<VendorDebitNoteReadDto>> GetListAsync();

        Task<List<VendorDebitNoteReadDto>> GetListWithTotalAsync();

        Task<List<VendorDebitNoteReadDto>> GetListWithTotalByVendorAsync(Guid vendorId);

        Task<List<VendorDebitNoteReadDto>> GetListWithTotalByBillAsync(Guid vendorBillId);

        Task<VendorDebitNoteReadDto> CreateAsync(VendorDebitNoteCreateDto input);

        Task UpdateAsync(Guid id, VendorDebitNoteUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<VendorLookupDto>> GetVendorLookupAsync();

        Task<ListResultDto<VendorBillLookupDto>> GetVendorBillLookupAsync();

        Task<VendorDebitNoteReadDto> ConfirmAsync(Guid vendorDebitNoteId);

        Task<VendorDebitNoteReadDto> CancelAsync(Guid vendorDebitNoteId);

        Task<VendorPaymentReadDto> GeneratePaymentAsync(Guid vendorBillId);
    }
}
