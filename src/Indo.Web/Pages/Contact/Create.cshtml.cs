using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Contact
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateContactViewModel Contact { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly IContactAppService _contactAppService;
        public CreateModel(IContactAppService contactAppService)
        {
            _contactAppService = contactAppService;
        }
        public async Task OnGet()
        {
            Contact = new CreateContactViewModel();

            var customerLookup = await _contactAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateContactViewModel, ContactCreateDto>(Contact);
                await _contactAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"{ex.InnerException.Message}");
            }
        }
        public class CreateContactViewModel
        {
            [Required]
            [StringLength(ContactConsts.MaxNameLength)]
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

            [TextArea]
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }

            [Required]
            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }
        }
    }
}
