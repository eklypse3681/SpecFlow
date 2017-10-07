using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Configuration;
using GherkinModel;
using GherkinModel.Configuration;

namespace NUnitHarness
{
    public class ScenarioFactory
    {
        public static IEnumerable<TestCaseData> GetScenarioTestData()
        {
            var section = ConfigurationManager.GetSection("TestConfig") as ConfigurationSectionHandler;
            if (section == null)
            {
                throw new ConfigurationErrorsException("Failed to load TestConfig section");
            }

            List<TestCaseData> data = new List<TestCaseData>();

            foreach (TestProviderElement element in section.TestProviders)
            {
                Type type = Type.GetType(element.Type);
                if (type == null)
                {
                    throw new ConfigurationErrorsException($"Failed to load TestProvider type {element.Type}");
                }
                var provider = (ITestProvider)Activator.CreateInstance(type);
                data.AddRange(ProcessGroup(new Stack<string>(), provider.GetRootFeatureGroup()));
            }

            AllScenarioLookup.RegisterScenarios(data);
            return data;
        }

        private static IEnumerable<TestCaseData> ProcessGroupFeatures(Stack<string> prefix, FeatureGroup group)
        {
            foreach (var feature in group.Features)
            {
                prefix.Push(feature.Title);
                foreach (var scenario in feature.GetScenarios())
                {
                    prefix.Push(scenario.Title);
                    yield return new TestCaseData(scenario) { TestName = $"{string.Join(".", prefix.Reverse())}" };
                    prefix.Pop();
                }
                prefix.Pop();
            }
        }

        private static IEnumerable<TestCaseData> ProcessGroup(Stack<string> prefix, FeatureGroup group)
        {
            prefix.Push(group.Name);
            var ret = ProcessGroupFeatures(prefix, group);
            ret = @group.Groups.Aggregate(ret, (current, child) => current.Concat(ProcessGroup(prefix, child))).ToArray();
            prefix.Pop();
            return ret;
        }
    }
}