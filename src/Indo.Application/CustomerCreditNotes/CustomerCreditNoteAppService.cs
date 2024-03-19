using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Currencies;
using Indo.Customers;
using Indo.CustomerInvoices;
using Indo.CustomerCreditNoteDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.CustomerPayments;

namespace Indo.CustomerCreditNotes
{
    public class CustomerCreditNoteAppService : IndoAppService, ICustomerCreditNoteAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerCreditNoteRepository _customerCreditNoteRepository;
        private readonly CustomerCreditNoteManager _customerCreditNoteManager;
        private readonly ICustomerCreditNoteDetailRepository _customerCreditNoteDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        private readonly ICustomerInvoiceRepository _customerInvoiceRepository;
        
        public CustomerCreditNoteAppService(
            ICustomerCreditNoteRepository customerCreditNoteRepository,
            CustomerCreditNoteManager customerCreditNoteManager,
            ICustomerRepository customerRepository,
            ICustomerCreditNoteDetailRepository customerCreditNoteDetailRepository,
            ICompanyAppService companyAppService,
            ICustomerInvoiceRepository customerInvoiceRepository
            )
        {
            _customerCreditNoteRepository = customerCreditNoteRepository;
            _customerCreditNoteManager = customerCreditNoteManager;
            _customerRepository = customerRepository;
            _customerCreditNoteDetailRepository = customerCreditNoteDetailRepository;
            _companyAppService = companyAppService;
            _customerInvoiceRepository = customerInvoiceRepository;
        }

