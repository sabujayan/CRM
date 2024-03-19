using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.StockAdjustments
{
    public class StockAdjustmentAppService : IndoAppService, IStockAdjustmentAppService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        private readonly IStockAdjustmentRepository _stockAdjustmentReturnRepository;
        private readonly IStockAdjustmentRepository _stockAdjustmentOriginRepository;
        private readonly StockAdjustmentManager _stockAdjustmentManager;
        public StockAdjustmentAppService(
            IStockAdjustmentRepository stockAdjustmentRepository,
            IStockAdjustmentRepository stockAdjustmentReturnRepository,
            IStockAdjustmentRepository stockAdjustmentOriginRepository,
            StockAdjustmentManager stockAdjustmentManager,
            IWarehouseRepository warehouseRepository)
        {
            _stockAdjustmentRepository = stockAdjustmentRepository;
            _stockAdjustmentReturnRepository = stockAdjustmentReturnRepository;
            _stockAdjustmentOriginRepository = stockAdjustmentOriginRepository;
            _stockAdjustmentManager = stockAdjustmentManager;
            _warehouseRepository = warehouseRepository;
        }
        public async Task<StockAdjustmentReadDto> GetAsync(Guid id)
        {
            var queryable = await _stockAdjustmentRepository.GetQueryableAsync();
            var query = from stockAdjustment in queryable
                        join warehouse in _warehouseRepository on stockAdjustment.WarehouseId equals warehouse.Id
                        join warehouseFrom in _warehouseRepository on stockAdjustment.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on stockAdjustment.ToWarehouseId equals warehouseTo.Id
                        where stockAdjustment.Id == id
                        select new { stockAdjustment, warehouse, warehouseFrom, warehouseTo };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(StockAdjustment), id);
            }
            var dto = ObjectMapper.Map<StockAdjustment, StockAdjustmentReadDto>(queryResult.stockAdjustment);
            dto.FromWarehouseName = queryResult.warehouseFrom.Name;
            dto.ToWarehouseName = queryResult.warehouseTo.Name;
            dto.WarehouseName = queryResult.warehouse.Name;
            dto.StatusString = L[$"Enum:StockAdjustmentStatus:{(int)queryResult.stockAdjustment.Status}"];
            dto.AdjustmentTypeString = L[$"Enum:StockAdjustmentType:{(int)queryResult.stockAdjustment.AdjustmentType}"];
            return dto;
        }
        public async Task<List<StockAdjustmentReadDto>> GetListAsync()
        {
            var queryable = await _stockAdjustmentRepository.GetQueryableAsync();
            var query = from stockAdjustment in queryable
                        join warehouse in _warehouseRepository on stockAdjustment.WarehouseId equals warehouse.Id
                        join warehouseFrom in _warehouseRepository on stockAdjustment.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on stockAdjustment.ToWarehouseId equals warehouseTo.Id
                        join @return in _stockAdjustmentReturnRepository on stockAdjustment.ReturnId equals @return.Id into subreturn from @return in subreturn.DefaultIfEmpty()
                        join origin in _stockAdjustmentOriginRepository on stockAdjustment.OriginalId equals origin.Id into suborigin from origin in suborigin.DefaultIfEmpty()
                        select new { stockAdjustment, warehouse, warehouseFrom, warehouseTo, @return, origin };
         
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<StockAdjustment, StockAdjustmentReadDto>(x.stockAdjustment);
                dto.FromWarehouseName = x.warehouseFrom.Name;
                dto.ToWarehouseName = x.warehouseTo.Name;
                dto.WarehouseName = x.warehouse.Name;
                dto.ReturnNumber = x.@return?.Number ?? string.Empty;
                dto.OriginalNumber = x.origin?.Number ?? string.Empty;
                dto.StatusString = L[$"Enum:StockAdjustmentStatus:{(int)x.stockAdjustment.Status}"];
                dto.AdjustmentTypeString = L[$"Enum:StockAdjustmentType:{(int)x.stockAdjustment.AdjustmentType}"];
                return dto;
            })
                .OrderByDescending(x => x.AdjustmentDate)
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
        public async Task<StockAdjustmentReadDto> CreateAsync(StockAdjustmentCreateDto input)
        {
            var obj = await _stockAdjustmentManager.CreateAsync(
                input.Number,
                input.WarehouseId,
                input.AdjustmentType,
                input.AdjustmentDate
            );

            obj.Description = input.Description;

            await _stockAdjustmentRepository.InsertAsync(obj);

            return ObjectMapper.Map<StockAdjustment, StockAdjustmentReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, StockAdjustmentUpdateDto input)
        {
            var obj = await _stockAdjustmentRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _stockAdjustmentManager.ChangeNameAsync(obj, input.Number);
            }

            obj.Description = input.Description;
            obj.AdjustmentType = input.AdjustmentType;
            obj.AdjustmentDate = input.AdjustmentDate;

            obj = await _stockAdjustmentManager.ChangeType(obj);

            await _stockAdjustmentRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _stockAdjustmentRepository.DeleteAsync(id);
        }
        public async Task<StockAdjustmentReadDto> ConfirmAsync(Guid stockAdjustmentId)
        {
            var obj = await _stockAdjustmentManager.ConfirmStockAdjustmentAsync(stockAdjustmentId);
            return ObjectMapper.Map<StockAdjustment, StockAdjustmentReadDto>(obj);
        }
        public async Task<StockAdjustmentReadDto> ReturnAsync(Guid stockAdjustmentId)
        {
            var returned = await _stockAdjustmentManager.GenerateReturn(stockAdjustmentId);
            var obj = await _stockAdjustmentManager.ConfirmStockAdjustmentAsync(returned.Id);
            var origin = await _stockAdjustmentRepository.GetAsync(stockAdjustmentId);
            origin.ReturnId = returned.Id;
            origin.Status = StockAdjustmentStatus.Returned;
            await _stockAdjustmentRepository.UpdateAsync(origin);
            return ObjectMapper.Map<StockAdjustment, StockAdjustmentReadDto>(obj);
        }
    }
}
