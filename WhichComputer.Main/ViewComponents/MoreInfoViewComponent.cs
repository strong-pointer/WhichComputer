using Microsoft.AspNetCore.Mvc;

namespace WhichComputer.Main.ViewComponents
{
    public class MoreInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string hash)
        {
            /*
             * This returns a ViewResult that represents
             * the rendering of the cshtml template related
             * to this view component
             */
            return View("~/Views/Shared/MoreInfo.cshtml", hash);
        }
    }
}