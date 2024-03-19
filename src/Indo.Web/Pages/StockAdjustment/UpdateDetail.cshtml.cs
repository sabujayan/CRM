using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.StockAdjustmentDetails;
using Indo.StockAdjustments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.StockAdjustment
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public StockAdjustmentDetailUpdateViewModel StockAdjustmentDetail { get; set; }
        public List<SelectListItem> StockAdjustments { get; set; }
        public List<SelectListItem> Products { get; set; }
        public StockAdjustmentStatus Status { get; set; }

        private readonly IStockAdjustmentDetailAppService _stockAdjustmentDetailAppService;
        public UpdateDetailModel(IStockAdjustmentDetailAppService stockAdjustmentDetailAppService)
        {
            _stockAdjustmentDetailAppService = stockAdjustmentDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _stockAdjustmentDetailAppService.GetAsync(id);
            Status = dto.Status;
            StockAdjustmentDetail = ObjectMapper.Map<StockAdjustmentDetailReadDto, StockAdjustmentDetailUpdateViewModel>(dto);

            var stockAdjustmentLookup = await _stockAdjustmentDetailAppService.GetStockAdjustmentLookupAsync();
            StockAdjustments = stockAdjustmentLookup.Items
                .Where(x => x.Id.Equals(StockAdjustmentDetail.StockAdjustmentId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _stockAdjustmentDetailAppService.GetProductLookupAsync();
            Products = productLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _stockAdjustmentDetailAppService.UpdateAsync(
                    StockAdjustmentDetail.Id,
                    ObjectMapper.Map<StockAdjustmentDetailUpdateViewModel, StockAdjustmentDetailUpdateDto>(StockAdjustmentDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class StockAdjustmentDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(StockAdjustments))]
            [DisplayName("Stock Adjustment")]
            public Guid StockAdjustmentId { get; set; }

            [Required]
            [SelectItems(nameof(Products))]
            [DisplayName("Product")]
            public Guid ProductId { get; set; }

            [Required]
            [Range(0f, float.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
            public float Quantity { get; set; }
        }
    }
}
