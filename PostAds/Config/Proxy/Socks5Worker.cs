namespace Motorcycle.Config.Proxy
{
    using XmlWorker;
    using System.Collections.Generic;

    internal static class Socks5Worker
    {
        static Socks5Worker()
        {
            ProxyListState = true;
        }

        private static List<string> proxyList = new List<string>(ProxyXmlWorker.GetProxyList());
        public static bool ProxyListState { get; private set; }

        private static readonly object Locking = new object();        

        private static void UpdateProxyListAndWriteToFile()
        {
            proxyList = ProxyData.GetProxyData();
            ProxyXmlWorker.AddNewProxyListToFile(proxyList);
        }

        public static string GetSocks5Proxy(string purpose)
        {
            lock (Locking)
            {
                if (proxyList == null || proxyList.Count == 0) UpdateProxyListAndWriteToFile();

                for (var i = 0; i < ProxyData.CountOfProxySites; i++)
                {
                    var proxyAddress = ProxyXmlWorker.GetProxyAddress(purpose);

                    if (proxyAddress != null) return proxyAddress;

                    UpdateProxyListAndWriteToFile();
                }

                ProxyListState = false;
                return null;
            }
        }

        public static void RefreshProxiesFromFile()
        {
            proxyList = ProxyXmlWorker.GetProxyList();
        }
    }
}