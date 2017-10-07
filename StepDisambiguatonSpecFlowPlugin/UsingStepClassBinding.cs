using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace StepDisambiguatonSpecFlowPlugin
{
    [Binding]
    internal class UsingStepClassBinding : Steps
    {
        private readonly CurrentCategoryManager _categoryManager;

        public UsingStepClassBinding(CurrentCategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [Before]
        public void ClearCurrentCategory()
        {
            _categoryManager.Category = null;
        }

        [Given(@"I am using \""([^""]+)\"" steps")]
        [When(@"I am using \""([^""]+)\"" steps")]
        [Then(@"I am using \""([^""]+)\"" steps")]
        public void UseStepsCategory(string category)
        {
            _categoryManager.Category = category;
        }

        [Given(@", using \""([^""]+)\"" steps,(.*)")]
        public void GivenTemporarilyUseStepsCategory(string category, string stepToExecute)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, Given);
        }
        [Given(@", using \""([^""]+)\"" steps,(.*)")]
        public void GivenTemporarilyUseStepsCategory(string category, string stepToExecute, Table tableArg)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, s => Given(s, tableArg));
        }
        [Given(@", using \""([^""]+)\"" steps,(.*)")]
        public void GivenTemporarilyUseStepsCategory(string category, string stepToExecute, string multiLineArg)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, s => Given(s, multiLineArg));
        }
        [Given(@", using \""([^""]+)\"" steps,(.*)")]
        public void GivenTemporarilyUseStepsCategory(string category, string stepToExecute, string multiLineArg, Table tableArg)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, s => Given(s, multiLineArg, tableArg));
        }

        [When(@", using \""([^""]+)\"" steps,(.*)")]
        public void WhenTemporarilyUseStepsCategory(string category, string stepToExecute)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, When);
        }
        [When(@", using \""([^""]+)\"" steps,(.*)")]
        public void WhenTemporarilyUseStepsCategory(string category, string stepToExecute, Table tableArg)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, s => When(s, tableArg));
        }
        [When(@", using \""([^""]+)\"" steps,(.*)")]
        public void WhenTemporarilyUseStepsCategory(string category, string stepToExecute, string multiLineArg)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, s => When(s, multiLineArg));
        }
        [When(@", using \""([^""]+)\"" steps,(.*)")]
        public void WhenTemporarilyUseStepsCategory(string category, string stepToExecute, string multiLineArg, Table tableArg)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, s => When(s, multiLineArg, tableArg));
        }

        [Then(@", using \""([^""]+)\"" steps,(.*)")]
        public void ThenTemporarilyUseStepsCategory(string category, string stepToExecute)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, Then);
        }
        [Then(@", using \""([^""]+)\"" steps,(.*)")]
        public void ThenTemporarilyUseStepsCategory(string category, string stepToExecute, Table tableArg)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, s => Then(s, tableArg));
        }
        [Then(@", using \""([^""]+)\"" steps,(.*)")]
        public void ThenTemporarilyUseStepsCategory(string category, string stepToExecute, string multiLineArg)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, s => Then(s, multiLineArg));
        }
        [Then(@", using \""([^""]+)\"" steps,(.*)")]
        public void ThenTemporarilyUseStepsCategory(string category, string stepToExecute, string multiLineArg, Table tableArg)
        {
            TemporarilyUseStepsCategory(category, stepToExecute, s => Then(s, multiLineArg, tableArg));
        }

        private void TemporarilyUseStepsCategory(string category, string stepToExecute, Action<string> step)
        {
            string currentCategory = _categoryManager.Category;
            _categoryManager.Category = category;

            step(stepToExecute);

            _categoryManager.Category = currentCategory;
        }
    }
}
