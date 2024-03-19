using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indo.Web.Pages.SalesOrder
{
    public class DeliveryLookupModel : IndoPageModel
    {

        public Guid SalesOrderId { get; set; }
        public void OnGet(Guid id)
        {
            SalesOrderId = id;
        }
    }
}
