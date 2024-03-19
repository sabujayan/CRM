using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indo.Web.Pages.ProjectOrder
{
    public class InvoiceLookupModel : IndoPageModel
    {

        public Guid ProjectOrderId { get; set; }
        public void OnGet(Guid id)
        {
            ProjectOrderId = id;
        }
    }
}
