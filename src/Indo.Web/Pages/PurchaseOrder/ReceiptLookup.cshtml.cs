using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indo.Web.Pages.PurchaseOrder
{
    public class ReceiptLookupModel : IndoPageModel
    {

        public Guid PurchaseOrderId { get; set; }
        public void OnGet(Guid id)
        {
            PurchaseOrderId = id;
        }
    }
}
