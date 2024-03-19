using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.PurchaseReceipts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.PurchaseReceipt
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public PurchaseReceiptUpdateViewModel PurchaseReceipt { get; set; }
        public List<SelectListItem> PurchaseOrders { get; set; }

        private readonly IPurchaseReceiptAppService _purchaseReceiptAppService;
        public UpdateModel(IPurchaseReceiptAppService purchaseReceiptAppService)
        {
            _purchaseReceiptAppService = purchaseReceiptAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _purchaseReceiptAppService.GetAsync(id);
            PurchaseReceipt = ObjectMapper.Map<PurchaseReceiptReadDto, PurchaseReceiptUpdateViewModel>(dto);

            var purchaseOrderLookup = await _purchaseReceiptAppService.GetPurchaseOrderLookupAsync();
            PurchaseOrders = purchaseOrderLookup.Items
                .Where(x => x.Id.Equals(PurchaseReceipt.PurchaseOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _purchaseReceiptAppService.UpdateAsync(
                    PurchaseReceipt.Id,
                    ObjectMapper.Map<PurchaseReceiptUpdateViewModel, PurchaseReceiptUpdateDto>(PurchaseReceipt)
                );
                return NoContent();

            }
            catch (PurchaseReceiptAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class PurchaseReceiptUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(PurchaseOrders))]
            [DisplayName("Purchase Order")]
            public Guid PurchaseOrderId { get; set; }

            [Required]
            [StringLength(PurchaseReceiptConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Receipt Date")]
            public DateTime ReceiptDate { get; set; }
        }
    }
}
