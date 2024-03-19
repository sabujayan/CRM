using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Currencies;
using Indo.Vendors;
using Indo.SalesQuotations;
using Indo.VendorBillDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.VendorPayments;
using Indo.VendorDebitNotes;

namespace Indo.VendorBills
{
    public class VendorBillAppService : IndoAppService, IVendorBillAppService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IVendorBillRepository _vendorBillRepository;
        private readonly VendorBillManager _vendorBillManager;
        private readonly IVendorBillDetailRepository _vendorBillDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        private readonly ISalesQuotationRepository _salesQuotationRepository;
        public VendorBillAppService(
            IVendorBillRepository vendorBillRepository,
            VendorBillManager vendorBillManager,
            IVendorRepository vendorRepository,
            IVendorBillDetailRepository vendorBillDetailRepository,
            ICompanyAppService companyAppService,
            ISalesQuotationRepository salesQuotationRepository
            )
        {
            _vendorBillRepository = vendorBillRepository;
            _vendorBillManager = vendorBillManager;
            _vendorRepository = vendorRepository;
            _vendorBillDetailRepository = vendorBillDetailRepository;
            _companyAppService = companyAppService;
            _salesQuotationRepository = salesQuotationRepository;
        }

        public async Task<VendorBillCountDto> GetCountOrderAsync()
        {
            await Task.Yield();
            var result = new VendorBillCountDto();
            result.CountDraft = _vendorBillRepository.Where(x => x.Status == VendorBillStatus.Draft).Count();
            result.CountConfirm = _vendorBillRepository.Where(x => x.Status == VendorBillStatus.Confirm).Count();
            result.CountCancelled = _vendorBillRepository.Where(x => x.Status == VendorBillStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummarySubTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _vendorBillDetailRepository
                .Where(x => x.VendorBillId.Equals(id))
                .Sum(x => x.SubTotal);
            return result;
        }
        public async Task<string> GetSummarySubTotalInStringAsync(Guid id)
        {
            var result = await GetSummarySubTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryDiscAmtAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _vendorBillDetailRepository
                .Where(x => x.VendorBillId.Equals(id))
                .Sum(x => x.DiscAmt);
            return result;
        }
        public async Task<string> GetSummaryDiscAmtInStringAsync(Guid id)
        {
            var result = await GetSummaryDiscAmtAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryBeforeTaxAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _vendorBillDetailRepository
                .Where(x => x.VendorBillId.Equals(id))
                .Sum(x => x.BeforeTax);
            return result;
        }
        public async Task<string> GetSummaryBeforeTaxInStringAsync(Guid id)
        {
            var result = await GetSummaryBeforeTaxAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryTaxAmountAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _vendorBillDetailRepository
                .Where(x => x.VendorBillId.Equals(id))
                .Sum(x => x.TaxAmount);
            return result;
        }
        public async Task<string> GetSummaryTaxAmountInStringAsync(Guid id)
        {
            var result = await GetSummaryTaxAmountAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _vendorBillDetailRepository
                .Where(x => x.VendorBillId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<VendorBillReadDto> GetAsync(Guid id)
        {
            var queryable = await _vendorBillRepository.GetQueryableAsync();
            var query = from vendorBill in queryable
                        join vendor in _vendorRepository on vendorBill.VendorId equals vendor.Id
                        where vendorBill.Id == id
                        select new { vendorBill, vendor };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(VendorBill), id);
            }
            var dto = ObjectMapper.Map<VendorBill, VendorBillReadDto>(queryResult.vendorBill);
            dto.VendorName = queryResult.vendor.Name;
            dto.StatusString = L[$"Enum:VendorBillStatus:{(int)queryResult.vendorBill.Status}"];
            dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)queryResult.vendorBill.SourceDocumentModule}"];
            if (queryResult.vendorBill.SourceDocumentModule == NumberSequences.NumberSequenceModules.PurchaseOrder)
            {
                dto.SourceDocumentPath = "PurchaseOrder";
            }

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<VendorBillReadDto>> GetListAsync()
        {
            var queryable = await _vendorBillRepository.GetQueryableAsync();
            var query = from vendorBill in queryable
                        join vendor in _vendorRepository on vendorBill.VendorId equals vendor.Id
                        select new { vendorBill, vendor };
                   
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorBill, VendorBillReadDto>(x.vendorBill);
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorBillStatus:{(int)x.vendorBill.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.vendorBill.SourceDocumentModule}"];
                if (x.vendorBill.SourceDocumentModule == NumberSequences.NumberSequenceModules.PurchaseOrder)
                {
                    dto.SourceDocumentPath = "PurchaseOrder";
                }
                return dto;
            })
                .OrderByDescending(x => x.BillDate)
                .ToList();

            return dtos;
        }
        public async Task<List<VendorBillReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _vendorBillRepository.GetQueryableAsync();
            var query = from vendorBill in queryable
                        join vendor in _vendorRepository on vendorBill.VendorId equals vendor.Id
                        select new { vendorBill, vendor };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorBill, VendorBillReadDto>(x.vendorBill);
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorBillStatus:{(int)x.vendorBill.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.vendorBill.SourceDocumentModule}"];
                if (x.vendorBill.SourceDocumentModule == NumberSequences.NumberSequenceModules.PurchaseOrder)
                {
                    dto.SourceDocumentPath = "PurchaseOrder";
                }
                dto.Total = _vendorBillDetailRepository.Where(y => y.VendorBillId.Equals(x.vendorBill.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.BillDate)
                .ToList();

            return dtos;
        }
        public async Task<List<VendorBillReadDto>> GetListWithTotalByVendorAsync(Guid vendorId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _vendorBillRepository.GetQueryableAsync();
            var query = from vendorBill in queryable
                        join vendor in _vendorRepository on vendorBill.VendorId equals vendor.Id
                        where vendorBill.VendorId == vendorId
                        select new { vendorBill, vendor };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorBill, VendorBillReadDto>(x.vendorBill);
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorBillStatus:{(int)x.vendorBill.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.vendorBill.SourceDocumentModule}"];
                if (x.vendorBill.SourceDocumentModule == NumberSequences.NumberSequenceModules.PurchaseOrder)
                {
                    dto.SourceDocumentPath = "PurchaseOrder";
                }
                dto.Total = _vendorBillDetailRepository.Where(y => y.VendorBillId.Equals(x.vendorBill.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.BillDate)
                .ToList();

            return dtos;
        }
        public async Task<List<VendorBillReadDto>> GetListWithTotalByPurchaseOrderAsync(Guid purchaseOrderId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _vendorBillRepository.GetQueryableAsync();
            var query = from vendorBill in queryable
                        join vendor in _vendorRepository on vendorBill.VendorId equals vendor.Id
                        where vendorBill.SourceDocumentId == purchaseOrderId && vendorBill.SourceDocumentModule == NumberSequences.NumberSequenceModules.PurchaseOrder
                        select new { vendorBill, vendor };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorBill, VendorBillReadDto>(x.vendorBill);
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorBillStatus:{(int)x.vendorBill.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.vendorBill.SourceDocumentModule}"];
                if (x.vendorBill.SourceDocumentModule == NumberSequences.NumberSequenceModules.PurchaseOrder)
                {
                    dto.SourceDocumentPath = "PurchaseOrder";
                }
                dto.Total = _vendorBillDetailRepository.Where(y => y.VendorBillId.Equals(x.vendorBill.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.BillDate)
                .ToList();

            return dtos;
        }
        public async Task<ListResultDto<VendorLookupDto>> GetVendorLookupAsync()
        {
            var list = await _vendorRepository.GetListAsync();
            return new ListResultDto<VendorLookupDto>(
                ObjectMapper.Map<List<Vendor>, List<VendorLookupDto>>(list)
            );
        }
        public async Task<VendorBillReadDto> CreateAsync(VendorBillCreateDto input)
        {
            var obj = await _vendorBillManager.CreateAsync(
                input.Number,
                input.VendorId,
                input.BillDate,
                input.BillDueDate
            );

            obj.Description = input.Description;
            obj.TermCondition = input.TermCondition;
            obj.PaymentNote = input.PaymentNote;

            await _vendorBillRepository.InsertAsync(obj);

            return ObjectMapper.Map<VendorBill, VendorBillReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, VendorBillUpdateDto input)
        {
            var obj = await _vendorBillRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _vendorBillManager.ChangeNameAsync(obj, input.Number);
            }

            obj.VendorId = input.VendorId;
            obj.Description = input.Description;
            obj.TermCondition = input.TermCondition;
            obj.PaymentNote = input.PaymentNote;
            obj.BillDate = input.BillDate;
            obj.BillDueDate = input.BillDueDate;

            await _vendorBillRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _vendorBillRepository.DeleteAsync(id);
        }

        public async Task<VendorBillReadDto> ConfirmAsync(Guid vendorBillId)
        {
            var obj = await _vendorBillManager.ConfirmAsync(vendorBillId);
            return ObjectMapper.Map<VendorBill, VendorBillReadDto>(obj);
        }

        public async Task<VendorBillReadDto> CancelAsync(Guid vendorBillId)
        {
            var obj = await _vendorBillManager.CancelAsync(vendorBillId);
            return ObjectMapper.Map<VendorBill, VendorBillReadDto>(obj);
        }
        public async Task<VendorDebitNoteReadDto> GenerateDebitNoteAsync(Guid vendorBillId)
        {
            var obj = await _vendorBillManager.GenerateDebitNote(vendorBillId);
            return ObjectMapper.Map<VendorDebitNote, VendorDebitNoteReadDto>(obj);
        }
        public async Task<VendorPaymentReadDto> GeneratePaymentAsync(Guid vendorBillId)
        {
            var obj = await _vendorBillManager.GeneratePayment(vendorBillId);
            return ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(obj);
        }
    }
}
