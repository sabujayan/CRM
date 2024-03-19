using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Currencies;
using Indo.Customers;
using Indo.SalesQuotations;
using Indo.CustomerInvoiceDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.CustomerCreditNotes;
using Indo.CustomerPayments;

namespace Indo.CustomerInvoices
{
    public class CustomerInvoiceAppService : IndoAppService, ICustomerInvoiceAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerInvoiceRepository _customerInvoiceRepository;
        private readonly CustomerInvoiceManager _customerInvoiceManager;
        private readonly ICustomerInvoiceDetailRepository _customerInvoiceDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        private readonly ISalesQuotationRepository _salesQuotationRepository;
        public CustomerInvoiceAppService(
            ICustomerInvoiceRepository customerInvoiceRepository,
            CustomerInvoiceManager customerInvoiceManager,
            ICustomerRepository customerRepository,
            ICustomerInvoiceDetailRepository customerInvoiceDetailRepository,
            ICompanyAppService companyAppService,
            ISalesQuotationRepository salesQuotationRepository
            )
        {
            _customerInvoiceRepository = customerInvoiceRepository;
            _customerInvoiceManager = customerInvoiceManager;
            _customerRepository = customerRepository;
            _customerInvoiceDetailRepository = customerInvoiceDetailRepository;
            _companyAppService = companyAppService;
            _salesQuotationRepository = salesQuotationRepository;
        }

