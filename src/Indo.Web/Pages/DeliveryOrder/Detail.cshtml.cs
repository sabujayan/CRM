using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.DeliveryOrders;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.DeliveryOrder
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public DeliveryOrderViewModel DeliveryOrder { get; set; }

        private readonly IDeliveryOrderAppService _deliveryOrderAppService;
        public DetailModel(
            IDeliveryOrderAppService deliveryOrderAppService
            )
        {
            _deliveryOrderAppService = deliveryOrderAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            DeliveryOrder = ObjectMapper.Map<DeliveryOrderReadDto, DeliveryOrderViewModel>(await _deliveryOrderAppService.GetAsync(id));

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
            public DeliveryOrderStatus Status { get; set; }
        }

    }
}
