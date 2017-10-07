using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using StepDisambiguatonSpecFlowPlugin;
using TechTalk.SpecFlow;

namespace NUnitHarness.Bindings
{
    [Binding]
    public class StepDefinitions
    {
        private readonly MyItems _items;

        private class MyFeatureContext
        {
            public int Accumulator { get; set; }
        }

        public class MyItems : List<int>
        {
        }

        public StepDefinitions(MyItems items)
        {
            _items = items;
        }

        [Given(@" My accumulator is initialized to (.*)")]
        public void GivenMyAccumulatorIsInitializedTo(int p0)
        {
            FeatureContext.Current.FeatureContainer.Resolve<MyFeatureContext>().Accumulator = p0;
        }

        [DisambiguationCategory("FunCategory")]
        [Given(@" My accumulator is initialized to (.*)")]
        public void GivenMyAccumulatorIsInitializedToWithCategory(int p0)
        {
            FeatureContext.Current.FeatureContainer.Resolve<MyFeatureContext>().Accumulator = p0;
        }

        [Given(@" I enter (.*) into the calculator")]
        public void GivenIEnterIntoTheCalculator(int p0)
        {
            _items.Add(p0);
        }

        [When(@" I accumulate")]
        public void WhenIAccumulate()
        {
            var ctx = FeatureContext.Current.FeatureContainer.Resolve<MyFeatureContext>();
            ctx.Accumulator = _items.Aggregate(ctx.Accumulator, (a, i) => a + i);
        }

        [Then(@" My accumulator should be (.*)")]
        public void ThenMyAccumulatorShouldBe(int p0)
        {
            var ctx = FeatureContext.Current.FeatureContainer.Resolve<MyFeatureContext>();
            Assert.AreEqual(p0, ctx.Accumulator);
        }
    }
}