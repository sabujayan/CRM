using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.VendorBillDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorBill
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateVendorBillDetailViewModel VendorBillDetail { get; set; }
        public List<SelectListItem> VendorBills { get; set; }
        public List<SelectListItem> Uoms { get; set; }

        private readonly IVendorBillDetailAppService _vendorBillDetailAppService;
        public CreateDetailModel(IVendorBillDetailAppService vendorBillDetailAppService)
        {
            _vendorBillDetailAppService = vendorBillDetailAppService;
        }
        public async Task OnGetAsync(Guid vendorBillId)
        {
            VendorBillDetail = new CreateVendorBillDetailViewModel();
            VendorBillDetail.VendorBillId = vendorBillId;

            var vendorBillLookup = await _vendorBillDetailAppService.GetVendorBillLookupAsync();
            VendorBills = vendorBillLookup.Items
                .Where(x => x.Id.Equals(vendorBillId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var uomLookup = await _vendorBillDetailAppService.GetUomLookupAsync();
            Uoms = uomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _vendorBillDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateVendorBillDetailViewModel, VendorBillDetailCreateDto>(VendorBillDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateVendorBillDetailViewModel
        {
            [Required]
            [SelectItems(nameof(VendorBills))]
            [DisplayName("Vendor Bill")]
            public Guid VendorBillId { get; set; }

            [Required]
            [SelectItems(nameof(Uoms))]
            [DisplayName("Uom")]
            public Guid UomId { get; set; }

            [Required]
            [DisplayName("Item")]
            public string ProductName { get; set; }

            [Required]
            public float Quantity { get; set; }

            [DisplayName("Discount Amount")]
            public float DiscAmt { get; set; }

            [DisplayName("Tax Rate(%)")]
            public float TaxRate { get; set; }

            [Required]
            public float Price { get; set; }
        }
    }
}
