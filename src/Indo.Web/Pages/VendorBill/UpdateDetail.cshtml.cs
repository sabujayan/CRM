using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.VendorBillDetails;
using Indo.VendorBills;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorBill
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public VendorBillDetailUpdateViewModel VendorBillDetail { get; set; }
        public List<SelectListItem> VendorBills { get; set; }
        public List<SelectListItem> Uoms { get; set; }
        public VendorBillStatus Status { get; set; }

        private readonly IVendorBillDetailAppService _vendorBillDetailAppService;
        public UpdateDetailModel(IVendorBillDetailAppService vendorBillDetailAppService)
        {
            _vendorBillDetailAppService = vendorBillDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _vendorBillDetailAppService.GetAsync(id);
            Status = dto.Status;
            VendorBillDetail = ObjectMapper.Map<VendorBillDetailReadDto, VendorBillDetailUpdateViewModel>(dto);

            var vendorBillLookup = await _vendorBillDetailAppService.GetVendorBillLookupAsync();
            VendorBills = vendorBillLookup.Items
                .Where(x => x.Id.Equals(VendorBillDetail.VendorBillId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var UomLookup = await _vendorBillDetailAppService.GetUomLookupAsync();
            Uoms = UomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _vendorBillDetailAppService.UpdateAsync(
                    VendorBillDetail.Id,
                    ObjectMapper.Map<VendorBillDetailUpdateViewModel, VendorBillDetailUpdateDto>(VendorBillDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class VendorBillDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
