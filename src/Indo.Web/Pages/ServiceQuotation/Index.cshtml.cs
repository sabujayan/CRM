

using Microsoft.AspNetCore.Mvc;

namespace Indo.Web.Pages.ServiceQuotation
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
