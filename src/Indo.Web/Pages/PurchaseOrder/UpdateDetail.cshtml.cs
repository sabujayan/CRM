using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.PurchaseOrderDetails;
using Indo.PurchaseOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.PurchaseOrder
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public PurchaseOrderDetailUpdateViewModel PurchaseOrderDetail { get; set; }
        public List<SelectListItem> PurchaseOrders { get; set; }
        public List<SelectListItem> Products { get; set; }
        public PurchaseOrderStatus Status { get; set; }

        private readonly IPurchaseOrderDetailAppService _purchaseOrderDetailAppService;
        public UpdateDetailModel(IPurchaseOrderDetailAppService purchaseOrderDetailAppService)
        {
            _purchaseOrderDetailAppService = purchaseOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _purchaseOrderDetailAppService.GetAsync(id);
            Status = dto.Status;
            PurchaseOrderDetail = ObjectMapper.Map<PurchaseOrderDetailReadDto, PurchaseOrderDetailUpdateViewModel>(dto);

            var purchaseOrderLookup = await _purchaseOrderDetailAppService.GetPurchaseOrderLookupAsync();
            PurchaseOrders = purchaseOrderLookup.Items
                .Where(x => x.Id.Equals(PurchaseOrderDetail.PurchaseOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _purchaseOrderDetailAppService.GetProductLookupAsync();
            Products = productLookup.Items
                .Select(x => new SelectListItem($"{x.Name} [Price: {x.Price.ToString("##,##.00")}]", x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _purchaseOrderDetailAppService.UpdateAsync(
                    PurchaseOrderDetail.Id,
                    ObjectMapper.Map<PurchaseOrderDetailUpdateViewModel, PurchaseOrderDetailUpdateDto>(PurchaseOrderDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class PurchaseOrderDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(PurchaseOrders))]
            [DisplayName("Purchase Order")]
            public Guid PurchaseOrderId { get; set; }

            [Required]
            [SelectItems(nameof(Products))]
            [DisplayName("Product")]
            public Guid ProductId { get; set; }

            [Required]
            [Range(0f, float.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
            public float Quantity { get; set; }

            [Range(0f, float.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
            [DisplayName("Discount Amount")]
            public float DiscAmt { get; set; }
        }
    }
}
