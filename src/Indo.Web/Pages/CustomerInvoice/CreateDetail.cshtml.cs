using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerInvoiceDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerInvoice
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateCustomerInvoiceDetailViewModel CustomerInvoiceDetail { get; set; }
        public List<SelectListItem> CustomerInvoices { get; set; }
        public List<SelectListItem> Uoms { get; set; }

        private readonly ICustomerInvoiceDetailAppService _customerInvoiceDetailAppService;
        public CreateDetailModel(ICustomerInvoiceDetailAppService customerInvoiceDetailAppService)
        {
            _customerInvoiceDetailAppService = customerInvoiceDetailAppService;
        }
        public async Task OnGetAsync(Guid customerInvoiceId)
        {
            CustomerInvoiceDetail = new CreateCustomerInvoiceDetailViewModel();
            CustomerInvoiceDetail.CustomerInvoiceId = customerInvoiceId;

            var customerInvoiceLookup = await _customerInvoiceDetailAppService.GetCustomerInvoiceLookupAsync();
            CustomerInvoices = customerInvoiceLookup.Items
                .Where(x => x.Id.Equals(customerInvoiceId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var uomLookup = await _customerInvoiceDetailAppService.GetUomLookupAsync();
            Uoms = uomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerInvoiceDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateCustomerInvoiceDetailViewModel, CustomerInvoiceDetailCreateDto>(CustomerInvoiceDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateCustomerInvoiceDetailViewModel
        {
            [Required]
            [SelectItems(nameof(CustomerInvoices))]
            [DisplayName("Customer Invoice")]
            public Guid CustomerInvoiceId { get; set; }

            [Required]
            [SelectItems(nameof(Uoms))]
            [DisplayName("Uom")]
            public Guid UomId { get; set; }

            [Required]
            [DisplayName("Item")]
            public string ProductName { get; set; }

            [Required]
            public float Quantity { get; set; }

            [DisplayName("Discount Amount")]
            public float DiscAmt { get; set; }

            [DisplayName("Tax Rate(%)")]
            public float TaxRate { get; set; }

            [Required]
            public float Price { get; set; }
        }
    }
}
