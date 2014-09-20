using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

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

        internal static List<string> SpysRu()
        {
            var valueCol = new NameValueCollection
            {
                {"xpp", "3"},
                {"xf1", "0"},
                {"xf2", "0"},
                {"xf3", "0"},
                {"xf4", "0"}
            };

            var responseByte = new WebClient().UploadValues("http://spys.ru/en/socks-proxy-list/", "POST", valueCol);
            var downloadString = Encoding.Default.GetString(responseByte);

            var startShifr = downloadString.IndexOf("http://pagead2.googlesyndication.com/pagead/show_ads.js");
            startShifr = downloadString.IndexOf("<script type=\"text/javascript\">", startShifr) +
                         "<script type=\"text/javascript\">".Length;
            var stopShifr = downloadString.IndexOf("</script>", startShifr);
            var shifr = downloadString.Substring(startShifr, stopShifr - startShifr).Split(';');

            var shifrDictionary =
                (from s in shifr where s != string.Empty && s.Contains("^") select s.Remove(s.IndexOf("^")).Split('='))
                    .ToDictionary(dic => dic[0], dic => dic[1]);


            var startMatch = downloadString.IndexOf("<tr class=spy1x");
            var stopMatch = downloadString.IndexOf("non anonymous proxy");
            var array = downloadString.Substring(startMatch, stopMatch - startMatch);

            var proxyArray = array.Replace("\n", "").Replace("<tr class=spy1x", "\n<tr class=spy1x").Split('\n');

            return (from value in proxyArray
                where value.Contains("SOCKS5")
                let startIp = value.IndexOf("<font class=spy14>") + "<font class=spy14>".Length
                let stopIp = value.IndexOf("<script", startIp)
                let ip = value.Substring(startIp, stopIp - startIp)
                let startPort =
                    value.IndexOf("<font class=spy2>:<\\/font>\"+(", stopIp) + "<font class=spy2>:<\\/font>\"+(".Length
                let stopPort = value.IndexOf("))</script>", startPort)
                let portKeys = value.Substring(startPort, stopPort - startPort).Replace(")+(", "\n").Split('\n')
                let port =
                    portKeys.Aggregate(string.Empty,
                        (current, portKey) => current + shifrDictionary[portKey.Remove(portKey.IndexOf("^"))])
                select ip + ":" + port).Distinct().ToList();
        }
    }
}