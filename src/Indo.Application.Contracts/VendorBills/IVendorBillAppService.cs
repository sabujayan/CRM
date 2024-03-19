using Indo.VendorDebitNotes;
using Indo.VendorPayments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.VendorBills
{
    public interface IVendorBillAppService : IApplicationService
    {
        Task<VendorBillReadDto> GetAsync(Guid id);

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

        Task<VendorBillCountDto> GetCountOrderAsync();

        Task<List<VendorBillReadDto>> GetListAsync();

        Task<List<VendorBillReadDto>> GetListWithTotalAsync();

        Task<List<VendorBillReadDto>> GetListWithTotalByVendorAsync(Guid customerId);

        Task<List<VendorBillReadDto>> GetListWithTotalByPurchaseOrderAsync(Guid purchaseOrderId);

        Task<VendorBillReadDto> CreateAsync(VendorBillCreateDto input);

        Task UpdateAsync(Guid id, VendorBillUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<VendorLookupDto>> GetVendorLookupAsync();

        Task<VendorBillReadDto> ConfirmAsync(Guid vendorBillId);

        Task<VendorBillReadDto> CancelAsync(Guid vendorBillId);

        Task<VendorDebitNoteReadDto> GenerateDebitNoteAsync(Guid vendorBillId);

        Task<VendorPaymentReadDto> GeneratePaymentAsync(Guid vendorBillId);
    }
}
