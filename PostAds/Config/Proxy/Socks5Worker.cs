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

        private static bool ProxyChecker(string proxy)
        {
            return true;
        }

        private static List<string> ProxyFind()
        {
            var list = new List<string>();
            list.Add("1.1.1.1:11");
            list.Add("2.2.2.2:22");
            list.Add("3.3.3.3:33");
            list.Add("4.4.4.4:44");
            return list;
        }

        public static string GetSocks5Proxy()
        {
            lock (Locking)
            {
                if (proxies == null || proxies.Count == 0) proxies = ProxyFind();

                var avoidLoopHangingVar = 0;

                for (var i = 0; i <= proxies.Count; i++)
                {
                    if (pointerToProxy == (proxies.Count)) pointerToProxy = 0;

                    if (ProxyChecker(proxies[pointerToProxy]))
                        return proxies[pointerToProxy++];

                    //if address doesn't work
                    proxies.RemoveAt(pointerToProxy);
                    i = 0;
                    if (proxies.Count == 0)
                    {
                        //if (avoidLoopHangingVar++ == 10) break;
                        proxies = ProxyFind();
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
