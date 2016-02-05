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
                    doc.XPathEvaluate($"//docs/item[@id='{itemId.ToLower()}']/@{site.ToLower()}");

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault?.Value ?? "1";
        }
    }
}