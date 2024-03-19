using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Contact
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public ContactUpdateViewModel Contact { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly IContactAppService _contactAppService;
        public UpdateModel(IContactAppService contactAppService)
        {
            _contactAppService = contactAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _contactAppService.GetAsync(id);
            Contact = ObjectMapper.Map<ContactReadDto, ContactUpdateViewModel>(dto);

            var customerLookup = await _contactAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Where(x => x.Id.Equals(Contact.CustomerId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _contactAppService.UpdateAsync(
                    Contact.Id,
                    ObjectMapper.Map<ContactUpdateViewModel, ContactUpdateDto>(Contact)
                    );
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"{ex.InnerException.Message}");
            }
        }
        public class ContactUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
