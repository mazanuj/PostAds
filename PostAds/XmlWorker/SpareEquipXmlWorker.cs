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
        
        private const string ItemXPath = "//equip/production/item[@id='{0}' and @pz='{1}' and @pe='{2}']";

        public static void AddNewItemNode(string id, string pz, string pe)
        {
            var doc = XDocument.Load(XmlFilePath);
            var production = doc.XPathSelectElement("//equip/production");

            production.Add(new XElement("item", new XAttribute("id", id.ToLower()), new XAttribute("pz", pz),
                new XAttribute("pe", pe)));

            doc.Save(XmlFilePath);
        }

        public static void ChangeItemNode(SpareEquipItem oldItem, SpareEquipItem newItem)
        {
            var doc = XDocument.Load(XmlFilePath);
            var item = doc.XPathSelectElement(string.Format(ItemXPath, oldItem.Id.ToLower(), oldItem.Pz, oldItem.Pe));

            if (item == null) return;

            item.Attribute("id").Value = newItem.Id.ToLower();
            item.Attribute("pz").Value = newItem.Pz;
            item.Attribute("pe").Value = newItem.Pe;

            doc.Save(XmlFilePath);
        }

        public static void RemoveItemNode(SpareEquipItem item)
        {
            var doc = XDocument.Load(XmlFilePath);
            var currentItem = doc.XPathSelectElement(string.Format(ItemXPath, item.Id.ToLower(),
                item.Pz, item.Pe));

            if (currentItem == null) return;

            currentItem.Remove();

            doc.Save(XmlFilePath);
        }

        public static IEnumerable<SpareEquipItem> GetAllItems()
        {
            var doc = XDocument.Load(XmlFilePath);
            var itemXElements = doc.XPathSelectElements("//equip/production/item");

            return
                itemXElements.Select(
                    e => new SpareEquipItem(e.Attribute("id").Value,
                        e.Attribute("pz").Value, e.Attribute("pe").Value))
                    .ToList();
        }

        public static string GetItemSiteValueUsingPlant(string itemId, string site)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate(string.Format("//equip/production/item[@id='{0}']/@{1}", itemId.ToLower(),
                        site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetSpareType(string itemId, string site)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate(string.Format("//equip/typeSpare/item[@id='{0}']/@{1}", itemId.ToLower(),
                        site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetEquipType(string itemId, string site)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate(string.Format("//equip/typeEquip/item[@id='{0}']/@{1}", itemId.ToLower(),
                        site.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }

        public static string GetItemSiteIdUsingPlant(string site, string value)
        {
            var doc = XDocument.Load(XmlFilePath);
            var att =
                (IEnumerable)
                    doc.XPathEvaluate(string.Format("//equip/production/item[@{0}='{1}']/@id", site.ToLower(),
                        value.ToLower()));

            var firstOrDefault = att.Cast<XAttribute>().FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : string.Empty;
        }
    }
}