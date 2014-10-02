namespace Motorcycle.XmlWorker
{
    using Config.Proxy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal static class ProxyXmlWorker
    {
        private const string XmlFilePath = "proxy.xml";

        private static readonly XDocument Doc = XDocument.Load(XmlFilePath);

        private static void CheckDateAndResetValues()
        {
            var todayDate = GenerateValidDateFormat(DateTime.Now);
            var proxyListToUpdate = Doc.XPathSelectElements(string.Format("//servers/server[@date < {0}]", todayDate));

            foreach (var proxyElement in proxyListToUpdate)
            {
                proxyElement.Attribute("moto").Value = "0";
                proxyElement.Attribute("equip").Value = "0";
                proxyElement.Attribute("spare").Value = "0";
                proxyElement.Attribute("date").Value = todayDate;
            }

            Doc.Save(XmlFilePath);
        }

        private static string GenerateValidDateFormat(DateTime date)
        {
            var month = date.Month < 10 ? string.Format("0{0}", date.Month) : date.Month.ToString();

            var day = date.Day < 10 ? string.Format("0{0}", date.Day) : date.Day.ToString();

            return string.Format("{0}{1}{2}", date.Year, month, day);
        }

        public static List<ProxyAddressStruct> GetProxyListFromFile()
        {
            var proxyList = Doc.XPathSelectElements("//servers/server");

            return proxyList.Select(proxyElement => new ProxyAddressStruct
            {
                ProxyAddresses = proxyElement.Attribute("address").Value,
                Type = ProxyAddressStruct.GetProxyTypeEnumFromString(proxyElement.Attribute("type").Value)
            }).ToList();
        }

        public static void AddNewProxy(ProxyAddressStruct proxy)
        {
            var servers = Doc.XPathSelectElement("//servers");

            servers.Add(
                new XElement(
                    "server",
                    new XAttribute("address", proxy.ProxyAddresses),
                    new XAttribute("type", proxy.Type),
                    new XAttribute("moto", "0"),
                    new XAttribute("equip", "0"),
                    new XAttribute("spare", "0"),
                    new XAttribute("date", GenerateValidDateFormat(DateTime.Now))));

            Doc.Save(XmlFilePath);
        }

        public static void AddNewProxyListToFile(IEnumerable<ProxyAddressStruct> proxyList)
        {
            var existingList = GetProxyListFromFile();

            var todayDate = GenerateValidDateFormat(DateTime.Now);

            var servers = Doc.XPathSelectElement("//servers");

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
            Doc.Save(XmlFilePath);
        }

        public static void RemoveProxiesFromFile()
        {
            var proxyList = Doc.XPathSelectElements("//servers/server");

            proxyList.Remove();

            Doc.Save(XmlFilePath);
        }

        public static ProxyAddressStruct GetProxyAddress(string purpose)
        {
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

            var proxyList = Doc.XPathSelectElements(xpath).ToList();

            if (proxyList.Any())
            {
                switch (purpose)
                {
                    case "moto":
                        proxyList[0].Attribute("moto").Value =
                            (int.Parse(proxyList[0].Attribute("moto").Value) + 1).ToString();
                        break;
                    case "equip":
                        proxyList[0].Attribute("equip").Value =
                            (int.Parse(proxyList[0].Attribute("equip").Value) + 1).ToString();
                        break;
                    case "spare":
                        proxyList[0].Attribute("spare").Value =
                            (int.Parse(proxyList[0].Attribute("spare").Value) + 1).ToString();
                        break;
                }
                Doc.Save(XmlFilePath);

                return new ProxyAddressStruct
                {
                    ProxyAddresses = proxyList[0].Attribute("address").Value,
                    Type = ProxyAddressStruct.GetProxyTypeEnumFromString(proxyList[0].Attribute("type").Value)
                };
            }
            return null;
        }

        public static void RemoveProxyAddressFromFile(string proxyAddress)
        {
            var server = Doc.XPathSelectElement(string.Format("//servers/server[@address='{0}']", proxyAddress));
            server.Remove();
            Doc.Save(XmlFilePath);
        }

        public static void RemoveAllProxyAddressesFromFile()
        {
            var servers = Doc.XPathSelectElements("//servers/server[@address!='localhost']");
            servers.Remove();
            Doc.Save(XmlFilePath);
        }

        public static IEnumerable<ProxyAddressItem> GetProxyAddressItemsListFromFile()
        {
            var proxyList = Doc.XPathSelectElements("//servers/server");

            return proxyList.Select(proxyElement => new ProxyAddressItem
            {
                ProxyAddress = proxyElement.Attribute("address").Value,
                Type = proxyElement.Attribute("type").Value
            }).ToList();
        }

        public static void ChangeProxyAddress(string oldProxyAddress, string newProxyAddress, string newType)
        {
            var item = Doc.XPathSelectElement(string.Format("//servers/server[@address='{0}']", oldProxyAddress));
            if (item == null) return;

            item.Attribute("address").Value = newProxyAddress;
            item.Attribute("type").Value = newType;

            Doc.Save(XmlFilePath);
        }
    }
}
