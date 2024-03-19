using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.PurchaseReceiptDetails;
using Indo.PurchaseReceipts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.PurchaseReceipt
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreatePurchaseReceiptDetailViewModel PurchaseReceiptDetail { get; set; }
        public List<SelectListItem> PurchaseReceipts { get; set; }
        public List<SelectListItem> Products { get; set; }

        private readonly IPurchaseReceiptDetailAppService _purchaseReceiptDetailAppService;
        public CreateDetailModel(IPurchaseReceiptDetailAppService purchaseReceiptDetailAppService)
        {
            _purchaseReceiptDetailAppService = purchaseReceiptDetailAppService;
        }
        public async Task OnGetAsync(Guid purchaseReceiptId)
        {
            PurchaseReceiptDetail = new CreatePurchaseReceiptDetailViewModel();
            PurchaseReceiptDetail.PurchaseReceiptId = purchaseReceiptId;

            var purchaseReceiptLookup = await _purchaseReceiptDetailAppService.GetPurchaseReceiptLookupAsync();
            PurchaseReceipts = purchaseReceiptLookup.Items
                .Where(x => x.Id.Equals(purchaseReceiptId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _purchaseReceiptDetailAppService.GetProductLookupByPurchaseReceiptAsync(purchaseReceiptId);
            Products = productLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _purchaseReceiptDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreatePurchaseReceiptDetailViewModel, PurchaseReceiptDetailCreateDto>(PurchaseReceiptDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreatePurchaseReceiptDetailViewModel
        {
            [Required]
            [SelectItems(nameof(PurchaseReceipts))]
            [DisplayName("Purchase Receipt")]
            public Guid PurchaseReceiptId { get; set; }

            [Required]
            [SelectItems(nameof(Products))]
            [DisplayName("Product")]
            public Guid ProductId { get; set; }

            [Required]
            public float Quantity { get; set; }
        }
    }
}
