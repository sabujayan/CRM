using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Vendors;
using Indo.CashAndBanks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.NumberSequences;

namespace Indo.VendorPayments
{
    public class VendorPaymentAppService : IndoAppService, IVendorPaymentAppService
    {
        private readonly IVendorPaymentRepository _vendorPaymentRepository;
        private readonly VendorPaymentManager _vendorPaymentManager;
        private readonly ICashAndBankRepository _cashAndBankRepository;
        private readonly CompanyAppService _companyAppService;
        private readonly IVendorRepository _vendorRepository;
        public VendorPaymentAppService(
            IVendorPaymentRepository vendorPaymentRepository,
            VendorPaymentManager vendorPaymentManager,
            ICashAndBankRepository cashAndBankRepository,
            CompanyAppService companyAppService,
            IVendorRepository vendorRepository
            )
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _vendorPaymentManager = vendorPaymentManager;
            _cashAndBankRepository = cashAndBankRepository;
            _companyAppService = companyAppService;
            _vendorRepository = vendorRepository;
        }
        public async Task<VendorPaymentReadDto> GetAsync(Guid id)
        {
            var queryable = await _vendorPaymentRepository.GetQueryableAsync();
            var query = from vendorPayment in queryable
                        join cashAndBank in _cashAndBankRepository on vendorPayment.CashAndBankId equals cashAndBank.Id
                        join vendor in _vendorRepository on vendorPayment.VendorId equals vendor.Id
                        where vendorPayment.Id == id
                        select new { vendorPayment, cashAndBank, vendor };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(VendorPayment), id);
            }
            var dto = ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(queryResult.vendorPayment);
            dto.CashAndBankName = queryResult.cashAndBank.Name;
            dto.VendorName = queryResult.vendor.Name;
            dto.StatusString = L[$"Enum:VendorPaymentStatus:{(int)queryResult.vendorPayment.Status}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<VendorPaymentReadDto>> GetListAsync()
        {
            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorPaymentRepository.GetQueryableAsync();
            var query = from vendorPayment in queryable
                        join cashAndBank in _cashAndBankRepository on vendorPayment.CashAndBankId equals cashAndBank.Id
                        join vendor in _vendorRepository on vendorPayment.VendorId equals vendor.Id
                        select new { vendorPayment, cashAndBank, vendor };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(x.vendorPayment);
                dto.CashAndBankName = x.cashAndBank.Name;
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorPaymentStatus:{(int)x.vendorPayment.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.vendorPayment.SourceDocumentModule}"];
                dto.Period = x.vendorPayment.PaymentDate.ToString("yyyy-MM");
                dto.CurrencyName = defaultCompany.CurrencyName;
                if (x.vendorPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.Bill)
                {
                    dto.SourceDocumentPath = "VendorBill";
                }
                if (x.vendorPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.DebitNote)
                {
                    dto.SourceDocumentPath = "VendorDebitNote";
                }
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<VendorPaymentReadDto>> GetListByVendorAsync(Guid vendorId)
        {
            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorPaymentRepository.GetQueryableAsync();
            var query = from vendorPayment in queryable
                        join cashAndBank in _cashAndBankRepository on vendorPayment.CashAndBankId equals cashAndBank.Id
                        join vendor in _vendorRepository on vendorPayment.VendorId equals vendor.Id
                        where vendorPayment.VendorId == vendorId
                        select new { vendorPayment, cashAndBank, vendor };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(x.vendorPayment);
                dto.CashAndBankName = x.cashAndBank.Name;
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorPaymentStatus:{(int)x.vendorPayment.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.vendorPayment.SourceDocumentModule}"];
                dto.CurrencyName = defaultCompany.CurrencyName;
                if (x.vendorPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.Bill)
                {
                    dto.SourceDocumentPath = "VendorBill";
                }
                if (x.vendorPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.DebitNote)
                {
                    dto.SourceDocumentPath = "VendorDebitNote";
                }
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<VendorPaymentReadDto>> GetListByBillAsync(Guid vendorBillId)
        {
            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorPaymentRepository.GetQueryableAsync();
            var query = from vendorPayment in queryable
                        join cashAndBank in _cashAndBankRepository on vendorPayment.CashAndBankId equals cashAndBank.Id
                        join vendor in _vendorRepository on vendorPayment.VendorId equals vendor.Id
                        where vendorPayment.SourceDocumentId == vendorBillId && vendorPayment.SourceDocumentModule == NumberSequenceModules.Bill
                        select new { vendorPayment, cashAndBank, vendor };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(x.vendorPayment);
                dto.CashAndBankName = x.cashAndBank.Name;
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorPaymentStatus:{(int)x.vendorPayment.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.vendorPayment.SourceDocumentModule}"];
                dto.CurrencyName = defaultCompany.CurrencyName;
                if (x.vendorPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.Bill)
                {
                    dto.SourceDocumentPath = "VendorBill";
                }
                if (x.vendorPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.DebitNote)
                {
                    dto.SourceDocumentPath = "VendorDebitNote";
                }
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<VendorPaymentReadDto>> GetListByDebitNoteAsync(Guid vendorDebitNoteId)
        {
            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorPaymentRepository.GetQueryableAsync();
            var query = from vendorPayment in queryable
                        join cashAndBank in _cashAndBankRepository on vendorPayment.CashAndBankId equals cashAndBank.Id
                        join vendor in _vendorRepository on vendorPayment.VendorId equals vendor.Id
                        where vendorPayment.SourceDocumentId == vendorDebitNoteId && vendorPayment.SourceDocumentModule == NumberSequenceModules.DebitNote
                        select new { vendorPayment, cashAndBank, vendor };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(x.vendorPayment);
                dto.CashAndBankName = x.cashAndBank.Name;
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:VendorPaymentStatus:{(int)x.vendorPayment.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.vendorPayment.SourceDocumentModule}"];
                dto.CurrencyName = defaultCompany.CurrencyName;
                if (x.vendorPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.Bill)
                {
                    dto.SourceDocumentPath = "VendorBill";
                }
                if (x.vendorPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.DebitNote)
                {
                    dto.SourceDocumentPath = "VendorDebitNote";
                }
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<ListResultDto<CashAndBankLookupDto>> GetCashAndBankLookupAsync()
        {
            var list = await _cashAndBankRepository.GetListAsync();
            return new ListResultDto<CashAndBankLookupDto>(
                ObjectMapper.Map<List<CashAndBank>, List<CashAndBankLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<VendorLookupDto>> GetVendorLookupAsync()
        {
            var list = await _vendorRepository.GetListAsync();
            return new ListResultDto<VendorLookupDto>(
                ObjectMapper.Map<List<Vendor>, List<VendorLookupDto>>(list)
            );
        }
        public async Task<VendorPaymentReadDto> CreateAsync(VendorPaymentCreateDto input)
        {
            var obj = await _vendorPaymentManager.CreateAsync(
                input.Number,
                input.PaymentDate,
                input.CashAndBankId,
                input.VendorId,
                input.Amount,
                input.SourceDocument,
                input.SourceDocumentId,
                input.SourceDocumentModule
            );

            obj.Description = input.Description;

            await _vendorPaymentRepository.InsertAsync(obj);

            return ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, VendorPaymentUpdateDto input)
        {
            var obj = await _vendorPaymentRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _vendorPaymentManager.ChangeNumberAsync(obj, input.Number);
            }

            obj.Description = input.Description;
            obj.PaymentDate = input.PaymentDate;
            obj.CashAndBankId = input.CashAndBankId;
            obj.VendorId = input.VendorId;
            obj.Amount = input.Amount;

            if (obj.SourceDocumentModule == NumberSequenceModules.Bill)
            {
                obj.Debit = 0;
                obj.Credit = obj.Amount;
                obj.Balance = obj.Debit - obj.Credit;
            }

            if (obj.SourceDocumentModule == NumberSequenceModules.DebitNote)
            {
                obj.Debit = obj.Amount;
                obj.Credit = 0;
                obj.Balance = obj.Debit - obj.Credit;
            }

            await _vendorPaymentRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _vendorPaymentRepository.DeleteAsync(id);
        }

        public async Task<VendorPaymentReadDto> ConfirmAsync(Guid vendorPaymentId)
        {
            var obj = await _vendorPaymentManager.ConfirmAsync(vendorPaymentId);
            return ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(obj);
        }

        public async Task<VendorPaymentReadDto> CancelAsync(Guid vendorPaymentId)
        {
            var obj = await _vendorPaymentManager.CancelAsync(vendorPaymentId);
            return ObjectMapper.Map<VendorPayment, VendorPaymentReadDto>(obj);
        }
    }
}