        public async Task<CustomerInvoiceCountDto> GetCountOrderAsync()
        {
            await Task.Yield();
            var result = new CustomerInvoiceCountDto();
            result.CountDraft = _customerInvoiceRepository.Where(x => x.Status == CustomerInvoiceStatus.Draft).Count();
            result.CountConfirm = _customerInvoiceRepository.Where(x => x.Status == CustomerInvoiceStatus.Confirm).Count();
            result.CountCancelled = _customerInvoiceRepository.Where(x => x.Status == CustomerInvoiceStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummarySubTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _customerInvoiceDetailRepository
                .Where(x => x.CustomerInvoiceId.Equals(id))
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
            result = _customerInvoiceDetailRepository
                .Where(x => x.CustomerInvoiceId.Equals(id))
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
            result = _customerInvoiceDetailRepository
                .Where(x => x.CustomerInvoiceId.Equals(id))
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
            result = _customerInvoiceDetailRepository
                .Where(x => x.CustomerInvoiceId.Equals(id))
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
            result = _customerInvoiceDetailRepository
                .Where(x => x.CustomerInvoiceId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<CustomerInvoiceReadDto> GetAsync(Guid id)
        {
            var queryable = await _customerInvoiceRepository.GetQueryableAsync();
            var query = from customerInvoice in queryable
                        join customer in _customerRepository on customerInvoice.CustomerId equals customer.Id
                        where customerInvoice.Id == id
                        select new { customerInvoice, customer };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(CustomerInvoice), id);
            }
            var dto = ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(queryResult.customerInvoice);
            dto.CustomerName = queryResult.customer.Name;
            dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)queryResult.customerInvoice.Status}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<CustomerInvoiceReadDto>> GetListAsync()
        {
            var queryable = await _customerInvoiceRepository.GetQueryableAsync();
            var query = from customerInvoice in queryable
                        join customer in _customerRepository on customerInvoice.CustomerId equals customer.Id
                        select new { customerInvoice, customer };
                   
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(x.customerInvoice);
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)x.customerInvoice.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerInvoice.SourceDocumentModule}"];
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ProjectOrder)
                {
                    dto.SourceDocumentPath = "ProjectOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ServiceOrder)
                {
                    dto.SourceDocumentPath = "ServiceOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.SalesOrder)
                {
                    dto.SourceDocumentPath = "SalesOrder";
                }
                return dto;
            })
                .OrderByDescending(x => x.InvoiceDate)
                .ToList();

            return dtos;
        }
        public async Task<List<CustomerInvoiceReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _customerInvoiceRepository.GetQueryableAsync();
            var query = from customerInvoice in queryable
                        join customer in _customerRepository on customerInvoice.CustomerId equals customer.Id
                        select new { customerInvoice, customer };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(x.customerInvoice);
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)x.customerInvoice.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerInvoice.SourceDocumentModule}"];
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ProjectOrder)
                {
                    dto.SourceDocumentPath = "ProjectOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ServiceOrder)
                {
                    dto.SourceDocumentPath = "ServiceOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.SalesOrder)
                {
                    dto.SourceDocumentPath = "SalesOrder";
                }
                dto.Total = _customerInvoiceDetailRepository.Where(y => y.CustomerInvoiceId.Equals(x.customerInvoice.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.InvoiceDate)
                .ToList();

            return dtos;
        }
        public async Task<List<CustomerInvoiceReadDto>> GetListWithTotalByCustomerAsync(Guid customerId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _customerInvoiceRepository.GetQueryableAsync();
            var query = from customerInvoice in queryable
                        join customer in _customerRepository on customerInvoice.CustomerId equals customer.Id
                        where customerInvoice.CustomerId == customerId
                        select new { customerInvoice, customer };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(x.customerInvoice);
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)x.customerInvoice.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerInvoice.SourceDocumentModule}"];
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ProjectOrder)
                {
                    dto.SourceDocumentPath = "ProjectOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ServiceOrder)
                {
                    dto.SourceDocumentPath = "ServiceOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.SalesOrder)
                {
                    dto.SourceDocumentPath = "SalesOrder";
                }
                dto.Total = _customerInvoiceDetailRepository.Where(y => y.CustomerInvoiceId.Equals(x.customerInvoice.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.InvoiceDate)
                .ToList();

            return dtos;
        }
        public async Task<List<CustomerInvoiceReadDto>> GetListWithTotalByServiceOrderAsync(Guid serviceOrderId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _customerInvoiceRepository.GetQueryableAsync();
            var query = from customerInvoice in queryable
                        join customer in _customerRepository on customerInvoice.CustomerId equals customer.Id
                        where customerInvoice.SourceDocumentId == serviceOrderId && customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ServiceOrder
                        select new { customerInvoice, customer };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(x.customerInvoice);
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)x.customerInvoice.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerInvoice.SourceDocumentModule}"];
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ProjectOrder)
                {
                    dto.SourceDocumentPath = "ProjectOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ServiceOrder)
                {
                    dto.SourceDocumentPath = "ServiceOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.SalesOrder)
                {
                    dto.SourceDocumentPath = "SalesOrder";
                }
                dto.Total = _customerInvoiceDetailRepository.Where(y => y.CustomerInvoiceId.Equals(x.customerInvoice.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.InvoiceDate)
                .ToList();

            return dtos;
        }
        public async Task<List<CustomerInvoiceReadDto>> GetListWithTotalByProjectOrderAsync(Guid projectOrderId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _customerInvoiceRepository.GetQueryableAsync();
            var query = from customerInvoice in queryable
                        join customer in _customerRepository on customerInvoice.CustomerId equals customer.Id
                        where customerInvoice.SourceDocumentId == projectOrderId && customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ProjectOrder
                        select new { customerInvoice, customer };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(x.customerInvoice);
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)x.customerInvoice.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerInvoice.SourceDocumentModule}"];
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ProjectOrder)
                {
                    dto.SourceDocumentPath = "ProjectOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ServiceOrder)
                {
                    dto.SourceDocumentPath = "ServiceOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.SalesOrder)
                {
                    dto.SourceDocumentPath = "SalesOrder";
                }
                dto.Total = _customerInvoiceDetailRepository.Where(y => y.CustomerInvoiceId.Equals(x.customerInvoice.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.InvoiceDate)
                .ToList();

            return dtos;
        }
        public async Task<List<CustomerInvoiceReadDto>> GetListWithTotalBySalesOrderAsync(Guid salesOrderId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _customerInvoiceRepository.GetQueryableAsync();
            var query = from customerInvoice in queryable
                        join customer in _customerRepository on customerInvoice.CustomerId equals customer.Id
                        where customerInvoice.SourceDocumentId == salesOrderId && customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.SalesOrder
                        select new { customerInvoice, customer };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(x.customerInvoice);
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)x.customerInvoice.Status}"];
                dto.SourceDocumentModuleString = L[$"Enum:NumberSequenceModules:{(int)x.customerInvoice.SourceDocumentModule}"];
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ProjectOrder)
                {
                    dto.SourceDocumentPath = "ProjectOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.ServiceOrder)
                {
                    dto.SourceDocumentPath = "ServiceOrder";
                }
                if (x.customerInvoice.SourceDocumentModule == NumberSequences.NumberSequenceModules.SalesOrder)
                {
                    dto.SourceDocumentPath = "SalesOrder";
                }
                dto.Total = _customerInvoiceDetailRepository.Where(y => y.CustomerInvoiceId.Equals(x.customerInvoice.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.InvoiceDate)
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
        public async Task<CustomerInvoiceReadDto> CreateAsync(CustomerInvoiceCreateDto input)
        {
            var obj = await _customerInvoiceManager.CreateAsync(
                input.Number,
                input.CustomerId,
                input.InvoiceDate,
                input.InvoiceDueDate
            );

            obj.Description = input.Description;
            obj.TermCondition = input.TermCondition;
            obj.PaymentNote = input.PaymentNote;

            await _customerInvoiceRepository.InsertAsync(obj);

            return ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, CustomerInvoiceUpdateDto input)
        {
            var obj = await _customerInvoiceRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _customerInvoiceManager.ChangeNameAsync(obj, input.Number);
            }

            obj.CustomerId = input.CustomerId;
            obj.Description = input.Description;
            obj.TermCondition = input.TermCondition;
            obj.PaymentNote = input.PaymentNote;
            obj.InvoiceDate = input.InvoiceDate;
            obj.InvoiceDueDate = input.InvoiceDueDate;

            await _customerInvoiceRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _customerInvoiceRepository.DeleteAsync(id);
        }

        public async Task<CustomerInvoiceReadDto> ConfirmAsync(Guid customerInvoiceId)
        {
            var obj = await _customerInvoiceManager.ConfirmAsync(customerInvoiceId);
            return ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(obj);
        }

        public async Task<CustomerInvoiceReadDto> CancelAsync(Guid customerInvoiceId)
        {
            var obj = await _customerInvoiceManager.CancelAsync(customerInvoiceId);
            return ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(obj);
        }
        public async Task<CustomerCreditNoteReadDto> GenerateCreditNoteAsync(Guid customerInvoiceId)
        {
            var obj = await _customerInvoiceManager.GenerateCreditNote(customerInvoiceId);
            return ObjectMapper.Map<CustomerCreditNote, CustomerCreditNoteReadDto>(obj);
        }
        public async Task<CustomerPaymentReadDto> GeneratePaymentAsync(Guid customerInvoiceId)
        {
            var obj = await _customerInvoiceManager.GeneratePayment(customerInvoiceId);
            return ObjectMapper.Map<CustomerPayment, CustomerPaymentReadDto>(obj);
        }
    }
}
