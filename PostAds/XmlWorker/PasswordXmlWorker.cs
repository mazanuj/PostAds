namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class PasswordXmlWorker
    {
        private const string XmlFilePath = "Main.config";

        public static void ChangePasswordNode(string newPassword)
        {
            var doc = XDocument.Load(XmlFilePath);
            var item = doc.XPathSelectElement("//passwordConfig");
            if (item == null) return;

            var passwordElement = item.Element("password");
            if (passwordElement != null)
            {
                passwordElement.Value = newPassword;
            }

            doc.Save(XmlFilePath);
        }

        public static void RemovePasswordNode()
        {
            var doc = XDocument.Load(XmlFilePath);
            var item = doc.XPathSelectElement("//passwordConfig");
            if (item == null) return;
            item.Remove();
            doc.Save(XmlFilePath);
        }

        public static string GetPasswordValue()
        {
            var doc = XDocument.Load(XmlFilePath);
            var att = (IEnumerable) doc.XPathEvaluate("//passwordConfig/password");
            var firstOrDefault = att.Cast<XElement>().FirstOrDefault();

            return firstOrDefault?.Value ?? "";
        }
    }
}