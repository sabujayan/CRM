using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.TransferOrders;
using Indo.Products;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.TransferOrderDetails
{
    public class TransferOrderDetailAppService : IndoAppService, ITransferOrderDetailAppService
    {
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly ITransferOrderDetailRepository _transferOrderDetailRepository;
        private readonly TransferOrderDetailManager _transferOrderDetailManager;
        public TransferOrderDetailAppService(
            ITransferOrderDetailRepository transferOrderDetailRepository,
            TransferOrderDetailManager transferOrderDetailManager,
            ITransferOrderRepository transferOrderRepository,
            IProductRepository productRepository,
            IUomRepository uomRepository)
        {
            _transferOrderDetailRepository = transferOrderDetailRepository;
            _transferOrderDetailManager = transferOrderDetailManager;
            _transferOrderRepository = transferOrderRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<TransferOrderDetailReadDto> GetAsync(Guid id)
        {
            var queryable = await _transferOrderDetailRepository.GetQueryableAsync();
            var query = from transferOrderDetail in queryable
                        join transferOrder in _transferOrderRepository on transferOrderDetail.TransferOrderId equals transferOrder.Id
                        join product in _productRepository on transferOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where transferOrderDetail.Id == id
                        select new { transferOrderDetail, transferOrder, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(TransferOrderDetail), id);
            }
            var dto = ObjectMapper.Map<TransferOrderDetail, TransferOrderDetailReadDto>(queryResult.transferOrderDetail);
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;
            dto.Status = queryResult.transferOrder.Status;
            dto.StatusString = L[$"Enum:TransferOrderStatus:{(int)queryResult.transferOrder.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<TransferOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _transferOrderDetailRepository.GetQueryableAsync();
            var query = from transferOrderDetail in queryable
                        join transferOrder in _transferOrderRepository on transferOrderDetail.TransferOrderId equals transferOrder.Id
                        join product in _productRepository on transferOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { transferOrderDetail, transferOrder, product, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<TransferOrderDetail, TransferOrderDetailReadDto>(x.transferOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.transferOrder.Status;
                dto.StatusString = L[$"Enum:TransferOrderStatus:{(int)x.transferOrder.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _transferOrderDetailRepository.GetCountAsync();

            return new PagedResultDto<TransferOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<PagedResultDto<TransferOrderDetailReadDto>> GetListByTransferOrderAsync(Guid transferOrderId)
        {
            var queryable = await _transferOrderDetailRepository.GetQueryableAsync();
            var query = from transferOrderDetail in queryable
                        join transferOrder in _transferOrderRepository on transferOrderDetail.TransferOrderId equals transferOrder.Id
                        join product in _productRepository on transferOrderDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where transferOrderDetail.TransferOrderId.Equals(transferOrderId)
                        select new { transferOrderDetail, transferOrder, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<TransferOrderDetail, TransferOrderDetailReadDto>(x.transferOrderDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.transferOrder.Status;
                dto.StatusString = L[$"Enum:TransferOrderStatus:{(int)x.transferOrder.Status}"];
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<TransferOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<TransferOrderLookupDto>> GetTransferOrderLookupAsync()
        {
            var list = await _transferOrderRepository.GetListAsync();
            return new ListResultDto<TransferOrderLookupDto>(
                ObjectMapper.Map<List<TransferOrder>, List<TransferOrderLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
        {
            var list = await _productRepository.GetListAsync();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<TransferOrderDetailReadDto> CreateAsync(TransferOrderDetailCreateDto input)
        {
            var obj = await _transferOrderDetailManager.CreateAsync(
                input.TransferOrderId,
                input.ProductId,
                input.Quantity
            );
            await _transferOrderDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<TransferOrderDetail, TransferOrderDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, TransferOrderDetailUpdateDto input)
        {
            var obj = await _transferOrderDetailRepository.GetAsync(id);

            obj.ProductId = input.ProductId;
            obj.Quantity = input.Quantity;

            var product = _productRepository.Where(x => x.Id.Equals(obj.ProductId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _transferOrderDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _transferOrderDetailRepository.DeleteAsync(id);
        }
    }
}
