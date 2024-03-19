using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerInvoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerInvoice
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public CustomerInvoiceUpdateViewModel CustomerInvoice { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly ICustomerInvoiceAppService _customerInvoiceAppService;
        public UpdateModel(ICustomerInvoiceAppService customerInvoiceAppService)
        {
            _customerInvoiceAppService = customerInvoiceAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _customerInvoiceAppService.GetAsync(id);
            CustomerInvoice = ObjectMapper.Map<CustomerInvoiceReadDto, CustomerInvoiceUpdateViewModel>(dto);

            var customerLookup = await _customerInvoiceAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Where(x => x.Id.Equals(dto.CustomerId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerInvoiceAppService.UpdateAsync(
                    CustomerInvoice.Id,
                    ObjectMapper.Map<CustomerInvoiceUpdateViewModel, CustomerInvoiceUpdateDto>(CustomerInvoice)
                );
                return NoContent();

            }
            catch (CustomerInvoiceAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class CustomerInvoiceUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
