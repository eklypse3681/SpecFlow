using System.Globalization;
using System.Linq;
using NUnit.Framework;
using NUnitHarness;
using TechTalk.SpecFlow;

namespace NUnitSpecflowRunner
{
    public class NUnitRunner
    {
        private ITestRunner _runner;
        private Feature _lastFeature;

        [OneTimeSetUp]
        public void Setup()
        {
            _runner = TestRunnerManager.GetTestRunner();
            _runner.OnTestRunStart();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            if (_lastFeature != null)
            {
                _runner.OnFeatureEnd();
            }
            _runner.OnTestRunEnd();
        }

        [Test, TestCaseSource(typeof(ScenarioFactory), nameof(ScenarioFactory.GetScenarioTestData))]
        public void ExecuteTests(object obj)
        {
            Scenario scenario = (Scenario) obj;
            ManageScenarioFeature(scenario.Feature);
            scenario.Execute(_runner);          
        }

        private void ManageScenarioFeature(Feature feature)
        {
            if (_lastFeature != feature)
            {
                if (_lastFeature != null)
                {
                    _runner.OnFeatureEnd();
                }
                
                _lastFeature = feature;
                var featureInfo = new FeatureInfo(CultureInfo.CurrentCulture, feature.Title, feature.Description, ProgrammingLanguage.CSharp, feature.Tags.ToArray());
                _runner.OnFeatureStart(featureInfo);
            }
        }
    }
}