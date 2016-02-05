using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Motorcycle.Utils;
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
                        "http://hideme.ru/api/checker.php?out=js&action=get&filters=progress!:queued;changed:1&fields=resolved_ip,progress,progress_http,progress_ssl,progress_socks4,progress_socks5,time_http,time_ssl,time_socks4,time_socks5,result_http,result_ssl,result_socks4,result_socks5&groups=" +
                        group;

                    dynamic responseJs;
                    while (true)
                    {
                        var respJs = JsonConvert.DeserializeObject(new HttpRequest().Get(checkUrl).ToString());
                        if (respJs.working.Value != 0 || respJs.queued.Value != 0)
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

                        try
                        {
                            var predicate =
                                proxyAddresses.FindIndex(
                                    y => y.Contains(dicIp.FirstOrDefault(x => x.Value == item.Name).Key));
                            addr.ProxyAddresses = proxyAddresses[predicate];
                        }
                        catch (Exception)
                        {
                        }

                        proxyAddressesList.Add(addr);
                    }
                }
            }
            return proxyAddressesList;
        }

        internal static async Task<List<ProxyAddressStruct>> CheckProxy(IEnumerable<ProxyAddressStruct> proxyStruct)
        {
            return await Task.Run(async () =>
            {
                var list = new List<ProxyAddressStruct>();

                try
                {
                    await proxyStruct.ForEachAsync(200, async proxy => //todo more than 1
                    {
                        await Task.Run(() =>
                        {
                            using (var req = new HttpRequest {ConnectTimeout = 10000, ReadWriteTimeout = 10000})
                            {
                                try
                                {
                                    var arr = proxy.ProxyAddresses.Split(':');
                                    if (proxy.Type == ProxyType.Http)
                                        req.Proxy = new HttpProxyClient(arr[0], int.Parse(arr[1]));
                                    else req.Proxy = new Socks5ProxyClient(arr[0], int.Parse(arr[1]));

                                    req.Get("http://www.motosale.com.ua").None();
                                    list.Add(proxy);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        });
                    });
                }
                catch (Exception)
                {
                }
                return list;
            });
        }
    }
}