using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Vendors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Vendor
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateVendorViewModel Vendor { get; set; }

        private readonly IVendorAppService _vendorAppService;
        public CreateModel(IVendorAppService vendorAppService)
        {
            _vendorAppService = vendorAppService;
        }
        public void OnGet()
        {
            Vendor = new CreateVendorViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateVendorViewModel, VendorCreateDto>(Vendor);
                await _vendorAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (VendorAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateVendorViewModel
        {
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
