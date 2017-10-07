using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow.Bindings.Reflection;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Tracing;

namespace TechTalk.SpecFlow.Bindings
{
    public interface IStepDefinitionBinding : IScopedBinding, IBinding
    {
        StepDefinitionType StepDefinitionType { get; }
        Regex Regex { get; }

        T GetExtension<T>(string key);
    }
}