using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.StockAdjustments;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.StockAdjustment
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public StockAdjustmentViewModel StockAdjustment { get; set; }

        private readonly IStockAdjustmentAppService _stockAdjustmentAppService;
        public DetailModel(
            IStockAdjustmentAppService stockAdjustmentAppService
            )
        {
            _stockAdjustmentAppService = stockAdjustmentAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            StockAdjustment = ObjectMapper.Map<StockAdjustmentReadDto, StockAdjustmentViewModel>(await _stockAdjustmentAppService.GetAsync(id));

        }

        public class StockAdjustmentViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public StockAdjustmentStatus Status { get; set; }
            public StockAdjustmentType AdjustmentType { get; set; }
            public DateTime AdjustmentDate { get; set; }
            public Guid WarehouseId { get; set; }
            public string WarehouseName { get; set; }
            public Guid FromWarehouseId { get; set; }
            public string FromWarehouseName { get; set; }
            public Guid ToWarehouseId { get; set; }
            public string ToWarehouseName { get; set; }
        }

    }
}
