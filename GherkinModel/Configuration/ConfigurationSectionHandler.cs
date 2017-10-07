using System.Configuration;
using System.IO;
using System.Xml;

namespace GherkinModel.Configuration
{
    public class ConfigurationSectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("TestProviders", IsDefaultCollection = false, IsRequired = false)]
        [ConfigurationCollection(typeof(TestProviderCollection), AddItemName = "add")]
        public TestProviderCollection TestProviders
        {
            get { return (TestProviderCollection)this["TestProviders"]; }
            set { this["TestProviders"] = value; }
        }

        public static ConfigurationSectionHandler CreateFromXml(string xmlContent)
        {
            ConfigurationSectionHandler section = new ConfigurationSectionHandler();
            section.Init();
            section.Reset(null);
            using (var reader = new XmlTextReader(new StringReader(xmlContent.Trim())))
            {
                section.DeserializeSection(reader);
            }
            section.ResetModified();
            return section;
        }

        public static ConfigurationSectionHandler CreateFromXml(XmlNode xmlContent)
        {
            ConfigurationSectionHandler section = new ConfigurationSectionHandler();
            section.Init();
            section.Reset(null);
            using (var reader = new XmlNodeReader(xmlContent))
            {
                section.DeserializeSection(reader);
            }
            section.ResetModified();
            return section;
        }
    }
}
