using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Vendors;
using Indo.VendorDebitNotes;
using Indo.VendorDebitNoteDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.VendorDebitNote
{
    public class PrintDebitNoteModel : IndoPageModel
    {
        [BindProperty]
        public VendorDebitNoteViewModel VendorDebitNote { get; set; }
        public CompanyViewModel Company { get; set; }        
        public VendorViewModel Vendor { get; set; }
        public PagedResultDto<VendorDebitNoteDetailViewModel> Details { get; set; }
        public float Total { get; set; }
        public float SubTotal { get; set; }
        public float DiscAmt { get; set; }
        public float BeforeTax { get; set; }
        public float TaxAmount { get; set; }

        private readonly IVendorDebitNoteAppService _vendorDebitNoteAppService;

        private readonly IVendorDebitNoteDetailAppService _vendorDebitNoteDetailAppService;

        private readonly IVendorAppService _vendorAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDebitNoteModel(
            IVendorDebitNoteAppService vendorDebitNoteAppService,
            IVendorDebitNoteDetailAppService vendorDebitNoteDetailAppService,
            CompanyAppService companyAppService,
            IVendorAppService vendorAppService
            )
        {
            _vendorDebitNoteAppService = vendorDebitNoteAppService;
            _companyAppService = companyAppService;
            _vendorAppService = vendorAppService;
            _vendorDebitNoteDetailAppService = vendorDebitNoteDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            VendorDebitNote = ObjectMapper.Map<VendorDebitNoteReadDto, VendorDebitNoteViewModel>(await _vendorDebitNoteAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Vendor = ObjectMapper.Map<VendorReadDto, VendorViewModel>(await _vendorAppService.GetAsync(VendorDebitNote.VendorId));

            Total = await _vendorDebitNoteAppService.GetSummaryTotalAsync(VendorDebitNote.Id);

            SubTotal = await _vendorDebitNoteAppService.GetSummarySubTotalAsync(VendorDebitNote.Id);

            DiscAmt = await _vendorDebitNoteAppService.GetSummaryDiscAmtAsync(VendorDebitNote.Id);

            BeforeTax = await _vendorDebitNoteAppService.GetSummaryBeforeTaxAsync(VendorDebitNote.Id);

            TaxAmount = await _vendorDebitNoteAppService.GetSummaryTaxAmountAsync(VendorDebitNote.Id);

            var dtos = await _vendorDebitNoteDetailAppService.GetListByVendorDebitNoteAsync(VendorDebitNote.Id);

            Details = ObjectMapper.Map<PagedResultDto<VendorDebitNoteDetailReadDto>, PagedResultDto<VendorDebitNoteDetailViewModel>>(dtos);

        }

        public class VendorDebitNoteViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid VendorId { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public string PaymentNote { get; set; }
            public DateTime DebitNoteDate { get; set; }
            public string CurrencyName { get; set; }
            public string VendorBillNumber { get; set; }
        }

        public class CompanyViewModel
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public Guid CurrencyId { get; set; }
            public string CurrencyName { get; set; }
        }

        public class VendorViewModel
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
        }

        public class VendorDebitNoteDetailViewModel
        {
            public Guid VendorDebitNoteId { get; set; }
            public Guid ServiceId { get; set; }
            public string ProductName { get; set; }
            public string UomName { get; set; }
            public float Price { get; set; }
            public string CurrencyName { get; set; }
            public float Quantity { get; set; }
            public float DiscAmt { get; set; }
            public float BeforeTax { get; set; }
            public float TaxAmount { get; set; }
            public float Total { get; set; }

        }
    }
}
