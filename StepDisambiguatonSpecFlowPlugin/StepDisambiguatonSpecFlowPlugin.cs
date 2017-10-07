using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Bindings.Discovery;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Plugins;

namespace StepDisambiguatonSpecFlowPlugin
{
    public class StepDisambiguatonSpecFlowPlugin : IRuntimePlugin
    {
        private static bool _initialized = false;
        private static readonly object InitializationLocker = new object();

        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters)
        {
            lock (InitializationLocker)
            {
                if (_initialized)
                    return;
                _initialized = true;
            }
            runtimePluginEvents.RegisterGlobalDependencies += RuntimePluginEvents_RegisterGlobalDependencies;
        }

        private void RuntimePluginEvents_RegisterGlobalDependencies(object sender, RegisterGlobalDependenciesEventArgs e)
        {
            var stepExtenionCalculatorRegistry =
                e.ObjectContainer.Resolve<IExtensionRegistry<IStepExtensionCalculator>>();

            var stepDisambiguationRegistry =
                e.ObjectContainer.Resolve<IExtensionRegistry<IStepDisambiguator>>();

            stepExtenionCalculatorRegistry.RegisterExtension(new CategoryExtensionCalculator());
            stepDisambiguationRegistry.RegisterExtension(new CategoryDisambiguator());
        }

        private class CategoryExtensionCalculator : IStepExtensionCalculator
        {
            public IEnumerable<KeyValuePair<string, object>> GetStepExtensions(BindingSourceMethod bindingSourceMethod, BindingSourceAttribute stepDefinitionAttribute,
                BindingScope scope)
            {
                string stepCategory = bindingSourceMethod.Attributes.FirstOrDefault(
                                              a => a.AttributeType.Name == "DisambiguationCategoryAttribute")?
                                          .TryGetAttributeValue<string>("Category");

                return string.IsNullOrEmpty(stepCategory)
                    ? Enumerable.Empty<KeyValuePair<string, object>>()
                    : new[] {new KeyValuePair<string, object>("StepDisambiguatonSpecFlowPlugin::Category", stepCategory)};
            }
        }

        private class CategoryDisambiguator : IStepDisambiguator
        {
            public float Rank => 1f;
            public IEnumerable<BindingMatch> FilterMatches(IEnumerable<BindingMatch> initialMatches)
            {
                var categoryMananger = ScenarioContext.Current.ScenarioContainer.Resolve<CurrentCategoryManager>();
                if (categoryMananger.Category == null)
                {
                    return initialMatches;
                }

                return initialMatches.Where(m => m.StepBinding.GetExtension<string>("StepDisambiguatonSpecFlowPlugin::Category") == categoryMananger.Category);
            }
        }
    }

    public class CurrentCategoryManager
    {
        public string Category { get; set; }
    }

    public class DisambiguationCategoryAttribute : Attribute
    {
        public string Category { get; set; }

        public DisambiguationCategoryAttribute(string category)
        {
            Category = category;
        }
    }
}
