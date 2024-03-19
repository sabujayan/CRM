using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Customer
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateCustomerViewModel Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;
        public CreateModel(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        public void OnGet()
        {
            Customer = new CreateCustomerViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateCustomerViewModel, CustomerCreateDto>(Customer);
                dto.Status = CustomerStatus.Customer;
                dto.Stage = CustomerStage.SalesQualified;
                await _customerAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (CustomerAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateCustomerViewModel
        {
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
        }
    }
}
