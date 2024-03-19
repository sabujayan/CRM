using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerCreditNotes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerCreditNote
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public CustomerCreditNoteUpdateViewModel CustomerCreditNote { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> CustomerInvoices { get; set; }

        private readonly ICustomerCreditNoteAppService _customerCreditNoteAppService;
        public UpdateModel(ICustomerCreditNoteAppService customerCreditNoteAppService)
        {
            _customerCreditNoteAppService = customerCreditNoteAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _customerCreditNoteAppService.GetAsync(id);
            CustomerCreditNote = ObjectMapper.Map<CustomerCreditNoteReadDto, CustomerCreditNoteUpdateViewModel>(dto);

            var customerLookup = await _customerCreditNoteAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Where(x => x.Id.Equals(dto.CustomerId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var customerInvoiceLookup = await _customerCreditNoteAppService.GetCustomerInvoiceLookupAsync();
            CustomerInvoices = customerInvoiceLookup.Items
                .Where(x => x.Id.Equals(dto.CustomerInvoiceId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerCreditNoteAppService.UpdateAsync(
                    CustomerCreditNote.Id,
                    ObjectMapper.Map<CustomerCreditNoteUpdateViewModel, CustomerCreditNoteUpdateDto>(CustomerCreditNote)
                );
                return NoContent();

            }
            catch (CustomerCreditNoteAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class CustomerCreditNoteUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
