using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using xNet.Net;

namespace Motorcycle.Config.Proxy
{
    internal static class ProxyChecker
    {
        public static List<ProxyAddressStruct> ProxyAddresses(List<string> proxyAddresses)
        {
            const string url =
                "http://hideme.ru/api/checker.php?out=js&action=list_new&tasks=http,ssl,socks4,socks5&parser=lines";
            var proxyAddressesList = new List<ProxyAddressStruct>();

            var i = 0;
            while (proxyAddresses.Count > i)
            {
                var requestString = string.Empty;
                var count = 0;
                var dicIp = new Dictionary<string, string>();

                for (; count < 100 && proxyAddresses.Count > i; count++)
                {
                    if (requestString != string.Empty) requestString += "\n";
                    requestString += proxyAddresses[i];

                    dicIp.Add(proxyAddresses[i++].Remove(proxyAddresses[0].IndexOf(":")), "");
                }

                using (var requestXNET = new HttpRequest(url))
                {
                    requestXNET.AddParam("data", requestString);
                    dynamic responseJsone = JsonConvert.DeserializeObject(requestXNET.Post(url).ToString());

                    var items = responseJsone.items;
                    foreach (var item in items)
                        dicIp[item.First.host.Value] = item.Name;

                    var group = responseJsone.group;
                    var checkUrl =
                        "http://hideme.ru/api/checker.php?out=js&action=get&filters=progress!:queued;changed:1&fields=resolved_ip,progress,progress_http,progress_ssl,progress_socks4,progress_socks5,time_http,time_ssl,time_socks4,time_socks5,result_http,result_ssl,result_socks4,result_socks5&groups=" +
                        group;

                    dynamic responseJs;
                    while (true)
                    {
                        var respJs = JsonConvert.DeserializeObject(new HttpRequest().Get(checkUrl).ToString());
                        if (respJs.finished.Value != count)
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