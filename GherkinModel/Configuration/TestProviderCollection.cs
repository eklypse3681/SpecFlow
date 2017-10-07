using System.Collections.Generic;
using System.Configuration;

namespace GherkinModel.Configuration
{
    public class TestProviderCollection : ConfigurationElementCollection, IEnumerable<TestProviderElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TestProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TestProviderElement)element).Name;
        }

        IEnumerator<TestProviderElement> IEnumerable<TestProviderElement>.GetEnumerator()
        {
            foreach (var item in this)
            {
                yield return (TestProviderElement)item;
            }
        }
    }
}