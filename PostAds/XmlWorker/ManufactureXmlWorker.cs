﻿namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class ManufactureXmlWorker
    {
        private const string XmlFilePath = "Main.config";

        private const string ItemXPath = "//moto/manufacture/item[@id='{0}' and @m='{1}' and @p='{2}' and @u='{3}' and @o='{4}']";
        private const string XPathForGettingValues = "//moto/manufacture/item[@id='{0}' and @m='{1}' and @p='{2}' and @u='{3}' and @o='{4}']/value";
        private const string ValueXPath = "//moto/manufacture/item/value[@name='{0}' and text()='{1}']";
        private const string ValueXPathWithItemParams = "//moto/manufacture/item[@id='{0}' and @m='{1}' and @p='{2}' and @u='{3}' and @o='{4}']/value[@name='{5}' and text()='{6}']";

        #region Work with Item node

        public static void AddNewItemNode(string id, string m, string p, string u, string o)
        {
            var doc = XDocument.Load(XmlFilePath);
            var manufacture = doc.XPathSelectElement("//moto/manufacture");

            manufacture.Add(new XElement("item", new XAttribute("id", id.ToLower()), new XAttribute("m", m),
                new XAttribute("p", p), new XAttribute("u", u), new XAttribute("o", o)));

            doc.Save(XmlFilePath);
        }

        public static void ChangeItemNode(ManufactureItem oldItem, ManufactureItem newItem)
        {
            var doc = XDocument.Load(XmlFilePath);
            var item = doc.XPathSelectElement(string.Format(ItemXPath, oldItem.Id, oldItem.M, oldItem.P, oldItem.U, oldItem.O));

            if (item == null) return;

            item.Attribute("id").Value = newItem.Id.ToLower();
            item.Attribute("m").Value = newItem.M;
            item.Attribute("p").Value = newItem.P;
            item.Attribute("u").Value = newItem.U;
            item.Attribute("o").Value = newItem.O;

            doc.Save(XmlFilePath);
        }

        public static void RemoveItemNode(ManufactureItem item)
        {
            var doc = XDocument.Load(XmlFilePath);
            var selectedItem = doc.XPathSelectElement(string.Format(ItemXPath, item.Id, item.M, item.P, item.U, item.O));

            if (selectedItem == null) return;

            selectedItem.Remove();

            doc.Save(XmlFilePath);
        }

        public static IEnumerable<ManufactureItem> GetItemsWithTheirValues()
        {
            var doc = XDocument.Load(XmlFilePath);
            var items = (from e in doc.Descendants("manufacture").Descendants("item")
                select new ManufactureItem
                {
                    Id = (string) e.Attribute("id"),
                    M = (string) e.Attribute("m"),
                    P = (string) e.Attribute("p"),
                    U = (string) e.Attribute("u"),
                    O = (string) e.Attribute("o"),
                    Values = e.Descendants("value")
                        .Select(r => new ManufactureValue
                        {
                            Name = (string) r.Attribute("name"),
                            Val = r.Value
                        })
                        .ToList()
                }).ToList();

            return items;
        }

        public static string GetItemSiteValueUsingPlant(string itemId, string site)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate($"//moto/manufacture/item[@id='{itemId.ToLower()}']/@{site.ToLower()}");

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault?.Value ?? string.Empty;
        }

        public static string GetItemSiteIdUsingPlant(string site, string value)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate($"//moto/manufacture/item[@{site.ToLower()}='{value.ToLower()}']/@id");

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault?.Value ?? string.Empty;
        }

        public static string GetMotoType(string itemId, string site)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate($"//type/item[@id='{itemId.ToLower()}']/@{site.ToLower()}");

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault?.Value ?? string.Empty;
        }

        public static string GetMotoColor(string itemId, string site)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate($"//color/item[@id='{itemId.ToLower()}']/@{site.ToLower()}");

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault?.Value ?? string.Empty;
        }

        public static string GetConditionState(string itemId, string site)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate($"//condition/item[@id='{itemId.ToLower()}']/@{site.ToLower()}");

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault?.Value ?? string.Empty;
        }

        public static string GetMadeYear(string itemId, string site)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate($"//year/item[@id='{itemId.ToLower()}']/@{site.ToLower()}");

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault?.Value ?? "1";
        }

        public static string GetItemValueUsingPlantAndName(string itemId, string name)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate(
                        $"//moto/manufacture/item[@id = '{itemId.ToLower()}']/value[@name = '{name.ToLower()}']");

            var firstOrDefault = att.Cast<XElement>().FirstOrDefault();

            return firstOrDefault?.Value ?? string.Empty;
        }

        public static string GetItemNameUsingValue(string site, string siteValue, string value)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate(
                        $"//moto/manufacture/item[@{site.ToLower()}='{siteValue.ToLower()}']/value[text() = '{value.ToLower()}']/@name");

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault?.Value ?? string.Empty;
        }

        #endregion

        #region Work with Value node

        public static void AddNewValueNode(ManufactureItem item, ManufactureValue value)
        {
            var doc = XDocument.Load(XmlFilePath);
            var ownerItem = doc.XPathSelectElement(string.Format(ItemXPath, item.Id, item.M, item.P, item.U, item.O));

            var val = new XElement("value", new XAttribute("name", value.Name.ToLower())) {Value = value.Val};

            ownerItem.Add(val);

            doc.Save(XmlFilePath);
        }

        public static void ChangeValueNode(string oldName, string oldValue, string newName, string newValue)
        {
            var doc = XDocument.Load(XmlFilePath);
            var value = doc.XPathSelectElement(string.Format(ValueXPath, oldName, oldValue));

            if (value == null) return;

            value.Attribute("name").Value = newName.ToLower();
            value.Value = newValue;

            doc.Save(XmlFilePath);
        }

        public static void ChangeValueNodeUsingItemNode(ManufactureItem item, ManufactureValue oldValue,
            ManufactureValue newValue)
        {
            var doc = XDocument.Load(XmlFilePath);
            var value = doc.XPathSelectElement(string.Format(ValueXPathWithItemParams, item.Id, item.M, item.P, item.U, item.O,
                oldValue.Name, oldValue.Val));

            if (value == null) return;

            value.Attribute("name").Value = newValue.Name.ToLower();
            value.Value = newValue.Val;

            doc.Save(XmlFilePath);
        }

        public static void RemoveValueNode(string name, string value)
        {
            var doc = XDocument.Load(XmlFilePath);
            var val = doc.XPathSelectElement(string.Format(ValueXPath, name, value));

            if (val == null) return;

            val.Remove();

            doc.Save(XmlFilePath);
        }

        public static void RemoveValueNodeUsingItemNode(ManufactureItem item, ManufactureValue value)
        {
            var doc = XDocument.Load(XmlFilePath);
            var val = doc.XPathSelectElement(string.Format(ValueXPathWithItemParams, item.Id, item.M, item.P, item.U, item.O,
                value.Name, value.Val));

            if (val == null) return;

            val.Remove();

            doc.Save(XmlFilePath);
        }

        public static IEnumerable<ManufactureValue> GetValuesForItem(ManufactureItem item)
        {
            var doc = XDocument.Load(XmlFilePath);
            var valueXElements = doc.XPathSelectElements(
                string.Format(XPathForGettingValues, item.Id, item.M, item.P, item.U, item.O));

            return
                valueXElements.Select(
                    valueXElement => new ManufactureValue(valueXElement.Attribute("name").Value, valueXElement.Value))
                    .ToList();
        }

        #endregion
    }
}