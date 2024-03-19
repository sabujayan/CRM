using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerCreditNoteDetails;
using Indo.CustomerCreditNotes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerCreditNote
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CustomerCreditNoteDetailUpdateViewModel CustomerCreditNoteDetail { get; set; }
        public List<SelectListItem> CustomerCreditNotes { get; set; }
        public List<SelectListItem> Uoms { get; set; }
        public CustomerCreditNoteStatus Status { get; set; }

        private readonly ICustomerCreditNoteDetailAppService _customerCreditNoteDetailAppService;
        public UpdateDetailModel(ICustomerCreditNoteDetailAppService customerCreditNoteDetailAppService)
        {
            _customerCreditNoteDetailAppService = customerCreditNoteDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _customerCreditNoteDetailAppService.GetAsync(id);
            Status = dto.Status;
            CustomerCreditNoteDetail = ObjectMapper.Map<CustomerCreditNoteDetailReadDto, CustomerCreditNoteDetailUpdateViewModel>(dto);

            var customerCreditNoteLookup = await _customerCreditNoteDetailAppService.GetCustomerCreditNoteLookupAsync();
            CustomerCreditNotes = customerCreditNoteLookup.Items
                .Where(x => x.Id.Equals(CustomerCreditNoteDetail.CustomerCreditNoteId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var UomLookup = await _customerCreditNoteDetailAppService.GetUomLookupAsync();
            Uoms = UomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerCreditNoteDetailAppService.UpdateAsync(
                    CustomerCreditNoteDetail.Id,
                    ObjectMapper.Map<CustomerCreditNoteDetailUpdateViewModel, CustomerCreditNoteDetailUpdateDto>(CustomerCreditNoteDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class CustomerCreditNoteDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(CustomerCreditNotes))]
            [DisplayName("Customer CreditNote")]
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
