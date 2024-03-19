

using System;

namespace Indo.Web.Pages.SalesOrder
{
    public class DeliveriesModel : IndoPageModel
    {
        public Guid SalesOrderId { get; set; }
        public void OnGet(Guid id)
        {
            SalesOrderId = id;
        }
    }
}
