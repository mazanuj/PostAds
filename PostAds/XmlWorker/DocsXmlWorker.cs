namespace Motorcycle.XmlWorker
{
    using System.Linq;
    using System.Collections;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class DocsXmlWorker
    {
        private const string XmlFilePath = "Main.config";

        public static string GetItemInfo(string itemId, string site)
        {
            var doc = XDocument.Load(XmlFilePath);

            var att =
                (IEnumerable)
                    doc.XPathEvaluate(string.Format("//docs/item[@id='{0}']/@{1}", itemId.ToLower(), site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : "1";
        }
    }
}