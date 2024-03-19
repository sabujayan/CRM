using Microsoft.AspNetCore.Mvc;

namespace Indo.Web.Components.Spinner
{
    public class SpinnerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Components/Spinner/Default.cshtml");
        }
    }
}
