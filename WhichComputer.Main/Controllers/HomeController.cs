using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WhichComputer.Models;

namespace WhichComputer.Main.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Questionnaire _loader = Program.GetQuestionnaireLoader().Questions;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Questionnaire()
    {
        ViewData["Question"] = Program.GetQuestionnaireLoader().Questions.GetFirstQuestion();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public ActionResult AnswerBtn_Click(int questionID, string choice)
    {
        Question question = _loader.GetQuestionWithID(questionID);
        Answer answer = question.GetAnswer(choice);
        return PartialView("QuestionCard", _loader.GetFollowUpQuestions(answer)[0]);
    }
}