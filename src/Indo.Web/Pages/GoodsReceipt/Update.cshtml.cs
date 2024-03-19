using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.GoodsReceipts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.GoodsReceipt
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public GoodsReceiptUpdateViewModel GoodsReceipt { get; set; }
        public List<SelectListItem> FromWarehouse { get; set; }
        public List<SelectListItem> ToWarehouse { get; set; }
        public List<SelectListItem> DeliveryOrder { get; set; }
        public GoodsReceiptStatus Status { get; set; }

        private readonly IGoodsReceiptAppService _goodsReceiptAppService;
        public UpdateModel(IGoodsReceiptAppService goodsReceiptAppService)
        {
            _goodsReceiptAppService = goodsReceiptAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _goodsReceiptAppService.GetAsync(id);
            Status = dto.Status;
            GoodsReceipt = ObjectMapper.Map<GoodsReceiptReadDto, GoodsReceiptUpdateViewModel>(dto);

            var fromLookup = await _goodsReceiptAppService.GetWarehouseLookupAsync();
            FromWarehouse = fromLookup.Items
                .Where(x => x.Id.Equals(dto.FromWarehouseId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var toLookup = await _goodsReceiptAppService.GetWarehouseLookupAsync();
            ToWarehouse = toLookup.Items
                .Where(x => x.Id.Equals(dto.ToWarehouseId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var deliveryOrderLookup = await _goodsReceiptAppService.GetDeliveryOrderLookupAsync();
            DeliveryOrder = deliveryOrderLookup.Items
                .Where(x => x.Id.Equals(dto.DeliveryOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _goodsReceiptAppService.UpdateAsync(
                    GoodsReceipt.Id,
                    ObjectMapper.Map<GoodsReceiptUpdateViewModel, GoodsReceiptUpdateDto>(GoodsReceipt)
                );
                return NoContent();

            }
            catch (GoodsReceiptAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class GoodsReceiptUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }


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
