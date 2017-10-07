using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NUnitHarness
{
    public class AllScenarioLookup
    {
        private static readonly Dictionary<string, List<Scenario>> _scenarios = new Dictionary<string, List<Scenario>>();

        public static void RegisterScenarios(IEnumerable<TestCaseData> data)
        {
            foreach (var tcd in data)
            {
                var scenario = tcd.Arguments[0] as Scenario;
                if (scenario == null)
                {
                    throw new ArgumentNullException();
                }
                List<Scenario> list;
                if (!_scenarios.TryGetValue(scenario.Title, out list))
                {
                    list = new List<Scenario>();
                    _scenarios.Add(scenario.Title, list);
                }
                list.Add(scenario);
            }
        }

        public IEnumerable<Scenario> FindMatchingScenarios(string title)
        {
            return _scenarios.TryGetValue(title, out List<Scenario> list) 
                ? list 
                : Enumerable.Empty<Scenario>();
        }
    }
}