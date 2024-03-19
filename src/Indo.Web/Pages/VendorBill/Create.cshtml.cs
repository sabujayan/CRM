using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.VendorBills;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorBill
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateVendorBillViewModel VendorBill { get; set; }
        public List<SelectListItem> Vendors { get; set; }

        private readonly IVendorBillAppService _vendorBillAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IVendorBillAppService vendorBillAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _vendorBillAppService = vendorBillAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            VendorBill = new CreateVendorBillViewModel();
            VendorBill.BillDate = DateTime.Now;
            VendorBill.BillDueDate = VendorBill.BillDate.AddDays(14);
            VendorBill.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.Bill);

            var customerLookup = await _vendorBillAppService.GetVendorLookupAsync();
            Vendors = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _vendorBillAppService.CreateAsync(
                    ObjectMapper.Map<CreateVendorBillViewModel, VendorBillCreateDto>(VendorBill)
                    );
                return NoContent();

            }
            catch (VendorBillAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateVendorBillViewModel
        {
            [Required]
            [SelectItems(nameof(Vendors))]
            [DisplayName("Vendor")]
            public Guid VendorId { get; set; }

            [Required]
            [StringLength(VendorBillConsts.MaxNumberLength)]
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
            [DisplayName("Bill Date")]
            public DateTime BillDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Bill Due Date")]
            public DateTime BillDueDate { get; set; }
        }
    }
}
