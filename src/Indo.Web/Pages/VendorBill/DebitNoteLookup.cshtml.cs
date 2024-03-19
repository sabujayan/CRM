using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indo.Web.Pages.VendorBill
{
    public class DebitNoteLookupModel : IndoPageModel
    {

        public Guid VendorBillId { get; set; }
        public void OnGet(Guid id)
        {
            VendorBillId = id;
        }
    }
}
