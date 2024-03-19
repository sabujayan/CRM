using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Lead
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public CustomerUpdateViewModel Customer { get; set; }
        public List<SelectListItem> LeadRatings { get; set; }
        public List<SelectListItem> LeadSources { get; set; }

        private readonly ICustomerAppService _customerAppService;
        public UpdateModel(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _customerAppService.GetAsync(id);
            Customer = ObjectMapper.Map<CustomerReadDto, CustomerUpdateViewModel>(dto);

            var leadRatingLookup = await _customerAppService.GetLeadRatingLookupAsync();
            LeadRatings = leadRatingLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var leadSourceLookup = await _customerAppService.GetLeadSourceLookupAsync();
            LeadSources = leadSourceLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerAppService.UpdateAsync(
                    Customer.Id,
                    ObjectMapper.Map<CustomerUpdateViewModel, CustomerUpdateDto>(Customer)
                    );
                return NoContent();
            }
            catch (CustomerAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CustomerUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(CustomerConsts.MaxNameLength)]
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

            [TextArea]
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }

            [SelectItems(nameof(LeadRatings))]
            [DisplayName("Lead Rating")]
            public Guid LeadRatingId { get; set; }

            [SelectItems(nameof(LeadSources))]
            [DisplayName("Lead Source")]
            public Guid LeadSourceId { get; set; }
            public CustomerStage Stage { get; set; }
        }
    }
}
