using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal class Proday2KolesaXmlWorker
    {
        private const string XmlFilePath = "Main.config";
        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);
        private const string ItemXPath = "//proday2kolesa/production/item[@id='{0}' and @s='{1}' and @e='{2}']";

        public static void AddNewItemNode(string id, string s, string e)
        {
            var manufacture = Doc.XPathSelectElement("//proday2kolesa/production");

            manufacture.Add(new XElement("item", new XAttribute("id", id.ToLower()), new XAttribute("s", s),
                new XAttribute("e", e)));

            Doc.Save(XmlFilePath);
        }

        public static void ChangeItemNode(Proday2KolesaItem oldItem, Proday2KolesaItem newItem)
        {
            var item = Doc.XPathSelectElement(string.Format(ItemXPath, oldItem.Id, oldItem.S, oldItem.E));

            if (item == null) return;

            item.Attribute("id").Value = newItem.Id.ToLower();
            item.Attribute("s").Value = newItem.S;
            item.Attribute("e").Value = newItem.E;

            Doc.Save(XmlFilePath);
        }

        public static void RemoveItemNode(Proday2KolesaItem item)
        {
            var selectedItem = Doc.XPathSelectElement(string.Format(ItemXPath, item.Id, item.S, item.E));

            if (selectedItem == null) return;

            selectedItem.Remove();

            Doc.Save(XmlFilePath);
        }

        public static IEnumerable<Proday2KolesaItem> GetItems()
        {
            var itemXElements = Doc.XPathSelectElements("//proday2kolesa/production/item");

            return
                itemXElements.Select(
                    e => new Proday2KolesaItem(e.Attribute("id").Value,
                        e.Attribute("s").Value, e.Attribute("e").Value))
                    .ToList();
        }

        public static string GetValueUsingPlant(string plant, string purpose)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//proday2kolesa/production/item[@id='{0}']/@{1}", plant.ToLower(),
                        purpose.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }
    }
}
