using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Uoms;
using Indo.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.Movements
{
    public class MovementAppService : IndoAppService, IMovementAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMovementRepository _movementRepository;
        private readonly MovementManager _movementManager;
        public MovementAppService(
            IMovementRepository movementRepository,
            MovementManager movementManager,
            IWarehouseRepository warehouseRepository,
            IProductRepository productRepository,
            IUomRepository uomRepository)
        {
            _movementRepository = movementRepository;
            _movementManager = movementManager;
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<MovementReadDto> GetAsync(Guid id)
        {
            var queryable = await _movementRepository.GetQueryableAsync();
            var query = from movement in queryable
                        join warehouseFrom in _warehouseRepository on movement.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on movement.ToWarehouseId equals warehouseTo.Id
                        join product in _productRepository on movement.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where movement.Id == id
                        select new { movement, warehouseFrom, warehouseTo, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Movement), id);
            }
            var dto = ObjectMapper.Map<Movement, MovementReadDto>(queryResult.movement);
            dto.FromWarehouseName = queryResult.warehouseFrom.Name;
            dto.ToWarehouseName = queryResult.warehouseTo.Name;
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;

            return dto;
        }
        public async Task<List<MovementReadDto>> GetListAsync()
        {
            var queryable = await _movementRepository.GetQueryableAsync();
            var query = from movement in queryable
                        join warehouseFrom in _warehouseRepository on movement.FromWarehouseId equals warehouseFrom.Id
                        join warehouseTo in _warehouseRepository on movement.ToWarehouseId equals warehouseTo.Id
                        join product in _productRepository on movement.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { movement, warehouseFrom, warehouseTo, product, uom };
        
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Movement, MovementReadDto>(x.movement);
                dto.FromWarehouseName = x.warehouseFrom.Name;
                dto.ToWarehouseName = x.warehouseTo.Name;
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                return dto;
            })
                .OrderByDescending(x => x.MovementDate)
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
        public async Task<MovementReadDto> CreateAsync(MovementCreateDto input)
        {
            var obj = await _movementManager.CreateAsync(
                input.Number,
                input.FromWarehouseId,
                input.ToWarehouseId,
                input.MovementDate,
                input.SourceDocument,
                input.Module,
                input.ProductId,
                input.Qty
            );

            await _movementRepository.InsertAsync(obj);

            return ObjectMapper.Map<Movement, MovementReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, MovementUpdateDto input)
        {
            var obj = await _movementRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _movementManager.ChangeNameAsync(obj, input.Number);
            }

            obj.FromWarehouseId = input.FromWarehouseId;
            obj.ToWarehouseId = input.ToWarehouseId;
            obj.MovementDate = input.MovementDate;
            obj.SourceDocument = input.SourceDocument;
            obj.Module = input.Module;
            obj.ProductId = input.ProductId;
            obj.Qty = input.Qty;

            await _movementRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _movementRepository.DeleteAsync(id);
        }
    }
}
