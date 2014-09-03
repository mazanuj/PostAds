
namespace Motorcycle.XmlWorker
{
    using System.Linq;
    using System.Collections;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal class DocsXmlWorker
    {
        private const string XmlFilePath = "Main.config";

        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);

        public static string GetItemInfo(string itemId, string site)
        {
            var att = (IEnumerable)Doc.XPathEvaluate(string.Format("//docs/item[@id='{0}']/@{1}", itemId.ToLower(), site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            if (firstOrDefault != null) return firstOrDefault.Value;

            switch (site)
            {
                case "m":
                    return "1";
                    
                case "p":
                    return "0";
                    
                default:
                    return "";
            }
        }
    }
}
