using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.CustomerCreditNotes;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.CustomerCreditNoteDetails
{
    public class CustomerCreditNoteDetailAppService : IndoAppService, ICustomerCreditNoteDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly ICustomerCreditNoteRepository _customerCreditNoteRepository;
        private readonly IUomRepository _uomRepository;
        private readonly ICustomerCreditNoteDetailRepository _customerCreditNoteDetailRepository;
        private readonly CustomerCreditNoteDetailManager _customerCreditNoteDetailManager;
        private readonly ICustomerRepository _customerRepository;
        public CustomerCreditNoteDetailAppService(
            CompanyAppService companyAppService,
            ICustomerCreditNoteDetailRepository customerCreditNoteDetailRepository,
            CustomerCreditNoteDetailManager customerCreditNoteDetailManager,
            ICustomerCreditNoteRepository customerCreditNoteRepository,
            ICustomerRepository customerRepository,
            IUomRepository uomRepository)
        {
            _customerCreditNoteDetailRepository = customerCreditNoteDetailRepository;
            _customerCreditNoteDetailManager = customerCreditNoteDetailManager;
            _customerCreditNoteRepository = customerCreditNoteRepository;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _customerRepository = customerRepository;
        }
        public async Task<CustomerCreditNoteDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerCreditNoteDetailRepository.GetQueryableAsync();
            var query = from customerCreditNoteDetail in queryable
                        join customerCreditNote in _customerCreditNoteRepository on customerCreditNoteDetail.CustomerCreditNoteId equals customerCreditNote.Id
                        join uom in _uomRepository on customerCreditNoteDetail.UomId equals uom.Id
                        where customerCreditNoteDetail.Id == id
                        select new { customerCreditNoteDetail, customerCreditNote, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(CustomerCreditNoteDetail), id);
            }
            var dto = ObjectMapper.Map<CustomerCreditNoteDetail, CustomerCreditNoteDetailReadDto>(queryResult.customerCreditNoteDetail);
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.customerCreditNote.Status;
            dto.StatusString = L[$"Enum:CustomerCreditNoteStatus:{(int)queryResult.customerCreditNote.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<CustomerCreditNoteDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerCreditNoteDetailRepository.GetQueryableAsync();
            var query = from customerCreditNoteDetail in queryable
                        join customerCreditNote in _customerCreditNoteRepository on customerCreditNoteDetail.CustomerCreditNoteId equals customerCreditNote.Id
                        join uom in _uomRepository on customerCreditNoteDetail.UomId equals uom.Id
                        select new { customerCreditNoteDetail, customerCreditNote, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerCreditNoteDetail, CustomerCreditNoteDetailReadDto>(x.customerCreditNoteDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.customerCreditNote.Status;
                dto.StatusString = L[$"Enum:CustomerCreditNoteStatus:{(int)x.customerCreditNote.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _customerCreditNoteDetailRepository.GetCountAsync();

            return new PagedResultDto<CustomerCreditNoteDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<CustomerCreditNoteDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerCreditNoteDetailRepository.GetQueryableAsync();
            var query = from customerCreditNoteDetail in queryable
                        join customerCreditNote in _customerCreditNoteRepository on customerCreditNoteDetail.CustomerCreditNoteId equals customerCreditNote.Id
                        join customer in _customerRepository on customerCreditNote.CustomerId equals customer.Id
                        join uom in _uomRepository on customerCreditNoteDetail.UomId equals uom.Id
                        select new { customerCreditNoteDetail, customerCreditNote, customer, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerCreditNoteDetail, CustomerCreditNoteDetailReadDto>(x.customerCreditNoteDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.customerCreditNote.Status;
                dto.StatusString = L[$"Enum:CustomerCreditNoteStatus:{(int)x.customerCreditNote.Status}"];
                dto.CustomerCreditNoteNumber = x.customerCreditNote.Number;
                dto.CreditNoteDate = x.customerCreditNote.CreditNoteDate;
                dto.CustomerName = x.customer.Name;
                dto.PriceString = x.customerCreditNoteDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.customerCreditNoteDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.customerCreditNoteDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.customerCreditNoteDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.customerCreditNoteDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.customerCreditNoteDetail.Total.ToString("##,##.00");
                dto.Period = x.customerCreditNote.CreditNoteDate.ToString("yyyy-MM");
                return dto;
            })
                .OrderByDescending(x => x.CreditNoteDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<CustomerCreditNoteDetailReadDto>> GetListByCustomerCreditNoteAsync(Guid customerCreditNoteId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerCreditNoteDetailRepository.GetQueryableAsync();
            var query = from customerCreditNoteDetail in queryable
                        join customerCreditNote in _customerCreditNoteRepository on customerCreditNoteDetail.CustomerCreditNoteId equals customerCreditNote.Id
                        join uom in _uomRepository on customerCreditNoteDetail.UomId equals uom.Id
                        where customerCreditNoteDetail.CustomerCreditNoteId.Equals(customerCreditNoteId)
                        select new { customerCreditNoteDetail, customerCreditNote, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerCreditNoteDetail, CustomerCreditNoteDetailReadDto>(x.customerCreditNoteDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.customerCreditNote.Status;
                dto.StatusString = L[$"Enum:CustomerCreditNoteStatus:{(int)x.customerCreditNote.Status}"];
                dto.PriceString = x.customerCreditNoteDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.customerCreditNoteDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.customerCreditNoteDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.customerCreditNoteDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.customerCreditNoteDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.customerCreditNoteDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<CustomerCreditNoteDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<CustomerCreditNoteLookupDto>> GetCustomerCreditNoteLookupAsync()
        {
            var list = await _customerCreditNoteRepository.GetListAsync();
            return new ListResultDto<CustomerCreditNoteLookupDto>(
                ObjectMapper.Map<List<CustomerCreditNote>, List<CustomerCreditNoteLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<UomLookupDto>> GetUomLookupAsync()
        {
            var list = await _uomRepository.GetListAsync();
            return new ListResultDto<UomLookupDto>(
                ObjectMapper.Map<List<Uom>, List<UomLookupDto>>(list)
            );
        }
        public async Task<CustomerCreditNoteDetailReadDto> CreateAsync(CustomerCreditNoteDetailCreateDto input)
        {
            var obj = await _customerCreditNoteDetailManager.CreateAsync(
                input.CustomerCreditNoteId,
                input.ProductName,
                input.UomId,
                input.Price,
                input.TaxRate,
                input.Quantity,
                input.DiscAmt
            );
            await _customerCreditNoteDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<CustomerCreditNoteDetail, CustomerCreditNoteDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, CustomerCreditNoteDetailUpdateDto input)
        {
            var obj = await _customerCreditNoteDetailRepository.GetAsync(id);

            obj.ProductName = input.ProductName;
            obj.UomId = input.UomId;
            obj.Price = input.Price;
            obj.TaxRate = input.TaxRate;
            obj.Quantity = input.Quantity;
            obj.DiscAmt = input.DiscAmt;

            obj.Recalculate();

            await _customerCreditNoteDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _customerCreditNoteDetailRepository.DeleteAsync(id);
        }
    }
}
