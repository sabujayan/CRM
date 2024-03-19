using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indo.Web.Pages.ServiceOrder
{
    public class InvoiceLookupModel : IndoPageModel
    {

        public Guid ServiceOrderId { get; set; }
        public void OnGet(Guid id)
        {
            ServiceOrderId = id;
        }
    }
}
