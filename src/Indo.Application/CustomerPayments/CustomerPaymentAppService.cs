using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.CashAndBanks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.NumberSequences;

namespace Indo.CustomerPayments
{
    public class CustomerPaymentAppService : IndoAppService, ICustomerPaymentAppService
    {
        private readonly ICustomerPaymentRepository _customerPaymentRepository;
        private readonly CustomerPaymentManager _customerPaymentManager;
        private readonly ICashAndBankRepository _cashAndBankRepository;
        private readonly CompanyAppService _companyAppService;
        private readonly ICustomerRepository _customerRepository;
        public CustomerPaymentAppService(
            ICustomerPaymentRepository customerPaymentRepository,
            CustomerPaymentManager customerPaymentManager,
            ICashAndBankRepository cashAndBankRepository,
            CompanyAppService companyAppService,
            ICustomerRepository customerRepository
            )
        {
            _customerPaymentRepository = customerPaymentRepository;
            _customerPaymentManager = customerPaymentManager;
            _cashAndBankRepository = cashAndBankRepository;
            _companyAppService = companyAppService;
            _customerRepository = customerRepository;
        }
        public async Task<CustomerPaymentReadDto> GetAsync(Guid id)
        {
            var queryable = await _customerPaymentRepository.GetQueryableAsync();
            var query = from customerPayment in queryable
                        join cashAndBank in _cashAndBankRepository on customerPayment.CashAndBankId equals cashAndBank.Id
                        join customer in _customerRepository on customerPayment.CustomerId equals customer.Id
                        where customerPayment.Id == id
                        select new { customerPayment, cashAndBank, customer };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(CustomerPayment), id);
            }
            var dto = ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(queryResult.customerPayment);
            dto.CashAndBankName = queryResult.cashAndBank.Name;
            dto.CustomerName = queryResult.customer.Name;
            dto.StatusString = L[$"Enum:CustomerPaymentStatus:{(int)queryResult.customerPayment.Status}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<CustomerPaymentReadDto>> GetListAsync()
        {
            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerPaymentRepository.GetQueryableAsync();
            var query = from customerPayment in queryable
                        join cashAndBank in _cashAndBankRepository on customerPayment.CashAndBankId equals cashAndBank.Id
                        join customer in _customerRepository on customerPayment.CustomerId equals customer.Id
                        select new { customerPayment, cashAndBank, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(x.customerPayment);
                dto.CashAndBankName = x.cashAndBank.Name;
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerPaymentStatus:{(int)x.customerPayment.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerPayment.SourceDocumentModule}"];
                dto.CurrencyName = defaultCompany.CurrencyName;
                dto.Period = x.customerPayment.PaymentDate.ToString("yyyy-MM");
                if (x.customerPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.Invoice)
                {
                    dto.SourceDocumentPath = "CustomerInvoice";
                }
                if (x.customerPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.CreditNote)
                {
                    dto.SourceDocumentPath = "CustomerCreditNote";
                }
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<CustomerPaymentReadDto>> GetListByCustomerAsync(Guid customerId)
        {
            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerPaymentRepository.GetQueryableAsync();
            var query = from customerPayment in queryable
                        join cashAndBank in _cashAndBankRepository on customerPayment.CashAndBankId equals cashAndBank.Id
                        join customer in _customerRepository on customerPayment.CustomerId equals customer.Id
                        where customerPayment.CustomerId == customerId
                        select new { customerPayment, cashAndBank, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(x.customerPayment);
                dto.CashAndBankName = x.cashAndBank.Name;
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerPaymentStatus:{(int)x.customerPayment.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerPayment.SourceDocumentModule}"];
                dto.CurrencyName = defaultCompany.CurrencyName;
                dto.Period = x.customerPayment.PaymentDate.ToString("yyyy-MM");
                if (x.customerPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.Invoice)
                {
                    dto.SourceDocumentPath = "CustomerInvoice";
                }
                if (x.customerPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.CreditNote)
                {
                    dto.SourceDocumentPath = "CustomerCreditNote";
                }
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<CustomerPaymentReadDto>> GetListByInvoiceAsync(Guid customerInvoiceId)
        {
            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerPaymentRepository.GetQueryableAsync();
            var query = from customerPayment in queryable
                        join cashAndBank in _cashAndBankRepository on customerPayment.CashAndBankId equals cashAndBank.Id
                        join customer in _customerRepository on customerPayment.CustomerId equals customer.Id
                        where customerPayment.SourceDocumentId == customerInvoiceId && customerPayment.SourceDocumentModule == NumberSequenceModules.Invoice
                        select new { customerPayment, cashAndBank, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(x.customerPayment);
                dto.CashAndBankName = x.cashAndBank.Name;
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerPaymentStatus:{(int)x.customerPayment.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerPayment.SourceDocumentModule}"];
                dto.CurrencyName = defaultCompany.CurrencyName;
                dto.Period = x.customerPayment.PaymentDate.ToString("yyyy-MM");
                if (x.customerPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.Invoice)
                {
                    dto.SourceDocumentPath = "CustomerInvoice";
                }
                if (x.customerPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.CreditNote)
                {
                    dto.SourceDocumentPath = "CustomerCreditNote";
                }
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<CustomerPaymentReadDto>> GetListByCreditNoteAsync(Guid customerCreditNoteId)
        {
            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerPaymentRepository.GetQueryableAsync();
            var query = from customerPayment in queryable
                        join cashAndBank in _cashAndBankRepository on customerPayment.CashAndBankId equals cashAndBank.Id
                        join customer in _customerRepository on customerPayment.CustomerId equals customer.Id
                        where customerPayment.SourceDocumentId == customerCreditNoteId && customerPayment.SourceDocumentModule == NumberSequenceModules.CreditNote
                        select new { customerPayment, cashAndBank, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(x.customerPayment);
                dto.CashAndBankName = x.cashAndBank.Name;
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerPaymentStatus:{(int)x.customerPayment.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerPayment.SourceDocumentModule}"];
                dto.CurrencyName = defaultCompany.CurrencyName;
                dto.Period = x.customerPayment.PaymentDate.ToString("yyyy-MM");
                if (x.customerPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.Invoice)
                {
                    dto.SourceDocumentPath = "CustomerInvoice";
                }
                if (x.customerPayment.SourceDocumentModule == NumberSequences.NumberSequenceModules.CreditNote)
                {
                    dto.SourceDocumentPath = "CustomerCreditNote";
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
        public async Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync()
        {
            var list = await _customerRepository.GetListAsync();
            list = list.Where(x => x.Status == CustomerStatus.Customer).ToList();
            return new ListResultDto<CustomerLookupDto>(
                ObjectMapper.Map<List<Customer>, List<CustomerLookupDto>>(list)
            );
        }
        public async Task<CustomerPaymentReadDto> CreateAsync(CustomerPaymentCreateDto input)
        {
            var obj = await _customerPaymentManager.CreateAsync(
                input.Number,
                input.PaymentDate,
                input.CashAndBankId,
                input.CustomerId,
                input.Amount,
                input.SourceDocument,
                input.SourceDocumentId,
                input.SourceDocumentModule
            );

            obj.Description = input.Description;

            await _customerPaymentRepository.InsertAsync(obj);

            return ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, CustomerPaymentUpdateDto input)
        {
            var obj = await _customerPaymentRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _customerPaymentManager.ChangeNumberAsync(obj, input.Number);
            }

            obj.Description = input.Description;
            obj.PaymentDate = input.PaymentDate;
            obj.CashAndBankId = input.CashAndBankId;
            obj.CustomerId = input.CustomerId;
            obj.Amount = input.Amount;

            if (obj.SourceDocumentModule == NumberSequenceModules.Invoice)
            {
                obj.Debit = obj.Amount;
                obj.Credit = 0;
                obj.Balance = obj.Debit - obj.Credit;
            }

            if (obj.SourceDocumentModule == NumberSequenceModules.CreditNote)
            {
                obj.Debit = 0;
                obj.Credit = obj.Amount;
                obj.Balance = obj.Debit - obj.Credit;
            }

            await _customerPaymentRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _customerPaymentRepository.DeleteAsync(id);
        }

        public async Task<CustomerPaymentReadDto> ConfirmAsync(Guid customerPaymentId)
        {
            var obj = await _customerPaymentManager.ConfirmAsync(customerPaymentId);
            return ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(obj);
        }

        public async Task<CustomerPaymentReadDto> CancelAsync(Guid customerPaymentId)
        {
            var obj = await _customerPaymentManager.CancelAsync(customerPaymentId);
            return ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(obj);
        }
    }
}
