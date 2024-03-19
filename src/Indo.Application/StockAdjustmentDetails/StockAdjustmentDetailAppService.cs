using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.StockAdjustments;
using Indo.Products;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.Warehouses;

namespace Indo.StockAdjustmentDetails
{
    public class StockAdjustmentDetailAppService : IndoAppService, IStockAdjustmentDetailAppService
    {
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IStockAdjustmentDetailRepository _stockAdjustmentDetailRepository;
        private readonly StockAdjustmentDetailManager _stockAdjustmentDetailManager;
        private readonly IWarehouseRepository _warehouseRepository;
        public StockAdjustmentDetailAppService(
            IStockAdjustmentDetailRepository stockAdjustmentDetailRepository,
            StockAdjustmentDetailManager stockAdjustmentDetailManager,
            IStockAdjustmentRepository stockAdjustmentRepository,
            IProductRepository productRepository,
            IWarehouseRepository warehouseRepository,
            IUomRepository uomRepository)
        {
            _stockAdjustmentDetailRepository = stockAdjustmentDetailRepository;
            _stockAdjustmentDetailManager = stockAdjustmentDetailManager;
            _stockAdjustmentRepository = stockAdjustmentRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _warehouseRepository = warehouseRepository;
        }
        public async Task<StockAdjustmentDetailReadDto> GetAsync(Guid id)
        {
            var queryable = await _stockAdjustmentDetailRepository.GetQueryableAsync();
            var query = from stockAdjustmentDetail in queryable
                        join stockAdjustment in _stockAdjustmentRepository on stockAdjustmentDetail.StockAdjustmentId equals stockAdjustment.Id
                        join product in _productRepository on stockAdjustmentDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where stockAdjustmentDetail.Id == id
                        select new { stockAdjustmentDetail, stockAdjustment, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(StockAdjustmentDetail), id);
            }
            var dto = ObjectMapper.Map<StockAdjustmentDetail, StockAdjustmentDetailReadDto>(queryResult.stockAdjustmentDetail);
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;
            dto.Status = queryResult.stockAdjustment.Status;
            dto.StatusString = L[$"Enum:StockAdjustmentStatus:{(int)queryResult.stockAdjustment.Status}"];
            dto.AdjustmentTypeString = L[$"Enum:StockAdjustmentType:{(int)queryResult.stockAdjustment.AdjustmentType}"];
            return dto;
        }
        public async Task<PagedResultDto<StockAdjustmentDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _stockAdjustmentDetailRepository.GetQueryableAsync();
            var query = from stockAdjustmentDetail in queryable
                        join stockAdjustment in _stockAdjustmentRepository on stockAdjustmentDetail.StockAdjustmentId equals stockAdjustment.Id
                        join product in _productRepository on stockAdjustmentDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { stockAdjustmentDetail, stockAdjustment, product, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<StockAdjustmentDetail, StockAdjustmentDetailReadDto>(x.stockAdjustmentDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.stockAdjustment.Status;
                dto.StatusString = L[$"Enum:StockAdjustmentStatus:{(int)x.stockAdjustment.Status}"];
                dto.AdjustmentTypeString = L[$"Enum:StockAdjustmentType:{(int)x.stockAdjustment.AdjustmentType}"];
                return dto;
            }).ToList();

            var totalCount = await _stockAdjustmentDetailRepository.GetCountAsync();

            return new PagedResultDto<StockAdjustmentDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<StockAdjustmentDetailReadDto>> GetListDetailAsync()
        {
            var queryable = await _stockAdjustmentDetailRepository.GetQueryableAsync();
            var query = from stockAdjustmentDetail in queryable
                        join stockAdjustment in _stockAdjustmentRepository on stockAdjustmentDetail.StockAdjustmentId equals stockAdjustment.Id
                        join warehouse in _warehouseRepository on stockAdjustment.WarehouseId equals warehouse.Id
                        join product in _productRepository on stockAdjustmentDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { stockAdjustmentDetail, stockAdjustment, warehouse, product, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<StockAdjustmentDetail, StockAdjustmentDetailReadDto>(x.stockAdjustmentDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.stockAdjustment.Status;
                dto.StatusString = L[$"Enum:StockAdjustmentStatus:{(int)x.stockAdjustment.Status}"];
                dto.StockAdjustmentNumber = x.stockAdjustment.Number;
                dto.AdjustmentDate = x.stockAdjustment.AdjustmentDate;
                dto.AdjustmentType = x.stockAdjustment.AdjustmentType;
                dto.AdjustmentTypeString = L[$"Enum:StockAdjustmentType:{(int)x.stockAdjustment.AdjustmentType}"];
                dto.WarehouseName = x.warehouse.Name;
                return dto;
            })
                .OrderByDescending(x => x.AdjustmentDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<StockAdjustmentDetailReadDto>> GetListByStockAdjustmentAsync(Guid stockAdjustmentId)
        {
            var queryable = await _stockAdjustmentDetailRepository.GetQueryableAsync();
            var query = from stockAdjustmentDetail in queryable
                        join stockAdjustment in _stockAdjustmentRepository on stockAdjustmentDetail.StockAdjustmentId equals stockAdjustment.Id
                        join product in _productRepository on stockAdjustmentDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where stockAdjustmentDetail.StockAdjustmentId.Equals(stockAdjustmentId)
                        select new { stockAdjustmentDetail, stockAdjustment, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<StockAdjustmentDetail, StockAdjustmentDetailReadDto>(x.stockAdjustmentDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.stockAdjustment.Status;
                dto.StatusString = L[$"Enum:StockAdjustmentStatus:{(int)x.stockAdjustment.Status}"];
                dto.AdjustmentTypeString = L[$"Enum:StockAdjustmentType:{(int)x.stockAdjustment.AdjustmentType}"];
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<StockAdjustmentDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<StockAdjustmentLookupDto>> GetStockAdjustmentLookupAsync()
        {
            var list = await _stockAdjustmentRepository.GetListAsync();
            return new ListResultDto<StockAdjustmentLookupDto>(
                ObjectMapper.Map<List<StockAdjustment>, List<StockAdjustmentLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
        {
            var list = await _productRepository.GetListAsync();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<StockAdjustmentDetailReadDto> CreateAsync(StockAdjustmentDetailCreateDto input)
        {
            var obj = await _stockAdjustmentDetailManager.CreateAsync(
                input.StockAdjustmentId,
                input.ProductId,
                input.Quantity
            );
            await _stockAdjustmentDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<StockAdjustmentDetail, StockAdjustmentDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, StockAdjustmentDetailUpdateDto input)
        {
            var obj = await _stockAdjustmentDetailRepository.GetAsync(id);

            obj.ProductId = input.ProductId;
            obj.Quantity = input.Quantity;

            var product = _productRepository.Where(x => x.Id.Equals(obj.ProductId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _stockAdjustmentDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _stockAdjustmentDetailRepository.DeleteAsync(id);
        }
    }
}
