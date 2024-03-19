

using System;

namespace Indo.Web.Pages.TransferOrder
{
    public class DeliveriesModel : IndoPageModel
    {
        public Guid TransferOrderId { get; set; }
        public void OnGet(Guid id)
        {
            TransferOrderId = id;
        }
    }
}
