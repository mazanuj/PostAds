namespace Motorcycle.XmlWorker
{
    using System.Linq;
    using System.Collections;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class CaptchaXmlWorker
    {
        private const string XmlFilePath = "Main.config";

        public static void AddNewCaptchaNode(string domain, string key)
        {
            var doc = XDocument.Load(XmlFilePath);
            var root = doc.XPathSelectElement("/configuration");
            var captchaElement = new XElement("captcha", new XElement("domain") {Value = domain.ToLower()},
                new XElement("key") {Value = key.ToLower()});
            root.Add(captchaElement);
            doc.Save(XmlFilePath);
        }

        public static void ChangeCaptchaNode(string newDomain, string newKey)
        {
            var doc = XDocument.Load(XmlFilePath);
            var item = doc.XPathSelectElement("//captcha");
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

            doc.Save(XmlFilePath);
        }

        public static void RemoveCaptchaNode()
        {
            var doc = XDocument.Load(XmlFilePath);
            var item = doc.XPathSelectElement("//captcha");
            if (item == null) return;
            item.Remove();
            doc.Save(XmlFilePath);
        }

        public static string GetCaptchaValues(string element)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att = (IEnumerable) doc.XPathEvaluate(string.Format("//captcha/{0}", element.ToLower()));
            var firstOrDefault = att.Cast<XElement>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : "";
        }
    }
}