namespace Motorcycle.Config.Proxy
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Socks5Worker
    {
        private static List<string> proxies = File.ReadAllLines("servers.txt").ToList();

        private static int pointerToProxy;

        private static readonly object Locking = new object();

        public static string GetSocks5Proxy()
        {
            lock (Locking)
            {
                if (proxies == null || proxies.Count == 0) proxies = Proxy.ProxyFind();

                var avoidLoopHangingVar = 0;

                for (var i = 0; i <= proxies.Count; i++)
                {
                    if (pointerToProxy == (proxies.Count)) pointerToProxy = 0;

                    if (Proxy.ProxyChecker(proxies[pointerToProxy]))
                        return proxies[pointerToProxy++];

                    //if address doesn't work
                    proxies.RemoveAt(pointerToProxy);
                    i = 0;
                    if (proxies.Count == 0)
                    {
                        //if (avoidLoopHangingVar++ == 10) break;
                        proxies = Proxy.ProxyFind();
                    }
                }
                return null;
            }
        }

        public static void RefreshProxiesFromFile()
        {
            proxies = File.ReadAllLines("servers.txt").ToList();
        }
    }
}
