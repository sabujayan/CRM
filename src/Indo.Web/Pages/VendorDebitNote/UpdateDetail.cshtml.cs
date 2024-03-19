using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.VendorDebitNoteDetails;
using Indo.VendorDebitNotes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorDebitNote
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public VendorDebitNoteDetailUpdateViewModel VendorDebitNoteDetail { get; set; }
        public List<SelectListItem> VendorDebitNotes { get; set; }
        public List<SelectListItem> Uoms { get; set; }
        public VendorDebitNoteStatus Status { get; set; }

        private readonly IVendorDebitNoteDetailAppService _vendorDebitNoteDetailAppService;
        public UpdateDetailModel(IVendorDebitNoteDetailAppService vendorDebitNoteDetailAppService)
        {
            _vendorDebitNoteDetailAppService = vendorDebitNoteDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _vendorDebitNoteDetailAppService.GetAsync(id);
            Status = dto.Status;
            VendorDebitNoteDetail = ObjectMapper.Map<VendorDebitNoteDetailReadDto, VendorDebitNoteDetailUpdateViewModel>(dto);

            var vendorDebitNoteLookup = await _vendorDebitNoteDetailAppService.GetVendorDebitNoteLookupAsync();
            VendorDebitNotes = vendorDebitNoteLookup.Items
                .Where(x => x.Id.Equals(VendorDebitNoteDetail.VendorDebitNoteId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var UomLookup = await _vendorDebitNoteDetailAppService.GetUomLookupAsync();
            Uoms = UomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _vendorDebitNoteDetailAppService.UpdateAsync(
                    VendorDebitNoteDetail.Id,
                    ObjectMapper.Map<VendorDebitNoteDetailUpdateViewModel, VendorDebitNoteDetailUpdateDto>(VendorDebitNoteDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class VendorDebitNoteDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(VendorDebitNotes))]
            [DisplayName("Vendor DebitNote")]
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
