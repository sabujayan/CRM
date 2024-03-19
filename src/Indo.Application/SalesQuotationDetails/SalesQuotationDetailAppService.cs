using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.SalesQuotations;
using Indo.Products;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.Companies;
using Indo.Customers;

namespace Indo.SalesQuotationDetails
{
    public class SalesQuotationDetailAppService : IndoAppService, ISalesQuotationDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly ISalesQuotationRepository _salesQuotationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly ISalesQuotationDetailRepository _salesQuotationDetailRepository;
        private readonly SalesQuotationDetailManager _salesQuotationDetailManager;
        private readonly ICustomerRepository _customerRepository;
        public SalesQuotationDetailAppService(
            CompanyAppService companyAppService,
            ISalesQuotationDetailRepository salesQuotationDetailRepository,
            SalesQuotationDetailManager salesQuotationDetailManager,
            ISalesQuotationRepository salesQuotationRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IUomRepository uomRepository)
        {
            _salesQuotationDetailRepository = salesQuotationDetailRepository;
            _salesQuotationDetailManager = salesQuotationDetailManager;
            _salesQuotationRepository = salesQuotationRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _customerRepository = customerRepository;
        }
        public async Task<SalesQuotationDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesQuotationDetailRepository.GetQueryableAsync();
            var query = from salesQuotationDetail in queryable
                        join salesQuotation in _salesQuotationRepository on salesQuotationDetail.SalesQuotationId equals salesQuotation.Id
                        join product in _productRepository on salesQuotationDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where salesQuotationDetail.Id == id
                        select new { salesQuotationDetail, salesQuotation, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(SalesQuotationDetail), id);
            }
            var dto = ObjectMapper.Map<SalesQuotationDetail, SalesQuotationDetailReadDto>(queryResult.salesQuotationDetail);
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.salesQuotation.Status;
            dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)queryResult.salesQuotation.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<SalesQuotationDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesQuotationDetailRepository.GetQueryableAsync();
            var query = from salesQuotationDetail in queryable
                        join salesQuotation in _salesQuotationRepository on salesQuotationDetail.SalesQuotationId equals salesQuotation.Id
                        join product in _productRepository on salesQuotationDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesQuotationDetail, salesQuotation, product, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesQuotationDetail, SalesQuotationDetailReadDto>(x.salesQuotationDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesQuotation.Status;
                dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)x.salesQuotation.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _salesQuotationDetailRepository.GetCountAsync();

            return new PagedResultDto<SalesQuotationDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<SalesQuotationDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesQuotationDetailRepository.GetQueryableAsync();
            var query = from salesQuotationDetail in queryable
                        join salesQuotation in _salesQuotationRepository on salesQuotationDetail.SalesQuotationId equals salesQuotation.Id
                        join customer in _customerRepository on salesQuotation.CustomerId equals customer.Id
                        join product in _productRepository on salesQuotationDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesQuotationDetail, salesQuotation, customer, product, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesQuotationDetail, SalesQuotationDetailReadDto>(x.salesQuotationDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesQuotation.Status;
                dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)x.salesQuotation.Status}"];
                dto.SalesQuotationNumber = x.salesQuotation.Number;
                dto.QuotationDate = x.salesQuotation.QuotationDate;
                dto.CustomerName = x.customer.Name;
                dto.PriceString = x.salesQuotationDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.salesQuotationDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.salesQuotationDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.salesQuotationDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.salesQuotationDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.salesQuotationDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            return dtos;
        }
        public async Task<List<SalesQuotationDetailReadDto>> GetListHighPerformerAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesQuotationDetailRepository.GetQueryableAsync();
            var query = from salesQuotationDetail in queryable
                        join salesQuotation in _salesQuotationRepository on salesQuotationDetail.SalesQuotationId equals salesQuotation.Id
                        join customer in _customerRepository on salesQuotation.CustomerId equals customer.Id
                        join product in _productRepository on salesQuotationDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesQuotationDetail, salesQuotation, customer, product, uom };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesQuotationDetail, SalesQuotationDetailReadDto>(x.salesQuotationDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesQuotation.Status;
                dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)x.salesQuotation.Status}"];
                dto.SalesQuotationNumber = x.salesQuotation.Number;
                dto.QuotationDate = x.salesQuotation.QuotationDate;
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();

            var groups = dtos.GroupBy(x => new { x.ProductName })
                .Select(x => new SalesQuotationDetailReadDto { 
                    ProductName = x.Key.ProductName,
                    Quantity = x.Sum(x => x.Quantity),
                    UomName = x.Max(x => x.UomName)
                })
                .ToList();

            groups = groups.OrderByDescending(x => x.Quantity)
                .Take(5).ToList();

            return groups;
        }
        public async Task<List<SalesQuotationDetailReadDto>> GetListLowPerformerAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesQuotationDetailRepository.GetQueryableAsync();
            var query = from salesQuotationDetail in queryable
                        join salesQuotation in _salesQuotationRepository on salesQuotationDetail.SalesQuotationId equals salesQuotation.Id
                        join customer in _customerRepository on salesQuotation.CustomerId equals customer.Id
                        join product in _productRepository on salesQuotationDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesQuotationDetail, salesQuotation, customer, product, uom };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesQuotationDetail, SalesQuotationDetailReadDto>(x.salesQuotationDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesQuotation.Status;
                dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)x.salesQuotation.Status}"];
                dto.SalesQuotationNumber = x.salesQuotation.Number;
                dto.QuotationDate = x.salesQuotation.QuotationDate;
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();

            var groups = dtos.GroupBy(x => new { x.ProductName })
                .Select(x => new SalesQuotationDetailReadDto
                {
                    ProductName = x.Key.ProductName,
                    Quantity = x.Sum(x => x.Quantity),
                    UomName = x.Max(x => x.UomName)
                })
                .ToList();

            groups = groups.OrderBy(x => x.Quantity)
                .Take(5).ToList();

            return groups;
        }
        public async Task<PagedResultDto<SalesQuotationDetailReadDto>> GetListBySalesQuotationAsync(Guid salesQuotationId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesQuotationDetailRepository.GetQueryableAsync();
            var query = from salesQuotationDetail in queryable
                        join salesQuotation in _salesQuotationRepository on salesQuotationDetail.SalesQuotationId equals salesQuotation.Id
                        join product in _productRepository on salesQuotationDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where salesQuotationDetail.SalesQuotationId.Equals(salesQuotationId)
                        select new { salesQuotationDetail, salesQuotation, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesQuotationDetail, SalesQuotationDetailReadDto>(x.salesQuotationDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesQuotation.Status;
                dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)x.salesQuotation.Status}"];
                dto.PriceString = x.salesQuotationDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.salesQuotationDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.salesQuotationDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.salesQuotationDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.salesQuotationDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.salesQuotationDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<SalesQuotationDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<SalesQuotationLookupDto>> GetSalesQuotationLookupAsync()
        {
            var list = await _salesQuotationRepository.GetListAsync();
            return new ListResultDto<SalesQuotationLookupDto>(
                ObjectMapper.Map<List<SalesQuotation>, List<SalesQuotationLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
        {
            var list = await _productRepository.GetListAsync();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<SalesQuotationDetailReadDto> CreateAsync(SalesQuotationDetailCreateDto input)
        {
            var obj = await _salesQuotationDetailManager.CreateAsync(
                input.SalesQuotationId,
                input.ProductId,
                input.Quantity,
                input.DiscAmt
            );
            await _salesQuotationDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<SalesQuotationDetail, SalesQuotationDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, SalesQuotationDetailUpdateDto input)
        {
            var obj = await _salesQuotationDetailRepository.GetAsync(id);

            obj.ProductId = input.ProductId;
            obj.Quantity = input.Quantity;
            obj.DiscAmt = input.DiscAmt;

            var product = _productRepository.Where(x => x.Id.Equals(obj.ProductId)).FirstOrDefault();
            
            obj.Price = product.RetailPrice;
            obj.TaxRate = product.TaxRate;
            obj.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _salesQuotationDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _salesQuotationDetailRepository.DeleteAsync(id);
        }
    }
}
