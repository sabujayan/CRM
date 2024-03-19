using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.CustomerInvoices;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.CustomerInvoiceDetails
{
    public class CustomerInvoiceDetailAppService : IndoAppService, ICustomerInvoiceDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly ICustomerInvoiceRepository _customerInvoiceRepository;
        private readonly IUomRepository _uomRepository;
        private readonly ICustomerInvoiceDetailRepository _customerInvoiceDetailRepository;
        private readonly CustomerInvoiceDetailManager _customerInvoiceDetailManager;
        private readonly ICustomerRepository _customerRepository;
        public CustomerInvoiceDetailAppService(
            CompanyAppService companyAppService,
            ICustomerInvoiceDetailRepository customerInvoiceDetailRepository,
            CustomerInvoiceDetailManager customerInvoiceDetailManager,
            ICustomerInvoiceRepository customerInvoiceRepository,
            ICustomerRepository customerRepository,
            IUomRepository uomRepository)
        {
            _customerInvoiceDetailRepository = customerInvoiceDetailRepository;
            _customerInvoiceDetailManager = customerInvoiceDetailManager;
            _customerInvoiceRepository = customerInvoiceRepository;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _customerRepository = customerRepository;
        }
        public async Task<CustomerInvoiceDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerInvoiceDetailRepository.GetQueryableAsync();
            var query = from customerInvoiceDetail in queryable
                        join customerInvoice in _customerInvoiceRepository on customerInvoiceDetail.CustomerInvoiceId equals customerInvoice.Id
                        join uom in _uomRepository on customerInvoiceDetail.UomId equals uom.Id
                        where customerInvoiceDetail.Id == id
                        select new { customerInvoiceDetail, customerInvoice, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(CustomerInvoiceDetail), id);
            }
            var dto = ObjectMapper.Map<CustomerInvoiceDetail, CustomerInvoiceDetailReadDto>(queryResult.customerInvoiceDetail);
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.customerInvoice.Status;
            dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)queryResult.customerInvoice.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<CustomerInvoiceDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerInvoiceDetailRepository.GetQueryableAsync();
            var query = from customerInvoiceDetail in queryable
                        join customerInvoice in _customerInvoiceRepository on customerInvoiceDetail.CustomerInvoiceId equals customerInvoice.Id
                        join uom in _uomRepository on customerInvoiceDetail.UomId equals uom.Id
                        select new { customerInvoiceDetail, customerInvoice, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerInvoiceDetail, CustomerInvoiceDetailReadDto>(x.customerInvoiceDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.customerInvoice.Status;
                dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)x.customerInvoice.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _customerInvoiceDetailRepository.GetCountAsync();

            return new PagedResultDto<CustomerInvoiceDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<CustomerInvoiceDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerInvoiceDetailRepository.GetQueryableAsync();
            var query = from customerInvoiceDetail in queryable
                        join customerInvoice in _customerInvoiceRepository on customerInvoiceDetail.CustomerInvoiceId equals customerInvoice.Id
                        join customer in _customerRepository on customerInvoice.CustomerId equals customer.Id
                        join uom in _uomRepository on customerInvoiceDetail.UomId equals uom.Id
                        select new { customerInvoiceDetail, customerInvoice, customer, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerInvoiceDetail, CustomerInvoiceDetailReadDto>(x.customerInvoiceDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.customerInvoice.Status;
                dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)x.customerInvoice.Status}"];
                dto.CustomerInvoiceNumber = x.customerInvoice.Number;
                dto.InvoiceDate = x.customerInvoice.InvoiceDate;
                dto.InvoiceDueDate = x.customerInvoice.InvoiceDueDate;
                dto.CustomerName = x.customer.Name;
                dto.PriceString = x.customerInvoiceDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.customerInvoiceDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.customerInvoiceDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.customerInvoiceDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.customerInvoiceDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.customerInvoiceDetail.Total.ToString("##,##.00");
                dto.Period = x.customerInvoice.InvoiceDate.ToString("yyyy-MM");
                return dto;
            })
                .OrderByDescending(x => x.InvoiceDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<CustomerInvoiceDetailReadDto>> GetListByCustomerInvoiceAsync(Guid customerInvoiceId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _customerInvoiceDetailRepository.GetQueryableAsync();
            var query = from customerInvoiceDetail in queryable
                        join customerInvoice in _customerInvoiceRepository on customerInvoiceDetail.CustomerInvoiceId equals customerInvoice.Id
                        join uom in _uomRepository on customerInvoiceDetail.UomId equals uom.Id
                        where customerInvoiceDetail.CustomerInvoiceId.Equals(customerInvoiceId)
                        select new { customerInvoiceDetail, customerInvoice, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CustomerInvoiceDetail, CustomerInvoiceDetailReadDto>(x.customerInvoiceDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.customerInvoice.Status;
                dto.StatusString = L[$"Enum:CustomerInvoiceStatus:{(int)x.customerInvoice.Status}"];
                dto.PriceString = x.customerInvoiceDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.customerInvoiceDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.customerInvoiceDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.customerInvoiceDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.customerInvoiceDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.customerInvoiceDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<CustomerInvoiceDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<CustomerInvoiceLookupDto>> GetCustomerInvoiceLookupAsync()
        {
            var list = await _customerInvoiceRepository.GetListAsync();
            return new ListResultDto<CustomerInvoiceLookupDto>(
                ObjectMapper.Map<List<CustomerInvoice>, List<CustomerInvoiceLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<UomLookupDto>> GetUomLookupAsync()
        {
            var list = await _uomRepository.GetListAsync();
            return new ListResultDto<UomLookupDto>(
                ObjectMapper.Map<List<Uom>, List<UomLookupDto>>(list)
            );
        }
        public async Task<CustomerInvoiceDetailReadDto> CreateAsync(CustomerInvoiceDetailCreateDto input)
        {
            var obj = await _customerInvoiceDetailManager.CreateAsync(
                input.CustomerInvoiceId,
                input.ProductName,
                input.UomId,
                input.Price,
                input.TaxRate,
                input.Quantity,
                input.DiscAmt
            );

            await _customerInvoiceDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<CustomerInvoiceDetail, CustomerInvoiceDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, CustomerInvoiceDetailUpdateDto input)
        {
            var obj = await _customerInvoiceDetailRepository.GetAsync(id);

            obj.ProductName = input.ProductName;
            obj.UomId = input.UomId;
            obj.Price = input.Price;
            obj.TaxRate = input.TaxRate;
            obj.Quantity = input.Quantity;
            obj.DiscAmt = input.DiscAmt;

            obj.Recalculate();

            await _customerInvoiceDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _customerInvoiceDetailRepository.DeleteAsync(id);
        }
    }
}
