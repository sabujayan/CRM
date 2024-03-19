using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.GoodsReceipts;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.GoodsReceipt
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public GoodsReceiptViewModel GoodsReceipt { get; set; }

        private readonly IGoodsReceiptAppService _goodsReceiptAppService;
        public DetailModel(
            IGoodsReceiptAppService goodsReceiptAppService
            )
        {
            _goodsReceiptAppService = goodsReceiptAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            GoodsReceipt = ObjectMapper.Map<GoodsReceiptReadDto, GoodsReceiptViewModel>(await _goodsReceiptAppService.GetAsync(id));

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
            public GoodsReceiptStatus Status { get; set; }
        }

    }
}
