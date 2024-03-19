using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Vendors;
using Indo.PurchaseOrders;
using Indo.PurchaseOrderDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.PurchaseOrder
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public PurchaseOrderViewModel PurchaseOrder { get; set; }
        public CompanyViewModel Company { get; set; }        
        public VendorViewModel Vendor { get; set; }
        public PagedResultDto<PurchaseOrderDetailViewModel> Details { get; set; }
        public float Total { get; set; }
        public float SubTotal { get; set; }
        public float DiscAmt { get; set; }
        public float BeforeTax { get; set; }
        public float TaxAmount { get; set; }

        private readonly IPurchaseOrderAppService _purchaseOrderAppService;

        private readonly IPurchaseOrderDetailAppService _purchaseOrderDetailAppService;

        private readonly IVendorAppService _vendorAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDetailModel(
            IPurchaseOrderAppService purchaseOrderAppService,
            IPurchaseOrderDetailAppService purchaseOrderDetailAppService,
            CompanyAppService companyAppService,
            IVendorAppService vendorAppService
            )
        {
            _purchaseOrderAppService = purchaseOrderAppService;
            _companyAppService = companyAppService;
            _vendorAppService = vendorAppService;
            _purchaseOrderDetailAppService = purchaseOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            PurchaseOrder = ObjectMapper.Map<PurchaseOrderReadDto, PurchaseOrderViewModel>(await _purchaseOrderAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Vendor = ObjectMapper.Map<VendorReadDto, VendorViewModel>(await _vendorAppService.GetAsync(PurchaseOrder.VendorId));

            Total = await _purchaseOrderAppService.GetSummaryTotalAsync(PurchaseOrder.Id);

            SubTotal = await _purchaseOrderAppService.GetSummarySubTotalAsync(PurchaseOrder.Id);

            DiscAmt = await _purchaseOrderAppService.GetSummaryDiscAmtAsync(PurchaseOrder.Id);

            BeforeTax = await _purchaseOrderAppService.GetSummaryBeforeTaxAsync(PurchaseOrder.Id);

            TaxAmount = await _purchaseOrderAppService.GetSummaryTaxAmountAsync(PurchaseOrder.Id);

            var dtos = await _purchaseOrderDetailAppService.GetListByPurchaseOrderAsync(PurchaseOrder.Id);

            Details = ObjectMapper.Map<PagedResultDto<PurchaseOrderDetailReadDto>, PagedResultDto<PurchaseOrderDetailViewModel>>(dtos);

        }

        public class PurchaseOrderViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid VendorId { get; set; }
            public Guid BuyerId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime OrderDate { get; set; }
            public string CurrencyName { get; set; }
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

        public class PurchaseOrderDetailViewModel
        {
            public Guid PurchaseOrderId { get; set; }
            public Guid ProductId { get; set; }
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
