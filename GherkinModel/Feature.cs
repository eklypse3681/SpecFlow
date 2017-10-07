using System.Collections.Generic;
using System.Linq;

namespace NUnitHarness
{
    public class Feature
    {
        private readonly List<IScenarioProvider> _providers = new List<IScenarioProvider>();
        public string Description { get; }
        public string Title { get; }
        public IEnumerable<string> Tags { get; }
        public IEnumerable<Step> Background { get; }

        public Feature(string title, string description, IEnumerable<string> tags = null, params Step[] backgroundSteps)
        {
            Title = title;
            Description = description;
            Tags = tags ?? Enumerable.Empty<string>();
            Background = backgroundSteps ?? Enumerable.Empty<Step>();
        }

        public IEnumerable<IScenarioProvider> Providers => _providers;

        public void AddProvider(IScenarioProvider provider) => _providers.Add(provider);

        public void AddProviders(params IScenarioProvider[] providers) => _providers.AddRange(providers);

        public void AddProviders(IEnumerable<IScenarioProvider> providers) => _providers.AddRange(providers);

        public IEnumerable<Scenario> GetScenarios() => Providers.SelectMany(p => p.Scenarios);
    }
}