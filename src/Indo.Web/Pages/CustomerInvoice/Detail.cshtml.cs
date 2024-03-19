using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerInvoices;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerInvoice
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public CustomerInvoiceViewModel CustomerInvoice { get; set; }
        public CustomerViewModel Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;

        private readonly ICustomerInvoiceAppService _customerInvoiceAppService;
        public DetailModel(
            ICustomerInvoiceAppService customerInvoiceAppService,
            ICustomerAppService customerAppService
            )
        {
            _customerInvoiceAppService = customerInvoiceAppService;
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            CustomerInvoice = ObjectMapper.Map<CustomerInvoiceReadDto, CustomerInvoiceViewModel>(await _customerInvoiceAppService.GetAsync(id));

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(CustomerInvoice.CustomerId));
        }

        public class CustomerInvoiceViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public string CustomerName { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime InvoiceDate { get; set; }
            public DateTime InvoiceDueDate { get; set; }
            public string CurrencyName { get; set; }
            public CustomerInvoiceStatus Status { get; set; }
            public string SourceDocument { get; set; }
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
