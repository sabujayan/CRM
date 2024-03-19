using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Currencies;
using Indo.Vendors;
using Indo.VendorBills;
using Indo.VendorDebitNoteDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.VendorPayments;

namespace Indo.VendorDebitNotes
{
    public class VendorDebitNoteAppService : IndoAppService, IVendorDebitNoteAppService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IVendorDebitNoteRepository _vendorDebitNoteRepository;
        private readonly VendorDebitNoteManager _vendorDebitNoteManager;
        private readonly IVendorDebitNoteDetailRepository _vendorDebitNoteDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        private readonly IVendorBillRepository _vendorBillRepository;
        public VendorDebitNoteAppService(
            IVendorDebitNoteRepository vendorDebitNoteRepository,
            VendorDebitNoteManager vendorDebitNoteManager,
            IVendorRepository vendorRepository,
            IVendorDebitNoteDetailRepository vendorDebitNoteDetailRepository,
            ICompanyAppService companyAppService,
            IVendorBillRepository vendorBillRepository
            )
        {
            _vendorDebitNoteRepository = vendorDebitNoteRepository;
            _vendorDebitNoteManager = vendorDebitNoteManager;
            _vendorRepository = vendorRepository;
            _vendorDebitNoteDetailRepository = vendorDebitNoteDetailRepository;
            _companyAppService = companyAppService;
            _vendorBillRepository = vendorBillRepository;
        }

