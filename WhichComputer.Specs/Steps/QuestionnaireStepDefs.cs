using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using WhichComputer.Main;

namespace WhichComputer.Specs.Steps;

[Binding]
public sealed class QuestionnaireStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    [NotNull] private readonly Questionnaire _questions = (new QuestionnaireLoader("questionnaire.yaml")).Questions;
    [AllowNull] private Answer _currentAnswer;

    [NotNull] private QuestionnaireResponse _response = new QuestionnaireResponse();
    [NotNull] private List<string> tagsList = new List<string>();

    public QuestionnaireStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Before()]
    public void Before()
    {
        _currentAnswer = null;
        tagsList.Clear();
    }

    [Given("all questions are loaded")]
    public void GivenAllQuestionsAreLoaded()
    {
        Assert.IsNotEmpty(_questions.Questions);
    }

    [Given("I answer {string} for question {int}")]
    public void GivenIAnswerQuestion(string choice, int question)
    {
        _currentAnswer = _questions.Questions.Find(q => q.Id == question)!.Answers.Find(a => a.Choice.Equals(choice));
    }

    [Then("I expect that the follow up questions for that answer are {string}")]
    public void ThenFollowUpsAre(string follows)
    {
        var truef = follows.Split(", ").Select(f => Convert.ToInt32(f));
        Assert.AreEqual(_questions.GetFollowUpQuestions(_currentAnswer).Select(q => q.Id), truef);
    }


    [Given("a QuestionnaireResponse object is initialized and not null")]
    public void GivenResponseObjectIsLoaded()
    {
        Assert.IsNotNull(_response);
    }

    [Given("I add a tag {string} with a score of {double}")]
    public void GivenAddedTagAndScore(string tag, double score)
    {
        _response.AddTagScore(tag, score);
    }

    [Then("I expect that when retreiving all tags, a list with each of the currently entered tags {string} are returned")]
    public void ThenAllTagsPresent(string tags)
    {
        tagsList = tags.Split(',').ToList();
        Assert.AreEqual(_response.GetAllTags(), tagsList);
    }

    [Then("I expect that when calling for the hashed string representation, {string} is returned")]
    public void ThenHashIsCorrect(string expectedHash)
    {
        Assert.AreEqual(_response.GetHashedResponse(), expectedHash);
    }
}