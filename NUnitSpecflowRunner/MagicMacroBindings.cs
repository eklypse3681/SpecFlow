using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnitHarness;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace NUnitSpecflowRunner
{
    [Binding]
    public class MagicMacroBindings : Steps
    {
        private readonly IContextManager _contextManager;

        public MagicMacroBindings(IContextManager contextManager)
        {
            _contextManager = contextManager;
        }

        [Given(@" \""([^""]+)\""")]
        [When(@" \""([^""]+)\""")]
        [Then(@" \""([^""]+)\""")]
        public void ExecuteMacro(string scenarioName)
        {
            AllScenarioLookup lookup =
                _contextManager.TestThreadContext.TestThreadContainer.Resolve<AllScenarioLookup>();

            var matches = lookup.FindMatchingScenarios(scenarioName).ToArray();

            switch (matches.Length)
            {
                case 0:
                    Assert.Fail($"Could not find scenario {scenarioName}");
                    break;
                case 1:
                    Scenario scenario = matches.Single();
                    foreach (var step in scenario.Feature.Background.Concat(scenario.Steps))
                    {
                        step.Run(TestRunner);
                    }
                    break;
                default:
                    Assert.Fail("More than one match found for specified scenario name");
                    break;
            }
        }

        [Given(@" \""([^""]+)\""")]
        [When(@" \""([^""]+)\""")]
        [Then(@" \""([^""]+)\""")]
        public void ExecuteMacro(string scenarioName, Table tableArg)
        {
            AllScenarioLookup lookup =
                _contextManager.TestThreadContext.TestThreadContainer.Resolve<AllScenarioLookup>();

            var matches = lookup.FindMatchingScenarios(scenarioName).ToArray();

            switch (matches.Length)
            {
                case 0:
                    Assert.Fail($"Could not find scenario {scenarioName}");
                    break;
                case 1:
                    Scenario scenario = matches.Single();
                    for (int i = 0; i < tableArg.Header.Count - 1; i++)
                    {
                        foreach (var step in scenario.Feature.Background.Concat(scenario.Steps))
                        {
                            step.ApplyMacroParameters(tableArg, i).Run(TestRunner);
                        }
                    }
                    break;
                default:
                    Assert.Fail("More than one match found for specified scenario name");
                    break;
            }
        }
    }
}
