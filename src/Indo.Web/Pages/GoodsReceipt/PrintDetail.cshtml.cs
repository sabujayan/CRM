using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.GoodsReceipts;
using Indo.GoodsReceiptDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.GoodsReceipt
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public GoodsReceiptViewModel GoodsReceipt { get; set; }
        public PagedResultDto<GoodsReceiptDetailViewModel> Details { get; set; }

        private readonly IGoodsReceiptAppService _goodsReceiptAppService;

        private readonly IGoodsReceiptDetailAppService _goodsReceiptDetailAppService;

        public PrintDetailModel(
            IGoodsReceiptAppService goodsReceiptAppService,
            IGoodsReceiptDetailAppService goodsReceiptDetailAppService
            )
        {
            _goodsReceiptAppService = goodsReceiptAppService;
            _goodsReceiptDetailAppService = goodsReceiptDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            GoodsReceipt = ObjectMapper.Map<GoodsReceiptReadDto, GoodsReceiptViewModel>(await _goodsReceiptAppService.GetAsync(id));

            var dtos = await _goodsReceiptDetailAppService.GetListByGoodsReceiptAsync(GoodsReceipt.Id);

            Details = ObjectMapper.Map<PagedResultDto<GoodsReceiptDetailReadDto>, PagedResultDto<GoodsReceiptDetailViewModel>>(dtos);

        }

        public class GoodsReceiptViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public string Number { get; set; }
            public Guid DeliveryOrderId { get; set; }
            public string DeliveryOrderNumber { get; set; }
            public string Description { get; set; }
            public DateTime OrderDate { get; set; }
            public Guid FromWarehouseId { get; set; }
            public string FromWarehouseName { get; set; }
            public Guid ToWarehouseId { get; set; }
            public string ToWarehouseName { get; set; }
        }


        public class GoodsReceiptDetailViewModel
        {
            public Guid GoodsReceiptId { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public string UomName { get; set; }
            public float Quantity { get; set; }

        }
    }
}
