using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.PurchaseOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.PurchaseOrder
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreatePurchaseOrderViewModel PurchaseOrder { get; set; }
        public List<SelectListItem> Vendors { get; set; }
        public List<SelectListItem> Buyers { get; set; }

        private readonly IPurchaseOrderAppService _purchaseOrderAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IPurchaseOrderAppService purchaseOrderAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _purchaseOrderAppService = purchaseOrderAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            PurchaseOrder = new CreatePurchaseOrderViewModel();
            PurchaseOrder.OrderDate = DateTime.Now;
            PurchaseOrder.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.PurchaseOrder);

            var vendorLookup = await _purchaseOrderAppService.GetVendorLookupAsync();
            Vendors = vendorLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var buyerLookup = await _purchaseOrderAppService.GetBuyerLookupAsync();
            Buyers = buyerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _purchaseOrderAppService.CreateAsync(
                    ObjectMapper.Map<CreatePurchaseOrderViewModel, PurchaseOrderCreateDto>(PurchaseOrder)
                    );
                return NoContent();

            }
            catch (PurchaseOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreatePurchaseOrderViewModel
        {
            [Required]
            [SelectItems(nameof(Vendors))]
            [DisplayName("Vendor")]
            public Guid VendorId { get; set; }

            [Required]
            [SelectItems(nameof(Buyers))]
            [DisplayName("Buyer")]
            public Guid BuyerId { get; set; }

            [Required]
            [StringLength(PurchaseOrderConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Order Date")]
            public DateTime OrderDate { get; set; }
        }
    }
}
