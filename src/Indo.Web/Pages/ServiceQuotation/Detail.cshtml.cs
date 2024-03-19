using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ServiceQuotations;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ServiceQuotation
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public ServiceQuotationViewModel ServiceQuotation { get; set; }
        public CustomerViewModel Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;

        private readonly IServiceQuotationAppService _serviceQuotationAppService;
        public DetailModel(
            IServiceQuotationAppService serviceQuotationAppService,
            ICustomerAppService customerAppService
            )
        {
            _serviceQuotationAppService = serviceQuotationAppService;
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            ServiceQuotation = ObjectMapper.Map<ServiceQuotationReadDto, ServiceQuotationViewModel>(await _serviceQuotationAppService.GetAsync(id));

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(ServiceQuotation.CustomerId));
        }

        public class ServiceQuotationViewModel
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
            public ServiceQuotationStatus Status { get; set; }
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
