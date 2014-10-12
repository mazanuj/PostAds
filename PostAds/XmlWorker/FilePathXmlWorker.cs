namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class FilePathXmlWorker
    {
        private const string XmlFilePath = "Main.config";
        private static readonly object Locker = new object();

        public static string GetFilePath(string purpose)
        {
            var doc = XDocument.Load(XmlFilePath);

            var att = (IEnumerable)doc.XPathEvaluate(string.Format("//file/item[@purpose='{0}']/@path", purpose.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : null;
        }

        public static void SetFilePath(string purpose, string path)
        {
            var doc = XDocument.Load(XmlFilePath);

            var att = (IEnumerable)doc.XPathEvaluate(string.Format("//file/item[@purpose='{0}']/@path", purpose.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            if (firstOrDefault == null) return;
            firstOrDefault.Value = path;
            lock (Locker)
            {
                doc.Save(XmlFilePath);
            }
        }

        public static void ResetFilePaths()
        {
            SetFilePath("moto", "");
            SetFilePath("spare", "");
            SetFilePath("equip", "");
            SetFilePath("photo", "");
        }
    }
}
