using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Motorcycle.Config.Proxy
{
    internal static class ProxyData
    {
        internal static List<string> XroxyComData()
        {
            var proxiesList = new List<string>();

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
                    proxiesList.Add(downloadString.Substring(start, stop - start).Replace("&port=", ":"));
                }
            }

            return proxiesList.Distinct().ToList();
        }

        internal static List<string> SocksProxyNetData()
        {
            var downloadString = new WebClient().DownloadString("http://www.socks-proxy.net/");
            var array = downloadString.Substring(
                downloadString.IndexOf("<tbody>"),
                downloadString.IndexOf("</tbody>",
                    downloadString.IndexOf("<tbody>")) -
                downloadString.IndexOf("<tbody>"));

            var proxyArray = array.Split('\n');

            var proxiesList = (from value in proxyArray
                where value.Contains("Socks5")
                let startIp = value.IndexOf("<tr><td>") + "<tr><td>".Length
                let stopIp = value.IndexOf("</td><td>", startIp)
                let ip = value.Substring(startIp, stopIp - startIp)
                let startPort = stopIp + "</td><td>".Length
                let stopPort = value.IndexOf("</td><td>", startPort)
                let port = value.Substring(startPort, stopPort - startPort)
                select ip + ":" + port).ToList();

            return proxiesList.Distinct().ToList();
        }
    }
}