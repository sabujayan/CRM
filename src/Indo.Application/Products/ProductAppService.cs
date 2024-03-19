using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.PurchaseOrderDetails;
using Indo.PurchaseOrders;
using Indo.SalesOrderDetails;
using Indo.SalesOrders;
using Indo.ServiceOrderDetails;
using Indo.TransferOrderDetails;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.Products
{
    public class ProductAppService : IndoAppService, IProductAppService
    {
        private readonly IUomRepository _uomRepository;
        private readonly CompanyAppService _companyAppService;
        private readonly IProductRepository _productRepository;
        private readonly ProductManager _productManager;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly ITransferOrderDetailRepository _transferOrderDetailRepository;
        public ProductAppService(
            IProductRepository productRepository,
            ProductManager productManager,
            IUomRepository uomRepository,
            CompanyAppService companyAppService,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
            ISalesOrderDetailRepository salesOrderDetailRepository,
            ITransferOrderDetailRepository transferOrderDetailRepository
            )
        {
            _productRepository = productRepository;
            _productManager = productManager;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _transferOrderDetailRepository = transferOrderDetailRepository;
        }
        public async Task<ProductReadDto> GetAsync(Guid id)
        {
            var queryable = await _productRepository.GetQueryableAsync();
            var query = from product in queryable
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where product.Id == id
                        select new { product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Product), id);
            }
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var dto = ObjectMapper.Map<Product, ProductReadDto>(queryResult.product);
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            return dto;
        }
        public async Task<List<ProductReadDto>> GetListAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _productRepository.GetQueryableAsync();
            var query = from product in queryable
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { product, uom };          
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Product, ProductReadDto>(x.product);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                return dto;
            }).ToList();

            return dtos;
        }
        public async Task<ListResultDto<UomLookupDto>> GetUomLookupAsync()
        {
            var list = await _uomRepository.GetListAsync();
            return new ListResultDto<UomLookupDto>(
                ObjectMapper.Map<List<Uom>, List<UomLookupDto>>(list)
            );
        }
        public async Task<ProductReadDto> CreateAsync(ProductCreateDto input)
        {
            var obj = await _productManager.CreateAsync(
                input.Name,
                input.UomId
            );

            obj.Price = input.Price;
            obj.TaxRate = input.TaxRate;
            obj.RetailPrice = input.RetailPrice;

            await _productRepository.InsertAsync(obj);

            return ObjectMapper.Map<Product, ProductReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ProductUpdateDto input)
        {
            var obj = await _productRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _productManager.ChangeNameAsync(obj, input.Name);
            }

            obj.UomId = input.UomId;
            obj.Price = input.Price;
            obj.TaxRate = input.TaxRate;
            obj.RetailPrice = input.RetailPrice;

            await _productRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_transferOrderDetailRepository.Where(x => x.ProductId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_salesOrderDetailRepository.Where(x => x.ProductId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_purchaseOrderDetailRepository.Where(x => x.ProductId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _productRepository.DeleteAsync(id);
        }
    }
}
