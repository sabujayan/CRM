using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.SalesDeliveries;
using Indo.Products;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.Companies;
using Indo.SalesOrders;
using Indo.Customers;
using Indo.SalesOrderDetails;

namespace Indo.SalesDeliveryDetails
{
    public class SalesDeliveryDetailAppService : IndoAppService, ISalesDeliveryDetailAppService
    {
        private readonly ISalesDeliveryRepository _salesDeliveryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly ISalesDeliveryDetailRepository _salesDeliveryDetailRepository;
        private readonly SalesDeliveryDetailManager _salesDeliveryDetailManager;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly ICustomerRepository _customerRepository;
        public SalesDeliveryDetailAppService(
            ISalesDeliveryDetailRepository salesDeliveryDetailRepository,
            SalesDeliveryDetailManager salesDeliveryDetailManager,
            ISalesDeliveryRepository salesDeliveryRepository,
            IProductRepository productRepository,
            ISalesOrderRepository salesOrderRepository,
            ISalesOrderDetailRepository salesOrderDetailRepository,
            ICustomerRepository customerRepository,
            IUomRepository uomRepository)
        {
            _salesDeliveryDetailRepository = salesDeliveryDetailRepository;
            _salesDeliveryDetailManager = salesDeliveryDetailManager;
            _salesDeliveryRepository = salesDeliveryRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _salesOrderRepository = salesOrderRepository;
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _customerRepository = customerRepository;
        }
        public async Task<SalesDeliveryDetailReadDto> GetAsync(Guid id)
        {
            var queryable = await _salesDeliveryDetailRepository.GetQueryableAsync();
            var query = from salesDeliveryDetail in queryable
                        join salesDelivery in _salesDeliveryRepository on salesDeliveryDetail.SalesDeliveryId equals salesDelivery.Id
                        join product in _productRepository on salesDeliveryDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where salesDeliveryDetail.Id == id
                        select new { salesDeliveryDetail, salesDelivery, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(SalesDeliveryDetail), id);
            }
            var dto = ObjectMapper.Map<SalesDeliveryDetail, SalesDeliveryDetailReadDto>(queryResult.salesDeliveryDetail);
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;
            dto.Status = queryResult.salesDelivery.Status;
            dto.StatusString = L[$"Enum:SalesDeliveryStatus:{(int)queryResult.salesDelivery.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<SalesDeliveryDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _salesDeliveryDetailRepository.GetQueryableAsync();
            var query = from salesDeliveryDetail in queryable
                        join salesDelivery in _salesDeliveryRepository on salesDeliveryDetail.SalesDeliveryId equals salesDelivery.Id
                        join product in _productRepository on salesDeliveryDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesDeliveryDetail, salesDelivery, product, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesDeliveryDetail, SalesDeliveryDetailReadDto>(x.salesDeliveryDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.salesDelivery.Status;
                dto.StatusString = L[$"Enum:SalesDeliveryStatus:{(int)x.salesDelivery.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _salesDeliveryDetailRepository.GetCountAsync();

            return new PagedResultDto<SalesDeliveryDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<SalesDeliveryDetailReadDto>> GetListDetailAsync()
        {
            var queryable = await _salesDeliveryDetailRepository.GetQueryableAsync();
            var query = from salesDeliveryDetail in queryable
                        join salesDelivery in _salesDeliveryRepository on salesDeliveryDetail.SalesDeliveryId equals salesDelivery.Id
                        join salesOrder in _salesOrderRepository on salesDelivery.SalesOrderId equals salesOrder.Id
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        join product in _productRepository on salesDeliveryDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { salesDeliveryDetail, salesDelivery, salesOrder, customer, product, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesDeliveryDetail, SalesDeliveryDetailReadDto>(x.salesDeliveryDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.salesDelivery.Status;
                dto.StatusString = L[$"Enum:SalesDeliveryStatus:{(int)x.salesDelivery.Status}"];
                dto.SalesDeliveryNumber = x.salesDelivery.Number;
                dto.DeliveryDate = x.salesDelivery.DeliveryDate;
                dto.SalesOrderNumber = x.salesOrder.Number;
                dto.CustomerName = x.customer.Name;
                return dto;
            })
                .OrderByDescending(x => x.DeliveryDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<SalesDeliveryDetailReadDto>> GetListBySalesDeliveryAsync(Guid salesDeliveryId)
        {
            var queryable = await _salesDeliveryDetailRepository.GetQueryableAsync();
            var query = from salesDeliveryDetail in queryable
                        join salesDelivery in _salesDeliveryRepository on salesDeliveryDetail.SalesDeliveryId equals salesDelivery.Id
                        join product in _productRepository on salesDeliveryDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where salesDeliveryDetail.SalesDeliveryId.Equals(salesDeliveryId)
                        select new { salesDeliveryDetail, salesDelivery, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesDeliveryDetail, SalesDeliveryDetailReadDto>(x.salesDeliveryDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.salesDelivery.Status;
                dto.StatusString = L[$"Enum:SalesDeliveryStatus:{(int)x.salesDelivery.Status}"];
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<SalesDeliveryDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<SalesDeliveryLookupDto>> GetSalesDeliveryLookupAsync()
        {
            var list = await _salesDeliveryRepository.GetListAsync();
            return new ListResultDto<SalesDeliveryLookupDto>(
                ObjectMapper.Map<List<SalesDelivery>, List<SalesDeliveryLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
        {
            var list = await _productRepository.GetListAsync();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductBySalesDeliveryLookupAsync(Guid id)
        {
            var queryable = await _salesOrderDetailRepository.GetQueryableAsync();
            var query = from salesOrderDetail in queryable
                        join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                        join salesOrder in _salesOrderRepository on salesOrderDetail.SalesOrderId equals salesOrder.Id
                        join salesDelivery in _salesDeliveryRepository on salesOrder.Id equals salesDelivery.SalesOrderId
                        where salesDelivery.Id.Equals(id)
                        select new { product };

            var queryResult = await AsyncExecuter.ToListAsync(query);

            var list = queryResult.Select(x => x.product).ToList();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<SalesDeliveryDetailReadDto> CreateAsync(SalesDeliveryDetailCreateDto input)
        {
            var obj = await _salesDeliveryDetailManager.CreateAsync(
                input.SalesDeliveryId,
                input.ProductId,
                input.Quantity
            );
            await _salesDeliveryDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<SalesDeliveryDetail, SalesDeliveryDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, SalesDeliveryDetailUpdateDto input)
        {
            var obj = await _salesDeliveryDetailRepository.GetAsync(id);

            obj.ProductId = input.ProductId;
            obj.Quantity = input.Quantity;

            var product = _productRepository.Where(x => x.Id.Equals(obj.ProductId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _salesDeliveryDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _salesDeliveryDetailRepository.DeleteAsync(id);
        }
    }
}
