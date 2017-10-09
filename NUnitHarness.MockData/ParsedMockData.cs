using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GherkinModel;
using TechTalk.SpecFlow.Parser;
using System.IO;
using System.Globalization;
using Gherkin;
using TechTalk.SpecFlow;

namespace NUnitHarness.MockData
{
    public class ParsedMockData : ITestProvider
    {
        public FeatureGroup GetRootFeatureGroup()
        {

            /*
                            f = new Feature("Feature I", "The first feature", null, 
                                new GivenStep(", using \"FunCategory\" steps, My accumulator is initialized to 0"));

                            f.AddProvider(new Scenario("Add two silly numbers", f, null, 
                                new GivenStep(" I enter 50 into the calculator"),
                                new AndStep(" I enter 70 into the calculator"),
                                new WhenStep(" I accumulate"),
                                new ThenStep(" My accumulator should be 120")));

                            f.AddProvider(new Scenario("Add two more numbers", f, null,
                                new GivenStep(" I enter 100 into the calculator"),
                                new AndStep(" I enter 300 into the calculator"),
                                new WhenStep(" I accumulate"),
                                new ThenStep(" My accumulator should be 400")));

                            root.AddFeature(f);

                            f = new Feature("Feature 2", "The second feature");

                            f.AddProvider(new Scenario("Add two numbers", f, null,
                                new GivenStep(" I enter 50 into the calculator"),
                                new AndStep(" I enter 70 into the calculator"),
                                new WhenStep(" I accumulate"),
                                new ThenStep(" My accumulator should be 120")));

                            f.AddProvider(new Scenario("Add two more numbers", f, null,
                                new GivenStep(" I enter 100 into the calculator"),
                                new AndStep(" I enter 300 into the calculator"),
                                new WhenStep(" I accumulate"),
                                new ThenStep(" My accumulator should be 400")));

                            nested.AddFeature(f);

                            f = new Feature("Feature 3", "With a macro!");

                            f.AddProvider(new Scenario("I will use a macro now.", f, null,
                                new GivenStep(" \"Add two silly numbers\"")));


             *  */


            string d1 = @"
Feature: Feature I
    The first feature

Background:  
    Given, using ""FunCategory"" steps, My accumulator is initialized to 0

Scenario: Add two silly numbers
    Given I enter 50 into the calculator
    And I enter 70 into the calculator
    When I accumulate
    Then My accumulator should be 120

Scenario: Add two more numbers
    Given I enter 100 into the calculator
    And I enter 300 into the calculator
    When I accumulate
    Then My accumulator should be 400
";

            string d2 = @"
Feature: Feature 2
    The second feature

Scenario: Add two numbers
    Given I enter 50 into the calculator
    And I enter 70 into the calculator
    When I accumulate
    Then My accumulator should be 120

Scenario: Add two more numbers
    Given I enter 100 into the calculator
    And I enter 300 into the calculator
    When I accumulate
    Then My accumulator should be 400
";


            string d3 = @"
Feature: Feature 3
    With a macro!

Scenario: I will use a macro now.
    Given ""Add two silly numbers""
";

            Parser parser = new Parser(new AstBuilder<Gherkin.Ast.GherkinDocument>());

            //SpecFlowGherkinParser parser = new SpecFlowGherkinParser(new CultureInfo("en-US"));
            //using (var reader = new StringReader(d1))
            //{
            //    SpecFlowDocument document = parser.Parse(reader, "some-file-name");
            //    document.SpecFlowFeature.Background.Steps.First().Text
            //}

            using (var reader = new StringReader(d1))
            {
                var document = parser.Parse(new TokenScanner(reader));
                var background = document.Feature.Children.OfType<Gherkin.Ast.Background>().SingleOrDefault();

                var feature = new Feature(document.Feature.Name, document.Feature.Description, document.Feature.Tags.Select(t => t.Name).ToArray(),
                    (background == null ? Enumerable.Empty<Step>() : background.Steps.Select(s => StepFactory(s))).ToArray());

                foreach(var child in document.Feature.Children)
                {
                    if((child is Gherkin.Ast.Background))
                    {
                        continue;
                    }

                    if(child is Gherkin.Ast.Scenario)
                    {

                    }

                    if(child is Gherkin.Ast.ScenarioOutline)
                    {
                        var scenarioOutline = (Gherkin.Ast.ScenarioOutline)child;

                        foreach(var examples in scenarioOutline.Examples)
                        {
                            examples.n
                        }

                        Table examples = new Table(scenarioOutline.Examples.First().TableHeader.Cells.Select(c => c.Value).ToArray());
                        foreach(var row in scenarioOutline.Examples.SelectMany(e => e.TableBody.))
                        
                        feature.AddProvider(new ScenarioOutline(scenarioOutline.Name, feature, scenarioOutline.Examples))

                    }
                }

                
            }
            
            //GherkinDialectProvider provider = new GherkinDialectProvider("en-US");
            //var dialect = provider.DefaultDialect;
            ////dialect.

            



                throw new NotImplementedException();
        }

        private Step StepFactory(Gherkin.Ast.Step step)
        {
            string text = step.Text;
            string multiline = null;
            if(step.Argument is Gherkin.Ast.DocString)
            {
                multiline = (step.Argument as Gherkin.Ast.DocString).Content;
            }
            Table table = CreateTable(step.Argument as Gherkin.Ast.DataTable);

            switch (step.Keyword)
            {
                case "Given":
                    return new GivenStep(text, multiline, table);
                case "When":
                    return new WhenStep(text, multiline, table);
                case "Then":
                    return new ThenStep(text, multiline, table);
                case "And":
                    return new AndStep(text, multiline, table);
                case "But":
                    return new ButStep(text, multiline, table);
                default:
                    throw new ArgumentOutOfRangeException($"step type {step.Keyword} not supported");
            }
        }

        private Table CreateTable(Gherkin.Ast.DataTable table)
        {
            if (table == null)
                return null;

            Table ret = new Table(table.Rows.First().Cells.Select(c => c.Value).ToArray());
            foreach(var row in table.Rows.Skip(1))
            {
                ret.AddRow(row.Cells.Select(c => c.Value).ToArray());
            }
            return ret;
            throw new NotImplementedException();
        }
    }
}
