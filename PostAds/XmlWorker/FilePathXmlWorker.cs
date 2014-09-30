namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class FilePathXmlWorker
    {
        private const string XmlFilePath = "Main.config";

        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);

        public static string GetFilePath(string purpose)
        {
            var att = (IEnumerable)Doc.XPathEvaluate(string.Format("//file/item[@purpose='{0}']/@path", purpose.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : null;
        }

        public static void SetFilePath(string purpose, string path)
        {
            var att = (IEnumerable)Doc.XPathEvaluate(string.Format("//file/item[@purpose='{0}']/@path", purpose.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            if (firstOrDefault != null)
            {
                firstOrDefault.Value = path;
                Doc.Save(XmlFilePath);
            }
        }

        public static void ResetFilePaths()
        {
            SetFilePath("moto", "");
            SetFilePath("spare", "");
            SetFilePath("equip", "");
        }
    }
}
