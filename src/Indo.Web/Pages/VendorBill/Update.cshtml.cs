using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.VendorBills;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorBill
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public VendorBillUpdateViewModel VendorBill { get; set; }
        public List<SelectListItem> Vendors { get; set; }

        private readonly IVendorBillAppService _vendorBillAppService;
        public UpdateModel(IVendorBillAppService vendorBillAppService)
        {
            _vendorBillAppService = vendorBillAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _vendorBillAppService.GetAsync(id);
            VendorBill = ObjectMapper.Map<VendorBillReadDto, VendorBillUpdateViewModel>(dto);

            var vendorLookup = await _vendorBillAppService.GetVendorLookupAsync();
            Vendors = vendorLookup.Items
                .Where(x => x.Id.Equals(dto.VendorId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _vendorBillAppService.UpdateAsync(
                    VendorBill.Id,
                    ObjectMapper.Map<VendorBillUpdateViewModel, VendorBillUpdateDto>(VendorBill)
                );
                return NoContent();

            }
            catch (VendorBillAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class VendorBillUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(Vendors))]
            [DisplayName("Vendor")]
            public Guid VendorId { get; set; }

            [Required]
            [StringLength(VendorBillConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [TextArea]
            [DisplayName("Term and Condition")]
            public string TermCondition { get; set; }

            [TextArea]
            [DisplayName("Payment Note")]
            public string PaymentNote { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Bill Date")]
            public DateTime BillDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Bill Due Date")]
            public DateTime BillDueDate { get; set; }
        }
    }
}
