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
        private object _lastFeature;

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
        public void ExecuteTests(IScenarioExecutor scenario)
        {
            ManageScenarioFeature(scenario.FeatureReference);
            scenario.Execute(_runner);          
        }

        private void ManageScenarioFeature(object feature)
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

    public interface IScenarioExecutor
    {
        void Execute(ITestRunner runner);
        object FeatureReference { get; }
    }
}