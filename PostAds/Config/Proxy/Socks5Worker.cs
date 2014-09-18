using System.Net;

namespace Motorcycle.Config.Proxy
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Socks5Worker
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

        private static bool ProxyChecker(string proxyAddress)
        {
            return true;
        }

        private static List<string> ProxyFind()
        {
            var proxyList = new List<string>();

            for (var i = 0;; i++)
            {
                var downloadString =
                    new WebClient().DownloadString(
                        string.Format(
                            "http://www.xroxy.com/proxylist.php?port=&type=Socks5&ssl=&country=&latency=&reliability=&sort=reliability&desc=true&pnum={0}#table",
                            i));

                if (!downloadString.Contains("View this Proxy details")) break;

                var stop = 0;
                while (true)
                {
                    var start = downloadString.IndexOf("host=", stop);
                    if (start == -1)
                        break;
                    start += "host=".Length;
                    stop = downloadString.IndexOf("&isSocks", start);
                    proxyList.Add(downloadString.Substring(start, stop - start).Replace("&port=", ":"));
                }
            }

            return proxyList.Distinct().ToList();
        }

        public static string GetSocks5Proxy()
        {
            lock (Locking)
            {
                if (proxies == null || proxies.Count == 0)
                {
                    proxies = ProxyFind();
                    WriteProxiesToFile(proxies);
                }

                var avoidLoopHangingVar = 0;

                while (true)
                {
                    if (pointerToProxy == (proxies.Count)) pointerToProxy = 0;

                    if (ProxyChecker(proxies[pointerToProxy]))
                        return proxies[pointerToProxy++];

                    //if address doesn't work
                    proxies.RemoveAt(pointerToProxy);
                    if (proxies.Count == 0)
                    {
                        if (avoidLoopHangingVar++ == 5) break;
                        proxies = ProxyFind();
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