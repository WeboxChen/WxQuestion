using System.Configuration;
using System.Xml;

namespace Wei.Core.Configuration
{
    /// <summary>
    /// Represents a WeiConfig
    /// </summary>
    public partial class WeiConfigSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new WeiConfig();
            var node = section.SelectSingleNode("Name");
            if (node != null)
                config.Name = node.InnerText;
            return config;
        }
    }
}

