using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.DeliveryOrders;
using Indo.TransferOrders;
using Indo.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.GoodsReceipts
{
    public class GoodsReceiptAppService : IndoAppService, IGoodsReceiptAppService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly IGoodsReceiptRepository _goodsReceiptRepository;
        private readonly GoodsReceiptManager _goodsReceiptManager;
        public GoodsReceiptAppService(
            IGoodsReceiptRepository goodsReceiptRepository,
            GoodsReceiptManager goodsReceiptManager,
            IWarehouseRepository warehouseRepository,
            IDeliveryOrderRepository deliveryOrderRepository)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
            _goodsReceiptManager = goodsReceiptManager;
            _warehouseRepository = warehouseRepository;
            _deliveryOrderRepository = deliveryOrderRepository;
        }
        public async Task<GoodsReceiptReadDto> GetAsync(Guid id)
        {
            var queryable = await _goodsReceiptRepository.GetQueryableAsync();
            var query = from goodsReceipt in queryable
                        join deliveryOrder in _deliveryOrderRepository on goodsReceipt.DeliveryOrderId equals deliveryOrder.Id
                        join warehouseFrom in _warehouseRepository on goodsReceipt.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on goodsReceipt.ToWarehouseId equals warehouseTo.Id
                        where goodsReceipt.Id == id
                        select new { goodsReceipt, deliveryOrder, warehouseFrom, warehouseTo };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(GoodsReceipt), id);
            }
            var dto = ObjectMapper.Map<GoodsReceipt, GoodsReceiptReadDto>(queryResult.goodsReceipt);
            dto.FromWarehouseName = queryResult.warehouseFrom.Name;
            dto.ToWarehouseName = queryResult.warehouseTo.Name;
            dto.DeliveryOrderNumber = queryResult.deliveryOrder.Number;
            dto.StatusString = L[$"Enum:GoodsReceiptStatus:{(int)queryResult.goodsReceipt.Status}"];

            return dto;
        }
        public async Task<List<GoodsReceiptReadDto>> GetListAsync()
        {
            var queryable = await _goodsReceiptRepository.GetQueryableAsync();
            var query = from goodsReceipt in queryable
                        join deliveryOrder in _deliveryOrderRepository on goodsReceipt.DeliveryOrderId equals deliveryOrder.Id
                        join warehouseFrom in _warehouseRepository on goodsReceipt.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on goodsReceipt.ToWarehouseId equals warehouseTo.Id
                        select new { goodsReceipt, deliveryOrder, warehouseFrom, warehouseTo };
         
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<GoodsReceipt, GoodsReceiptReadDto>(x.goodsReceipt);
                dto.FromWarehouseName = x.warehouseFrom.Name;
                dto.ToWarehouseName = x.warehouseTo.Name;
                dto.DeliveryOrderNumber = x.deliveryOrder.Number;
                dto.StatusString = L[$"Enum:GoodsReceiptStatus:{(int)x.goodsReceipt.Status}"];
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();
            return dtos;
        }
        public async Task<List<GoodsReceiptReadDto>> GetListByTransferOrderAsync(Guid transferOrderId)
        {
            var queryable = await _goodsReceiptRepository.GetQueryableAsync();
            var query = from goodsReceipt in queryable
                        join deliveryOrder in _deliveryOrderRepository on goodsReceipt.DeliveryOrderId equals deliveryOrder.Id
                        join warehouseFrom in _warehouseRepository on goodsReceipt.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on goodsReceipt.ToWarehouseId equals warehouseTo.Id
                        where deliveryOrder.TransferOrderId.Equals(transferOrderId)
                        select new { goodsReceipt, deliveryOrder, warehouseFrom, warehouseTo };
         
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<GoodsReceipt, GoodsReceiptReadDto>(x.goodsReceipt);
                dto.FromWarehouseName = x.warehouseFrom.Name;
                dto.ToWarehouseName = x.warehouseTo.Name;
                dto.DeliveryOrderNumber = x.deliveryOrder.Number;
                dto.StatusString = L[$"Enum:GoodsReceiptStatus:{(int)x.goodsReceipt.Status}"];
                return dto;
            }).ToList();

            return dtos;

        }
        public async Task<ListResultDto<DeliveryOrderLookupDto>> GetDeliveryOrderLookupAsync()
        {
            var list = await _deliveryOrderRepository.ToListAsync();
            return new ListResultDto<DeliveryOrderLookupDto>(
                ObjectMapper.Map<List<DeliveryOrder>, List<DeliveryOrderLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<WarehouseLookupDto>> GetWarehouseLookupAsync()
        {
            await Task.Yield();

            var list = _warehouseRepository.Where(x => x.Virtual.Equals(false)).ToList();
            return new ListResultDto<WarehouseLookupDto>(
                ObjectMapper.Map<List<Warehouse>, List<WarehouseLookupDto>>(list)
            );
        }
        public async Task<GoodsReceiptReadDto> CreateAsync(GoodsReceiptCreateDto input)
        {
            var obj = await _goodsReceiptManager.CreateAsync(
                input.Number,
                input.DeliveryOrderId,
                input.FromWarehouseId,
                input.ToWarehouseId,
                input.OrderDate
            );

            obj.Description = input.Description;
            obj.Status = input.Status;

            await _goodsReceiptRepository.InsertAsync(obj);

            return ObjectMapper.Map<GoodsReceipt, GoodsReceiptReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, GoodsReceiptUpdateDto input)
        {
            var obj = await _goodsReceiptRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _goodsReceiptManager.ChangeNameAsync(obj, input.Number);
            }

            obj.DeliveryOrderId = input.DeliveryOrderId;
            obj.FromWarehouseId = input.FromWarehouseId;
            obj.ToWarehouseId = input.ToWarehouseId;
            obj.Description = input.Description;
            obj.Status = input.Status;
            obj.OrderDate = input.OrderDate;

            await _goodsReceiptRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _goodsReceiptRepository.DeleteAsync(id);
        }
    }
}
