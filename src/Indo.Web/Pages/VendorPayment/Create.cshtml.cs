using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.VendorPayments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorPayment
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateVendorPaymentViewModel VendorPayment { get; set; }

        public List<SelectListItem> Vendors { get; set; }

        public List<SelectListItem> PaymentMethods { get; set; }

        private readonly IVendorPaymentAppService _vendorPaymentAppService;
        public CreateModel(IVendorPaymentAppService vendorPaymentAppService)
        {
            _vendorPaymentAppService = vendorPaymentAppService;
        }
        public async Task OnGet()
        {
            VendorPayment = new CreateVendorPaymentViewModel();

            var vendorLookup = await _vendorPaymentAppService.GetVendorLookupAsync();
            Vendors = vendorLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var cashBankLookup = await _vendorPaymentAppService.GetCashAndBankLookupAsync();
            PaymentMethods = cashBankLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateVendorPaymentViewModel, VendorPaymentCreateDto>(VendorPayment);
                await _vendorPaymentAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (VendorPaymentAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateVendorPaymentViewModel
        {
            [Required]
            [StringLength(VendorPaymentConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Payment Date")]
            public DateTime PaymentDate { get; set; }

            [Required]
            [SelectItems(nameof(Vendors))]
            [DisplayName("Vendor")]
            public Guid VendorId { get; set; }

            [Required]
            [SelectItems(nameof(PaymentMethods))]
            [DisplayName("Payment Methods")]
            public Guid CashAndBankId { get; set; }

            [Required]
            public float Amount { get; set; }
        }
    }
}
