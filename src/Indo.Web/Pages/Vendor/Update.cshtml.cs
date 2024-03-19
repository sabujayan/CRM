using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Vendors;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Vendor
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public VendorUpdateViewModel Vendor { get; set; }

        private readonly IVendorAppService _vendorAppService;
        public UpdateModel(IVendorAppService vendorAppService)
        {
            _vendorAppService = vendorAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _vendorAppService.GetAsync(id);
            Vendor = ObjectMapper.Map<VendorReadDto, VendorUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _vendorAppService.UpdateAsync(
                    Vendor.Id,
                    ObjectMapper.Map<VendorUpdateViewModel, VendorUpdateDto>(Vendor)
                    );
                return NoContent();
            }
            catch (VendorAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class VendorUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(VendorConsts.MaxNameLength)]
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

            [TextArea]
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
        }
    }
}
