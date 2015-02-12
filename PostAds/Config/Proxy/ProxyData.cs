using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;
using xNet.Net;

namespace Motorcycle.Config.Proxy
{
    using System.Collections.Generic;
    using NLog;

    internal static class ProxyData
    {
        private static int loopVar;
        private const string ErrorMsg = "Error in getting proxy list from";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public const int CountOfProxySites = 7;

        public static List<ProxyAddressStruct> GetProxyData()
        {
            if (loopVar >= CountOfProxySites) return null; //loopVar = 0;

            switch (loopVar)
            {
                case 0:
                    loopVar++;
                    return ProxyChecker.ProxyAddresses(HideMyAssCom());
                case 1:
                    loopVar++;
                    return ProxyChecker.ProxyAddresses(RosinstrumentCom());

                case 2:
                    loopVar++;
                    return ProxyChecker.ProxyAddresses(LetUsHideComData());

                case 3:
                    loopVar++;
                    return ProxyChecker.ProxyAddresses(SpysRu());

                case 4:
                    loopVar++;
                    return ProxyChecker.ProxyAddresses(XroxyComData());

                case 5:
                    loopVar++;
                    return ProxyChecker.ProxyAddresses(MyIpTestCom());

                case 6:
                    loopVar++;
                    return ProxyChecker.ProxyAddresses(SocksProxyNetData());

                default:
                    loopVar++;
                    return ProxyChecker.ProxyAddresses(HideMyAssCom());
            }
        }

        public static IEnumerable<ProxyAddressStruct> GetProxyDataAllAtOnce()
        {
            var proxyList = new List<ProxyAddressStruct>();
            proxyList.AddRange(ProxyChecker.ProxyAddresses(HideMyAssCom()));
            proxyList.AddRange(ProxyChecker.ProxyAddresses(RosinstrumentCom()));
            //proxyList.AddRange(ProxyChecker.ProxyAddresses(SocksProxyNetData()));
            //proxyList.AddRange(ProxyChecker.ProxyAddresses(LetUsHideComData()));
            //proxyList.AddRange(ProxyChecker.ProxyAddresses(SpysRu()));
            //proxyList.AddRange(ProxyChecker.ProxyAddresses(XroxyComData()));
            //proxyList.AddRange(ProxyChecker.ProxyAddresses(MyIpTestCom()));

            return proxyList;
        }

        private static IEnumerable<string> XroxyComData()
        {
            try
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
                        if (start == -1) break;
                        start += "host=".Length;
                        stop = downloadString.IndexOf("&isSocks", start);
                        proxiesList.Add(downloadString.Substring(start, stop - start).Replace("&port=", ":"));
                    }
                }

