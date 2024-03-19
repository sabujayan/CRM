using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.DeliveryOrders;
using Indo.GoodsReceipts;
using Indo.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.TransferOrders
{
    public class TransferOrderAppService : IndoAppService, ITransferOrderAppService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly ITransferOrderRepository _transferOrderReturnRepository;
        private readonly ITransferOrderRepository _transferOrderOriginRepository;
        private readonly TransferOrderManager _transferOrderManager;
        private readonly DeliveryOrderManager _deliveryOrderManager;
        private readonly GoodsReceiptManager _goodsReceiptManager;
        public TransferOrderAppService(
            ITransferOrderRepository transferOrderRepository,
            ITransferOrderRepository transferOrderReturnRepository,
            ITransferOrderRepository transferOrderOriginRepository,
            TransferOrderManager transferOrderManager,
            DeliveryOrderManager deliveryOrderManager,
            GoodsReceiptManager goodsReceiptManager,
            IWarehouseRepository warehouseRepository)
        {
            _transferOrderRepository = transferOrderRepository;
            _transferOrderReturnRepository = transferOrderReturnRepository;
            _transferOrderOriginRepository = transferOrderOriginRepository;
            _transferOrderManager = transferOrderManager;
            _warehouseRepository = warehouseRepository;
            _deliveryOrderManager = deliveryOrderManager;
            _goodsReceiptManager = goodsReceiptManager;
        }
        public async Task<TransferOrderReadDto> GetAsync(Guid id)
        {
            var queryable = await _transferOrderRepository.GetQueryableAsync();
            var query = from transferOrder in queryable
                        join warehouseFrom in _warehouseRepository on transferOrder.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on transferOrder.ToWarehouseId equals warehouseTo.Id
                        where transferOrder.Id == id
                        select new { transferOrder, warehouseFrom, warehouseTo };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(TransferOrder), id);
            }
            var dto = ObjectMapper.Map<TransferOrder, TransferOrderReadDto>(queryResult.transferOrder);
            dto.FromWarehouseName = queryResult.warehouseFrom.Name;
            dto.ToWarehouseName = queryResult.warehouseTo.Name;
            dto.StatusString = L[$"Enum:TransferOrderStatus:{(int)queryResult.transferOrder.Status}"];

            return dto;
        }
        public async Task<List<TransferOrderReadDto>> GetListAsync()
        {
            var queryable = await _transferOrderRepository.GetQueryableAsync();
            var query = from transferOrder in queryable
                        join warehouseFrom in _warehouseRepository on transferOrder.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on transferOrder.ToWarehouseId equals warehouseTo.Id
                        join @return in _transferOrderReturnRepository on transferOrder.ReturnId equals @return.Id into subreturn from @return in subreturn.DefaultIfEmpty()
                        join origin in _transferOrderOriginRepository on transferOrder.OriginalId equals origin.Id into suborigin from origin in suborigin.DefaultIfEmpty()
                        select new { transferOrder, warehouseFrom, warehouseTo, @return, origin };
         
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<TransferOrder, TransferOrderReadDto>(x.transferOrder);
                dto.FromWarehouseName = x.warehouseFrom.Name;
                dto.ToWarehouseName = x.warehouseTo.Name;
                dto.OriginalNumber = x.origin?.Number ?? string.Empty;
                dto.ReturnNumber = x.@return?.Number ?? string.Empty;
                dto.StatusString = L[$"Enum:TransferOrderStatus:{(int)x.transferOrder.Status}"];
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<ListResultDto<WarehouseLookupDto>> GetWarehouseLookupAsync()
        {
            await Task.Yield();

            var list = _warehouseRepository.Where(x => x.Virtual.Equals(false)).ToList();
            return new ListResultDto<WarehouseLookupDto>(
                ObjectMapper.Map<List<Warehouse>, List<WarehouseLookupDto>>(list)
            );
        }
        public async Task<TransferOrderReadDto> CreateAsync(TransferOrderCreateDto input)
        {
            var obj = await _transferOrderManager.CreateAsync(
                input.Number,
                input.FromWarehouseId,
                input.ToWarehouseId,
                input.OrderDate
            );

            obj.Description = input.Description;

            await _transferOrderRepository.InsertAsync(obj);

            return ObjectMapper.Map<TransferOrder, TransferOrderReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, TransferOrderUpdateDto input)
        {
            var obj = await _transferOrderRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _transferOrderManager.ChangeNameAsync(obj, input.Number);
            }

            obj.FromWarehouseId = input.FromWarehouseId;
            obj.ToWarehouseId = input.ToWarehouseId;
            obj.Description = input.Description;
            obj.OrderDate = input.OrderDate;

            await _transferOrderRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _transferOrderRepository.DeleteAsync(id);
        }
        public async Task<TransferOrderReadDto> ConfirmAsync(Guid transferOrderId)
        {
            var obj = await _transferOrderManager.ConfirmAsync(transferOrderId);
            var delivery = await _deliveryOrderManager.GenerateDeliveryOrderFromTransferAsync(obj.Id);
            await _deliveryOrderManager.ConfirmDeliveryOrderAsync(delivery.Id);
            var receipt = await _goodsReceiptManager.GenerateGoodsReceiptFromDeliveryAsync(delivery.Id);
            await _goodsReceiptManager.ConfirmGoodsReceiptAsync(receipt.Id);
            return ObjectMapper.Map<TransferOrder, TransferOrderReadDto>(obj);
        }
        public async Task<TransferOrderReadDto> ReturnAsync(Guid transferOrderId)
        {
            var returned = await _transferOrderManager.GenerateReturn(transferOrderId);
            var obj = await _transferOrderManager.ConfirmAsync(returned.Id);
            var delivery = await _deliveryOrderManager.GenerateDeliveryOrderFromTransferAsync(obj.Id);
            await _deliveryOrderManager.ConfirmDeliveryOrderAsync(delivery.Id);
            var receipt = await _goodsReceiptManager.GenerateGoodsReceiptFromDeliveryAsync(delivery.Id);
            await _goodsReceiptManager.ConfirmGoodsReceiptAsync(receipt.Id);
            var origin = await _transferOrderRepository.GetAsync(transferOrderId);
            origin.Status = TransferOrderStatus.Returned;
            origin.ReturnId = returned.Id;
            await _transferOrderRepository.UpdateAsync(origin);
            return ObjectMapper.Map<TransferOrder, TransferOrderReadDto>(obj);
        }
    }
}
