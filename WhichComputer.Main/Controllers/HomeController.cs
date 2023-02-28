using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public ActionResult GetFollowUpToQuestion(int questionID, string choice)
    {
        Question question = _loader.GetQuestionWithID(questionID);
        Answer answer = question.GetAnswer(choice);
        return Json(new Dictionary<string, object>()
        {
            { "followUps", _loader.GetFollowUpQuestions(answer).Select(q => q.Id) },
        });
    }

    [HttpPost]
    public ActionResult GetQuestionHTML(int questionID)
    {
        Question question = _loader.GetQuestionWithID(questionID);
        return PartialView("QuestionCard", question);
    }

    [HttpPost]
    public async Task<ActionResult> UploadQuestionnaireResponse()
    {
        try
        {
            using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
            QuestionnaireResponse questionnaireResponse = new QuestionnaireResponse();
            var responses = JsonConvert.DeserializeObject<Dictionary<int, string>>(await reader.ReadToEndAsync());

            foreach (var kvp in responses)
            {
                Question question = _loader.GetQuestionWithID(kvp.Key);
                Answer answer = question.GetAnswer(kvp.Value);
                answer.Tags.ToList().ForEach(tag => questionnaireResponse.AddTagScore(tag.Key, tag.Value));
            }

            return Ok(Json("Success"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            return BadRequest(Json("Unable to upload those responses"));
        }
    }
}