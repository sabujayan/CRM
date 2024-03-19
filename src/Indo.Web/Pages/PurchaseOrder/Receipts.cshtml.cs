

using System;

namespace Indo.Web.Pages.PurchaseOrder
{
    public class ReceiptsModel : IndoPageModel
    {
        public Guid PurchaseOrderId { get; set; }
        public void OnGet(Guid id)
        {
            PurchaseOrderId = id;
        }
    }
}
