using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.GoodsReceipts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.GoodsReceipt
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateGoodsReceiptViewModel GoodsReceipt { get; set; }
        public List<SelectListItem> FromWarehouse { get; set; }
        public List<SelectListItem> ToWarehouse { get; set; }
        public List<SelectListItem> DeliveryOrder { get; set; }

        private readonly IGoodsReceiptAppService _goodsReceiptAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IGoodsReceiptAppService goodsReceiptAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _goodsReceiptAppService = goodsReceiptAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            GoodsReceipt = new CreateGoodsReceiptViewModel();
            GoodsReceipt.OrderDate = DateTime.Now;
            GoodsReceipt.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.GoodsReceipt);

            var fromLookup = await _goodsReceiptAppService.GetWarehouseLookupAsync();
            FromWarehouse = fromLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var toLookup = await _goodsReceiptAppService.GetWarehouseLookupAsync();
            ToWarehouse = toLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var deliveryOrderLookup = await _goodsReceiptAppService.GetDeliveryOrderLookupAsync();
            DeliveryOrder = deliveryOrderLookup.Items
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _goodsReceiptAppService.CreateAsync(
                    ObjectMapper.Map<CreateGoodsReceiptViewModel, GoodsReceiptCreateDto>(GoodsReceipt)
                    );
                return NoContent();

            }
            catch (GoodsReceiptAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateGoodsReceiptViewModel
        {
            [Required]
            [SelectItems(nameof(DeliveryOrder))]
            [DisplayName("DeliveryOrder")]
            public Guid DeliveryOrderId { get; set; }


            [Required]
            [SelectItems(nameof(FromWarehouse))]
            [DisplayName("FromWarehouse")]
            public Guid FromWarehouseId { get; set; }

            [Required]
            [SelectItems(nameof(ToWarehouse))]
            [DisplayName("ToWarehouse")]
            public Guid ToWarehouseId { get; set; }

            [Required]
            [StringLength(GoodsReceiptConsts.MaxNumberLength)]
            public string Number { get; set; }
            public string Description { get; set; }

            [Required]
            public GoodsReceiptStatus Status { get; set; }

            [Required]
            [DisplayName("Receipt Date")]
            public DateTime OrderDate { get; set; }
        }
    }
}
