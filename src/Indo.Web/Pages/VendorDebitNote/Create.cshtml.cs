using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.VendorDebitNotes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorDebitNote
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateVendorDebitNoteViewModel VendorDebitNote { get; set; }
        public List<SelectListItem> Vendors { get; set; }
        public List<SelectListItem> VendorBills { get; set; }

        private readonly IVendorDebitNoteAppService _vendorDebitNoteAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IVendorDebitNoteAppService vendorDebitNoteAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _vendorDebitNoteAppService = vendorDebitNoteAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            VendorDebitNote = new CreateVendorDebitNoteViewModel();
            VendorDebitNote.DebitNoteDate = DateTime.Now;
            VendorDebitNote.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.DebitNote);

            var vendorLookup = await _vendorDebitNoteAppService.GetVendorLookupAsync();
            Vendors = vendorLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var vendorBillLookup = await _vendorDebitNoteAppService.GetVendorBillLookupAsync();
            VendorBills = vendorBillLookup.Items
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _vendorDebitNoteAppService.CreateAsync(
                    ObjectMapper.Map<CreateVendorDebitNoteViewModel, VendorDebitNoteCreateDto>(VendorDebitNote)
                    );
                return NoContent();

            }
            catch (VendorDebitNoteAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateVendorDebitNoteViewModel
        {
            [Required]
            [SelectItems(nameof(Vendors))]
            [DisplayName("Vendor")]
            public Guid VendorId { get; set; }

            [Required]
            [SelectItems(nameof(VendorBills))]
            [DisplayName("Vendor Invoice")]
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
