using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerCreditNoteDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerCreditNote
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateCustomerCreditNoteDetailViewModel CustomerCreditNoteDetail { get; set; }
        public List<SelectListItem> CustomerCreditNotes { get; set; }
        public List<SelectListItem> Uoms { get; set; }

        private readonly ICustomerCreditNoteDetailAppService _customerCreditNoteDetailAppService;
        public CreateDetailModel(ICustomerCreditNoteDetailAppService customerCreditNoteDetailAppService)
        {
            _customerCreditNoteDetailAppService = customerCreditNoteDetailAppService;
        }
        public async Task OnGetAsync(Guid customerCreditNoteId)
        {
            CustomerCreditNoteDetail = new CreateCustomerCreditNoteDetailViewModel();
            CustomerCreditNoteDetail.CustomerCreditNoteId = customerCreditNoteId;

            var customerCreditNoteLookup = await _customerCreditNoteDetailAppService.GetCustomerCreditNoteLookupAsync();
            CustomerCreditNotes = customerCreditNoteLookup.Items
                .Where(x => x.Id.Equals(customerCreditNoteId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var uomLookup = await _customerCreditNoteDetailAppService.GetUomLookupAsync();
            Uoms = uomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerCreditNoteDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateCustomerCreditNoteDetailViewModel, CustomerCreditNoteDetailCreateDto>(CustomerCreditNoteDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateCustomerCreditNoteDetailViewModel
        {
            [Required]
            [SelectItems(nameof(CustomerCreditNotes))]
            [DisplayName("Customer Credit Note")]
            public Guid CustomerCreditNoteId { get; set; }

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
