using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Customer
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public CustomerUpdateViewModel Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;
        public UpdateModel(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _customerAppService.GetAsync(id);
            Customer = ObjectMapper.Map<CustomerReadDto, CustomerUpdateViewModel>(dto);
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

            [HiddenInput]
            public Guid LeadRatingId { get; set; }

            [HiddenInput]
            public Guid LeadSourceId { get; set; }

            [HiddenInput]
            public CustomerStage Stage { get; set; }
        }
    }
}
