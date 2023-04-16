using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using WhichComputer.Main.Models.JSON;
using YamlDotNet.Core.Tokens;
using MySqlConnector;

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

    public IActionResult Survey()
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
        if (!string.IsNullOrEmpty(HttpContext.Request.Query["QResponse"]))
        {
            queryParam = HttpContext.Request.Query["QResponse"];
        }

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
        return View(_computerLoader.Computers);
    }

    [HttpGet]
    public IActionResult MoreInfo()
    {
        Computer temp = Program.GetComputerLoader().Computers.GetComputer(Request.Query["computer"]).Value;
        return View(temp);
    }

    [HttpPost]
    public ActionResult UploadSurveyResponse(SurveyResponse model)
    {
        try
        {
            string connection = @"Data Source=whichcomputer4720.cnhnorewhlzk.us-east-1.rds.amazonaws.com;Port=3306;Initial Catalog=whichschema;User ID=whichcomputer;Password=whichcomputer4720pass$;";

            using (var con = new MySqlConnection(connection))
            {
                string sql = "INSERT INTO SurveyResponse (email, likedResponse, dislikedResponse, recommend, satisfaction) VALUES (@email, @liked, @disliked, @recommend, @satisfaction);";
                var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@email", model.Email);
                cmd.Parameters.AddWithValue("@liked", model.LikedResponse);
                cmd.Parameters.AddWithValue("@disliked", model.DislikedResponse);
                cmd.Parameters.AddWithValue("@recommend", model.Recommend);
                cmd.Parameters.AddWithValue("@satisfaction", model.Statisfaction);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Records Inserted Successfully");
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("Error Generated. Details: " + e.ToString());
                }
            }

            return RedirectToAction("Index");
        }
        catch (MySqlException ex)
        {
            throw ex;
        }
    }
}