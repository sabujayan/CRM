using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.DeliveryOrderDetails;
using Indo.DeliveryOrders;
using Indo.GoodsReceipts;
using Indo.Products;
using Indo.TransferOrders;
using Indo.Uoms;
using Indo.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.GoodsReceiptDetails
{
    public class GoodsReceiptDetailAppService : IndoAppService, IGoodsReceiptDetailAppService
    {
        private readonly IGoodsReceiptRepository _goodsReceiptRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IGoodsReceiptDetailRepository _goodsReceiptDetailRepository;
        private readonly GoodsReceiptDetailManager _goodsReceiptDetailManager;
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly IDeliveryOrderDetailRepository _deliveryOrderDetailRepository;
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        public GoodsReceiptDetailAppService(
            IGoodsReceiptDetailRepository goodsReceiptDetailRepository,
            GoodsReceiptDetailManager goodsReceiptDetailManager,
            IGoodsReceiptRepository goodsReceiptRepository,
            IProductRepository productRepository,
            IDeliveryOrderRepository deliveryOrderRepository,
            IDeliveryOrderDetailRepository deliveryOrderDetailRepository,
            ITransferOrderRepository transferOrderRepository,
            IWarehouseRepository warehouseRepository,
            IUomRepository uomRepository)
        {
            _goodsReceiptDetailRepository = goodsReceiptDetailRepository;
            _goodsReceiptDetailManager = goodsReceiptDetailManager;
            _goodsReceiptRepository = goodsReceiptRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _deliveryOrderRepository = deliveryOrderRepository;
            _deliveryOrderDetailRepository = deliveryOrderDetailRepository;
            _transferOrderRepository = transferOrderRepository;
            _warehouseRepository = warehouseRepository;
        }
        public async Task<GoodsReceiptDetailReadDto> GetAsync(Guid id)
        {
            var queryable = await _goodsReceiptDetailRepository.GetQueryableAsync();
            var query = from goodsReceiptDetail in queryable
                        join goodsReceipt in _goodsReceiptRepository on goodsReceiptDetail.GoodsReceiptId equals goodsReceipt.Id
                        join product in _productRepository on goodsReceiptDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where goodsReceiptDetail.Id == id
                        select new { goodsReceiptDetail, goodsReceipt, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(GoodsReceiptDetail), id);
            }
            var dto = ObjectMapper.Map<GoodsReceiptDetail, GoodsReceiptDetailReadDto>(queryResult.goodsReceiptDetail);
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;
            dto.Status = queryResult.goodsReceipt.Status;
            dto.StatusString = L[$"Enum:GoodsReceiptStatus:{(int)queryResult.goodsReceipt.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<GoodsReceiptDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _goodsReceiptDetailRepository.GetQueryableAsync();
            var query = from goodsReceiptDetail in queryable
                        join goodsReceipt in _goodsReceiptRepository on goodsReceiptDetail.GoodsReceiptId equals goodsReceipt.Id
                        join product in _productRepository on goodsReceiptDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { goodsReceiptDetail, goodsReceipt, product, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<GoodsReceiptDetail, GoodsReceiptDetailReadDto>(x.goodsReceiptDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.goodsReceipt.Status;
                dto.StatusString = L[$"Enum:GoodsReceiptStatus:{(int)x.goodsReceipt.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _goodsReceiptDetailRepository.GetCountAsync();

            return new PagedResultDto<GoodsReceiptDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<GoodsReceiptDetailReadDto>> GetListDetailAsync()
        {
            var queryable = await _goodsReceiptDetailRepository.GetQueryableAsync();
            var query = from goodsReceiptDetail in queryable
                        join goodsReceipt in _goodsReceiptRepository on goodsReceiptDetail.GoodsReceiptId equals goodsReceipt.Id
                        join deliveryOrder in _deliveryOrderRepository on goodsReceipt.DeliveryOrderId equals deliveryOrder.Id
                        join transferOrder in _transferOrderRepository on deliveryOrder.TransferOrderId equals transferOrder.Id
                        join fromWarehouse in _warehouseRepository on transferOrder.FromWarehouseId equals fromWarehouse.Id
                        join toWarehouse in _warehouseRepository on transferOrder.ToWarehouseId equals toWarehouse.Id
                        join product in _productRepository on goodsReceiptDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { goodsReceiptDetail, goodsReceipt, deliveryOrder, transferOrder, fromWarehouse, toWarehouse, product, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<GoodsReceiptDetail, GoodsReceiptDetailReadDto>(x.goodsReceiptDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.goodsReceipt.Status;
                dto.StatusString = L[$"Enum:GoodsReceiptStatus:{(int)x.goodsReceipt.Status}"];
                dto.ReceiptOrderNumber = x.goodsReceipt.Number;
                dto.OrderDate = x.goodsReceipt.OrderDate;
                dto.DeliveryOrderNumber = x.deliveryOrder.Number;
                dto.TransferOrderNumber = x.transferOrder.Number;
                dto.FromWarehouse = x.fromWarehouse.Name;
                dto.ToWarehouse = x.toWarehouse.Name;
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<GoodsReceiptDetailReadDto>> GetListByGoodsReceiptAsync(Guid goodsReceiptId)
        {
            var queryable = await _goodsReceiptDetailRepository.GetQueryableAsync();
            var query = from goodsReceiptDetail in queryable
                        join goodsReceipt in _goodsReceiptRepository on goodsReceiptDetail.GoodsReceiptId equals goodsReceipt.Id
                        join product in _productRepository on goodsReceiptDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where goodsReceiptDetail.GoodsReceiptId.Equals(goodsReceiptId)
                        select new { goodsReceiptDetail, goodsReceipt, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<GoodsReceiptDetail, GoodsReceiptDetailReadDto>(x.goodsReceiptDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.goodsReceipt.Status;
                dto.StatusString = L[$"Enum:GoodsReceiptStatus:{(int)x.goodsReceipt.Status}"];
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<GoodsReceiptDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<GoodsReceiptLookupDto>> GetGoodsReceiptLookupAsync()
        {
            var list = await _goodsReceiptRepository.GetListAsync();
            return new ListResultDto<GoodsReceiptLookupDto>(
                ObjectMapper.Map<List<GoodsReceipt>, List<GoodsReceiptLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
        {
            var list = await _productRepository.GetListAsync();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductByGoodsReceiptLookupAsync(Guid id)
        {
            var queryable = await _deliveryOrderDetailRepository.GetQueryableAsync();
            var query = from deliveryOrderDetail in queryable
                        join product in _productRepository on deliveryOrderDetail.ProductId equals product.Id
                        join deliveryOrder in _deliveryOrderRepository on deliveryOrderDetail.DeliveryOrderId equals deliveryOrder.Id
                        join goodsReceipt in _goodsReceiptRepository on  deliveryOrder.Id equals goodsReceipt.DeliveryOrderId
                        where goodsReceipt.Id.Equals(id)
                        select new { product };

            var queryResult = await AsyncExecuter.ToListAsync(query);

            var list = queryResult.Select(x => x.product).ToList();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<GoodsReceiptDetailReadDto> CreateAsync(GoodsReceiptDetailCreateDto input)
        {
            var obj = await _goodsReceiptDetailManager.CreateAsync(
                input.GoodsReceiptId,
                input.ProductId,
                input.Quantity
            );
            await _goodsReceiptDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<GoodsReceiptDetail, GoodsReceiptDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, GoodsReceiptDetailUpdateDto input)
        {
            var obj = await _goodsReceiptDetailRepository.GetAsync(id);

            obj.ProductId = input.ProductId;
            obj.Quantity = input.Quantity;

            var product = _productRepository.Where(x => x.Id.Equals(obj.ProductId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _goodsReceiptDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _goodsReceiptDetailRepository.DeleteAsync(id);
        }
    }
}
