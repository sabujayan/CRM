using Indo.VendorBills;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.VendorPayments
{
    public interface IVendorPaymentAppService : IApplicationService
    {
        Task<VendorPaymentReadDto> GetAsync(Guid id);

        Task<List<VendorPaymentReadDto>> GetListAsync();

        Task<List<VendorPaymentReadDto>> GetListByVendorAsync(Guid vendorId);

        Task<List<VendorPaymentReadDto>> GetListByBillAsync(Guid vendorBillId);

        Task<List<VendorPaymentReadDto>> GetListByDebitNoteAsync(Guid vendorDebitNoteId);

        Task<VendorPaymentReadDto> CreateAsync(VendorPaymentCreateDto input);

        Task UpdateAsync(Guid id, VendorPaymentUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CashAndBankLookupDto>> GetCashAndBankLookupAsync();

        Task<ListResultDto<VendorLookupDto>> GetVendorLookupAsync();

        Task<VendorPaymentReadDto> ConfirmAsync(Guid vendorPaymentId);

        Task<VendorPaymentReadDto> CancelAsync(Guid vendorPaymentId);
    }
}
