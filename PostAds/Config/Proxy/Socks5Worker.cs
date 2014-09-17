namespace Motorcycle.Config.Proxy
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Socks5Worker
    {
        private const string FilePath = "servers.txt";

        private static List<string> proxies = File.ReadAllLines(FilePath).ToList();

        private static int pointerToProxy;

        private static readonly object Locking = new object();

        private static void WriteProxiesToFile(IEnumerable<string> proxiesList)
        {
            using (var sw = new StreamWriter(FilePath, false))
            {
                foreach (var proxy in proxiesList)
                {
                    sw.WriteLine(proxy);
                }
            }
        }

        public static string GetSocks5Proxy()
        {
            lock (Locking)
            {
                if (proxies == null || proxies.Count == 0)
                {
                    proxies = Proxy.ProxyFind();
                    WriteProxiesToFile(proxies);
                }

                var avoidLoopHangingVar = 0;

                while (true)
                {
                    if (pointerToProxy == (proxies.Count)) pointerToProxy = 0;

                    if (Proxy.ProxyChecker(proxies[pointerToProxy]))
                        return proxies[pointerToProxy++];

                    //if address doesn't work
                    proxies.RemoveAt(pointerToProxy);
                    if (proxies.Count == 0)
                    {
                        if (avoidLoopHangingVar++ == 5) break;
                        proxies = Proxy.ProxyFind();
                        WriteProxiesToFile(proxies);
                    }
                }
                return null;
            }
        }

        public static void RefreshProxiesFromFile()
        {
            proxies = File.ReadAllLines(FilePath).ToList();
        }
    }
}
