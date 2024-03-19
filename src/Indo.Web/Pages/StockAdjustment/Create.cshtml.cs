using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.StockAdjustments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.StockAdjustment
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateStockAdjustmentViewModel StockAdjustment { get; set; }
        public List<SelectListItem> Warehouse { get; set; }

        private readonly IStockAdjustmentAppService _stockAdjustmentAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IStockAdjustmentAppService stockAdjustmentAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _stockAdjustmentAppService = stockAdjustmentAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            StockAdjustment = new CreateStockAdjustmentViewModel();
            StockAdjustment.AdjustmentDate = DateTime.Now;
            StockAdjustment.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.StockAdjustment);

            var warehouseLookup = await _stockAdjustmentAppService.GetWarehouseLookupAsync();
            Warehouse = warehouseLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _stockAdjustmentAppService.CreateAsync(
                    ObjectMapper.Map<CreateStockAdjustmentViewModel, StockAdjustmentCreateDto>(StockAdjustment)
                    );
                return NoContent();

            }
            catch (StockAdjustmentAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateStockAdjustmentViewModel
        {

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
