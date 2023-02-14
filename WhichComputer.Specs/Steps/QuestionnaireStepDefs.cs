using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using WhichComputer.App_Start;

namespace WhichComputer.Specs.Steps;

[Binding]
public sealed class QuestionnaireStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    [NotNull] private readonly Questionnaire _questions = (new QuestionnaireLoader("questionnaire.yaml")).Questions;
    [AllowNull] private Answer _currentAnswer;

    public QuestionnaireStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Before()]
    public void Before()
    {
        _currentAnswer = null;
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
}