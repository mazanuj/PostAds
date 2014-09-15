namespace Motorcycle.XmlWorker
{
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class ProdayEquipXmlWorker
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

        public static void ChangeItemNode(string oldId, string oldPz, string oldPe,
            string newId, string newPz, string newPe)
        {
            var item = Doc.XPathSelectElement(string.Format(ItemXPath, oldId.ToLower(), oldPe, oldPz));

            if (item == null) return;

            item.Attribute("id").Value = newId.ToLower();
            item.Attribute("pz").Value = newPz;
            item.Attribute("pe").Value = newPe;

            Doc.Save(XmlFilePath);
        }

        public static void RemoveItemNode(string id, string pz, string pe)
        {
            var item = Doc.XPathSelectElement(string.Format(ItemXPath, id.ToLower(), pz, pe));

            if (item == null) return;

            item.Remove();

            Doc.Save(XmlFilePath);
        }

        public static IEnumerable GetAllItems()
        {
            var items = (from e in Doc.Descendants("production").Descendants("item")
                         select new
                         {
                             Id = (string)e.Attribute("id"),
                             M = (string)e.Attribute("pz"),
                             P = (string)e.Attribute("pe"),
                         }).ToList();

            return items;
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
    }
}
