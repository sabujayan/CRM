using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.PurchaseOrders;
using Indo.Vendors;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.PurchaseReceipts
{
    public class PurchaseReceiptAppService : IndoAppService, IPurchaseReceiptAppService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPurchaseReceiptRepository _purchaseReceiptRepository;
        private readonly PurchaseReceiptManager _purchaseReceiptManager;
        private readonly ICompanyAppService _companyAppService;
        private readonly IVendorRepository _vendorRepository;
        public PurchaseReceiptAppService(
            IPurchaseReceiptRepository purchaseReceiptRepository,
            PurchaseReceiptManager purchaseReceiptManager,
            IPurchaseOrderRepository purchaseOrderRepository,
            IVendorRepository vendorRepository,
            ICompanyAppService companyAppService)
        {
            _purchaseReceiptRepository = purchaseReceiptRepository;
            _purchaseReceiptManager = purchaseReceiptManager;
            _purchaseOrderRepository = purchaseOrderRepository;
            _companyAppService = companyAppService;
            _vendorRepository = vendorRepository;
        }
        public async Task<PurchaseReceiptReadDto> GetAsync(Guid id)
        {
            var queryable = await _purchaseReceiptRepository.GetQueryableAsync();
            var query = from purchaseReceipt in queryable
                        join purchaseOrder in _purchaseOrderRepository on purchaseReceipt.PurchaseOrderId equals purchaseOrder.Id
                        where purchaseReceipt.Id == id
                        select new { purchaseReceipt, purchaseOrder };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(PurchaseReceipt), id);
            }
            var dto = ObjectMapper.Map<PurchaseReceipt, PurchaseReceiptReadDto>(queryResult.purchaseReceipt);
            dto.PurchaseOrderNumber = queryResult.purchaseOrder.Number;
            dto.VendorId = queryResult.purchaseOrder.VendorId;
            dto.StatusString = L[$"Enum:PurchaseReceiptStatus:{(int)queryResult.purchaseReceipt.Status}"];
            return dto;
        }
        public async Task<List<PurchaseReceiptReadDto>> GetListAsync()
        {
            var queryable = await _purchaseReceiptRepository.GetQueryableAsync();
            var query = from purchaseReceipt in queryable
                        join purchaseOrder in _purchaseOrderRepository on purchaseReceipt.PurchaseOrderId equals purchaseOrder.Id
                        join vendor in _vendorRepository on purchaseOrder.VendorId equals vendor.Id
                        select new { purchaseReceipt, purchaseOrder, vendor };            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseReceipt, PurchaseReceiptReadDto>(x.purchaseReceipt);
                dto.PurchaseOrderNumber = x.purchaseOrder.Number;
                dto.VendorId = x.purchaseOrder.VendorId;
                dto.VendorName = x.vendor.Name;
                dto.StatusString = L[$"Enum:PurchaseReceiptStatus:{(int)x.purchaseReceipt.Status}"];
                return dto;
            })
                .OrderByDescending(x => x.ReceiptDate)
                .ToList();
            return dtos;
        }
        public async Task<List<PurchaseReceiptReadDto>> GetListByPurchaseOrderAsync(Guid purchaseOrderId)
        {
            var queryable = await _purchaseReceiptRepository.GetQueryableAsync();
            var query = from purchaseReceipt in queryable
                        join purchaseOrder in _purchaseOrderRepository on purchaseReceipt.PurchaseOrderId equals purchaseOrder.Id
                        where purchaseReceipt.PurchaseOrderId.Equals(purchaseOrderId)
                        select new { purchaseReceipt, purchaseOrder };          
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseReceipt, PurchaseReceiptReadDto>(x.purchaseReceipt);
                dto.PurchaseOrderNumber = x.purchaseOrder.Number;
                dto.VendorId = x.purchaseOrder.VendorId;
                dto.StatusString = L[$"Enum:PurchaseReceiptStatus:{(int)x.purchaseReceipt.Status}"];
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<ListResultDto<PurchaseOrderLookupDto>> GetPurchaseOrderLookupAsync()
        {
            var list = await _purchaseOrderRepository.GetListAsync();
            return new ListResultDto<PurchaseOrderLookupDto>(
                ObjectMapper.Map<List<PurchaseOrder>, List<PurchaseOrderLookupDto>>(list)
            );
        }
        public async Task<PurchaseReceiptReadDto> CreateAsync(PurchaseReceiptCreateDto input)
        {
            var obj = await _purchaseReceiptManager.CreateAsync(
                input.Number,
                input.PurchaseOrderId,
                input.ReceiptDate
            );

            obj.Description = input.Description;

            await _purchaseReceiptRepository.InsertAsync(obj);

            return ObjectMapper.Map<PurchaseReceipt, PurchaseReceiptReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, PurchaseReceiptUpdateDto input)
        {
            var obj = await _purchaseReceiptRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _purchaseReceiptManager.ChangeNameAsync(obj, input.Number);
            }

            obj.PurchaseOrderId = input.PurchaseOrderId;
            obj.Description = input.Description;
            obj.ReceiptDate = input.ReceiptDate;

            await _purchaseReceiptRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _purchaseReceiptRepository.DeleteAsync(id);
        }

        public async Task ConfirmAsync(Guid purchaseReceiptId)
        {
            await _purchaseReceiptManager.ConfirmPurchaseReceiptAsync(purchaseReceiptId);
        }

        public async Task ReturnAsync(Guid purchaseReceiptId)
        {
            var returned = await _purchaseReceiptManager.GeneratePurchaseReceiptReturnFromReceiptAsync(purchaseReceiptId);
            await _purchaseReceiptManager.ConfirmPurchaseReceiptReturnAsync(returned.Id);
        }
    }
}
