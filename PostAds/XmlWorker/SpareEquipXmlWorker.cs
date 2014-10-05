namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class SpareEquipXmlWorker
    {
        private const string XmlFilePath = "Main.config";
        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);
        private const string ItemXPath = "//equip/production/item[@id='{0}' and @pz='{1}' and @pe='{2}']";

        public static void AddNewItemNode(string id, string pz, string pe)
        {
            var production = Doc.XPathSelectElement("//equip/production");

            production.Add(new XElement("item", new XAttribute("id", id.ToLower()), new XAttribute("pz", pz),
                new XAttribute("pe", pe)));

            Doc.Save(XmlFilePath);
        }

        public static void ChangeItemNode(SpareEquipItem oldItem, SpareEquipItem newItem)
        {
            var item = Doc.XPathSelectElement(string.Format(ItemXPath, oldItem.Id.ToLower(), oldItem.Pz, oldItem.Pe));

            if (item == null) return;

            item.Attribute("id").Value = newItem.Id.ToLower();
            item.Attribute("pz").Value = newItem.Pz;
            item.Attribute("pe").Value = newItem.Pe;

            Doc.Save(XmlFilePath);
        }

        public static void RemoveItemNode(SpareEquipItem item)
        {
            var currentItem = Doc.XPathSelectElement(string.Format(ItemXPath, item.Id.ToLower(),
                item.Pz, item.Pe));

            if (currentItem == null) return;

            currentItem.Remove();

            Doc.Save(XmlFilePath);
        }

        public static IEnumerable<SpareEquipItem> GetAllItems()
        {
            var itemXElements = Doc.XPathSelectElements("//equip/production/item");

            return
                itemXElements.Select(
                    e => new SpareEquipItem(e.Attribute("id").Value,
                        e.Attribute("pz").Value, e.Attribute("pe").Value))
                    .ToList();
        }

        public static string GetItemSiteValueUsingPlant(string itemId, string site)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//equip/production/item[@id='{0}']/@{1}", itemId.ToLower(),
                        site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetSpareType(string itemId, string site)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//equip/typeSpare/item[@id='{0}']/@{1}", itemId.ToLower(),
                        site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : "36";
        }

        public static string GetEquipType(string itemId, string site)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//equip/typeEquip/item[@id='{0}']/@{1}", itemId.ToLower(),
                        site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : "36";
        }

        public static string GetItemSiteIdUsingPlant(string site, string value)
        {
            var att =
                (IEnumerable)
                    Doc.XPathEvaluate(string.Format("//equip/production/item[@{0}='{1}']/@id", site.ToLower(),
                        value.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }
    }
}