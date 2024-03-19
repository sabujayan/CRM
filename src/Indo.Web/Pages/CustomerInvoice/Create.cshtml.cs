using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.CustomerInvoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerInvoice
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateCustomerInvoiceViewModel CustomerInvoice { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly ICustomerInvoiceAppService _customerInvoiceAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            ICustomerInvoiceAppService customerInvoiceAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _customerInvoiceAppService = customerInvoiceAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            CustomerInvoice = new CreateCustomerInvoiceViewModel();
            CustomerInvoice.InvoiceDate = DateTime.Now;
            CustomerInvoice.InvoiceDueDate = CustomerInvoice.InvoiceDate.AddDays(14);
            CustomerInvoice.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.Invoice);

            var customerLookup = await _customerInvoiceAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerInvoiceAppService.CreateAsync(
                    ObjectMapper.Map<CreateCustomerInvoiceViewModel, CustomerInvoiceCreateDto>(CustomerInvoice)
                    );
                return NoContent();

            }
            catch (CustomerInvoiceAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateCustomerInvoiceViewModel
        {
            [Required]
            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [Required]
            [StringLength(CustomerInvoiceConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [TextArea]
            [DisplayName("Term and Condition")]
            public string TermCondition { get; set; }

            [TextArea]
            [DisplayName("Payment Note")]
            public string PaymentNote { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Invoice Date")]
            public DateTime InvoiceDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Invoice Due Date")]
            public DateTime InvoiceDueDate { get; set; }
        }
    }
}