                return proxiesList.Distinct().ToList();
            }
            catch (Exception)
            {
                Log.Error(string.Format("{0} {1}", ErrorMsg, "www.xroxy.com"), null, null);
                return new List<string>();
            }
        }

        private static IEnumerable<string> SocksProxyNetData()
        {
            try
            {
                var downloadString = new WebClient().DownloadString("http://www.socks-proxy.net/");
                var array = downloadString.Substring(
                    downloadString.IndexOf("<tbody>"),
                    downloadString.IndexOf("</tbody>", downloadString.IndexOf("<tbody>"))
                    - downloadString.IndexOf("<tbody>"));

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
            catch (Exception)
            {
                Log.Error(string.Format("{0} {1}", ErrorMsg, "www.socks-proxy.net"), null, null);
                return new List<string>();
            }
        }

        private static IEnumerable<string> LetUsHideComData()
        {
            try
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
            catch (Exception)
            {
                Log.Error(string.Format("{0} {1}", ErrorMsg, "www.letushide.com"), null, null);
                return new List<string>();
            }
        }

        private static IEnumerable<string> MyIpTestCom()
        {
            try
            {
                var downloadString =
                    new WebClient().DownloadString(
                        "http://www.myiptest.com/staticpages/index.php/Free-SOCKS5-SOCKS4-Proxy-lists.html");

                var startMatch = downloadString.IndexOf("<td style='width:3px;'>");
                var stopMatch = downloadString.LastIndexOf("<td style='width:3px;'>", StringComparison.Ordinal);
                var array = downloadString.Substring(startMatch, stopMatch - startMatch);

                var proxyArray = array.Replace("\n", "")
                    .Replace("<tr><td style='width:3px;'>", "^")
                    .Split('^');

                return (from value in proxyArray
                    where value.Contains("Socks5")
                    let startIp =
                        value.IndexOf("<td style='width:3px;'>") + "<td style='width:3px;'>".Length
                    let stopIp = value.IndexOf("</td><td style='width:3px;'>", startIp)
                    let ip = value.Substring(startIp, stopIp - startIp)
                    select ip).Distinct().ToList();
            }
            catch (Exception)
            {
                Log.Error(string.Format("{0} {1}", ErrorMsg, "www.myiptest.com"), null, null);
                return new List<string>();
            }
        }

        private static IEnumerable<string> SpysRu()
        {
            try
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
                    (from s in shifr
                        where s != string.Empty && s.Contains("^")
                        select s.Remove(s.IndexOf("^")).Split('='))
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
                        value.IndexOf("<font class=spy2>:<\\/font>\"+(", stopIp) +
                        "<font class=spy2>:<\\/font>\"+(".Length
                    let stopPort = value.IndexOf("))</script>", startPort)
                    let portKeys = value.Substring(startPort, stopPort - startPort).Replace(")+(", "\n").Split('\n')
                    let port =
                        portKeys.Aggregate(string.Empty,
                            (current, portKey) => current + shifrDictionary[portKey.Remove(portKey.IndexOf("^"))])
                    select ip + ":" + port).Distinct().ToList();
            }
            catch (Exception)
            {
                Log.Error(string.Format("{0} {1}", ErrorMsg, "www.spys.ru"), null, null);
                return new List<string>();
            }
        }

        private static IEnumerable<string> RosinstrumentCom()
        {
            try
            {
                using (var request = new HttpRequest())
                {
                    request.KeepAlive = true;
                    request.AddHeader(HttpHeader.Accept,
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    request.UserAgent =
                        "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.120 Safari/537.36";
                    request.Referer = "http://tools.rosinstrument.com/cgi-bin/login.pl";
                    request.AddHeader(HttpHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                    request.Get("http://tools.rosinstrument.com/proxy/");

                    request.Referer = "http://tools.rosinstrument.com/proxy/";
                    var responseStr = request.Get("http://tools.rosinstrument.com/proxy/l100.xml").ToString();
                    var respStr = request.Get("http://tools.rosinstrument.com/proxy/plab100.xml").ToString();

                    var Doc = XDocument.Parse(responseStr);
                    var att =
                        (IEnumerable)
                            Doc.XPathEvaluate(string.Format("//item/title"));

                    Doc = XDocument.Parse(respStr);
                    var abb =
                        (IEnumerable)
                            Doc.XPathEvaluate(string.Format("//item/title"));

                    return
                        att.Cast<XElement>().Concat(abb.Cast<XElement>()).Select(val => val.Value).Distinct().ToList();
                }
            }
            catch (Exception)
            {
                Log.Error(string.Format("{0} {1}", ErrorMsg, "www.rosinstrument.com"), null, null);
                return new List<string>();
            }
        }

        private static IEnumerable<string> HideMyAssCom()
        {
            try
            {
                var list = new List<string>();

                var doc = new HtmlWeb().Load("http://proxylist.hidemyass.com/search-1292985/1#listable");
                var lastPage = int.Parse(
                    doc.DocumentNode.Descendants("ul")
                        .First(
                            x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "pagination ng-scope")
                        .Descendants("li")
                        .Last(x => !x.Attributes.Contains("class"))
                        .Descendants("a")
                        .First(x => x.Attributes.Contains("href"))
                        .InnerText
                    );

                for (var i = 1; i <= lastPage; i++)
                {
                    doc =
                        new HtmlWeb().Load(string.Format("http://proxylist.hidemyass.com/search-1292985/{0}#listable", i));
                    if (
                        !doc.DocumentNode.Descendants("tr")
                            .Any(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "altshade"))
                        continue;

                    var urls =
                        doc.DocumentNode.Descendants("table")
                            .First(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "hma-table")
                            .Descendants("tr").Skip(1);

                    foreach (var url in urls)
                    {
                        var port = Regex.Match(url.Descendants("td").ElementAt(2).InnerText, @"\d+").Value;
                        var ipSpan = url.Descendants("td").ElementAt(1).ChildNodes.First(x => x.Name == "span");
                        var style =
                            ipSpan.ChildNodes.First(x => x.Name == "style")
                                .InnerText.Split('\n')
                                .Where(x => !string.IsNullOrEmpty(x))
                                .ToList();
                        //var inline = style.Where(x => x.Contains("inline")).Select(x => Regex.Match(x, @"(?<=\.).+(?=\{)").Value);
                        var none =
                            style.Where(x => x.Contains("none")).Select(x => Regex.Match(x, @"(?<=\.).+(?=\{)").Value);

                        var ip = string.Empty;
                        ipSpan.ChildNodes.Where(
                            x =>
                                (x.HasAttributes &&
                                 (x.Attributes.Contains("class") || x.Attributes.Contains("style"))) ||
                                x.Name == "#text").ToList().ForEach(
                                    x =>
                                    {
                                        if (x.Name == "#text")
                                            ip += x.InnerText;
                                        if (x.Attributes.Contains("style") &&
                                            !x.Attributes["style"].Value.Contains("none"))
                                            ip += x.InnerText;
                                        else if (x.Attributes.Contains("class") &&
                                                 !none.Contains(x.Attributes["class"].Value))
                                            ip += x.InnerText;
                                    });
                        list.Add(ip + ":" + port);
                    }
                }

                return list.Distinct();
            }
            catch (Exception)
            {
                Log.Error(string.Format("{0} {1}", ErrorMsg, "www.HideMyAss.com"), null, null);
                return new List<string>();
            }
        }
    }
}