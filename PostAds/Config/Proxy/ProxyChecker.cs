using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using xNet.Net;

namespace Motorcycle.Config.Proxy
{
    internal static class ProxyChecker
    {
        internal static List<ProxyAddressStruct> ProxyAddresses(IEnumerable<string> _proxyAddresses)
        {
            //Get ip from domain name
            var proxyAddresses = new List<string>();
            foreach (var address in _proxyAddresses)
            {
                if (!Regex.IsMatch(address, @"[a-zA-Z]"))
                {
                    proxyAddresses.Add(address);
                    continue;
                }

                var ipPort = address.Split(':');
                var ip = string.Empty;
                try
                {
                    ip = Dns.GetHostAddresses(ipPort[0]).GetValue(0).ToString();
                }
                catch
                {
                }
                proxyAddresses.Add(ip + ":" + ipPort[1]);
            }
            proxyAddresses = proxyAddresses.Where(t => !string.IsNullOrEmpty(t)).Distinct().ToList();
            //==============================================//

            const string url =
                "http://hideme.ru/api/checker.php?out=js&action=list_new&tasks=http,ssl,socks4,socks5&parser=lines";
            var proxyAddressesList = new List<ProxyAddressStruct>();

            var i = 0;
            while (proxyAddresses.Count > i)
            {
                var dicIp = new Dictionary<string, string>();
                var requestString = string.Empty;
                var count = 0;                

                for (; count < 100 && proxyAddresses.Count > i; count++)
                {
                    if (requestString != string.Empty) requestString += "\n";
                    requestString += proxyAddresses[i];

                    dicIp.Add(proxyAddresses[i++], "");
                }

                using (var requestXNET = new HttpRequest(url))
                {
                    requestXNET.AddParam("data", requestString);
                    dynamic responseJsone = JsonConvert.DeserializeObject(requestXNET.Post(url).ToString());

                    foreach (var item in responseJsone.items)
                        dicIp[item.Value.host.Value + ":" + item.Value.port.Value] = item.Name;

                    var group = responseJsone.group;
                    var checkUrl =
                        "http://hideme.ru/api/checker.php?out=js&action=get&fields=resolved_ip,result_http,result_ssl,result_socks4,result_socks5&groups=" +
                        group;

                    dynamic responseJs;
                    while (true)
                    {
                        var respJs = JsonConvert.DeserializeObject(new HttpRequest().Get(checkUrl).ToString());
                        if (respJs.finished.Value != count - 1)
                        {
                            Thread.Sleep(2000);
                            continue;
                        }
                        responseJs = respJs;
                        break;
                    }

                    foreach (var item in responseJs.items)
                    {
                        var addr = new ProxyAddressStruct();

                        if (item.Value.result_http.status.Value == 1)
                            addr.Type = ProxyType.Http;
                        else if (item.Value.result_socks4.status.Value == 1)
                            addr.Type = ProxyType.Socks4;
                        else if (item.Value.result_socks5.status.Value == 1)
                            addr.Type = ProxyType.Socks5;
                        else if (item.Value.result_ssl.status.Value == 1)
                            addr.Type = ProxyType.Http;
                        else continue;

                        addr.ProxyAddresses =
                            proxyAddresses[
                                proxyAddresses.FindIndex(
                                    y => y.Contains(dicIp.FirstOrDefault(x => x.Value == item.Name).Key))];
                        proxyAddressesList.Add(addr);
                    }
                }
            }
            return proxyAddressesList;
        }
    }
}