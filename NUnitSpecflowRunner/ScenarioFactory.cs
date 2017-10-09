using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Configuration;
using GherkinModel;
using GherkinModel.Configuration;
using Gherkin.Ast;
using NUnitSpecflowRunner;
using TechTalk.SpecFlow;

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

    public class AstScenarioFactory
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

        private static IEnumerable<TestCaseData> ProcessGroupFeatures(Stack<string> prefix, AstFeatureGroup group)
        {
            foreach (var feature in group.Features)
            {
                prefix.Push(feature.Name);

                Background bg = feature.Children.OfType<Background>().SingleOrDefault();

                foreach (ScenarioDefinition sd in feature.Children.Where(c => !(c is Background)))
                {
                    if(sd is Gherkin.Ast.Scenario)
                    {
                        prefix.Push(sd.Name);
                        var se = new ScenarioExecutor(feature, runner =>
                        {
                            ScenarioInfo info = new ScenarioInfo(sd.Name, feature.Tags.Select(t => t.Name).Union(group.Tags).ToArray());
                            runner.OnScenarioStart(info);
                            if (bg != null)
                            {
                                foreach (var step in bg.Steps)
                                {
                                    ExecuteStep(runner, step);
                                }
                            }
                            foreach (var step in sd.Steps)
                            {
                                ExecuteStep(runner, step);
                            }
                            runner.CollectScenarioErrors();
                            runner.OnScenarioEnd();
                        });

                        yield return new TestCaseData(se) { TestName = string.Join(".", prefix.Reverse()) };
                        prefix.Pop();
                    }
                    else if(sd is Gherkin.Ast.ScenarioOutline)
                    {
                        var so = (Gherkin.Ast.ScenarioOutline)sd;

                        foreach(var exampleBlock in so.Examples)
                        {
                            exampleBlock.TableBody.Select(
                                (row, i) => new ScenarioExecutor(feature, runner =>
                                {
                                    ScenarioInfo info = new ScenarioInfo($"{so.Name}:{exampleBlock.Name}:{i}", feature.Tags.Union(so.Tags).Union(exampleBlock.Tags).Select(t => t.Name).Union(group.Tags).ToArray());
                                    runner.OnScenarioStart(info);
                                    if (bg != null)
                                    {
                                        foreach (var step in bg.Steps)
                                        {
                                            ExecuteStep(runner, step);
                                        }
                                    }
                                    foreach (var step in sd.Steps)
                                    {
                                        ExecuteStep(runner, step);
                                    }
                                    runner.CollectScenarioErrors();
                                    runner.OnScenarioEnd();
                                });
                        }

                        //Examples.Rows.Select(
                        //    (row, i) =>
                        //        new Scenario($"{Title}:{i}", Feature, Tags,
                        //            Steps.Select(s => s.ApplyExampleParameters(row)).ToArray()));

                    }


                    prefix.Push(scenario.Title);
                    yield return new TestCaseData(scenario) { TestName = $"{string.Join(".", prefix.Reverse())}" };
                    prefix.Pop();
                }
                prefix.Pop();
            }
        }

        private static Gherkin.Ast.Step ApplyExampleParameters(Gherkin.Ast.Step step, Gherkin.Ast.TableRow examples)
        {
            string text = step.Text;
            string multiLine = (step.Argument as DocString)?.Content;
            DataTable table = step.Argument as DataTable;

            foreach (TableCell cell in examples.Cells)
            {
                text = text.Replace($"<{cell.Value.Key}>", kvp.Value);
                multiLine = multiLine.Replace($"<{kvp.Key}>", kvp.Value);
                foreach (var row in ret.TableArg.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        row[key] = row[key].Replace($"<{kvp.Key}>", kvp.Value);
                    }
                }
            }
            return ret;
        }

        private static void ExecuteStep(ITestRunner runner, Gherkin.Ast.Step step)
        {
            string multiLineArg = (step.Argument as DocString)?.Content;
            Table tableArg = CreateTableArg(step.Argument as DataTable);

            switch (step.Keyword)
            {
                case "Given":
                    runner.Given(step.Text, multiLineArg, tableArg, step.Keyword);
                    break;
                case "When":
                    runner.When(step.Text, multiLineArg, tableArg, step.Keyword);
                    break;
                case "Then":
                    runner.Then(step.Text, multiLineArg, tableArg, step.Keyword);
                    break;
                case "And":
                    runner.And(step.Text, multiLineArg, tableArg, step.Keyword);
                    break;
                case "But":
                    runner.But(step.Text, multiLineArg, tableArg, step.Keyword);
                    break;
            }
        }

        private Table CreateTableArg(DataTable dt)
        {
            if(dt == null)
            {
                return null;
            }

            Table ret = new Table(dt.Rows.First().Cells.Select(c => c.Value).ToArray());
            foreach(var dr in dt.Rows.Skip(1))
            {
                ret.AddRow(dr.Cells.Select(c => c.Value).ToArray());
            }
            return ret;
        }

        private class ScenarioExecutor : IScenarioExecutor
        {
            private readonly Action<ITestRunner> _execute;

            public object FeatureReference { get; }

            public ScenarioExecutor(object feature, Action<ITestRunner> execute)
            {
                FeatureReference = feature;
                _execute = execute;
            }

            public void Execute(ITestRunner runner) => _execute(runner);
        }

        private static IEnumerable<TestCaseData> ProcessGroup(Stack<string> prefix, AstFeatureGroup group)
        {
            prefix.Push(group.Name);
            var ret = ProcessGroupFeatures(prefix, group);
            ret = @group.Groups.Aggregate(ret, (current, child) => current.Concat(ProcessGroup(prefix, child))).ToArray();
            prefix.Pop();
            return ret;
        }
    }



    public class AstFeatureGroup
    {
        public string Name { get; }
        private readonly List<Gherkin.Ast.Feature> _features = new List<Gherkin.Ast.Feature>();
        private readonly List<AstFeatureGroup> _groups = new List<AstFeatureGroup>();
        public IEnumerable<string> Tags { get; }
        public IEnumerable<Gherkin.Ast.Feature> Features => _features;
        public IEnumerable<AstFeatureGroup> Groups => _groups;

        public void AddFeature(Gherkin.Ast.Feature feature) => _features.Add(feature);
        public void AddGroup(AstFeatureGroup group) => _groups.Add(group);
        public AstFeatureGroup AddGroup(string name, params string[] tags)
        {
            var group = new AstFeatureGroup(name, tags);
            _groups.Add(group);
            return group;
        }

        public AstFeatureGroup(string name, params string[] tags)
        {
            Name = name;
            Tags = tags;
        }
    }
}