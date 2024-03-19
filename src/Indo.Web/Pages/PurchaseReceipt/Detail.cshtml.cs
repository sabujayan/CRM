using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.PurchaseReceipts;
using Indo.Vendors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.PurchaseReceipt
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public PurchaseReceiptViewModel PurchaseReceipt { get; set; }
        public VendorViewModel Vendor { get; set; }

        private readonly IVendorAppService _vendorAppService;

        private readonly IPurchaseReceiptAppService _purchaseReceiptAppService;
        public DetailModel(
            IPurchaseReceiptAppService purchaseReceiptAppService,
            IVendorAppService vendorAppService
            )
        {
            _purchaseReceiptAppService = purchaseReceiptAppService;
            _vendorAppService = vendorAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            PurchaseReceipt = ObjectMapper.Map<PurchaseReceiptReadDto, PurchaseReceiptViewModel>(await _purchaseReceiptAppService.GetAsync(id));

            Vendor = ObjectMapper.Map<VendorReadDto, VendorViewModel>(await _vendorAppService.GetAsync(PurchaseReceipt.VendorId));
        }

        public class PurchaseReceiptViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid PurchaseOrderId { get; set; }
            public Guid VendorId { get; set; }
            public string Number { get; set; }
            public string PurchaseOrderNumber { get; set; }
            public string Description { get; set; }
            public DateTime ReceiptDate { get; set; }
            public PurchaseReceiptStatus Status { get; set; }
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
    }
}
