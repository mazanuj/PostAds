namespace Motorcycle.Config.Proxy
{
    using XmlWorker;
    using System.Collections.Generic;

    internal static class Socks5Worker
    {
        private static List<string> proxyList = new List<string>(ProxyXmlWorker.GetProxyList());

        private static readonly object Locking = new object();

        public static string GetSocks5Proxy(string purpose)
        {
            lock (Locking)
            {
                if (proxyList == null || proxyList.Count == 0)
                {
                    proxyList = ProxyData.XroxyData();
                    ProxyXmlWorker.AddNewProxyListToFile(proxyList);
                }

                var proxyAddress = ProxyXmlWorker.GetProxyAddress(purpose);

                if (proxyAddress != null) return proxyAddress;
                proxyList = ProxyData.XroxyData();
                ProxyXmlWorker.AddNewProxyListToFile(proxyList);

                return ProxyXmlWorker.GetProxyAddress(purpose);
            }
        }

        public static void RefreshProxiesFromFile()
        {
            proxyList = ProxyXmlWorker.GetProxyList();
        }
    }
}