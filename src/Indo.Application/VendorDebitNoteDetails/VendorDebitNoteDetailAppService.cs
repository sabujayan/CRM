using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Vendors;
using Indo.VendorDebitNotes;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.VendorDebitNoteDetails
{
    public class VendorDebitNoteDetailAppService : IndoAppService, IVendorDebitNoteDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly IVendorDebitNoteRepository _vendorDebitNoteRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IVendorDebitNoteDetailRepository _vendorDebitNoteDetailRepository;
        private readonly VendorDebitNoteDetailManager _vendorDebitNoteDetailManager;
        private readonly IVendorRepository _vendorRepository;
        public VendorDebitNoteDetailAppService(
            CompanyAppService companyAppService,
            IVendorDebitNoteDetailRepository vendorDebitNoteDetailRepository,
            VendorDebitNoteDetailManager vendorDebitNoteDetailManager,
            IVendorDebitNoteRepository vendorDebitNoteRepository,
            IVendorRepository vendorRepository,
            IUomRepository uomRepository)
        {
            _vendorDebitNoteDetailRepository = vendorDebitNoteDetailRepository;
            _vendorDebitNoteDetailManager = vendorDebitNoteDetailManager;
            _vendorDebitNoteRepository = vendorDebitNoteRepository;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _vendorRepository = vendorRepository;
        }
        public async Task<VendorDebitNoteDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorDebitNoteDetailRepository.GetQueryableAsync();
            var query = from vendorDebitNoteDetail in queryable
                        join vendorDebitNote in _vendorDebitNoteRepository on vendorDebitNoteDetail.VendorDebitNoteId equals vendorDebitNote.Id
                        join uom in _uomRepository on vendorDebitNoteDetail.UomId equals uom.Id
                        where vendorDebitNoteDetail.Id == id
                        select new { vendorDebitNoteDetail, vendorDebitNote, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(VendorDebitNoteDetail), id);
            }
            var dto = ObjectMapper.Map<VendorDebitNoteDetail, VendorDebitNoteDetailReadDto>(queryResult.vendorDebitNoteDetail);
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.vendorDebitNote.Status;
            dto.StatusString = L[$"Enum:VendorDebitNoteStatus:{(int)queryResult.vendorDebitNote.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<VendorDebitNoteDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorDebitNoteDetailRepository.GetQueryableAsync();
            var query = from vendorDebitNoteDetail in queryable
                        join vendorDebitNote in _vendorDebitNoteRepository on vendorDebitNoteDetail.VendorDebitNoteId equals vendorDebitNote.Id
                        join uom in _uomRepository on vendorDebitNoteDetail.UomId equals uom.Id
                        select new { vendorDebitNoteDetail, vendorDebitNote, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorDebitNoteDetail, VendorDebitNoteDetailReadDto>(x.vendorDebitNoteDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.vendorDebitNote.Status;
                dto.StatusString = L[$"Enum:VendorDebitNoteStatus:{(int)x.vendorDebitNote.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _vendorDebitNoteDetailRepository.GetCountAsync();

            return new PagedResultDto<VendorDebitNoteDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<VendorDebitNoteDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorDebitNoteDetailRepository.GetQueryableAsync();
            var query = from vendorDebitNoteDetail in queryable
                        join vendorDebitNote in _vendorDebitNoteRepository on vendorDebitNoteDetail.VendorDebitNoteId equals vendorDebitNote.Id
                        join vendor in _vendorRepository on vendorDebitNote.VendorId equals vendor.Id
                        join uom in _uomRepository on vendorDebitNoteDetail.UomId equals uom.Id
                        select new { vendorDebitNoteDetail, vendorDebitNote, vendor, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorDebitNoteDetail, VendorDebitNoteDetailReadDto>(x.vendorDebitNoteDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.vendorDebitNote.Status;
                dto.StatusString = L[$"Enum:VendorDebitNoteStatus:{(int)x.vendorDebitNote.Status}"];
                dto.VendorDebitNoteNumber = x.vendorDebitNote.Number;
                dto.DebitNoteDate = x.vendorDebitNote.DebitNoteDate;
                dto.VendorName = x.vendor.Name;
                dto.PriceString = x.vendorDebitNoteDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.vendorDebitNoteDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.vendorDebitNoteDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.vendorDebitNoteDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.vendorDebitNoteDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.vendorDebitNoteDetail.Total.ToString("##,##.00");
                dto.Period = x.vendorDebitNote.DebitNoteDate.ToString("yyyy-MM");
                return dto;
            })
                .OrderByDescending(x => x.DebitNoteDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<VendorDebitNoteDetailReadDto>> GetListByVendorDebitNoteAsync(Guid vendorDebitNoteId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _vendorDebitNoteDetailRepository.GetQueryableAsync();
            var query = from vendorDebitNoteDetail in queryable
                        join vendorDebitNote in _vendorDebitNoteRepository on vendorDebitNoteDetail.VendorDebitNoteId equals vendorDebitNote.Id
                        join uom in _uomRepository on vendorDebitNoteDetail.UomId equals uom.Id
                        where vendorDebitNoteDetail.VendorDebitNoteId.Equals(vendorDebitNoteId)
                        select new { vendorDebitNoteDetail, vendorDebitNote, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<VendorDebitNoteDetail, VendorDebitNoteDetailReadDto>(x.vendorDebitNoteDetail);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.vendorDebitNote.Status;
                dto.StatusString = L[$"Enum:VendorDebitNoteStatus:{(int)x.vendorDebitNote.Status}"];
                dto.PriceString = x.vendorDebitNoteDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.vendorDebitNoteDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.vendorDebitNoteDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.vendorDebitNoteDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.vendorDebitNoteDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.vendorDebitNoteDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<VendorDebitNoteDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<VendorDebitNoteLookupDto>> GetVendorDebitNoteLookupAsync()
        {
            var list = await _vendorDebitNoteRepository.GetListAsync();
            return new ListResultDto<VendorDebitNoteLookupDto>(
                ObjectMapper.Map<List<VendorDebitNote>, List<VendorDebitNoteLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<UomLookupDto>> GetUomLookupAsync()
        {
            var list = await _uomRepository.GetListAsync();
            return new ListResultDto<UomLookupDto>(
                ObjectMapper.Map<List<Uom>, List<UomLookupDto>>(list)
            );
        }
        public async Task<VendorDebitNoteDetailReadDto> CreateAsync(VendorDebitNoteDetailCreateDto input)
        {
            var obj = await _vendorDebitNoteDetailManager.CreateAsync(
                input.VendorDebitNoteId,
                input.ProductName,
                input.UomId,
                input.Price,
                input.TaxRate,
                input.Quantity,
                input.DiscAmt
            );
            await _vendorDebitNoteDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<VendorDebitNoteDetail, VendorDebitNoteDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, VendorDebitNoteDetailUpdateDto input)
        {
            var obj = await _vendorDebitNoteDetailRepository.GetAsync(id);

            obj.ProductName = input.ProductName;
            obj.UomId = input.UomId;
            obj.Price = input.Price;
            obj.TaxRate = input.TaxRate;
            obj.Quantity = input.Quantity;
            obj.DiscAmt = input.DiscAmt;

            obj.Recalculate();

            await _vendorDebitNoteDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _vendorDebitNoteDetailRepository.DeleteAsync(id);
        }
    }
}
