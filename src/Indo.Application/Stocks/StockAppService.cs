using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Movements;
using Indo.Products;
using Indo.Uoms;
using Indo.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.Stocks
{
    public class StockAppService : IndoAppService, IStockAppService
    {
        private readonly IMovementRepository _movementRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IStockRepository _stockRepository;
        private readonly StockManager _stockManager;
        public StockAppService(
            IStockRepository stockRepository,
            StockManager stockManager,
            IWarehouseRepository warehouseRepository,
            IProductRepository productRepository,
            IUomRepository uomRepository,
            IMovementRepository movementRepository)
        {
            _stockRepository = stockRepository;
            _stockManager = stockManager;
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _movementRepository = movementRepository;
        }
        private IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddMonths(1))
                yield return day;
        }
        public async Task<StockReadDto> GetAsync(Guid id)
        {
            var queryable = await _stockRepository.GetQueryableAsync();
            var query = from stock in queryable
                        join movement in _movementRepository on stock.MovementId equals movement.Id
                        join warehouse in _warehouseRepository on stock.WarehouseId equals warehouse.Id
                        join product in _productRepository on stock.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where stock.Id == id
                        select new { stock, movement, warehouse, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Stock), id);
            }
            var dto = ObjectMapper.Map<Stock, StockReadDto>(queryResult.stock);
            dto.MovementNumber = queryResult.movement.Number;
            dto.WarehouseName = queryResult.warehouse.Name;
            dto.ProductName = queryResult.product.Name;

            return dto;
        }
        public async Task<List<StockReadDto>> GetListAsync()
        {
            var queryable = await _stockRepository.GetQueryableAsync();
            var query = from stock in queryable
                        join movement in _movementRepository on stock.MovementId equals movement.Id
                        join warehouse in _warehouseRepository on stock.WarehouseId equals warehouse.Id
                        join product in _productRepository on stock.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where warehouse.Virtual.Equals(false)
                        select new { stock, movement, warehouse, product, uom };
          
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Stock, StockReadDto>(x.stock);
                dto.MovementNumber = x.movement.Number;
                dto.WarehouseName = x.warehouse.Name;
                dto.ProductName = x.product.Name;
                dto.UnitPrice = x.product.Price;
                dto.Valuation = dto.UnitPrice * dto.Qty;
                dto.UomName = x.uom.Name;
                return dto;
            }).ToList();

            var groups = dtos.GroupBy(x => new { x.WarehouseName, x.ProductName })
                                .Select(x => new StockReadDto { 
                                    WarehouseName = x.Key.WarehouseName,
                                    ProductName = x.Key.ProductName,
                                    Qty = x.Sum(x => x.Qty),
                                    UomName = x.Max(x => x.UomName)
                                }).ToList();

            return groups;
        }
        public async Task<List<MonthlyValuationDto>> GetListMonthlyValuation(int monthsCount)
        {
            var result = new List<MonthlyValuationDto>();
            var endDate = DateTime.Now;
            var startDate = endDate.AddMonths(-monthsCount);
            foreach (var item in EachMonth(startDate, endDate))
            {
                var obj = new MonthlyValuationDto
                {
                    MonthName = item.ToString("MMM"),
                    Amount = await GetSummaryValuationByYearMonthAsync(item.Year, item.Month)
                };
                result.Add(obj);
            }
            return result;
        }
        public async Task<float> GetSummaryValuationByYearMonthAsync(int year, int month)
        {
            var queryable = await _stockRepository.GetQueryableAsync();
            var query = from stock in queryable
                        join movement in _movementRepository on stock.MovementId equals movement.Id
                        join warehouse in _warehouseRepository on stock.WarehouseId equals warehouse.Id
                        join product in _productRepository on stock.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where warehouse.Virtual.Equals(false) && (movement.MovementDate.Year <= year && movement.MovementDate.Month <= month)
                        select new { stock, movement, warehouse, product, uom };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Stock, StockReadDto>(x.stock);
                dto.MovementNumber = x.movement.Number;
                dto.WarehouseName = x.warehouse.Name;
                dto.ProductName = x.product.Name;
                dto.UnitPrice = x.product.Price;
                dto.Valuation = dto.UnitPrice * dto.Qty;
                dto.UomName = x.uom.Name;
                return dto;
            }).ToList();

            return dtos.Sum(x => x.Valuation);
        }
        public async Task<float> GetTotalValuationAsync()
        {
            var queryable = await _stockRepository.GetQueryableAsync();
            var query = from stock in queryable
                        join movement in _movementRepository on stock.MovementId equals movement.Id
                        join warehouse in _warehouseRepository on stock.WarehouseId equals warehouse.Id
                        join product in _productRepository on stock.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where warehouse.Virtual.Equals(false) 
                        select new { stock, movement, warehouse, product, uom };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Stock, StockReadDto>(x.stock);
                dto.MovementNumber = x.movement.Number;
                dto.WarehouseName = x.warehouse.Name;
                dto.ProductName = x.product.Name;
                dto.UnitPrice = x.product.Price;
                dto.Valuation = dto.UnitPrice * dto.Qty;
                dto.UomName = x.uom.Name;
                return dto;
            }).ToList();

            return dtos.Sum(x => x.Valuation);
        }
        public async Task<StockReadDto> CreateAsync(StockCreateDto input)
        {
            var obj = await _stockManager.CreateAsync(
                input.MovementId,
                input.WarehouseId,
                input.TransactionDate,
                input.SourceDocument,
                input.Flow,
                input.ProductId,
                input.Qty
            );

            await _stockRepository.InsertAsync(obj);

            return ObjectMapper.Map<Stock, StockReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, StockUpdateDto input)
        {
            var obj = await _stockRepository.GetAsync(id);

            obj.MovementId = input.MovementId;
            obj.WarehouseId = input.WarehouseId;
            obj.TransactionDate = input.TransactionDate;
            obj.SourceDocument = input.SourceDocument;
            obj.Flow = input.Flow;
            obj.ProductId = input.ProductId;
            obj.Qty = input.Qty;

            await _stockRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _stockRepository.DeleteAsync(id);
        }
    }
}
