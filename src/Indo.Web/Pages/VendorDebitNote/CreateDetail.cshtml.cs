using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.VendorDebitNoteDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorDebitNote
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateVendorDebitNoteDetailViewModel VendorDebitNoteDetail { get; set; }
        public List<SelectListItem> VendorDebitNotes { get; set; }
        public List<SelectListItem> Uoms { get; set; }

        private readonly IVendorDebitNoteDetailAppService _vendorDebitNoteDetailAppService;
        public CreateDetailModel(IVendorDebitNoteDetailAppService vendorDebitNoteDetailAppService)
        {
            _vendorDebitNoteDetailAppService = vendorDebitNoteDetailAppService;
        }
        public async Task OnGetAsync(Guid vendorDebitNoteId)
        {
            VendorDebitNoteDetail = new CreateVendorDebitNoteDetailViewModel();
            VendorDebitNoteDetail.VendorDebitNoteId = vendorDebitNoteId;

            var vendorDebitNoteLookup = await _vendorDebitNoteDetailAppService.GetVendorDebitNoteLookupAsync();
            VendorDebitNotes = vendorDebitNoteLookup.Items
                .Where(x => x.Id.Equals(vendorDebitNoteId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var uomLookup = await _vendorDebitNoteDetailAppService.GetUomLookupAsync();
            Uoms = uomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _vendorDebitNoteDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateVendorDebitNoteDetailViewModel, VendorDebitNoteDetailCreateDto>(VendorDebitNoteDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateVendorDebitNoteDetailViewModel
        {
            [Required]
            [SelectItems(nameof(VendorDebitNotes))]
            [DisplayName("Vendor Debit Note")]
            public Guid VendorDebitNoteId { get; set; }

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
