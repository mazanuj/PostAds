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

            var att = (IEnumerable)doc.XPathEvaluate($"//file/item[@purpose='{purpose.ToLower()}']/@path");

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault?.Value ?? string.Empty;
        }

        public static void SetFilePath(string purpose, string path)
        {
            var doc = XDocument.Load(XmlFilePath);

            var att = (IEnumerable)doc.XPathEvaluate($"//file/item[@purpose='{purpose.ToLower()}']/@path");

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
