using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Vendors;
using Indo.VendorBills;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.VendorBillDetails
{
    public class VendorBillDetailAppService : IndoAppService, IVendorBillDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly IVendorBillRepository _vendorBillRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IVendorBillDetailRepository _vendorBillDetailRepository;
        private readonly VendorBillDetailManager _vendorBillDetailManager;
        private readonly IVendorRepository _vendorRepository;
        public VendorBillDetailAppService(
            CompanyAppService companyAppService,
            IVendorBillDetailRepository vendorBillDetailRepository,
            VendorBillDetailManager vendorBillDetailManager,
            IVendorBillRepository vendorBillRepository,
            IVendorRepository vendorRepository,
            IUomRepository uomRepository)
        {
            _vendorBillDetailRepository = vendorBillDetailRepository;
            _vendorBillDetailManager = vendorBillDetailManager;
            _vendorBillRepository = vendorBillRepository;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _vendorRepository = vendorRepository;
        }
        public async Task<VendorBillDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorBillDetailRepository.GetQueryableAsync();
            var query = from vendorBillDetail in queryable
                        join vendorBill in _vendorBillRepository on vendorBillDetail.VendorBillId equals vendorBill.Id
                        join uom in _uomRepository on vendorBillDetail.UomId equals uom.Id
                        where vendorBillDetail.Id == id
                        select new { vendorBillDetail, vendorBill, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(VendorBillDetail), id);
            }
            var dto = ObjectMapper.Map<VendorBillDetail, VendorBillDetailReadDto>(queryResult.vendorBillDetail);
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.vendorBill.Status;
            dto.StatusString = L[$"Enum:VendorBillStatus:{(int)queryResult.vendorBill.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<VendorBillDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorBillDetailRepository.GetQueryableAsync();
            var query = from vendorBillDetail in queryable
                        join vendorBill in _vendorBillRepository on vendorBillDetail.VendorBillId equals vendorBill.Id
                        join uom in _uomRepository on vendorBillDetail.UomId equals uom.Id
                        select new { vendorBillDetail, vendorBill, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorBillDetail, VendorBillDetailReadDto>(x.vendorBillDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.vendorBill.Status;
                dto.StatusString = L[$"Enum:VendorBillStatus:{(int)x.vendorBill.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _vendorBillDetailRepository.GetCountAsync();

            return new PagedResultDto<VendorBillDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<VendorBillDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorBillDetailRepository.GetQueryableAsync();
            var query = from vendorBillDetail in queryable
                        join vendorBill in _vendorBillRepository on vendorBillDetail.VendorBillId equals vendorBill.Id
                        join vendor in _vendorRepository on vendorBill.VendorId equals vendor.Id
                        join uom in _uomRepository on vendorBillDetail.UomId equals uom.Id
                        select new { vendorBillDetail, vendorBill, vendor, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorBillDetail, VendorBillDetailReadDto>(x.vendorBillDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.vendorBill.Status;
                dto.StatusString = L[$"Enum:VendorBillStatus:{(int)x.vendorBill.Status}"];
                dto.VendorBillNumber = x.vendorBill.Number;
                dto.BillDate = x.vendorBill.BillDate;
                dto.BillDueDate = x.vendorBill.BillDueDate;
                dto.VendorName = x.vendor.Name;
                dto.PriceString = x.vendorBillDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.vendorBillDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.vendorBillDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.vendorBillDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.vendorBillDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.vendorBillDetail.Total.ToString("##,##.00");
                dto.Period = x.vendorBill.BillDate.ToString("yyyy-MM");
                return dto;
            })
                .OrderByDescending(x => x.BillDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<VendorBillDetailReadDto>> GetListByVendorBillAsync(Guid vendorBillId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorBillDetailRepository.GetQueryableAsync();
            var query = from vendorBillDetail in queryable
                        join vendorBill in _vendorBillRepository on vendorBillDetail.VendorBillId equals vendorBill.Id
                        join uom in _uomRepository on vendorBillDetail.UomId equals uom.Id
                        where vendorBillDetail.VendorBillId.Equals(vendorBillId)
                        select new { vendorBillDetail, vendorBill, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorBillDetail, VendorBillDetailReadDto>(x.vendorBillDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.vendorBill.Status;
                dto.StatusString = L[$"Enum:VendorBillStatus:{(int)x.vendorBill.Status}"];
                dto.PriceString = x.vendorBillDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.vendorBillDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.vendorBillDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.vendorBillDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.vendorBillDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.vendorBillDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<VendorBillDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<VendorBillLookupDto>> GetVendorBillLookupAsync()
        {
            var list = await _vendorBillRepository.GetListAsync();
            return new ListResultDto<VendorBillLookupDto>(
                ObjectMapper.Map<List<VendorBill>, List<VendorBillLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<UomLookupDto>> GetUomLookupAsync()
        {
            var list = await _uomRepository.GetListAsync();
            return new ListResultDto<UomLookupDto>(
                ObjectMapper.Map<List<Uom>, List<UomLookupDto>>(list)
            );
        }
        public async Task<VendorBillDetailReadDto> CreateAsync(VendorBillDetailCreateDto input)
        {
            var obj = await _vendorBillDetailManager.CreateAsync(
                input.VendorBillId,
                input.ProductName,
                input.UomId,
                input.Price,
                input.TaxRate,
                input.Quantity,
                input.DiscAmt
            );
            await _vendorBillDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<VendorBillDetail, VendorBillDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, VendorBillDetailUpdateDto input)
        {
            var obj = await _vendorBillDetailRepository.GetAsync(id);

            obj.ProductName = input.ProductName;
            obj.UomId = input.UomId;
            obj.Price = input.Price;
            obj.TaxRate = input.TaxRate;
            obj.Quantity = input.Quantity;
            obj.DiscAmt = input.DiscAmt;

            obj.Recalculate();

            await _vendorBillDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _vendorBillDetailRepository.DeleteAsync(id);
        }
    }
}
