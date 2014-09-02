namespace Motorcycle.XmlWorker
{
    using System.Linq;
    using System.Collections;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class CaptchaXmlWorker
    {
        private const string XmlFilePath = "Main.config";

        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);

        public static void AddNewCaptchaNode(string domain, string key)
        {
            var root = Doc.XPathSelectElement("/configuration");
            var captchaElement = new XElement("captcha", new XElement("domain") {Value = domain.ToLower()},
                new XElement("key") {Value = key.ToLower()});
            root.Add(captchaElement);
            Doc.Save(XmlFilePath);
        }

        public static void ChangeCaptchaNode(string newDomain, string newKey)
        {
            var item = Doc.XPathSelectElement("//captcha");
            if (item == null) return;

            var domainElement = item.Element("domain");
            if (domainElement != null)
            {
                domainElement.Value = newDomain.ToLower();
            }

            var keyElement = item.Element("key");
            if (keyElement != null)
            {
                keyElement.Value = newKey.ToLower();
            }

            Doc.Save(XmlFilePath);
        }

        public static void RemoveCaptchaNode()
        {
            var item = Doc.XPathSelectElement("//captcha");
            if (item == null) return;
            item.Remove();
            Doc.Save(XmlFilePath);
        }

        public static string GetCaptchaValues(string element)
        {
            var att = (IEnumerable) Doc.XPathEvaluate(string.Format("//captcha/{0}", element.ToLower()));
            var firstOrDefault = att.Cast<XElement>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : "";
        }
    }
}