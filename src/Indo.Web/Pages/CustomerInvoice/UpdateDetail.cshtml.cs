using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerInvoiceDetails;
using Indo.CustomerInvoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerInvoice
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CustomerInvoiceDetailUpdateViewModel CustomerInvoiceDetail { get; set; }
        public List<SelectListItem> CustomerInvoices { get; set; }
        public List<SelectListItem> Uoms { get; set; }
        public CustomerInvoiceStatus Status { get; set; }

        private readonly ICustomerInvoiceDetailAppService _customerInvoiceDetailAppService;
        public UpdateDetailModel(ICustomerInvoiceDetailAppService customerInvoiceDetailAppService)
        {
            _customerInvoiceDetailAppService = customerInvoiceDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _customerInvoiceDetailAppService.GetAsync(id);
            Status = dto.Status;
            CustomerInvoiceDetail = ObjectMapper.Map<CustomerInvoiceDetailReadDto, CustomerInvoiceDetailUpdateViewModel>(dto);

            var customerInvoiceLookup = await _customerInvoiceDetailAppService.GetCustomerInvoiceLookupAsync();
            CustomerInvoices = customerInvoiceLookup.Items
                .Where(x => x.Id.Equals(CustomerInvoiceDetail.CustomerInvoiceId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var UomLookup = await _customerInvoiceDetailAppService.GetUomLookupAsync();
            Uoms = UomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerInvoiceDetailAppService.UpdateAsync(
                    CustomerInvoiceDetail.Id,
                    ObjectMapper.Map<CustomerInvoiceDetailUpdateViewModel, CustomerInvoiceDetailUpdateDto>(CustomerInvoiceDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class CustomerInvoiceDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
