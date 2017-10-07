using GherkinModel;

namespace NUnitHarness.MockData
{
    public class MockData : ITestProvider
    {
        public FeatureGroup GetRootFeatureGroup()
        {
            var root = new FeatureGroup("Default");
                var nested = root.AddGroup("Nested");

                Feature f;

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

                nested.AddFeature(f);
                return root;
        }
    }
}