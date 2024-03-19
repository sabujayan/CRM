using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.DeliveryOrders;
using Indo.Products;
using Indo.TransferOrderDetails;
using Indo.TransferOrders;
using Indo.Uoms;
using Indo.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.DeliveryOrderDetails
{
    public class DeliveryOrderDetailAppService : IndoAppService, IDeliveryOrderDetailAppService
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IDeliveryOrderDetailRepository _deliveryOrderDetailRepository;
        private readonly DeliveryOrderDetailManager _deliveryOrderDetailManager;
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly ITransferOrderDetailRepository _transferOrderDetailRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        public DeliveryOrderDetailAppService(
            IDeliveryOrderDetailRepository deliveryOrderDetailRepository,
            DeliveryOrderDetailManager deliveryOrderDetailManager,
            IDeliveryOrderRepository deliveryOrderRepository,
            IProductRepository productRepository,
            ITransferOrderRepository transferOrderRepository,
            ITransferOrderDetailRepository transferOrderDetailRepository,
            IWarehouseRepository warehouseRepository,
            IUomRepository uomRepository)
        {
            _deliveryOrderDetailRepository = deliveryOrderDetailRepository;
            _deliveryOrderDetailManager = deliveryOrderDetailManager;
            _deliveryOrderRepository = deliveryOrderRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _transferOrderRepository = transferOrderRepository;
            _transferOrderDetailRepository = transferOrderDetailRepository;
            _warehouseRepository = warehouseRepository;
        }
        public async Task<DeliveryOrderDetailReadDto> GetAsync(Guid id)
        {
            var queryable = await _deliveryOrderDetailRepository.GetQueryableAsync();
            var query = from deliveryOrderDetail in queryable
                        join deliveryOrder in _deliveryOrderRepository on deliveryOrderDetail.DeliveryOrderId equals deliveryOrder.Id
                        join product in _productRepository on deliveryOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where deliveryOrderDetail.Id == id
                        select new { deliveryOrderDetail, deliveryOrder, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(DeliveryOrderDetail), id);
            }
            var dto = ObjectMapper.Map<DeliveryOrderDetail, DeliveryOrderDetailReadDto>(queryResult.deliveryOrderDetail);
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;
            dto.Status = queryResult.deliveryOrder.Status;
            dto.StatusString = L[$"Enum:DeliveryOrderStatus:{(int)queryResult.deliveryOrder.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<DeliveryOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _deliveryOrderDetailRepository.GetQueryableAsync();
            var query = from deliveryOrderDetail in queryable
                        join deliveryOrder in _deliveryOrderRepository on deliveryOrderDetail.DeliveryOrderId equals deliveryOrder.Id
                        join product in _productRepository on deliveryOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { deliveryOrderDetail, deliveryOrder, product, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<DeliveryOrderDetail, DeliveryOrderDetailReadDto>(x.deliveryOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.deliveryOrder.Status;
                dto.StatusString = L[$"Enum:DeliveryOrderStatus:{(int)x.deliveryOrder.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _deliveryOrderDetailRepository.GetCountAsync();

            return new PagedResultDto<DeliveryOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<DeliveryOrderDetailReadDto>> GetListDetailAsync()
        {
            var queryable = await _deliveryOrderDetailRepository.GetQueryableAsync();
            var query = from deliveryOrderDetail in queryable
                        join deliveryOrder in _deliveryOrderRepository on deliveryOrderDetail.DeliveryOrderId equals deliveryOrder.Id
                        join transferOrder in _transferOrderRepository on deliveryOrder.TransferOrderId equals transferOrder.Id
                        join fromWarehouse in _warehouseRepository on transferOrder.FromWarehouseId equals fromWarehouse.Id
                        join toWarehouse in _warehouseRepository on transferOrder.ToWarehouseId equals toWarehouse.Id
                        join product in _productRepository on deliveryOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { deliveryOrderDetail, deliveryOrder, transferOrder, fromWarehouse, toWarehouse, product, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<DeliveryOrderDetail, DeliveryOrderDetailReadDto>(x.deliveryOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.deliveryOrder.Status;
                dto.StatusString = L[$"Enum:DeliveryOrderStatus:{(int)x.deliveryOrder.Status}"];
                dto.DeliveryOrderNumber = x.deliveryOrder.Number;
                dto.OrderDate = x.deliveryOrder.OrderDate;
                dto.TransferOrderNumber = x.transferOrder.Number;
                dto.FromWarehouse = x.fromWarehouse.Name;
                dto.ToWarehouse = x.toWarehouse.Name;
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<DeliveryOrderDetailReadDto>> GetListByDeliveryOrderAsync(Guid deliveryOrderId)
        {
            var queryable = await _deliveryOrderDetailRepository.GetQueryableAsync();
            var query = from deliveryOrderDetail in queryable
                        join deliveryOrder in _deliveryOrderRepository on deliveryOrderDetail.DeliveryOrderId equals deliveryOrder.Id
                        join product in _productRepository on deliveryOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where deliveryOrderDetail.DeliveryOrderId.Equals(deliveryOrderId)
                        select new { deliveryOrderDetail, deliveryOrder, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<DeliveryOrderDetail, DeliveryOrderDetailReadDto>(x.deliveryOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.deliveryOrder.Status;
                dto.StatusString = L[$"Enum:DeliveryOrderStatus:{(int)x.deliveryOrder.Status}"];
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<DeliveryOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<DeliveryOrderLookupDto>> GetDeliveryOrderLookupAsync()
        {
            var list = await _deliveryOrderRepository.GetListAsync();
            return new ListResultDto<DeliveryOrderLookupDto>(
                ObjectMapper.Map<List<DeliveryOrder>, List<DeliveryOrderLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
        {
            var list = await _productRepository.GetListAsync();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductByDeliveryOrderLookupAsync(Guid id)
        {
            var queryable = await _transferOrderDetailRepository.GetQueryableAsync();
            var query = from transferOrderDetail in queryable
                        join product in _productRepository on transferOrderDetail.ProductId equals product.Id
                        join transferOrder in _transferOrderRepository on transferOrderDetail.TransferOrderId equals transferOrder.Id
                        join deliveryOrder in _deliveryOrderRepository on transferOrder.Id equals deliveryOrder.TransferOrderId
                        where deliveryOrder.Id.Equals(id)
                        select new { product };

            var queryResult = await AsyncExecuter.ToListAsync(query);

            var list = queryResult.Select(x => x.product).ToList();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<DeliveryOrderDetailReadDto> CreateAsync(DeliveryOrderDetailCreateDto input)
        {
            var obj = await _deliveryOrderDetailManager.CreateAsync(
                input.DeliveryOrderId,
                input.ProductId,
                input.Quantity
            );
            await _deliveryOrderDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<DeliveryOrderDetail, DeliveryOrderDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, DeliveryOrderDetailUpdateDto input)
        {
            var obj = await _deliveryOrderDetailRepository.GetAsync(id);

            obj.ProductId = input.ProductId;
            obj.Quantity = input.Quantity;

            var product = _productRepository.Where(x => x.Id.Equals(obj.ProductId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _deliveryOrderDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _deliveryOrderDetailRepository.DeleteAsync(id);
        }
    }
}
