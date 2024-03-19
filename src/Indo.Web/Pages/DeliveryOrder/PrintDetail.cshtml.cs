using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.DeliveryOrders;
using Indo.DeliveryOrderDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.DeliveryOrder
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public DeliveryOrderViewModel DeliveryOrder { get; set; }
        public PagedResultDto<DeliveryOrderDetailViewModel> Details { get; set; }

        private readonly IDeliveryOrderAppService _deliveryOrderAppService;

        private readonly IDeliveryOrderDetailAppService _deliveryOrderDetailAppService;

        public PrintDetailModel(
            IDeliveryOrderAppService deliveryOrderAppService,
            IDeliveryOrderDetailAppService deliveryOrderDetailAppService
            )
        {
            _deliveryOrderAppService = deliveryOrderAppService;
            _deliveryOrderDetailAppService = deliveryOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            DeliveryOrder = ObjectMapper.Map<DeliveryOrderReadDto, DeliveryOrderViewModel>(await _deliveryOrderAppService.GetAsync(id));

            var dtos = await _deliveryOrderDetailAppService.GetListByDeliveryOrderAsync(DeliveryOrder.Id);

            Details = ObjectMapper.Map<PagedResultDto<DeliveryOrderDetailReadDto>, PagedResultDto<DeliveryOrderDetailViewModel>>(dtos);

        }

        public class DeliveryOrderViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public string Number { get; set; }
            public Guid TransferOrderId { get; set; }
            public string TransferOrderNumber { get; set; }
            public string Description { get; set; }
            public DateTime OrderDate { get; set; }
            public Guid FromWarehouseId { get; set; }
            public string FromWarehouseName { get; set; }
            public Guid ToWarehouseId { get; set; }
            public string ToWarehouseName { get; set; }
        }


        public class DeliveryOrderDetailViewModel
        {
            public Guid DeliveryOrderId { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public string UomName { get; set; }
            public float Quantity { get; set; }

        }
    }
}
