namespace Motorcycle.XmlWorker
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class CityXmlWorker
    {
        private const string XmlFilePath = "Main.config";
        private const string ItemXPath = "//city/item[text() = '{0}']";

        public static void AddNewItemNode(string cityName, string m, string p, string u)
        {
            var doc = XDocument.Load(XmlFilePath);
            var city = doc.XPathSelectElement("//city");

            var element = new XElement("item", new XAttribute("m", m), new XAttribute("p", p), new XAttribute("u", u))
            {
                Value = cityName.ToLower()
            };
            city.Add(element);
            doc.Save(XmlFilePath);
        }

        public static void ChangeItemNode(string oldCityName, CityItem newItem)
        {
            var doc = XDocument.Load(XmlFilePath);
            var item = doc.XPathSelectElement(string.Format(ItemXPath, oldCityName.ToLower()));
            if (item == null) return;

            item.Attribute("m").Value = newItem.M;
            item.Attribute("p").Value = newItem.P;
            item.Attribute("u").Value = newItem.U;
            item.Value = newItem.CityName.ToLower();

            doc.Save(XmlFilePath);
        }

        public static void RemoveItemNode(string cityName)
        {
            var doc = XDocument.Load(XmlFilePath);
            var item = doc.XPathSelectElement(string.Format(ItemXPath, cityName));
            if (item == null) return;
            item.Remove();
            doc.Save(XmlFilePath);
        }

        public static string GetItemSiteValueUsingCity(string city, string site)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate(string.Format("//city/item[text() = '{0}']/@{1}", city.ToLower(), site.ToLower()));
            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static IEnumerable<CityItem> GetItems()
        {
            var doc = XDocument.Load(XmlFilePath);
            var items = (from e in doc.Descendants("city").Descendants("item")
                select new CityItem
                {
                    CityName = e.Value,
                    M = (string) e.Attribute("m"),
                    P = (string) e.Attribute("p"),
                    U = (string) e.Attribute("u")
                }).ToList();

            return items;
        }
    }
}