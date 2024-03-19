using Indo.PurchaseReceipts;
using Indo.VendorBills;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.PurchaseOrders
{
    public interface IPurchaseOrderAppService : IApplicationService
    {
        Task<PurchaseOrderReadDto> GetAsync(Guid id);

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

        Task<float> GetTotalQtyByYearMonthAsync(int year, int month);

        Task<OrderCountDto> GetCountOrderAsync();

        Task<List<PurchaseOrderReadDto>> GetListAsync();

        Task<List<PurchaseOrderReadDto>> GetListWithTotalAsync();

        Task<PurchaseOrderReadDto> CreateAsync(PurchaseOrderCreateDto input);

        Task UpdateAsync(Guid id, PurchaseOrderUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<VendorLookupDto>> GetVendorLookupAsync();
        Task<ListResultDto<BuyerLookupDto>> GetBuyerLookupAsync();

        Task<PurchaseOrderReadDto> ConfirmAsync(Guid purchaseOrderId);

        Task<PurchaseOrderReadDto> CancelAsync(Guid purchaseOrderId);

        Task<PurchaseReceiptReadDto> GenerateConfirmReceiptAsync(Guid purchaseOrderId);

        Task<PurchaseReceiptReadDto> GenerateDraftReceiptAsync(Guid purchaseOrderId);

        Task<VendorBillReadDto> GenerateBillAsync(Guid purchaseOrderId);
    }
}
