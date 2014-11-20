namespace Motorcycle.XmlWorker
{
    using Config.Proxy;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class ProxyXmlWorker
    {
        private const string XmlFilePath = "proxy.xml";

        private static void CheckDateAndResetValues()
        {
            var doc = XDocument.Load(XmlFilePath);
            var todayDate = DateTime.Now.AddDays(-1).ToString("yyMMddHHmmss");
            var proxyListToUpdate = doc.XPathSelectElements(string.Format("//servers/server[@date < {0}]", todayDate));

            foreach (var proxyElement in proxyListToUpdate)
            {
                proxyElement.Attribute("moto").Value = "0";
                proxyElement.Attribute("equip").Value = "0";
                proxyElement.Attribute("spare").Value = "0";
                proxyElement.Attribute("date").Value = todayDate;
            }

            doc.Save(XmlFilePath);
        }

        public static List<ProxyAddressStruct> GetProxyListFromFile()
        {
            var doc = XDocument.Load(XmlFilePath);
            var proxyList = doc.XPathSelectElements("//servers/server");

            return proxyList.Select(proxyElement => new ProxyAddressStruct
            {
                ProxyAddresses = proxyElement.Attribute("address").Value,
                Type = ProxyAddressStruct.GetProxyTypeEnumFromString(proxyElement.Attribute("type").Value)
            }).ToList();
        }

        public static void AddNewProxy(ProxyAddressStruct proxy)
        {
            var doc = XDocument.Load(XmlFilePath);
            var servers = doc.XPathSelectElement("//servers");

            servers.Add(
                new XElement(
                    "server",
                    new XAttribute("address", proxy.ProxyAddresses),
                    new XAttribute("type", proxy.Type),
                    new XAttribute("moto", "0"),
                    new XAttribute("equip", "0"),
                    new XAttribute("spare", "0"),
                    new XAttribute("date", DateTime.Now.ToString("yyMMddHHmmss"))));

            doc.Save(XmlFilePath);
        }

        public static void AddNewProxyListToFile(IEnumerable<ProxyAddressStruct> proxyList)
        {
            var doc = XDocument.Load(XmlFilePath);
            var existingList = GetProxyListFromFile();

            var todayDate = DateTime.Now.ToString("yyMMddHHmmss");
            var servers = doc.XPathSelectElement("//servers");

            if (proxyList == null) return;

            foreach (var proxy in proxyList.Where(proxy => !existingList.Contains(proxy)))
            {
                servers.Add(
                    new XElement(
                        "server",
                        new XAttribute("address", proxy.ProxyAddresses),
                        new XAttribute("type", proxy.Type),
                        new XAttribute("moto", "0"),
                        new XAttribute("equip", "0"),
                        new XAttribute("spare", "0"),
                        new XAttribute("date", todayDate)));
            }
            doc.Save(XmlFilePath);
        }

        public static void RemoveProxiesFromFile()
        {
            var doc = XDocument.Load(XmlFilePath);
            var proxyList = doc.XPathSelectElements("//servers/server");

            proxyList.Remove();

            doc.Save(XmlFilePath);
        }

        public static ProxyAddressStruct GetProxyAddress(string purpose)
        {
            var doc = XDocument.Load(XmlFilePath);
            CheckDateAndResetValues();

            var xpath = "";
            switch (purpose)
            {
                case "moto":
                    xpath = "//servers/server[@moto = 0]";
                    break;
                case "equip":
                    xpath = "//servers/server[@equip <= 2]";
                    break;
                case "spare":
                    xpath = "//servers/server[@spare <= 2]";
                    break;
            }

            var proxyList = doc.XPathSelectElements(xpath).ToList();

            if (!proxyList.Any()) return null;
            switch (purpose)
            {
                case "moto":
                    proxyList[0].Attribute("moto").Value =
                        (int.Parse(proxyList[0].Attribute("moto").Value) + 1).ToString(CultureInfo.InvariantCulture);
                    break;
                case "equip":
                    proxyList[0].Attribute("equip").Value =
                        (int.Parse(proxyList[0].Attribute("equip").Value) + 1).ToString(CultureInfo.InvariantCulture);
                    break;
                case "spare":
                    proxyList[0].Attribute("spare").Value =
                        (int.Parse(proxyList[0].Attribute("spare").Value) + 1).ToString(CultureInfo.InvariantCulture);
                    break;
            }
            doc.Save(XmlFilePath);

            return new ProxyAddressStruct
            {
                ProxyAddresses = proxyList[0].Attribute("address").Value,
                Type = ProxyAddressStruct.GetProxyTypeEnumFromString(proxyList[0].Attribute("type").Value)
            };
        }

        public static void RemoveProxyAddressFromFile(string proxyAddress)
        {
            var doc = XDocument.Load(XmlFilePath);
            var server = doc.XPathSelectElement(string.Format("//servers/server[@address='{0}']", proxyAddress));
            server.Remove();
            doc.Save(XmlFilePath);
        }

        public static void RemoveAllProxyAddressesFromFile()
        {
            var doc = XDocument.Load(XmlFilePath);
            var servers = doc.XPathSelectElements("//servers/server[@address!='localhost']");
            servers.Remove();
            doc.Save(XmlFilePath);
        }

        public static IEnumerable<ProxyAddressItem> GetProxyAddressItemsListFromFile()
        {
            var doc = XDocument.Load(XmlFilePath);
            var proxyList = doc.XPathSelectElements("//servers/server");

            return proxyList.Select(proxyElement => new ProxyAddressItem
            {
                ProxyAddress = proxyElement.Attribute("address").Value,
                Type = proxyElement.Attribute("type").Value
            }).ToList();
        }

        public static void ChangeProxyAddress(string oldProxyAddress, string newProxyAddress, string newType)
        {
            var doc = XDocument.Load(XmlFilePath);
            var item = doc.XPathSelectElement(string.Format("//servers/server[@address='{0}']", oldProxyAddress));
            if (item == null) return;

            item.Attribute("address").Value = newProxyAddress;
            item.Attribute("type").Value = newType;

            doc.Save(XmlFilePath);
        }
    }
}