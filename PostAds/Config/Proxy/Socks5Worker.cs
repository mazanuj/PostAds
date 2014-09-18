namespace Motorcycle.Config.Proxy
{
    using Motorcycle.XmlWorker;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    internal class Socks5Worker
    {
        private static List<string> proxyList = ProxyXmlWorker.GetProxyList();

        private static readonly object Locking = new object();

        private static List<string> ProxyFind()
        {
            var proxyList = new List<string>();

            for (var i = 0; ; i++)
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

        public static string GetSocks5Proxy(string purpose)
        {
            lock (Locking)
            {
                if (proxyList == null || proxyList.Count == 0)
                {
                    proxyList = ProxyFind();
                    ProxyXmlWorker.AddNewProxyListToFile(proxyList);
                }

                var proxyAddress = ProxyXmlWorker.GetProxyAddress(purpose);

                if (proxyAddress == null)
                {
                    proxyList = ProxyFind();
                    ProxyXmlWorker.AddNewProxyListToFile(proxyList);

                    return ProxyXmlWorker.GetProxyAddress(purpose);
                }

                return proxyAddress;
            }
        }

        public static void RefreshProxiesFromFile()
        {
            proxyList = ProxyXmlWorker.GetProxyList();
        }
    }
}