using Microsoft.AspNetCore.Mvc;

namespace WhichComputer.Main.ViewComponents
{
    public class MostPopularComputerCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Computer computer)
        {
            /*
             * This returns a ViewResult that represents
             * the rendering of the cshtml template related
             * to this view component
             */
            return View("~/Views/Shared/MostPopularComputerCard.cshtml", computer);
        }
    }
}
