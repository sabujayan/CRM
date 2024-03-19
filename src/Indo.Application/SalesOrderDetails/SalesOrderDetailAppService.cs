using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.SalesOrders;
using Indo.Products;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.Companies;
using Indo.Customers;

namespace Indo.SalesOrderDetails
{
    public class SalesOrderDetailAppService : IndoAppService, ISalesOrderDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly SalesOrderDetailManager _salesOrderDetailManager;
        private readonly ICustomerRepository _customerRepository;
        public SalesOrderDetailAppService(
            CompanyAppService companyAppService,
            ISalesOrderDetailRepository salesOrderDetailRepository,
            SalesOrderDetailManager salesOrderDetailManager,
            ISalesOrderRepository salesOrderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IUomRepository uomRepository)
        {
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _salesOrderDetailManager = salesOrderDetailManager;
            _salesOrderRepository = salesOrderRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _customerRepository = customerRepository;
        }
        public async Task<SalesOrderDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesOrderDetailRepository.GetQueryableAsync();
            var query = from salesOrderDetail in queryable
                        join salesOrder in _salesOrderRepository on salesOrderDetail.SalesOrderId equals salesOrder.Id
                        join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where salesOrderDetail.Id == id
                        select new { salesOrderDetail, salesOrder, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(SalesOrderDetail), id);
            }
            var dto = ObjectMapper.Map<SalesOrderDetail, SalesOrderDetailReadDto>(queryResult.salesOrderDetail);
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.salesOrder.Status;
            dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)queryResult.salesOrder.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<SalesOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesOrderDetailRepository.GetQueryableAsync();
            var query = from salesOrderDetail in queryable
                        join salesOrder in _salesOrderRepository on salesOrderDetail.SalesOrderId equals salesOrder.Id
                        join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesOrderDetail, salesOrder, product, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesOrderDetail, SalesOrderDetailReadDto>(x.salesOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesOrder.Status;
                dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)x.salesOrder.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _salesOrderDetailRepository.GetCountAsync();

            return new PagedResultDto<SalesOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<SalesOrderDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesOrderDetailRepository.GetQueryableAsync();
            var query = from salesOrderDetail in queryable
                        join salesOrder in _salesOrderRepository on salesOrderDetail.SalesOrderId equals salesOrder.Id
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesOrderDetail, salesOrder, customer, product, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesOrderDetail, SalesOrderDetailReadDto>(x.salesOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesOrder.Status;
                dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)x.salesOrder.Status}"];
                dto.SalesOrderNumber = x.salesOrder.Number;
                dto.OrderDate = x.salesOrder.OrderDate;
                dto.CustomerName = x.customer.Name;
                dto.PriceString = x.salesOrderDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.salesOrderDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.salesOrderDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.salesOrderDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.salesOrderDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.salesOrderDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            return dtos;
        }
        public async Task<List<SalesOrderDetailReadDto>> GetListHighPerformerAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesOrderDetailRepository.GetQueryableAsync();
            var query = from salesOrderDetail in queryable
                        join salesOrder in _salesOrderRepository on salesOrderDetail.SalesOrderId equals salesOrder.Id
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesOrderDetail, salesOrder, customer, product, uom };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesOrderDetail, SalesOrderDetailReadDto>(x.salesOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesOrder.Status;
                dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)x.salesOrder.Status}"];
                dto.SalesOrderNumber = x.salesOrder.Number;
                dto.OrderDate = x.salesOrder.OrderDate;
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();

            var groups = dtos.GroupBy(x => new { x.ProductName })
                .Select(x => new SalesOrderDetailReadDto { 
                    ProductName = x.Key.ProductName,
                    Quantity = x.Sum(x => x.Quantity),
                    UomName = x.Max(x => x.UomName)
                })
                .ToList();

            groups = groups.OrderByDescending(x => x.Quantity)
                .Take(5).ToList();

            return groups;
        }
        public async Task<List<SalesOrderDetailReadDto>> GetListLowPerformerAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesOrderDetailRepository.GetQueryableAsync();
            var query = from salesOrderDetail in queryable
                        join salesOrder in _salesOrderRepository on salesOrderDetail.SalesOrderId equals salesOrder.Id
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesOrderDetail, salesOrder, customer, product, uom };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesOrderDetail, SalesOrderDetailReadDto>(x.salesOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesOrder.Status;
                dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)x.salesOrder.Status}"];
                dto.SalesOrderNumber = x.salesOrder.Number;
                dto.OrderDate = x.salesOrder.OrderDate;
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();

            var groups = dtos.GroupBy(x => new { x.ProductName })
                .Select(x => new SalesOrderDetailReadDto
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
        public async Task<PagedResultDto<SalesOrderDetailReadDto>> GetListBySalesOrderAsync(Guid salesOrderId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesOrderDetailRepository.GetQueryableAsync();
            var query = from salesOrderDetail in queryable
                        join salesOrder in _salesOrderRepository on salesOrderDetail.SalesOrderId equals salesOrder.Id
                        join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where salesOrderDetail.SalesOrderId.Equals(salesOrderId)
                        select new { salesOrderDetail, salesOrder, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesOrderDetail, SalesOrderDetailReadDto>(x.salesOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.salesOrder.Status;
                dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)x.salesOrder.Status}"];
                dto.PriceString = x.salesOrderDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.salesOrderDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.salesOrderDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.salesOrderDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.salesOrderDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.salesOrderDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<SalesOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<SalesOrderLookupDto>> GetSalesOrderLookupAsync()
        {
            var list = await _salesOrderRepository.GetListAsync();
            return new ListResultDto<SalesOrderLookupDto>(
                ObjectMapper.Map<List<SalesOrder>, List<SalesOrderLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
        {
            var list = await _productRepository.GetListAsync();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<SalesOrderDetailReadDto> CreateAsync(SalesOrderDetailCreateDto input)
        {
            var obj = await _salesOrderDetailManager.CreateAsync(
                input.SalesOrderId,
                input.ProductId,
                input.Quantity,
                input.DiscAmt
            );
            await _salesOrderDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<SalesOrderDetail, SalesOrderDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, SalesOrderDetailUpdateDto input)
        {
            var obj = await _salesOrderDetailRepository.GetAsync(id);

            obj.ProductId = input.ProductId;
            obj.Quantity = input.Quantity;
            obj.DiscAmt = input.DiscAmt;

            var product = _productRepository.Where(x => x.Id.Equals(obj.ProductId)).FirstOrDefault();
            
            obj.Price = product.RetailPrice;
            obj.TaxRate = product.TaxRate;
            obj.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _salesOrderDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _salesOrderDetailRepository.DeleteAsync(id);
        }
    }
}