        public async Task<DebitNoteCountDto> GetCountOrderAsync()
        {
            await Task.Yield();
            var result = new DebitNoteCountDto();
            result.CountDraft = _vendorDebitNoteRepository.Where(x => x.Status == VendorDebitNoteStatus.Draft).Count();
            result.CountConfirm = _vendorDebitNoteRepository.Where(x => x.Status == VendorDebitNoteStatus.Confirm).Count();
            result.CountCancelled = _vendorDebitNoteRepository.Where(x => x.Status == VendorDebitNoteStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummarySubTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _vendorDebitNoteDetailRepository
                .Where(x => x.VendorDebitNoteId.Equals(id))
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
            result = _vendorDebitNoteDetailRepository
                .Where(x => x.VendorDebitNoteId.Equals(id))
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
            result = _vendorDebitNoteDetailRepository
                .Where(x => x.VendorDebitNoteId.Equals(id))
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
            result = _vendorDebitNoteDetailRepository
                .Where(x => x.VendorDebitNoteId.Equals(id))
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
            result = _vendorDebitNoteDetailRepository
                .Where(x => x.VendorDebitNoteId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<VendorDebitNoteReadDto> GetAsync(Guid id)
        {
            var queryable = await _vendorDebitNoteRepository.GetQueryableAsync();
            var query = from vendorDebitNote in queryable
                        join vendor in _vendorRepository on vendorDebitNote.VendorId equals vendor.Id
                        where vendorDebitNote.Id == id
                        select new { vendorDebitNote, vendor };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(VendorDebitNote), id);
            }
            var dto = ObjectMapper.Map<VendorDebitNote, VendorDebitNoteReadDto>(queryResult.vendorDebitNote);
            dto.VendorName = queryResult.vendor.Name;
            dto.StatusString = L[$"Enum:VendorDebitNoteStatus:{(int)queryResult.vendorDebitNote.Status}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<VendorDebitNoteReadDto>> GetListAsync()
        {
            var queryable = await _vendorDebitNoteRepository.GetQueryableAsync();
            var query = from vendorDebitNote in queryable
                        join vendor in _vendorRepository on vendorDebitNote.VendorId equals vendor.Id
                        join vendorBill in _vendorBillRepository on vendorDebitNote.VendorBillId equals vendorBill.Id
                        select new { vendorDebitNote, vendor, vendorBill };
                   
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorDebitNote, VendorDebitNoteReadDto>(x.vendorDebitNote);
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorDebitNoteStatus:{(int)x.vendorDebitNote.Status}"];
                dto.VendorBillNumber = x.vendorBill.Number;
                return dto;
            })
                .OrderByDescending(x => x.DebitNoteDate)
                .ToList();

            return dtos;
        }
        public async Task<List<VendorDebitNoteReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _vendorDebitNoteRepository.GetQueryableAsync();
            var query = from vendorDebitNote in queryable
                        join vendor in _vendorRepository on vendorDebitNote.VendorId equals vendor.Id
                        join vendorBill in _vendorBillRepository on vendorDebitNote.VendorBillId equals vendorBill.Id
                        select new { vendorDebitNote, vendor, vendorBill };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorDebitNote, VendorDebitNoteReadDto>(x.vendorDebitNote);
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorDebitNoteStatus:{(int)x.vendorDebitNote.Status}"];
                dto.VendorBillNumber = x.vendorBill.Number;
                dto.Total = _vendorDebitNoteDetailRepository.Where(y => y.VendorDebitNoteId.Equals(x.vendorDebitNote.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.DebitNoteDate)
                .ToList();

            return dtos;
        }
        public async Task<List<VendorDebitNoteReadDto>> GetListWithTotalByVendorAsync(Guid vendorId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _vendorDebitNoteRepository.GetQueryableAsync();
            var query = from vendorDebitNote in queryable
                        join vendor in _vendorRepository on vendorDebitNote.VendorId equals vendor.Id
                        join vendorBill in _vendorBillRepository on vendorDebitNote.VendorBillId equals vendorBill.Id
                        where vendorDebitNote.VendorId == vendorId
                        select new { vendorDebitNote, vendor, vendorBill };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorDebitNote, VendorDebitNoteReadDto>(x.vendorDebitNote);
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorDebitNoteStatus:{(int)x.vendorDebitNote.Status}"];
                dto.VendorBillNumber = x.vendorBill.Number;
                dto.Total = _vendorDebitNoteDetailRepository.Where(y => y.VendorDebitNoteId.Equals(x.vendorDebitNote.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.DebitNoteDate)
                .ToList();

            return dtos;
        }
        public async Task<List<VendorDebitNoteReadDto>> GetListWithTotalByBillAsync(Guid vendorBillId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _vendorDebitNoteRepository.GetQueryableAsync();
            var query = from vendorDebitNote in queryable
                        join vendor in _vendorRepository on vendorDebitNote.VendorId equals vendor.Id
                        join vendorBill in _vendorBillRepository on vendorDebitNote.VendorBillId equals vendorBill.Id
                        where vendorDebitNote.VendorBillId == vendorBillId
                        select new { vendorDebitNote, vendor, vendorBill };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorDebitNote, VendorDebitNoteReadDto>(x.vendorDebitNote);
                dto.VendorName = x.vendor.Name;
                dto.VendorBillNumber = x.vendorBill.Number;
                dto.StatusString = L[$"Enum:VendorDebitNoteStatus:{(int)x.vendorDebitNote.Status}"];
                dto.Total = _vendorDebitNoteDetailRepository.Where(y => y.VendorDebitNoteId.Equals(x.vendorDebitNote.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.DebitNoteDate)
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
        public async Task<ListResultDto<VendorBillLookupDto>> GetVendorBillLookupAsync()
        {
            var list = await _vendorBillRepository.GetListAsync();
            return new ListResultDto<VendorBillLookupDto>(
                ObjectMapper.Map<List<VendorBill>, List<VendorBillLookupDto>>(list)
            );
        }
        public async Task<VendorDebitNoteReadDto> CreateAsync(VendorDebitNoteCreateDto input)
        {
            var obj = await _vendorDebitNoteManager.CreateAsync(
                input.Number,
                input.VendorId,
                input.DebitNoteDate,
                input.VendorBillId
            );

            obj.Description = input.Description;
            obj.PaymentNote = input.PaymentNote;

            await _vendorDebitNoteRepository.InsertAsync(obj);

            return ObjectMapper.Map<VendorDebitNote, VendorDebitNoteReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, VendorDebitNoteUpdateDto input)
        {
            var obj = await _vendorDebitNoteRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _vendorDebitNoteManager.ChangeNameAsync(obj, input.Number);
            }

            obj.VendorId = input.VendorId;
            obj.Description = input.Description;
            obj.PaymentNote = input.PaymentNote;
            obj.DebitNoteDate = input.DebitNoteDate;
            obj.VendorBillId = input.VendorBillId;

            await _vendorDebitNoteRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _vendorDebitNoteRepository.DeleteAsync(id);
        }

        public async Task<VendorDebitNoteReadDto> ConfirmAsync(Guid vendorDebitNoteId)
        {
            var obj = await _vendorDebitNoteManager.ConfirmAsync(vendorDebitNoteId);
            return ObjectMapper.Map<VendorDebitNote, VendorDebitNoteReadDto>(obj);
        }

        public async Task<VendorDebitNoteReadDto> CancelAsync(Guid vendorDebitNoteId)
        {
            var obj = await _vendorDebitNoteManager.CancelAsync(vendorDebitNoteId);
            return ObjectMapper.Map<VendorDebitNote, VendorDebitNoteReadDto>(obj);
        }
        public async Task<VendorPaymentReadDto> GeneratePaymentAsync(Guid vendorDebitNoteId)
        {
            var obj = await _vendorDebitNoteManager.GeneratePayment(vendorDebitNoteId);
            return ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(obj);
        }
    }
}
