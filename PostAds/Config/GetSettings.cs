using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Motorcycle.Config
{
    internal static class GetSettings
    {
        internal static string GetCaptcha(string key)
        {
            var xElement = XDocument.Load("Main.config").Root;
            if (xElement == null) return string.Empty;
            var element = xElement.Element("captcha");
            if (element == null) return string.Empty;
            var xElement1 = element.Element(key);
            return xElement1 == null ? null : xElement1.Value;
        }

        internal static IEnumerable<AddManufacture> GetManufactures
        {
            get
            {
                var xElement = XDocument.Load("Main.config").Root;
                if (xElement == null) return null;
                var xml = xElement.Element("manufacture");

                return xml == null
                    ? null
                    : xml.Elements("item")
                        .Select(
                            item =>
                                new AddManufacture(item.Attribute("id").Value, item.Attribute("m").Value,
                                    item.Attribute("p").Value, item.Attribute("u").Value)).ToList();
            }
        }

        internal static Dictionary<string, string> GetModels(string name)
        {
            var xElement = XDocument.Load("Main.config").Root;
            if (xElement == null)
                return null;
            var xml = xElement.Element("manufacture");

            return xml == null
                ? null
                : new Dictionary<string, string>(
                    xml.Elements("item")
                        .Where(e => e.Attribute("id").Value.ToLower() == name).Elements()
                        .ToDictionary(element => element.Attribute("name").Value, element => element.Value));
        }
    }
}