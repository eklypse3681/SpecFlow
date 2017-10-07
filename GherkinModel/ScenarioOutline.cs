using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace NUnitHarness
{
    public class ScenarioOutline : IScenarioProvider
    {
        public Feature Feature { get; }
        public string Title { get; }
        public IEnumerable<string> Tags { get; }

        public Table Examples { get; }
        public IEnumerable<Step> Steps { get; }

        public ScenarioOutline(string title, Feature feature, Table examples, IEnumerable<string> tags = null, params Step[] steps)
        {
            Title = title;
            Feature = feature;
            Examples = examples;
            Tags = tags ?? Enumerable.Empty<string>();
            Steps = steps ?? Enumerable.Empty<Step>();
        }

        public IEnumerable<Scenario> Scenarios => 
            Examples.Rows.Select(
                (row, i) =>
                    new Scenario($"{Title}:{i}", Feature, Tags,
                        Steps.Select(s => s.ApplyExampleParameters(row)).ToArray()));
    }
}