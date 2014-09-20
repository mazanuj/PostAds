using System;
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

        internal static List<string> LetUsHideComData()
        {
            var proxiesList = new List<string>();
            var matchString =
                new WebClient().DownloadString(
                    "http://letushide.com/filter/socks5,all,all/list_of_free_SOCKS5_proxy_servers");
            var startMatch = matchString.IndexOf("<div class=\"count\">") + "<div class=\"count\">".Length;
            var stopMatch = matchString.IndexOf(" ", startMatch);
            var result = int.Parse(matchString.Substring(startMatch, stopMatch - startMatch));

            for (var i = 0; proxiesList.Count() < result; i++)
            {
                var downloadString =
                    new WebClient().DownloadString(
                        string.Format(
                            "http://letushide.com/filter/socks5,all,all/{0}/list_of_free_SOCKS5_proxy_servers", i));

                var stopIp = 0;
                while (true)
                {
                    var startIp = downloadString.IndexOf("<a href=\"/ip/", stopIp);
                    if (startIp == -1)
                        break;
                    startIp = downloadString.IndexOf("\">", startIp) + "\">".Length;

                    stopIp = downloadString.IndexOf("</td><td><a href=", startIp);

                    var address = downloadString.Substring(startIp, stopIp - startIp);
                    proxiesList.Add(address.Replace("</a></td><td>", ":"));
                }
            }
            return proxiesList.Distinct().ToList();
        }

        internal static List<string> MyIpTestCom()
        {
            var downloadString =
                new WebClient().DownloadString(
                    "http://www.myiptest.com/staticpages/index.php/Free-SOCKS5-SOCKS4-Proxy-lists.html");


            var startMatch = downloadString.IndexOf("<td style='width:3px;'>");
            var stopMatch = downloadString.LastIndexOf("<td style='width:3px;'>", StringComparison.Ordinal);
            var array = downloadString.Substring(startMatch, stopMatch - startMatch);

            var proxyArray = downloadString.Replace("\n", "").Replace("<tr><td style='width:3px;'>", "^").Split('^');

            var proxiesList = (from value in proxyArray
                where value.Contains("Socks5")
                let startIp = value.IndexOf("<td style='width:3px;'>") + "<td style='width:3px;'>".Length
                let stopIp = value.IndexOf("</td><td style='width:3px;'>", startIp)
                let ip = value.Substring(startIp, stopIp - startIp)
                select ip).ToList();

            return proxiesList.Distinct().ToList();
        }
    }
}