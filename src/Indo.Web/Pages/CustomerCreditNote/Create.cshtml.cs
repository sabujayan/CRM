using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.CustomerCreditNotes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerCreditNote
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateCustomerCreditNoteViewModel CustomerCreditNote { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> CustomerInvoices { get; set; }

        private readonly ICustomerCreditNoteAppService _customerCreditNoteAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            ICustomerCreditNoteAppService customerCreditNoteAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _customerCreditNoteAppService = customerCreditNoteAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            CustomerCreditNote = new CreateCustomerCreditNoteViewModel();
            CustomerCreditNote.CreditNoteDate = DateTime.Now;
            CustomerCreditNote.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.CreditNote);

            var customerLookup = await _customerCreditNoteAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var customerInvoiceLookup = await _customerCreditNoteAppService.GetCustomerInvoiceLookupAsync();
            CustomerInvoices = customerInvoiceLookup.Items
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerCreditNoteAppService.CreateAsync(
                    ObjectMapper.Map<CreateCustomerCreditNoteViewModel, CustomerCreditNoteCreateDto>(CustomerCreditNote)
                    );
                return NoContent();

            }
            catch (CustomerCreditNoteAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateCustomerCreditNoteViewModel
        {
            [Required]
            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [Required]
            [SelectItems(nameof(CustomerInvoices))]
            [DisplayName("Customer Invoice")]
            public Guid CustomerInvoiceId { get; set; }

            [Required]
            [StringLength(CustomerCreditNoteConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [TextArea]
            [DisplayName("Payment Note")]
            public string PaymentNote { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Credit Note Date")]
            public DateTime CreditNoteDate { get; set; }
        }
    }
}
