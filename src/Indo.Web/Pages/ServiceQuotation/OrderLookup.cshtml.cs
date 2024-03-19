using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indo.Web.Pages.ServiceQuotation
{
    public class OrderLookupModel : IndoPageModel
    {

        public Guid ServiceQuotationId { get; set; }
        public void OnGet(Guid id)
        {
            ServiceQuotationId = id;
        }
    }
}
