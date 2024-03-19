using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indo.Web.Pages.CustomerCreditNote
{
    public class PaymentLookupModel : IndoPageModel
    {

        public Guid CustomerCreditNoteId { get; set; }
        public void OnGet(Guid id)
        {
            CustomerCreditNoteId = id;
        }
    }
}
