using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerCreditNotes;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerCreditNote
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public CustomerCreditNoteViewModel CustomerCreditNote { get; set; }
        public CustomerViewModel Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;

        private readonly ICustomerCreditNoteAppService _customerCreditNoteAppService;
        public DetailModel(
            ICustomerCreditNoteAppService customerCreditNoteAppService,
            ICustomerAppService customerAppService
            )
        {
            _customerCreditNoteAppService = customerCreditNoteAppService;
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            CustomerCreditNote = ObjectMapper.Map<CustomerCreditNoteReadDto, CustomerCreditNoteViewModel>(await _customerCreditNoteAppService.GetAsync(id));

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(CustomerCreditNote.CustomerId));
        }

        public class CustomerCreditNoteViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public string CustomerName { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime CreditNoteDate { get; set; }
            public string CurrencyName { get; set; }
            public CustomerCreditNoteStatus Status { get; set; }
            public string CustomerInvoiceNumber { get; set; }
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
