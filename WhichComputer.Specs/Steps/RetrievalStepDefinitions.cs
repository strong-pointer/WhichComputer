using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Gherkin.Ast;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WhichComputer.Main;

namespace WhichComputer.Specs.Steps;

[Binding]
public sealed class RetrievalStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    [AllowNull] private IComputerResultHandler _handler;
    private ComputerLoader _computerLoader = new("computers.yaml");

    public RetrievalStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario()]
    public void Before()
    {
        _handler = null;
    }

    [StepArgumentTransformation]
    public SupportedServices TransformToService(String service)
    {
        Enum.TryParse(service, out SupportedServices res);
        return res;
    }

    [When("I load the HTML located at {string} for the {string} service")]
    public void whenLoadHTMLFile(string filename, SupportedServices service) => _handler = (IComputerResultHandler) Activator.CreateInstance(Util.GetHandlerForService(service), _computerLoader, filename)! ?? throw new InvalidOperationException();

    [When("I use the {string} service")]
    public void SetService(SupportedServices service)
    {
        _handler = (IComputerResultHandler) Activator.CreateInstance(Util.GetHandlerForService(service), _computerLoader)! ?? throw new InvalidOperationException();
    }

    [Then("I expect the application to throw an error when I query with {string}")]
    public void errorMustBeThrownUponQuery(string query)
    {
        Assert.NotNull(_handler);
        Assert.Catch<InvalidOperationException>(() => _handler.Fetch(query, false, 2));
    }
    
    [Then("I expect the following {int} results when I fetch results for the computer {string}:")]
    public void expectResultsFromSearch(int number, string computerName, Table table)
    {
        Assert.NotNull(_handler);
        Computer? computer = _computerLoader.Computers?.GetComputer(computerName);
        Assert.NotNull(computer);
        if (computer != null)
        {
            var results = _handler.Fetch(computer.Value, true, number).ToList();
            foreach (var row in table.Rows)
            {
                ComputerResult trueResult = results[0];
                results.RemoveAt(0);
                ComputerResult expectedResult = new ComputerResult();
                expectedResult.Computer = computer.Value;
                expectedResult.ListingName = row["listing"];
                expectedResult.Price = double.Parse(row["price"]);
                expectedResult.Used = Boolean.Parse(row["used"]);
                expectedResult.Url = row["url"];
                expectedResult.Source = _handler.Service;
                
                Assert.AreEqual(trueResult, expectedResult);
            }
        }
    }
}