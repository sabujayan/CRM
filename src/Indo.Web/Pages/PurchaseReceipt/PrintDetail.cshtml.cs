using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Vendors;
using Indo.PurchaseReceipts;
using Indo.PurchaseReceiptDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.PurchaseReceipt
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public PurchaseReceiptViewModel PurchaseReceipt { get; set; }
        public CompanyViewModel Company { get; set; }        
        public VendorViewModel Vendor { get; set; }
        public PagedResultDto<PurchaseReceiptDetailViewModel> Details { get; set; }

        private readonly IPurchaseReceiptAppService _purchaseReceiptAppService;

        private readonly IPurchaseReceiptDetailAppService _purchaseReceiptDetailAppService;

        private readonly IVendorAppService _vendorAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDetailModel(
            IPurchaseReceiptAppService purchaseReceiptAppService,
            IPurchaseReceiptDetailAppService purchaseReceiptDetailAppService,
            CompanyAppService companyAppService,
            IVendorAppService vendorAppService
            )
        {
            _purchaseReceiptAppService = purchaseReceiptAppService;
            _companyAppService = companyAppService;
            _vendorAppService = vendorAppService;
            _purchaseReceiptDetailAppService = purchaseReceiptDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            PurchaseReceipt = ObjectMapper.Map<PurchaseReceiptReadDto, PurchaseReceiptViewModel>(await _purchaseReceiptAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Vendor = ObjectMapper.Map<VendorReadDto, VendorViewModel>(await _vendorAppService.GetAsync(PurchaseReceipt.VendorId));

            var dtos = await _purchaseReceiptDetailAppService.GetListByPurchaseReceiptAsync(PurchaseReceipt.Id);

            Details = ObjectMapper.Map<PagedResultDto<PurchaseReceiptDetailReadDto>, PagedResultDto<PurchaseReceiptDetailViewModel>>(dtos);

        }

        public class PurchaseReceiptViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid VendorId { get; set; }
            public Guid PurchaseOrderId { get; set; }
            public string Number { get; set; }
            public string PurchaseOrderNumber { get; set; }
            public string Description { get; set; }
            public DateTime ReceiptDate { get; set; }
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

        public class PurchaseReceiptDetailViewModel
        {
            public Guid PurchaseReceiptId { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public string UomName { get; set; }
            public float Quantity { get; set; }

        }
    }
}
