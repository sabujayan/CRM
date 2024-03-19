using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.PurchaseReceipts;
using Indo.Products;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.PurchaseOrders;
using Indo.Vendors;
using Indo.PurchaseOrderDetails;

namespace Indo.PurchaseReceiptDetails
{
    public class PurchaseReceiptDetailAppService : IndoAppService, IPurchaseReceiptDetailAppService
    {
        private readonly IPurchaseReceiptRepository _purchaseReceiptRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IPurchaseReceiptDetailRepository _purchaseReceiptDetailRepository;
        private readonly PurchaseReceiptDetailManager _purchaseReceiptDetailManager;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        private readonly IVendorRepository _vendorRepository;
        public PurchaseReceiptDetailAppService(
            IPurchaseReceiptDetailRepository purchaseReceiptDetailRepository,
            PurchaseReceiptDetailManager purchaseReceiptDetailManager,
            IPurchaseReceiptRepository purchaseReceiptRepository,
            IProductRepository productRepository,
            IPurchaseOrderRepository purchaseOrderRepository,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
            IVendorRepository vendorRepository,
            IUomRepository uomRepository)
        {
            _purchaseReceiptDetailRepository = purchaseReceiptDetailRepository;
            _purchaseReceiptDetailManager = purchaseReceiptDetailManager;
            _purchaseReceiptRepository = purchaseReceiptRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
            _vendorRepository = vendorRepository;
        }
        public async Task<PurchaseReceiptDetailReadDto> GetAsync(Guid id)
        {
            var queryable = await _purchaseReceiptDetailRepository.GetQueryableAsync();
            var query = from purchaseReceiptDetail in queryable
                        join purchaseReceipt in _purchaseReceiptRepository on purchaseReceiptDetail.PurchaseReceiptId equals purchaseReceipt.Id
                        join product in _productRepository on purchaseReceiptDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where purchaseReceiptDetail.Id == id
                        select new { purchaseReceiptDetail, purchaseReceipt, product, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(PurchaseReceiptDetail), id);
            }
            var dto = ObjectMapper.Map<PurchaseReceiptDetail, PurchaseReceiptDetailReadDto>(queryResult.purchaseReceiptDetail);
            dto.ProductName = queryResult.product.Name;
            dto.UomName = queryResult.uom.Name;
            dto.Status = queryResult.purchaseReceipt.Status;
            dto.StatusString = L[$"Enum:PurchaseReceiptStatus:{(int)queryResult.purchaseReceipt.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<PurchaseReceiptDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _purchaseReceiptDetailRepository.GetQueryableAsync();
            var query = from purchaseReceiptDetail in queryable
                        join purchaseReceipt in _purchaseReceiptRepository on purchaseReceiptDetail.PurchaseReceiptId equals purchaseReceipt.Id
                        join product in _productRepository on purchaseReceiptDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { purchaseReceiptDetail, purchaseReceipt, product, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseReceiptDetail, PurchaseReceiptDetailReadDto>(x.purchaseReceiptDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.purchaseReceipt.Status;
                dto.StatusString = L[$"Enum:PurchaseReceiptStatus:{(int)x.purchaseReceipt.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _purchaseReceiptDetailRepository.GetCountAsync();

            return new PagedResultDto<PurchaseReceiptDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<PurchaseReceiptDetailReadDto>> GetListDetailAsync()
        {
            var queryable = await _purchaseReceiptDetailRepository.GetQueryableAsync();
            var query = from purchaseReceiptDetail in queryable
                        join purchaseReceipt in _purchaseReceiptRepository on purchaseReceiptDetail.PurchaseReceiptId equals purchaseReceipt.Id
                        join purchaseOrder in _purchaseOrderRepository on purchaseReceipt.PurchaseOrderId equals purchaseOrder.Id
                        join vendor in _vendorRepository on purchaseOrder.VendorId equals vendor.Id
                        join product in _productRepository on purchaseReceiptDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        select new { purchaseReceiptDetail, purchaseReceipt, purchaseOrder, vendor, product, uom };
           
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseReceiptDetail, PurchaseReceiptDetailReadDto>(x.purchaseReceiptDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.purchaseReceipt.Status;
                dto.StatusString = L[$"Enum:PurchaseReceiptStatus:{(int)x.purchaseReceipt.Status}"];
                dto.PurchaseReceiptNumber = x.purchaseReceipt.Number;
                dto.PurchaseReceiptDate = x.purchaseReceipt.ReceiptDate;
                dto.PurchaseOrderNumber = x.purchaseOrder.Number;
                dto.VendorName = x.vendor.Name;
                return dto;
            })
                .OrderByDescending(x => x.PurchaseReceiptDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<PurchaseReceiptDetailReadDto>> GetListByPurchaseReceiptAsync(Guid purchaseReceiptId)
        {
            var queryable = await _purchaseReceiptDetailRepository.GetQueryableAsync();
            var query = from purchaseReceiptDetail in queryable
                        join purchaseReceipt in _purchaseReceiptRepository on purchaseReceiptDetail.PurchaseReceiptId equals purchaseReceipt.Id
                        join product in _productRepository on purchaseReceiptDetail.ProductId equals product.Id
                        join uom in _uomRepository on product.UomId equals uom.Id
                        where purchaseReceiptDetail.PurchaseReceiptId.Equals(purchaseReceiptId)
                        select new { purchaseReceiptDetail, purchaseReceipt, product, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseReceiptDetail, PurchaseReceiptDetailReadDto>(x.purchaseReceiptDetail);
                dto.ProductName = x.product.Name;
                dto.UomName = x.uom.Name;
                dto.Status = x.purchaseReceipt.Status;
                dto.StatusString = L[$"Enum:PurchaseReceiptStatus:{(int)x.purchaseReceipt.Status}"];
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<PurchaseReceiptDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<PurchaseReceiptLookupDto>> GetPurchaseReceiptLookupAsync()
        {
            var list = await _purchaseReceiptRepository.GetListAsync();
            return new ListResultDto<PurchaseReceiptLookupDto>(
                ObjectMapper.Map<List<PurchaseReceipt>, List<PurchaseReceiptLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
        {
            var list = await _productRepository.GetListAsync();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ProductLookupDto>> GetProductLookupByPurchaseReceiptAsync(Guid id)
        {
            var queryable = await _purchaseOrderDetailRepository.GetQueryableAsync();
            var query = from purchaseOrderDetail in queryable
                        join product in _productRepository on purchaseOrderDetail.ProductId equals product.Id
                        join purchaseOrder in _purchaseOrderRepository on purchaseOrderDetail.PurchaseOrderId equals purchaseOrder.Id
                        join purchaseReceipt in _purchaseReceiptRepository on purchaseOrder.Id equals purchaseReceipt.PurchaseOrderId
                        where purchaseReceipt.Id.Equals(id)
                        select new { product };

            var queryResult = await AsyncExecuter.ToListAsync(query);

            var list = queryResult.Select(x => x.product).ToList();
            return new ListResultDto<ProductLookupDto>(
                ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(list)
            );
        }
        public async Task<PurchaseReceiptDetailReadDto> CreateAsync(PurchaseReceiptDetailCreateDto input)
        {
            var obj = await _purchaseReceiptDetailManager.CreateAsync(
                input.PurchaseReceiptId,
                input.ProductId,
                input.Quantity
            );
            await _purchaseReceiptDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<PurchaseReceiptDetail, PurchaseReceiptDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, PurchaseReceiptDetailUpdateDto input)
        {
            var obj = await _purchaseReceiptDetailRepository.GetAsync(id);

            obj.ProductId = input.ProductId;
            obj.Quantity = input.Quantity;

            var product = _productRepository.Where(x => x.Id.Equals(obj.ProductId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _purchaseReceiptDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _purchaseReceiptDetailRepository.DeleteAsync(id);
        }
    }
}
