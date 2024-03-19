

using Microsoft.AspNetCore.Mvc;

namespace Indo.Web.Pages.SalesQuotation
{
    public class IndexModel : IndoPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ViewMode { get; set; }
        public void OnGet()
        {
        }
    }
}
