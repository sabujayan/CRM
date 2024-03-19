using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.StockAdjustments;
using Indo.StockAdjustmentDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.StockAdjustment
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public StockAdjustmentViewModel StockAdjustment { get; set; }
        public PagedResultDto<StockAdjustmentDetailViewModel> Details { get; set; }

        private readonly IStockAdjustmentAppService _stockAdjustmentAppService;

        private readonly IStockAdjustmentDetailAppService _stockAdjustmentDetailAppService;

        public PrintDetailModel(
            IStockAdjustmentAppService stockAdjustmentAppService,
            IStockAdjustmentDetailAppService stockAdjustmentDetailAppService
            )
        {
            _stockAdjustmentAppService = stockAdjustmentAppService;
            _stockAdjustmentDetailAppService = stockAdjustmentDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            StockAdjustment = ObjectMapper.Map<StockAdjustmentReadDto, StockAdjustmentViewModel>(await _stockAdjustmentAppService.GetAsync(id));

            var dtos = await _stockAdjustmentDetailAppService.GetListByStockAdjustmentAsync(StockAdjustment.Id);

            Details = ObjectMapper.Map<PagedResultDto<StockAdjustmentDetailReadDto>, PagedResultDto<StockAdjustmentDetailViewModel>>(dtos);

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


        public class StockAdjustmentDetailViewModel
        {
            public Guid StockAdjustmentId { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public string UomName { get; set; }
            public float Quantity { get; set; }

        }
    }
}
