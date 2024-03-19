using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.PurchaseOrders;
using Indo.Products;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.Companies;
using Indo.Vendors;

namespace Indo.PurchaseOrderDetails
{
    public class PurchaseOrderDetailAppService : IndoAppService, IPurchaseOrderDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        private readonly PurchaseOrderDetailManager _purchaseOrderDetailManager;
        private readonly IVendorRepository _vendorRepository;
        public PurchaseOrderDetailAppService(
            CompanyAppService companyAppService,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
            PurchaseOrderDetailManager purchaseOrderDetailManager,
            IPurchaseOrderRepository purchaseOrderRepository,
            IProductRepository productRepository,
            IVendorRepository vendorRepository,
            IUomRepository uomRepository)
        {
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
            _purchaseOrderDetailManager = purchaseOrderDetailManager;
            _purchaseOrderRepository = purchaseOrderRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _vendorRepository = vendorRepository;
        }
        public async Task<PurchaseOrderDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _purchaseOrderDetailRepository.GetQueryableAsync();
            var query = from purchaseOrderDetail in queryable
                        join purchaseOrder in _purchaseOrderRepository on purchaseOrderDetail.PurchaseOrderId equals purchaseOrder.Id
                        join product in _productRepository on purchaseOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where purchaseOrderDetail.Id == id
                        select new { purchaseOrderDetail, purchaseOrder, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(PurchaseOrderDetail), id);
            }
            var dto = ObjectMapper.Map<PurchaseOrderDetail, PurchaseOrderDetailReadDto>(queryResult.purchaseOrderDetail);
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.purchaseOrder.Status;
            dto.StatusString = L[$"Enum:PurchaseOrderStatus:{(int)queryResult.purchaseOrder.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<PurchaseOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _purchaseOrderDetailRepository.GetQueryableAsync();
            var query = from purchaseOrderDetail in queryable
                        join purchaseOrder in _purchaseOrderRepository on purchaseOrderDetail.PurchaseOrderId equals purchaseOrder.Id
                        join product in _productRepository on purchaseOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { purchaseOrderDetail, purchaseOrder, product, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseOrderDetail, PurchaseOrderDetailReadDto>(x.purchaseOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.purchaseOrder.Status;
                dto.StatusString = L[$"Enum:PurchaseOrderStatus:{(int)x.purchaseOrder.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _purchaseOrderDetailRepository.GetCountAsync();

            return new PagedResultDto<PurchaseOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<PurchaseOrderDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _purchaseOrderDetailRepository.GetQueryableAsync();
            var query = from purchaseOrderDetail in queryable
                        join purchaseOrder in _purchaseOrderRepository on purchaseOrderDetail.PurchaseOrderId equals purchaseOrder.Id
                        join vendor in _vendorRepository on purchaseOrder.VendorId equals vendor.Id
                        join product in _productRepository on purchaseOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { purchaseOrderDetail, purchaseOrder, vendor, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseOrderDetail, PurchaseOrderDetailReadDto>(x.purchaseOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.purchaseOrder.Status;
                dto.StatusString = L[$"Enum:PurchaseOrderStatus:{(int)x.purchaseOrder.Status}"];
                dto.PurchaseOrderNumber = x.purchaseOrder.Number;
                dto.VendorName = x.vendor.Name;
                dto.OrderDate = x.purchaseOrder.OrderDate;
                dto.PriceString = x.purchaseOrderDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.purchaseOrderDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.purchaseOrderDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.purchaseOrderDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.purchaseOrderDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.purchaseOrderDetail.Total.ToString("##,##.00");
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<PurchaseOrderDetailReadDto>> GetListByPurchaseOrderAsync(Guid purchaseOrderId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _purchaseOrderDetailRepository.GetQueryableAsync();
            var query = from purchaseOrderDetail in queryable
                        join purchaseOrder in _purchaseOrderRepository on purchaseOrderDetail.PurchaseOrderId equals purchaseOrder.Id
                        join product in _productRepository on purchaseOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where purchaseOrderDetail.PurchaseOrderId.Equals(purchaseOrderId)
                        select new { purchaseOrderDetail, purchaseOrder, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseOrderDetail, PurchaseOrderDetailReadDto>(x.purchaseOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.purchaseOrder.Status;
                dto.StatusString = L[$"Enum:PurchaseOrderStatus:{(int)x.purchaseOrder.Status}"];
                dto.PriceString = x.purchaseOrderDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.purchaseOrderDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.purchaseOrderDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.purchaseOrderDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.purchaseOrderDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.purchaseOrderDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<PurchaseOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<PurchaseOrderLookupDto>> GetPurchaseOrderLookupAsync()
        {
            var list = await _purchaseOrderRepository.GetListAsync();
            return new ListResultDto<PurchaseOrderLookupDto>(
                ObjectMapper.Map<List<PurchaseOrder>, List<PurchaseOrderLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
        {
            var list = await _productRepository.GetListAsync();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<PurchaseOrderDetailReadDto> CreateAsync(PurchaseOrderDetailCreateDto input)
        {
            var obj = await _purchaseOrderDetailManager.CreateAsync(
                input.PurchaseOrderId,
                input.ProductId,
                input.Quantity,
                input.DiscAmt
            );
            await _purchaseOrderDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<PurchaseOrderDetail, PurchaseOrderDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, PurchaseOrderDetailUpdateDto input)
        {
            var obj = await _purchaseOrderDetailRepository.GetAsync(id);

            obj.ProductId = input.ProductId;
            obj.Quantity = input.Quantity;
            obj.DiscAmt = input.DiscAmt;

            var product = _productRepository.Where(x => x.Id.Equals(obj.ProductId)).FirstOrDefault();
            
            obj.Price = product.Price;
            obj.TaxRate = product.TaxRate;
            obj.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _purchaseOrderDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _purchaseOrderDetailRepository.DeleteAsync(id);
        }
    }
}
