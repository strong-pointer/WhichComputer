using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WhichComputer.Main.Models.JSON;
using YamlDotNet.Core.Tokens;

namespace WhichComputer.Main.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Questionnaire _loader = Program.GetQuestionnaireLoader().Questions;
    private readonly ComputerLoader _computerLoader = Program.GetComputerLoader();

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

    [HttpPost]
    public ActionResult GetFollowUpToQuestion(int questionId, string choice)
    {
        var question = _loader.GetQuestionWithID(questionId);
        if (question == null)
        {
            return Json(new ErrorResponse($"There was no question with the ID {questionId}."));
        }

        var answer = question.GetAnswer(choice);
        if (answer == null)
        {
            return Json(new ErrorResponse($"There was no answer '{choice}' for question ID {questionId}."));
        }

        return Json(new Dictionary<string, object>()
        {
            { "followUps", _loader.GetFollowUpQuestions(answer).Select(q => q.Id) },
        });
    }

    [HttpPost]
    public ActionResult GetQuestionHtml(int questionId)
    {
        Question? question = _loader.GetQuestionWithID(questionId);

        if (question != null)
        {
            return PartialView("QuestionCard", question);
        }
        else
        {
            return PartialView("Error");
        }
    }

    [HttpPost]
    public async Task<ActionResult> UploadQuestionnaireResponse()
    {
        try
        {
            using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
            QuestionnaireResponse questionnaireResponse = new();
            var responses = JsonConvert.DeserializeObject<Dictionary<int, string>>(await reader.ReadToEndAsync());
            var failures = new List<int>();
            if (responses == null)
            {
                throw new Exception();
            }

            foreach (var kvp in responses)
            {
                Question? question = _loader.GetQuestionWithID(kvp.Key);
                Answer? answer = question?.GetAnswer(kvp.Value);
                if (question != null && answer != null)
                    answer.Tags.ToList().ForEach(tag => questionnaireResponse.AddTagScore(tag.Key, tag.Value));
                else
                    failures.Add(kvp.Key);
            }

            if (failures.Count != 0)
            {
                return BadRequest(Json(new ErrorResponse($"The following responses could not be parsed: {failures}")));
            }

            Dictionary<string, string> response = new Dictionary<string, string>();
            response.Add("hash", questionnaireResponse.GetHashedAndEncryptedResponse());
            return Ok(Json(response));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            return BadRequest(Json("Unable to upload those responses"));
        }
    }

    [HttpGet]
    public IActionResult ComputerResults()
    {
        string queryParam = string.Empty;

        // "QResponse" is QuestionnaireResponse, the hashed and then encrypted string for the results
        if (!string.IsNullOrEmpty(HttpContext.Request.Query["q"]))
        {
            queryParam = HttpContext.Request.Query["q"];
        }

        // Put this hash into the "responses" table
        // Get the response_id that matches this (which is autoincremented), and send it to the card
        // (in SubmitRating)
        // The card sends over the response_id along with hash, name, and rating

        // Get the computers that match the decrypted hash's criteria
        QuestionnaireResponse response = QuestionnaireResponse.FromEncryptedHash(queryParam);

        /* Verify that the response was valid
        if (response == null)
        {
            // Not a valid query parameter, throw an error
            return View("ResultsError");
            // Or we could do a simple 404 return: Response.StatusCode = 404; return View();
        }*/

        ViewData["CompLoader"] = Program.GetComputerLoader();
        /* Testing stuff
        AmazonComputerResultHandler handler = new AmazonComputerResultHandler(Program.GetComputerLoader());
        var tester = handler.Fetch("Samsung Galaxy Chromebook 2", false, 1);*/

        // Replace this with computer matching function call that returns a list of computers that is matching the tags?
        List<Computer> results = ScoringCalculation.CalculateScore(response);
        return View(new ComputerList(results));
    }

    [HttpGet]
    public IActionResult MoreInfo()
    {
        Computer temp = Program.GetComputerLoader().Computers.GetComputer(Request.Query["computer"]).Value;
        return View(temp);
    }

    [HttpPost]
    public ActionResult SubmitRating(string itemName, double ratingValue)
    {
        Debug.WriteLine(itemName + " " + ratingValue);
        // Send rating to ratings table in the db
        return Json(new { success = true });
    }
}