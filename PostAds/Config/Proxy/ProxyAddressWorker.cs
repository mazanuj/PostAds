namespace Motorcycle.Config.Proxy
{
    using XmlWorker;
    using System.Collections.Generic;

    internal static class ProxyAddressWorker
    {
        static ProxyAddressWorker()
        {
            ProxyListState = true;
        }

        private static List<ProxyAddressStruct> proxyList =
            new List<ProxyAddressStruct>(ProxyXmlWorker.GetProxyListFromFile());

        public static bool ProxyListState { get; private set; }

        private static readonly object Locking = new object();

        private static void UpdateProxyListAndWriteToFile()
        {
            proxyList = ProxyData.GetProxyData();
            ProxyXmlWorker.AddNewProxyListToFile(proxyList);
        }

        public static ProxyAddressStruct GetValidProxyAddress(string purpose)
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
            proxyList = ProxyXmlWorker.GetProxyListFromFile();
        }
    }
}