        public async Task<CreditNoteCountDto> GetCountOrderAsync()
        {
            await Task.Yield();
            var result = new CreditNoteCountDto();
            result.CountDraft = _customerCreditNoteRepository.Where(x => x.Status == CustomerCreditNoteStatus.Draft).Count();
            result.CountConfirm = _customerCreditNoteRepository.Where(x => x.Status == CustomerCreditNoteStatus.Confirm).Count();
            result.CountCancelled = _customerCreditNoteRepository.Where(x => x.Status == CustomerCreditNoteStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummarySubTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _customerCreditNoteDetailRepository
                .Where(x => x.CustomerCreditNoteId.Equals(id))
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
            result = _customerCreditNoteDetailRepository
                .Where(x => x.CustomerCreditNoteId.Equals(id))
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
            result = _customerCreditNoteDetailRepository
                .Where(x => x.CustomerCreditNoteId.Equals(id))
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
            result = _customerCreditNoteDetailRepository
                .Where(x => x.CustomerCreditNoteId.Equals(id))
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
            result = _customerCreditNoteDetailRepository
                .Where(x => x.CustomerCreditNoteId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<CustomerCreditNoteReadDto> GetAsync(Guid id)
        {
            var queryable = await _customerCreditNoteRepository.GetQueryableAsync();
            var query = from customerCreditNote in queryable
                        join customer in _customerRepository on customerCreditNote.CustomerId equals customer.Id
                        join customerInvoice in _customerInvoiceRepository on customerCreditNote.CustomerInvoiceId equals customerInvoice.Id
                        where customerCreditNote.Id == id
                        select new { customerCreditNote, customer, customerInvoice };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(CustomerCreditNote), id);
            }
            var dto = ObjectMapper.Map<CustomerCreditNote, CustomerCreditNoteReadDto>(queryResult.customerCreditNote);
            dto.CustomerName = queryResult.customer.Name;
            dto.CustomerInvoiceNumber = queryResult.customerInvoice.Number;
            dto.StatusString = L[$"Enum:CustomerCreditNoteStatus:{(int)queryResult.customerCreditNote.Status}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<CustomerCreditNoteReadDto>> GetListAsync()
        {
            var queryable = await _customerCreditNoteRepository.GetQueryableAsync();
            var query = from customerCreditNote in queryable
                        join customer in _customerRepository on customerCreditNote.CustomerId equals customer.Id
                        join customerInvoice in _customerInvoiceRepository on customerCreditNote.CustomerInvoiceId equals customerInvoice.Id
                        select new { customerCreditNote, customer, customerInvoice };
                   
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerCreditNote, CustomerCreditNoteReadDto>(x.customerCreditNote);
                dto.CustomerName = x.customer.Name;
                dto.CustomerInvoiceNumber = x.customerInvoice.Number;
                dto.StatusString = L[$"Enum:CustomerCreditNoteStatus:{(int)x.customerCreditNote.Status}"];
                return dto;
            })
                .OrderByDescending(x => x.CreditNoteDate)
                .ToList();

            return dtos;
        }
        public async Task<List<CustomerCreditNoteReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _customerCreditNoteRepository.GetQueryableAsync();
            var query = from customerCreditNote in queryable
                        join customer in _customerRepository on customerCreditNote.CustomerId equals customer.Id
                        join customerInvoice in _customerInvoiceRepository on customerCreditNote.CustomerInvoiceId equals customerInvoice.Id
                        select new { customerCreditNote, customer, customerInvoice };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerCreditNote, CustomerCreditNoteReadDto>(x.customerCreditNote);
                dto.CustomerName = x.customer.Name;
                dto.CustomerInvoiceNumber = x.customerInvoice.Number;
                dto.StatusString = L[$"Enum:CustomerCreditNoteStatus:{(int)x.customerCreditNote.Status}"];
                dto.Total = _customerCreditNoteDetailRepository.Where(y => y.CustomerCreditNoteId.Equals(x.customerCreditNote.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.CreditNoteDate)
                .ToList();

            return dtos;
        }
        public async Task<List<CustomerCreditNoteReadDto>> GetListWithTotalByCustomerAsync(Guid customerId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _customerCreditNoteRepository.GetQueryableAsync();
            var query = from customerCreditNote in queryable
                        join customer in _customerRepository on customerCreditNote.CustomerId equals customer.Id
                        join customerInvoice in _customerInvoiceRepository on customerCreditNote.CustomerInvoiceId equals customerInvoice.Id
                        where customerCreditNote.CustomerId == customerId
                        select new { customerCreditNote, customer, customerInvoice };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerCreditNote, CustomerCreditNoteReadDto>(x.customerCreditNote);
                dto.CustomerName = x.customer.Name;
                dto.CustomerInvoiceNumber = x.customerInvoice.Number;
                dto.StatusString = L[$"Enum:CustomerCreditNoteStatus:{(int)x.customerCreditNote.Status}"];
                dto.Total = _customerCreditNoteDetailRepository.Where(y => y.CustomerCreditNoteId.Equals(x.customerCreditNote.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.CreditNoteDate)
                .ToList();

            return dtos;
        }
        public async Task<List<CustomerCreditNoteReadDto>> GetListWithTotalByInvoiceAsync(Guid customerInvoiceId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _customerCreditNoteRepository.GetQueryableAsync();
            var query = from customerCreditNote in queryable
                        join customer in _customerRepository on customerCreditNote.CustomerId equals customer.Id
                        join customerInvoice in _customerInvoiceRepository on customerCreditNote.CustomerInvoiceId equals customerInvoice.Id
                        where customerCreditNote.CustomerInvoiceId == customerInvoiceId
                        select new { customerCreditNote, customer, customerInvoice };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerCreditNote, CustomerCreditNoteReadDto>(x.customerCreditNote);
                dto.CustomerName = x.customer.Name;
                dto.CustomerInvoiceNumber = x.customerInvoice.Number;
                dto.StatusString = L[$"Enum:CustomerCreditNoteStatus:{(int)x.customerCreditNote.Status}"];
                dto.Total = _customerCreditNoteDetailRepository.Where(y => y.CustomerCreditNoteId.Equals(x.customerCreditNote.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.CreditNoteDate)
                .ToList();

            return dtos;
        }
        public async Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync()
        {
            var list = await _customerRepository.GetListAsync();
            list = list.Where(x => x.Status == CustomerStatus.Customer).ToList();
            return new ListResultDto<CustomerLookupDto>(
                ObjectMapper.Map<List<Customer>, List<CustomerLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<CustomerInvoiceLookupDto>> GetCustomerInvoiceLookupAsync()
        {
            var list = await _customerInvoiceRepository.GetListAsync();
            return new ListResultDto<CustomerInvoiceLookupDto>(
                ObjectMapper.Map<List<CustomerInvoice>, List<CustomerInvoiceLookupDto>>(list)
            );
        }
        public async Task<CustomerCreditNoteReadDto> CreateAsync(CustomerCreditNoteCreateDto input)
        {
            var obj = await _customerCreditNoteManager.CreateAsync(
                input.Number,
                input.CustomerId,
                input.CreditNoteDate,
                input.CustomerInvoiceId
            );

            obj.Description = input.Description;
            obj.PaymentNote = input.PaymentNote;

            await _customerCreditNoteRepository.InsertAsync(obj);

            return ObjectMapper.Map<CustomerCreditNote, CustomerCreditNoteReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, CustomerCreditNoteUpdateDto input)
        {
            var obj = await _customerCreditNoteRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _customerCreditNoteManager.ChangeNameAsync(obj, input.Number);
            }

            obj.CustomerId = input.CustomerId;
            obj.Description = input.Description;
            obj.PaymentNote = input.PaymentNote;
            obj.CreditNoteDate = input.CreditNoteDate;
            obj.CustomerInvoiceId = input.CustomerInvoiceId;

            await _customerCreditNoteRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _customerCreditNoteRepository.DeleteAsync(id);
        }

        public async Task<CustomerCreditNoteReadDto> ConfirmAsync(Guid customerCreditNoteId)
        {
            var obj = await _customerCreditNoteManager.ConfirmAsync(customerCreditNoteId);
            return ObjectMapper.Map<CustomerCreditNote, CustomerCreditNoteReadDto>(obj);
        }

        public async Task<CustomerCreditNoteReadDto> CancelAsync(Guid customerCreditNoteId)
        {
            var obj = await _customerCreditNoteManager.CancelAsync(customerCreditNoteId);
            return ObjectMapper.Map<CustomerCreditNote, CustomerCreditNoteReadDto>(obj);
        }
        public async Task<CustomerPaymentReadDto> GeneratePaymentAsync(Guid customerCreditNoteId)
        {
            var obj = await _customerCreditNoteManager.GeneratePayment(customerCreditNoteId);
            return ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(obj);
        }
    }
}
