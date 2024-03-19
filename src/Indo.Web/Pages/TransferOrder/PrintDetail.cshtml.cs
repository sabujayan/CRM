using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.TransferOrders;
using Indo.TransferOrderDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.TransferOrder
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public TransferOrderViewModel TransferOrder { get; set; }
        public PagedResultDto<TransferOrderDetailViewModel> Details { get; set; }

        private readonly ITransferOrderAppService _transferOrderAppService;

        private readonly ITransferOrderDetailAppService _transferOrderDetailAppService;

        public PrintDetailModel(
            ITransferOrderAppService transferOrderAppService,
            ITransferOrderDetailAppService transferOrderDetailAppService
            )
        {
            _transferOrderAppService = transferOrderAppService;
            _transferOrderDetailAppService = transferOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            TransferOrder = ObjectMapper.Map<TransferOrderReadDto, TransferOrderViewModel>(await _transferOrderAppService.GetAsync(id));

            var dtos = await _transferOrderDetailAppService.GetListByTransferOrderAsync(TransferOrder.Id);

            Details = ObjectMapper.Map<PagedResultDto<TransferOrderDetailReadDto>, PagedResultDto<TransferOrderDetailViewModel>>(dtos);

        }

        public class TransferOrderViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime OrderDate { get; set; }
            public Guid FromWarehouseId { get; set; }
            public string FromWarehouseName { get; set; }
            public Guid ToWarehouseId { get; set; }
            public string ToWarehouseName { get; set; }
        }


        public class TransferOrderDetailViewModel
        {
            public Guid TransferOrderId { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public string UomName { get; set; }
            public float Quantity { get; set; }

        }
    }
}
