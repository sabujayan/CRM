using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.StockAdjustments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.StockAdjustment
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public StockAdjustmentUpdateViewModel StockAdjustment { get; set; }
        public List<SelectListItem> Warehouse { get; set; }

        private readonly IStockAdjustmentAppService _stockAdjustmentAppService;
        public UpdateModel(IStockAdjustmentAppService stockAdjustmentAppService)
        {
            _stockAdjustmentAppService = stockAdjustmentAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _stockAdjustmentAppService.GetAsync(id);
            StockAdjustment = ObjectMapper.Map<StockAdjustmentReadDto, StockAdjustmentUpdateViewModel>(dto);

            var warehouseLookup = await _stockAdjustmentAppService.GetWarehouseLookupAsync();
            Warehouse = warehouseLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _stockAdjustmentAppService.UpdateAsync(
                    StockAdjustment.Id,
                    ObjectMapper.Map<StockAdjustmentUpdateViewModel, StockAdjustmentUpdateDto>(StockAdjustment)
                );
                return NoContent();

            }
            catch (StockAdjustmentAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class StockAdjustmentUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(StockAdjustmentConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            public StockAdjustmentType AdjustmentType { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Adjustment Date")]
            public DateTime AdjustmentDate { get; set; }


            [Required]
            [SelectItems(nameof(Warehouse))]
            [DisplayName("Warehouse")]
            public Guid WarehouseId { get; set; }
        }
    }
}
