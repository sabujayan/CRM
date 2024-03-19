using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.GoodsReceiptDetails;
using Indo.GoodsReceipts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.GoodsReceipt
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public GoodsReceiptDetailUpdateViewModel GoodsReceiptDetail { get; set; }
        public List<SelectListItem> GoodsReceipts { get; set; }
        public List<SelectListItem> Products { get; set; }
        public GoodsReceiptStatus Status { get; set; }

        private readonly IGoodsReceiptDetailAppService _goodsReceiptDetailAppService;
        public UpdateDetailModel(IGoodsReceiptDetailAppService goodsReceiptDetailAppService)
        {
            _goodsReceiptDetailAppService = goodsReceiptDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _goodsReceiptDetailAppService.GetAsync(id);
            Status = dto.Status;
            GoodsReceiptDetail = ObjectMapper.Map<GoodsReceiptDetailReadDto, GoodsReceiptDetailUpdateViewModel>(dto);

            var goodsReceiptLookup = await _goodsReceiptDetailAppService.GetGoodsReceiptLookupAsync();
            GoodsReceipts = goodsReceiptLookup.Items
                .Where(x => x.Id.Equals(GoodsReceiptDetail.GoodsReceiptId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _goodsReceiptDetailAppService.GetProductByGoodsReceiptLookupAsync(GoodsReceiptDetail.GoodsReceiptId);
            Products = productLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _goodsReceiptDetailAppService.UpdateAsync(
                    GoodsReceiptDetail.Id,
                    ObjectMapper.Map<GoodsReceiptDetailUpdateViewModel, GoodsReceiptDetailUpdateDto>(GoodsReceiptDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class GoodsReceiptDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(GoodsReceipts))]
            [DisplayName("Goods Receipts")]
            public Guid GoodsReceiptId { get; set; }

            [Required]
            [SelectItems(nameof(Products))]
            [DisplayName("Products")]
            public Guid ProductId { get; set; }

            [Required]
            [Range(0f, float.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
            public float Quantity { get; set; }
        }
    }
}
