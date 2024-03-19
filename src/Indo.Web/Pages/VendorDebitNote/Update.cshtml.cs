using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.VendorDebitNotes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorDebitNote
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public VendorDebitNoteUpdateViewModel VendorDebitNote { get; set; }
        public List<SelectListItem> Vendors { get; set; }
        public List<SelectListItem> VendorBills { get; set; }

        private readonly IVendorDebitNoteAppService _vendorDebitNoteAppService;
        public UpdateModel(IVendorDebitNoteAppService vendorDebitNoteAppService)
        {
            _vendorDebitNoteAppService = vendorDebitNoteAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _vendorDebitNoteAppService.GetAsync(id);
            VendorDebitNote = ObjectMapper.Map<VendorDebitNoteReadDto, VendorDebitNoteUpdateViewModel>(dto);

            var vendorLookup = await _vendorDebitNoteAppService.GetVendorLookupAsync();
            Vendors = vendorLookup.Items
                .Where(x => x.Id.Equals(dto.VendorId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var vendorBillLookup = await _vendorDebitNoteAppService.GetVendorBillLookupAsync();
            VendorBills = vendorBillLookup.Items
                .Where(x => x.Id.Equals(dto.VendorBillId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _vendorDebitNoteAppService.UpdateAsync(
                    VendorDebitNote.Id,
                    ObjectMapper.Map<VendorDebitNoteUpdateViewModel, VendorDebitNoteUpdateDto>(VendorDebitNote)
                );
                return NoContent();

            }
            catch (VendorDebitNoteAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class VendorDebitNoteUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(Vendors))]
            [DisplayName("Vendor")]
            public Guid VendorId { get; set; }

            [Required]
            [SelectItems(nameof(VendorBills))]
            [DisplayName("Vendor Bill")]
            public Guid VendorBillId { get; set; }

            [Required]
            [StringLength(VendorDebitNoteConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [TextArea]
            [DisplayName("Payment Note")]
            public string PaymentNote { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Debit Note Date")]
            public DateTime DebitNoteDate { get; set; }
        }
    }
}
