using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.PurchaseOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.PurchaseOrder
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public PurchaseOrderUpdateViewModel PurchaseOrder { get; set; }
        public List<SelectListItem> Vendors { get; set; }
        public List<SelectListItem> Buyers { get; set; }

        private readonly IPurchaseOrderAppService _purchaseOrderAppService;
        public UpdateModel(IPurchaseOrderAppService purchaseOrderAppService)
        {
            _purchaseOrderAppService = purchaseOrderAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _purchaseOrderAppService.GetAsync(id);
            PurchaseOrder = ObjectMapper.Map<PurchaseOrderReadDto, PurchaseOrderUpdateViewModel>(dto);

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
                await _purchaseOrderAppService.UpdateAsync(
                    PurchaseOrder.Id,
                    ObjectMapper.Map<PurchaseOrderUpdateViewModel, PurchaseOrderUpdateDto>(PurchaseOrder)
                );
                return NoContent();

            }
            catch (PurchaseOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class PurchaseOrderUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
