namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal class TimerXmlWorker
    {
        private const string XmlFilePath = "Main.config";
        
        public static int GetTimerValue(string site, string purpose)
        {
            var doc = XDocument.Load(XmlFilePath);

            var att = (IEnumerable)doc.XPathEvaluate(string.Format("//timers/item[@id='{0}']/@{1}", site, purpose));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null && !string.IsNullOrEmpty(firstOrDefault.Value) ? int.Parse(firstOrDefault.Value) : 0;
        }

        public static void SetTimerValue(string site, string purpose, byte value)
        {
            var doc = XDocument.Load(XmlFilePath);

            var att = (IEnumerable)doc.XPathEvaluate(string.Format("//timers/item[@id='{0}']/@{1}", site, purpose));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            if (firstOrDefault == null) return;
            firstOrDefault.Value = value.ToString();

            doc.Save(XmlFilePath);
        }
    }
}
