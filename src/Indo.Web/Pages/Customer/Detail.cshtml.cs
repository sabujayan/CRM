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

namespace Indo.Web.Pages.Customer
{
    public class DetailModel : IndoPageModel
    {
        public DetailCustomerViewModel Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;
        public DetailModel(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _customerAppService.GetAsync(id);
            Customer = ObjectMapper.Map<CustomerReadDto, DetailCustomerViewModel>(dto);
        }

        public class DetailCustomerViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string RootFolder { get; set; }

        }


	}
}
