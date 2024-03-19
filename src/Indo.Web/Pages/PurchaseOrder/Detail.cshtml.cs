using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.PurchaseOrders;
using Indo.Vendors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.PurchaseOrder
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public PurchaseOrderViewModel PurchaseOrder { get; set; }
        public VendorViewModel Vendor { get; set; }

        private readonly IVendorAppService _vendorAppService;

        private readonly IPurchaseOrderAppService _purchaseOrderAppService;
        public DetailModel(
            IPurchaseOrderAppService purchaseOrderAppService,
            IVendorAppService vendorAppService
            )
        {
            _purchaseOrderAppService = purchaseOrderAppService;
            _vendorAppService = vendorAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            PurchaseOrder = ObjectMapper.Map<PurchaseOrderReadDto, PurchaseOrderViewModel>(await _purchaseOrderAppService.GetAsync(id));

            Vendor = ObjectMapper.Map<VendorReadDto, VendorViewModel>(await _vendorAppService.GetAsync(PurchaseOrder.VendorId));
        }

        public class PurchaseOrderViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid VendorId { get; set; }
            public string VendorName { get; set; }
            public Guid BuyerId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime OrderDate { get; set; }
            public string CurrencyName { get; set; }
            public PurchaseOrderStatus Status { get; set; }
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
