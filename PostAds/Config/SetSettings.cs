using System.Linq;
using System.Xml.Linq;

namespace Motorcycle.Config
{
    internal static class SetSettings
    {
        internal static bool DeleteManufacture(string id)
        {
            var xml = XDocument.Load("Main.config").Root;
            if (xml == null) return false;
            var xElement = xml.Element("manufacture");
            if (xElement == null) return false;

            foreach (var item in xElement.Elements("item").Where(item => item.Attribute("id").Value == id.ToUpper()))
                item.Remove();
            xml.Save("Main.config");
            return true;
        }

        internal static bool ChangeManufacture(string id, string newID, string m, string p, string u)
        {
            var xml = XDocument.Load("Main.config").Root;
            if (xml == null) return false;
            var xElement = xml.Element("manufacture");
            if (xElement == null) return false;

            foreach (var item in xElement.Elements("item").Where(item => item.Attribute("id").Value == id.ToUpper()))
            {
                item.Attribute("id").Value = newID.ToUpper();
                item.Attribute("m").Value = m.ToUpper();
                item.Attribute("p").Value = p.ToUpper();
                item.Attribute("u").Value = u.ToUpper();
            }
            xml.Save("Main.config");
            return true;
        }

        internal static void SetCaptcha(string key, string value)
        {
            var xml = XDocument.Load("Main.config");
            if (xml.Root == null) return;
            var xElement = xml.Root.Element("captcha");
            if (xElement == null) return;
            var element = xElement.Element(key);
            if (element == null) return;
            element.Value = value.ToLower();
            xml.Save("Main.config");
        }

        internal static bool SetManufacture(string id, string m, string p, string u)
        {
            var xml = XDocument.Load("Main.config").Root;
            if (xml == null) return false;
            var xElement = xml.Element("manufacture");
            if (xElement == null) return false;

            if (
                xElement.Elements("item")
                    .Any(
                        item =>
                            item.Attribute("id").Value == id.ToUpper() || 
                            item.Attribute("m").Value == m.ToUpper() || 
                            item.Attribute("p").Value == p.ToUpper() || 
                            item.Attribute("u").Value == u.ToUpper()))
                return false;

            var newElem = new XElement("item");
            newElem.SetAttributeValue("id", id.ToUpper());
            newElem.SetAttributeValue("m", m.ToUpper());
            newElem.SetAttributeValue("p", p.ToUpper());
            newElem.SetAttributeValue("u", u.ToUpper());
            xElement.Add(newElem);
            xml.Save("Main.config");
            return true;
        }

        internal static bool SetModel(string id, string name, string value)
        {
            var xml = XDocument.Load("Main.config").Root;
            if (xml == null) return false;
            var xElement = xml.Element("manufacture");
            if (xElement == null) return false;

            if (xElement.Elements("item")
                .Where(item => item.Attribute("id").Value == id.ToUpper())
                .Elements("value")
                .Any(t => t.Attribute("name").Value == name || t.Value == value)
                )
                return false;

            var newElem = new XElement("value");
            newElem.SetAttributeValue("name", name.ToUpper());
            newElem.SetValue(value.ToUpper());

            foreach (var item in xElement.Elements("item").Where(item => item.Attribute("id").Value == id.ToUpper()))
            {
                item.Add(newElem);
            }

            xml.Save("Main.config");
            return true;
        }

        internal static bool DeleteModel(string id, string idModel)
        {
            var xml = XDocument.Load("Main.config").Root;
            if (xml == null) return false;
            var xElement = xml.Element("manufacture");
            if (xElement == null) return false;

            foreach (var item in xElement.Elements("item")
                .Where(item => item.Attribute("id").Value == id.ToUpper()).Elements("value")
                .Where(t => t.Attribute("name").Value == idModel.ToUpper()))
                item.Remove();
            xml.Save("Main.config");
            return true;
        }

        internal static bool ChangeModel(string id, string idModel, string newID, string newKEY)
        {
            var xml = XDocument.Load("Main.config").Root;
            if (xml == null) return false;
            var xElement = xml.Element("manufacture");
            if (xElement == null) return false;

            foreach (var item in xElement.Elements("item")
                .Where(item => item.Attribute("id").Value == id.ToUpper()).Elements("value")
                .Where(t => t.Attribute("name").Value == idModel.ToUpper()))
            {
                item.Attribute("name").Value = newID.ToUpper();
                item.Value = newKEY.ToUpper();
            }
            xml.Save("Main.config");
            return true;
        }
    }
}