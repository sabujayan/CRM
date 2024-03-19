using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Vendors;
using Indo.Employees;
using Indo.PurchaseOrderDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.PurchaseReceipts;
using Indo.VendorBills;

namespace Indo.PurchaseOrders
{
    public class PurchaseOrderAppService : IndoAppService, IPurchaseOrderAppService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly PurchaseOrderManager _purchaseOrderManager;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        private readonly PurchaseReceiptManager _purchaseReceiptManager;
        private readonly IPurchaseReceiptRepository _purchaseReceiptRepository;
        public PurchaseOrderAppService(
            IPurchaseOrderRepository purchaseOrderRepository,
            PurchaseOrderManager purchaseOrderManager,
            IVendorRepository vendorRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeAppService employeeAppService,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
            PurchaseReceiptManager purchaseReceiptManager,
            IPurchaseReceiptRepository purchaseReceiptRepository,
            ICompanyAppService companyAppService)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderManager = purchaseOrderManager;
            _vendorRepository = vendorRepository;
            _employeeRepository = employeeRepository;
            _employeeAppService = employeeAppService;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
            _companyAppService = companyAppService;
            _purchaseReceiptManager = purchaseReceiptManager;
            _purchaseReceiptRepository = purchaseReceiptRepository;
        }

        public async Task<OrderCountDto> GetCountOrderAsync()
        {
            await Task.Yield();
            var result = new OrderCountDto();
            result.CountDraft = _purchaseOrderRepository.Where(x => x.Status == PurchaseOrderStatus.Draft).Count();
            result.CountConfirm = _purchaseOrderRepository.Where(x => x.Status == PurchaseOrderStatus.Confirm).Count();
            result.CountCancelled = _purchaseOrderRepository.Where(x => x.Status == PurchaseOrderStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummarySubTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _purchaseOrderDetailRepository
                .Where(x => x.PurchaseOrderId.Equals(id))
                .Sum(x => x.SubTotal);
            return result;
        }
        public async Task<string> GetSummarySubTotalInStringAsync(Guid id)
        {
            var result = await GetSummarySubTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryDiscAmtAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _purchaseOrderDetailRepository
                .Where(x => x.PurchaseOrderId.Equals(id))
                .Sum(x => x.DiscAmt);
            return result;
        }
        public async Task<string> GetSummaryDiscAmtInStringAsync(Guid id)
        {
            var result = await GetSummaryDiscAmtAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryBeforeTaxAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _purchaseOrderDetailRepository
                .Where(x => x.PurchaseOrderId.Equals(id))
                .Sum(x => x.BeforeTax);
            return result;
        }
        public async Task<string> GetSummaryBeforeTaxInStringAsync(Guid id)
        {
            var result = await GetSummaryBeforeTaxAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryTaxAmountAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _purchaseOrderDetailRepository
                .Where(x => x.PurchaseOrderId.Equals(id))
                .Sum(x => x.TaxAmount);
            return result;
        }
        public async Task<string> GetSummaryTaxAmountInStringAsync(Guid id)
        {
            var result = await GetSummaryTaxAmountAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _purchaseOrderDetailRepository
                .Where(x => x.PurchaseOrderId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetTotalQtyAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _purchaseOrderDetailRepository
                .Where(x => x.PurchaseOrderId.Equals(id))
                .Sum(x => x.Quantity);
            return result;
        }
        public async Task<float> GetTotalQtyByYearMonthAsync(int year, int month)
        {
            var result = 0f;
            var lists = _purchaseOrderRepository
                .Where(x => x.OrderDate.Year.Equals(year) && x.OrderDate.Month.Equals(month))
                .ToList();
            foreach (var item in lists)
            {
                result = result + await GetTotalQtyAsync(item.Id);
            }
            return result;
        }
        public async Task<PurchaseOrderReadDto> GetAsync(Guid id)
        {
            var queryable = await _purchaseOrderRepository.GetQueryableAsync();
            var query = from purchaseOrder in queryable
                        join vendor in _vendorRepository on purchaseOrder.VendorId equals vendor.Id
                        join employee in _employeeRepository on purchaseOrder.BuyerId equals employee.Id
                        where purchaseOrder.Id == id
                        select new { purchaseOrder, vendor, employee };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(PurchaseOrder), id);
            }
            var dto = ObjectMapper.Map<PurchaseOrder, PurchaseOrderReadDto>(queryResult.purchaseOrder);
            dto.VendorName = queryResult.vendor.Name;
            dto.BuyerName = queryResult.employee.Name;
            dto.StatusString = L[$"Enum:PurchaseOrderStatus:{(int)queryResult.purchaseOrder.Status}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<PurchaseOrderReadDto>> GetListAsync()
        {
            var queryable = await _purchaseOrderRepository.GetQueryableAsync();
            var query = from purchaseOrder in queryable
                        join vendor in _vendorRepository on purchaseOrder.VendorId equals vendor.Id
                        join employee in _employeeRepository on purchaseOrder.BuyerId equals employee.Id
                        select new { purchaseOrder, vendor, employee };
                 
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseOrder, PurchaseOrderReadDto>(x.purchaseOrder);
                dto.VendorName = x.vendor.Name;
                dto.BuyerName = x.employee.Name;
                dto.StatusString = L[$"Enum:PurchaseOrderStatus:{(int)x.purchaseOrder.Status}"];
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<PurchaseOrderReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _purchaseOrderRepository.GetQueryableAsync();
            var query = from purchaseOrder in queryable
                        join vendor in _vendorRepository on purchaseOrder.VendorId equals vendor.Id
                        join employee in _employeeRepository on purchaseOrder.BuyerId equals employee.Id
                        select new { purchaseOrder, vendor, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<PurchaseOrder, PurchaseOrderReadDto>(x.purchaseOrder);
                dto.VendorName = x.vendor.Name;
                dto.BuyerName = x.employee.Name;
                dto.StatusString = L[$"Enum:PurchaseOrderStatus:{(int)x.purchaseOrder.Status}"];
                dto.Total = _purchaseOrderDetailRepository.Where(y => y.PurchaseOrderId.Equals(x.purchaseOrder.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<ListResultDto<VendorLookupDto>> GetVendorLookupAsync()
        {
            var list = await _vendorRepository.GetListAsync();
            return new ListResultDto<VendorLookupDto>(
                ObjectMapper.Map<List<Vendor>, List<VendorLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<BuyerLookupDto>> GetBuyerLookupAsync()
        {
            var list = await _employeeAppService.GetBuyerListAsync();
            return new ListResultDto<BuyerLookupDto>(
                ObjectMapper.Map<List<EmployeeReadDto>, List<BuyerLookupDto>>(list)
            );
        }
        public async Task<PurchaseOrderReadDto> CreateAsync(PurchaseOrderCreateDto input)
        {
            var obj = await _purchaseOrderManager.CreateAsync(
                input.Number,
                input.VendorId,
                input.BuyerId,
                input.OrderDate
            );

            obj.Description = input.Description;

            await _purchaseOrderRepository.InsertAsync(obj);

            return ObjectMapper.Map<PurchaseOrder, PurchaseOrderReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, PurchaseOrderUpdateDto input)
        {
            var obj = await _purchaseOrderRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _purchaseOrderManager.ChangeNameAsync(obj, input.Number);
            }

            obj.VendorId = input.VendorId;
            obj.BuyerId = input.BuyerId;
            obj.Description = input.Description;
            obj.OrderDate = input.OrderDate;

            await _purchaseOrderRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _purchaseOrderRepository.DeleteAsync(id);
        }
        public async Task<PurchaseOrderReadDto> ConfirmAsync(Guid purchaseOrderId)
        {
            var obj = await _purchaseOrderManager.ConfirmAsync(purchaseOrderId);
            return ObjectMapper.Map<PurchaseOrder, PurchaseOrderReadDto>(obj);
        }
        public async Task<PurchaseReceiptReadDto> GenerateConfirmReceiptAsync(Guid purchaseOrderId)
        {
            var receipt = await _purchaseReceiptManager.GeneratePurchaseReceiptFromPurchaseAsync(purchaseOrderId);
            await _purchaseReceiptManager.ConfirmPurchaseReceiptAsync(receipt.Id);
            return ObjectMapper.Map<PurchaseReceipt, PurchaseReceiptReadDto>(receipt);
        }
        public async Task<PurchaseReceiptReadDto> GenerateDraftReceiptAsync(Guid purchaseOrderId)
        {
            var receipt = await _purchaseReceiptManager.GeneratePurchaseReceiptFromPurchaseAsync(purchaseOrderId);
            return ObjectMapper.Map<PurchaseReceipt, PurchaseReceiptReadDto>(receipt);
        }
        public async Task<PurchaseOrderReadDto> CancelAsync(Guid purchaseOrderId)
        {
            var obj = await _purchaseOrderManager.CancelAsync(purchaseOrderId);
            return ObjectMapper.Map<PurchaseOrder, PurchaseOrderReadDto>(obj);
        }
        public async Task<VendorBillReadDto> GenerateBillAsync(Guid purchaseOrderId)
        {
            var obj = await _purchaseOrderManager.GenerateBill(purchaseOrderId);
            return ObjectMapper.Map<VendorBill, VendorBillReadDto>(obj);
        }
    }
}
