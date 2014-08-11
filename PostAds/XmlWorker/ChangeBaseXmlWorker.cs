namespace Motorcycle.XmlWorker
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal class ChangeBaseXmlWorker
    {
        private const string XmlFilePath = "Main.config";

        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);

        private const string ItemXPath = "//manufacture/item[@id='{0}' and @m='{1}' and @p='{2}' and @u='{3}']";

        private const string XPathForGettingValues = "//manufacture/item[@id='{0}' and @m='{1}' and @p='{2}' and @u='{3}']/value";

        private const string ValueXPath = "//manufacture/item/value[@name='{0}' and text()='{1}']";

        private const string ValueXPathWithItemParams =
            "//manufacture/item[@id='{0}' and @m='{1}' and @p='{2}' and @u='{3}']/value[@name='{4}' and text()='{5}']";

        #region Work with Item node

        public static void AddNewItemNode(string id, string m, string p, string u)
        {
            var manufacture = Doc.XPathSelectElement("//manufacture");

            manufacture.Add(new XElement("item", new XAttribute("id", id), new XAttribute("m", m),
                new XAttribute("p", p), new XAttribute("u", u)));

            Doc.Save(XmlFilePath);
        }

        public static void ChangeItemNode(Item oldItem, Item newItem)
        {
            var item = Doc.XPathSelectElement(string.Format(ItemXPath, oldItem.Id, oldItem.M, oldItem.P, oldItem.U));

            if (item == null) return;

            item.Attribute("id").Value = newItem.Id;
            item.Attribute("m").Value = newItem.M;
            item.Attribute("p").Value = newItem.P;
            item.Attribute("u").Value = newItem.U;

            Doc.Save(XmlFilePath);
        }

        public static void RemoveItemNode(Item item)
        {
            var selectedItem = Doc.XPathSelectElement(string.Format(ItemXPath, item.Id, item.M, item.P, item.U));

            if (selectedItem == null) return;

            selectedItem.Remove();

            Doc.Save(XmlFilePath);
        }

        public static IEnumerable<Item> GetItemsWithTheirValues()
        {
            var items = (from e in Doc.Descendants("manufacture").Descendants("item")
                         select new Item
                         {
                             Id = (string)e.Attribute("id"),
                             M = (string)e.Attribute("m"),
                             P = (string)e.Attribute("p"),
                             U = (string)e.Attribute("u"),
                             Values = e.Descendants("value")
                                 .Select(r => new Value
                                 {
                                     Name = (string)r.Attribute("name"),
                                     Val = r.Value
                                 })
                                 .ToList()
                         }).ToList();

            return items;
        }

        #endregion

        #region Work with Value node

        public static void AddNewValueNode(Item item, Value value)
        {
            var ownerItem = Doc.XPathSelectElement(string.Format(ItemXPath, item.Id, item.M, item.P, item.U));

            var val = new XElement("value", new XAttribute("name", value.Name)) { Value = value.Val };

            ownerItem.Add(val);

            Doc.Save(XmlFilePath);
        }

        public static void ChangeValueNode(string oldName, string oldValue, string newName, string newValue)
        {
            var value = Doc.XPathSelectElement(string.Format(ValueXPath, oldName, oldValue));

            if (value == null) return;

            value.Attribute("name").Value = newName;
            value.Value = newValue;

            Doc.Save(XmlFilePath);
        }

        public static void ChangeValueNodeUsingItemNode(Item item, Value oldValue, Value newValue)
        {
            var value = Doc.XPathSelectElement(string.Format(ValueXPathWithItemParams, item.Id, item.M, item.P, item.U,
                oldValue.Name, oldValue.Val));

            if (value == null) return;

            value.Attribute("name").Value = newValue.Name;
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

        public static void RemoveValueNodeUsingItemNode(Item item, Value value)
        {
            var val = Doc.XPathSelectElement(string.Format(ValueXPathWithItemParams, item.Id, item.M, item.P, item.U,
                value.Name, value.Val));

            if (val == null) return;

            val.Remove();

            Doc.Save(XmlFilePath);
        }

        public static List<Value> GetValuesForItem(Item item)
        {
            var valueXElements = Doc.XPathSelectElements(
                string.Format(XPathForGettingValues, item.Id, item.M, item.P, item.U));

            return valueXElements.Select(valueXElement => new Value(valueXElement.Attribute("name").Value, valueXElement.Value)).ToList();
        }
        #endregion
    }
}