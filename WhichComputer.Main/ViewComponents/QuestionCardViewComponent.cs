using Microsoft.AspNetCore.Mvc;

namespace WhichComputer.Main.ViewComponents;

public class QuestionCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Question question)
    {
        /*
         * This returns a ViewResult that represents
         * the rendering of the cshtml template related
         * to this view component
         */
        return View("~/Views/Shared/QuestionCard.cshtml", question);
    }
}