namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal class PasswordXmlWorker
    {
        private const string XmlFilePath = "Main.config";

        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);

        public static void ChangePasswordNode(string newPassword)
        {
            var item = Doc.XPathSelectElement("//passwordConfig");
            if (item == null) return;

            var passwordElement = item.Element("password");
            if (passwordElement != null)
            {
                passwordElement.Value = newPassword;
            }

            Doc.Save(XmlFilePath);
        }

        public static void RemovePasswordNode()
        {
            var item = Doc.XPathSelectElement("//passwordConfig");
            if (item == null) return;
            item.Remove();
            Doc.Save(XmlFilePath);
        }

        public static string GetPasswordValue()
        {
            var att = (IEnumerable)Doc.XPathEvaluate("//passwordConfig/password");
            var firstOrDefault = att.Cast<XElement>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : "";
        }
    }
}
