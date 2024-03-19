using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesQuotations;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesQuotation
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public SalesQuotationViewModel SalesQuotation { get; set; }
        public CustomerViewModel Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;

        private readonly ISalesQuotationAppService _salesQuotationAppService;
        public DetailModel(
            ISalesQuotationAppService salesQuotationAppService,
            ICustomerAppService customerAppService
            )
        {
            _salesQuotationAppService = salesQuotationAppService;
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            SalesQuotation = ObjectMapper.Map<SalesQuotationReadDto, SalesQuotationViewModel>(await _salesQuotationAppService.GetAsync(id));

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(SalesQuotation.CustomerId));
        }

        public class SalesQuotationViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public string CustomerName { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime QuotationDate { get; set; }
            public DateTime QuotationValidUntilDate { get; set; }
            public string CurrencyName { get; set; }
            public SalesQuotationStatus Status { get; set; }
        }

        public class CustomerViewModel
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
        }
    }
}
