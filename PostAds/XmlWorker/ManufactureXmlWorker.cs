namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class ManufactureXmlWorker
    {
        private const string XmlFilePath = "Main.config";
        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);
        private const string ItemXPath = "//moto/manufacture/item[@id='{0}' and @m='{1}' and @p='{2}' and @u='{3}']";
        private const string XPathForGettingValues = "//moto/manufacture/item[@id='{0}' and @m='{1}' and @p='{2}' and @u='{3}']/value";
        private const string ValueXPath = "//moto/manufacture/item/value[@name='{0}' and text()='{1}']";
        private const string ValueXPathWithItemParams = "//moto/manufacture/item[@id='{0}' and @m='{1}' and @p='{2}' and @u='{3}']/value[@name='{4}' and text()='{5}']";

        #region Work with Item node

        public static void AddNewItemNode(string id, string m, string p, string u)
        {
            var manufacture = Doc.XPathSelectElement("//moto/manufacture");

            manufacture.Add(new XElement("item", new XAttribute("id", id.ToLower()), new XAttribute("m", m),
                new XAttribute("p", p), new XAttribute("u", u)));

            Doc.Save(XmlFilePath);
        }

        public static void ChangeItemNode(ManufactureItem oldItem, ManufactureItem newItem)
        {
            var item = Doc.XPathSelectElement(string.Format(ItemXPath, oldItem.Id, oldItem.M, oldItem.P, oldItem.U));

            if (item == null) return;

            item.Attribute("id").Value = newItem.Id.ToLower();
            item.Attribute("m").Value = newItem.M;
            item.Attribute("p").Value = newItem.P;
            item.Attribute("u").Value = newItem.U;

            Doc.Save(XmlFilePath);
        }

        public static void RemoveItemNode(ManufactureItem item)
        {
            var selectedItem = Doc.XPathSelectElement(string.Format(ItemXPath, item.Id, item.M, item.P, item.U));

            if (selectedItem == null) return;

            selectedItem.Remove();

            Doc.Save(XmlFilePath);
        }

        public static IEnumerable<ManufactureItem> GetItemsWithTheirValues()
        {
            var items = (from e in Doc.Descendants("manufacture").Descendants("item")
                select new ManufactureItem
                {
                    Id = (string) e.Attribute("id"),
                    M = (string) e.Attribute("m"),
                    P = (string) e.Attribute("p"),
                    U = (string) e.Attribute("u"),
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
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//moto/manufacture/item[@id='{0}']/@{1}", itemId.ToLower(),
                        site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetItemSiteIdUsingPlant(string site, string value)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//moto/manufacture/item[@{0}='{1}']/@id", site.ToLower(),
                        value.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetMotoType(string itemId, string site)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//type/item[@id='{0}']/@{1}", itemId.ToLower(), site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetMotoColor(string itemId, string site)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//type/color[@id='{0}']/@{1}", itemId.ToLower(), site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetConditionState(string itemId, string site)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//condition/item[@id='{0}']/@{1}", itemId.ToLower(), site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetMadeYear(string itemId, string site)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//year/item[@id='{0}']/@{1}", itemId.ToLower(), site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : "1";
        }

        public static string GetItemValueUsingPlantAndName(string itemId, string name)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//moto/manufacture/item[@id = '{0}']/value[@name = '{1}']",
                        itemId.ToLower(), name.ToLower()));

            var firstOrDefault = att.Cast<XElement>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetItemNameUsingValue(string site, string siteValue, string value)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//moto/manufacture/item[@{0}='{1}']/value[text() = '{2}']/@name",
                        site.ToLower(), siteValue.ToLower(), value.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        #endregion

        #region Work with Value node

        public static void AddNewValueNode(ManufactureItem item, ManufactureValue value)
        {
            var ownerItem = Doc.XPathSelectElement(string.Format(ItemXPath, item.Id, item.M, item.P, item.U));

            var val = new XElement("value", new XAttribute("name", value.Name.ToLower())) {Value = value.Val};

            ownerItem.Add(val);

            Doc.Save(XmlFilePath);
        }

        public static void ChangeValueNode(string oldName, string oldValue, string newName, string newValue)
        {
            var value = Doc.XPathSelectElement(string.Format(ValueXPath, oldName, oldValue));

            if (value == null) return;

            value.Attribute("name").Value = newName.ToLower();
            value.Value = newValue;

            Doc.Save(XmlFilePath);
        }

        public static void ChangeValueNodeUsingItemNode(ManufactureItem item, ManufactureValue oldValue,
            ManufactureValue newValue)
        {
            var value = Doc.XPathSelectElement(string.Format(ValueXPathWithItemParams, item.Id, item.M, item.P, item.U,
                oldValue.Name, oldValue.Val));

            if (value == null) return;

            value.Attribute("name").Value = newValue.Name.ToLower();
            value.Value = newValue.Val;

            Doc.Save(XmlFilePath);
        }

        public static void RemoveValueNode(string name, string value)
        {
            var val = Doc.XPathSelectElement(string.Format(ValueXPath, name, value));

            if (val == null) return;

            val.Remove();

            Doc.Save(XmlFilePath);
        }

        public static void RemoveValueNodeUsingItemNode(ManufactureItem item, ManufactureValue value)
        {
            var val = Doc.XPathSelectElement(string.Format(ValueXPathWithItemParams, item.Id, item.M, item.P, item.U,
                value.Name, value.Val));

            if (val == null) return;

            val.Remove();

            Doc.Save(XmlFilePath);
        }

        public static IEnumerable<ManufactureValue> GetValuesForItem(ManufactureItem item)
        {
            var valueXElements = Doc.XPathSelectElements(
                string.Format(XPathForGettingValues, item.Id, item.M, item.P, item.U));

            return
                valueXElements.Select(
                    valueXElement => new ManufactureValue(valueXElement.Attribute("name").Value, valueXElement.Value))
                    .ToList();
        }

        #endregion
    }
}