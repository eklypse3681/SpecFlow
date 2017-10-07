using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow.Bindings.Reflection;

namespace TechTalk.SpecFlow.Bindings
{
    public class StepDefinitionBinding : MethodBinding, IStepDefinitionBinding
    {
        private readonly IReadOnlyDictionary<string, object> _extensions;
        public StepDefinitionType StepDefinitionType { get; private set; }
        public Regex Regex { get; private set; }

        public BindingScope BindingScope { get; private set; }
        public bool IsScoped { get { return BindingScope != null; } }


        public StepDefinitionBinding(StepDefinitionType stepDefinitionType, Regex regex, IBindingMethod bindingMethod, BindingScope bindingScope, IReadOnlyDictionary<string, object> extensions = null)
            : base(bindingMethod)
        {
            StepDefinitionType = stepDefinitionType;
            Regex = regex;
            BindingScope = bindingScope;
            _extensions = extensions;
        }

        public StepDefinitionBinding(StepDefinitionType stepDefinitionType, string regexString, IBindingMethod bindingMethod, BindingScope bindingScope, IReadOnlyDictionary<string, object> extensions = null)
            : this(stepDefinitionType, RegexFactory.Create(regexString), bindingMethod, bindingScope, extensions)
        {
        }

        public T GetExtension<T>(string key)
        {
            return _extensions.TryGetValue(key, out object o) && o is T ext ? ext : default(T);
        }
    }
}