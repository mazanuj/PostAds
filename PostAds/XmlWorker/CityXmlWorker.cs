﻿using System.Linq;
using System.Collections;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Motorcycle.XmlWorker
{    
    internal static class CityXmlWorker
    {
        private const string XmlFilePath = "Main.config";
        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);
        private const string ItemXPath = "//city/item[text() = '{0}']";

        public static void AddNewItemNode(string cityName, string m, string p, string u)
        {
            var city = Doc.XPathSelectElement("//city");
            var element = new XElement("item", new XAttribute("m", m), new XAttribute("p", p), new XAttribute("u", u))
            {
                Value = cityName
            };

            city.Add(element);
            Doc.Save(XmlFilePath);
        }

        public static void ChangeItemNode(string oldCityName, CityItem newItem)
        {
            var item = Doc.XPathSelectElement(string.Format(ItemXPath, oldCityName));
            if (item == null) return;

            item.Attribute("m").Value = newItem.M;
            item.Attribute("p").Value = newItem.P;
            item.Attribute("u").Value = newItem.U;
            item.Value = newItem.CityName;

            Doc.Save(XmlFilePath);
        }

        public static void RemoveItemNode(string cityName)
        {
            var item = Doc.XPathSelectElement(string.Format(ItemXPath, cityName));
            if (item == null) return;
            item.Remove();
            Doc.Save(XmlFilePath);
        }

        public static string GetItemSiteValueUsingCity(string city, string site)
        {
            var att = (IEnumerable) Doc.XPathEvaluate(string.Format("//city/item[text() = '{0}']/@{1}", city, site));
            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Value : "";
        }
    }
}