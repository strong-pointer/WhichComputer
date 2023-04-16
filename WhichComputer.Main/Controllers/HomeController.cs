using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FSharp.Core;
using Microsoft.VisualBasic;
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
    private DatabaseRepository _db = new();

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

            Dictionary<string, object> response = new();
            response.Add("hash", questionnaireResponse.GetHashedAndEncryptedResponse());

            // Put this hash into the "responses" table
            // Get the response_id that matches this (which is auto-incremented), and send it to the card
            long responseId = _db.AddResponse(questionnaireResponse);
            response.Add("responseId", responseId);

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
        long responseId = -1;

        // "QResponse" is QuestionnaireResponse, the hashed and then encrypted string for the results
        if (!string.IsNullOrEmpty(HttpContext.Request.Query["q"]))
        {
            queryParam = HttpContext.Request.Query["q"];
            responseId = long.Parse(HttpContext.Request.Query["responseId"]);
        }

        // Get the computers that match the decrypted hash's criteria
        QuestionnaireResponse response = QuestionnaireResponse.FromEncryptedHash(queryParam);

        ViewData["CompLoader"] = Program.GetComputerLoader();
        ViewData["ResponseId"] = responseId;

        // Gets list of Computer objects that match the scores
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
    public ActionResult SubmitRating(string itemName, double ratingValue, long responseId)
    {
        // The card sends over the response_id along with name and rating
        Debug.WriteLine(itemName + " " + ratingValue + " " + responseId);
        Ratings currRating = new();
        currRating.ResponseId = (int)responseId;
        currRating.Rating = ratingValue;
        currRating.ComputerName = itemName;

        // New rating object is stored in the db (Ratings table) - also returns that row's id if needed
        long ratingId = _db.AddRating(currRating);

        return Json(new { success = true });
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