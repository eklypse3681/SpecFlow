using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace NUnitHarness
{
    public class Scenario : IScenarioProvider
    {
        public Feature Feature { get; }
        public string Title { get; }
        public IEnumerable<string> Tags { get; }
        public IEnumerable<Step> Steps { get; }

        public Scenario(string title, Feature feature, IEnumerable<string> tags = null, params Step[] steps)
        {
            Title = title;
            Feature = feature;
            Tags = tags ?? Enumerable.Empty<string>();
            Steps = steps ?? Enumerable.Empty<Step>();
        }

        public void Execute(ITestRunner runner)
        {
            ScenarioInfo info = new ScenarioInfo(Title, Tags.ToArray());
            runner.OnScenarioStart(info);
            RunBackground(runner);
            foreach (var step in Steps)
            {
                step.Run(runner);
            }
            runner.CollectScenarioErrors();
            runner.OnScenarioEnd();
        }

        private void RunBackground(ITestRunner runner)
        {
            foreach (var step in Feature.Background)
            {
                step.Run(runner);
            }
        }

        public IEnumerable<Scenario> Scenarios { get { yield return this; } }
    }
}