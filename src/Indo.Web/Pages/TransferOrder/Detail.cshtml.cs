using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.TransferOrders;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.TransferOrder
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public TransferOrderViewModel TransferOrder { get; set; }

        private readonly ITransferOrderAppService _transferOrderAppService;
        public DetailModel(
            ITransferOrderAppService transferOrderAppService
            )
        {
            _transferOrderAppService = transferOrderAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            TransferOrder = ObjectMapper.Map<TransferOrderReadDto, TransferOrderViewModel>(await _transferOrderAppService.GetAsync(id));

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
            public TransferOrderStatus Status { get; set; }
        }

    }
}
