using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.VendorBills;
using Indo.Vendors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorBill
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public VendorBillViewModel VendorBill { get; set; }
        public VendorViewModel Vendor { get; set; }

        private readonly IVendorAppService _customerAppService;

        private readonly IVendorBillAppService _vendorBillAppService;
        public DetailModel(
            IVendorBillAppService vendorBillAppService,
            IVendorAppService customerAppService
            )
        {
            _vendorBillAppService = vendorBillAppService;
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            VendorBill = ObjectMapper.Map<VendorBillReadDto, VendorBillViewModel>(await _vendorBillAppService.GetAsync(id));

            Vendor = ObjectMapper.Map<VendorReadDto, VendorViewModel>(await _customerAppService.GetAsync(VendorBill.VendorId));
        }

        public class VendorBillViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid VendorId { get; set; }
            public string VendorName { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime BillDate { get; set; }
            public DateTime BillDueDate { get; set; }
            public string CurrencyName { get; set; }
            public VendorBillStatus Status { get; set; }
            public string SourceDocument { get; set; }
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
