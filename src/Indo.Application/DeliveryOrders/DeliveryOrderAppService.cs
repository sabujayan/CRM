using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.TransferOrders;
using Indo.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.DeliveryOrders
{
    public class DeliveryOrderAppService : IndoAppService, IDeliveryOrderAppService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly DeliveryOrderManager _deliveryOrderManager;
        public DeliveryOrderAppService(
            IDeliveryOrderRepository deliveryOrderRepository,
            DeliveryOrderManager deliveryOrderManager,
            IWarehouseRepository warehouseRepository,
            ITransferOrderRepository transferOrderRepository)
        {
            _deliveryOrderRepository = deliveryOrderRepository;
            _deliveryOrderManager = deliveryOrderManager;
            _warehouseRepository = warehouseRepository;
            _transferOrderRepository = transferOrderRepository;
        }
        public async Task<DeliveryOrderReadDto> GetAsync(Guid id)
        {
            var queryable = await _deliveryOrderRepository.GetQueryableAsync();
            var query = from deliveryOrder in queryable
                        join transferOrder in _transferOrderRepository on deliveryOrder.TransferOrderId equals transferOrder.Id
                        join warehouseFrom in _warehouseRepository on deliveryOrder.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on deliveryOrder.ToWarehouseId equals warehouseTo.Id
                        where deliveryOrder.Id == id
                        select new { deliveryOrder, transferOrder, warehouseFrom, warehouseTo };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(DeliveryOrder), id);
            }
            var dto = ObjectMapper.Map<DeliveryOrder, DeliveryOrderReadDto>(queryResult.deliveryOrder);
            dto.FromWarehouseName = queryResult.warehouseFrom.Name;
            dto.ToWarehouseName = queryResult.warehouseTo.Name;
            dto.TransferOrderNumber = queryResult.transferOrder.Number;
            dto.StatusString = L[$"Enum:DeliveryOrderStatus:{(int)queryResult.deliveryOrder.Status}"];

            return dto;
        }
        public async Task<List<DeliveryOrderReadDto>> GetListAsync()
        {
            var queryable = await _deliveryOrderRepository.GetQueryableAsync();
            var query = from deliveryOrder in queryable
                        join transferOrder in _transferOrderRepository on deliveryOrder.TransferOrderId equals transferOrder.Id
                        join warehouseFrom in _warehouseRepository on deliveryOrder.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on deliveryOrder.ToWarehouseId equals warehouseTo.Id
                        select new { deliveryOrder, transferOrder, warehouseFrom, warehouseTo };
          
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<DeliveryOrder, DeliveryOrderReadDto>(x.deliveryOrder);
                dto.FromWarehouseName = x.warehouseFrom.Name;
                dto.ToWarehouseName = x.warehouseTo.Name;
                dto.TransferOrderNumber = x.transferOrder.Number;
                dto.StatusString = L[$"Enum:DeliveryOrderStatus:{(int)x.deliveryOrder.Status}"];
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();
            return dtos;
        }
        public async Task<List<DeliveryOrderReadDto>> GetListByTransferOrderAsync(Guid transferOrderId)
        {
            var queryable = await _deliveryOrderRepository.GetQueryableAsync();
            var query = from deliveryOrder in queryable
                        join transferOrder in _transferOrderRepository on deliveryOrder.TransferOrderId equals transferOrder.Id
                        join warehouseFrom in _warehouseRepository on deliveryOrder.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on deliveryOrder.ToWarehouseId equals warehouseTo.Id
                        where deliveryOrder.TransferOrderId.Equals(transferOrderId)
                        select new { deliveryOrder, transferOrder, warehouseFrom, warehouseTo };
           
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<DeliveryOrder, DeliveryOrderReadDto>(x.deliveryOrder);
                dto.FromWarehouseName = x.warehouseFrom.Name;
                dto.ToWarehouseName = x.warehouseTo.Name;
                dto.TransferOrderNumber = x.transferOrder.Number;
                dto.StatusString = L[$"Enum:DeliveryOrderStatus:{(int)x.deliveryOrder.Status}"];
                return dto;
            }).ToList();

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
        public async Task<ListResultDto<TransferOrderLookupDto>> GetTransferOrderLookupAsync()
        {
            var list = await _transferOrderRepository.ToListAsync();
            return new ListResultDto<TransferOrderLookupDto>(
                ObjectMapper.Map<List<TransferOrder>, List<TransferOrderLookupDto>>(list)
            );
        }
        public async Task<DeliveryOrderReadDto> CreateAsync(DeliveryOrderCreateDto input)
        {
            var obj = await _deliveryOrderManager.CreateAsync(
                input.Number,
                input.TransferOrderId,
                input.FromWarehouseId,
                input.ToWarehouseId,
                input.OrderDate
            );

            obj.Description = input.Description;

            await _deliveryOrderRepository.InsertAsync(obj);

            return ObjectMapper.Map<DeliveryOrder, DeliveryOrderReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, DeliveryOrderUpdateDto input)
        {
            var obj = await _deliveryOrderRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _deliveryOrderManager.ChangeNameAsync(obj, input.Number);
            }

            obj.TransferOrderId = input.TransferOrderId;
            obj.FromWarehouseId = input.FromWarehouseId;
            obj.ToWarehouseId = input.ToWarehouseId;
            obj.Description = input.Description;
            obj.OrderDate = input.OrderDate;

            await _deliveryOrderRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _deliveryOrderRepository.DeleteAsync(id);
        }
    }
}
