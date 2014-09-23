using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Media.Imaging;
using Motorcycle.Captcha;
using Motorcycle.Config.Proxy;
using xNet.Net;

namespace Motorcycle.HTTP
{
    internal static class Response
    {
        internal static string GetResponseString(HttpWebRequest request)
        {
            using (var response = (HttpWebResponse) request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return null;
                using (var reader = new StreamReader(responseStream, Encoding.GetEncoding("Windows-1251")))
                    return reader.ReadToEnd();
            }
        }

        internal static string GetResponseString(HttpWebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return null;
                using (var reader = new StreamReader(responseStream, Encoding.GetEncoding("Windows-1251")))
                    return reader.ReadToEnd();
            }
        }

        internal static string GetResponseString(CookieContainer cookieContainer,
            Dictionary<string, string> dataDictionary, Dictionary<string, string> fileDictionary, string url,
            ProxyAddressStruct proxyAddress)
        {
            using (var requestXNET = new HttpRequest(url))
            {
                var cookieDic = new CookieDictionary();
                var cookieColl = cookieContainer.GetCookies(new Uri("http://www.motosale.com.ua"));
                var cookieArray = new Cookie[cookieColl.Count];
                cookieColl.CopyTo(cookieArray, 0);
                foreach (var cookie in cookieArray)
                {
                    cookieDic.Add(cookie.Name, cookie.Value);
                }

                requestXNET.ConnectTimeout = requestXNET.ReadWriteTimeout = 15000;
                requestXNET.UserAgent = HttpHelper.ChromeUserAgent();
                requestXNET.Cookies = cookieDic;

                if (proxyAddress.ProxyAddresses != "localhost")
                {
                    switch (proxyAddress.Type)
                    {
                        case ProxyType.Http:
                            requestXNET.Proxy = HttpProxyClient.Parse(proxyAddress.ProxyAddresses);
                            break;
                        case ProxyType.Socks4:
                            requestXNET.Proxy = Socks4ProxyClient.Parse(proxyAddress.ProxyAddresses);
                            break;
                        case ProxyType.Socks5:
                            requestXNET.Proxy = Socks5ProxyClient.Parse(proxyAddress.ProxyAddresses);
                            break;
                    }
                }

                foreach (var value in dataDictionary)
                    requestXNET.AddField(value.Key, value.Value);
                foreach (var value in fileDictionary.Where(value => value.Value != string.Empty))
                    requestXNET.AddFile(value.Key, value.Value);

                return requestXNET.Post(url).ToString();
            }
        }

        internal static CookieCollection GetResponseCookies(HttpWebRequest request)
        {
            using (var response = (HttpWebResponse) request.GetResponse())
                return response.Cookies;
        }

        internal static HttpWebResponse GetResponse(HttpWebRequest request)
        {
            return (HttpWebResponse) request.GetResponse();
        }

        internal static string GetImageFromResponse(HttpWebRequest request)
        {
            var fileName = CaptchaFileNameGenerator.GetFileName();

            using (var response = (HttpWebResponse) request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);

                if (stream == null)
                    return "";

                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);

                var jpegEncoder = new JpegBitmapEncoder();
                jpegEncoder.Frames.Add(BitmapFrame.Create(memoryStream, BitmapCreateOptions.None,
                    BitmapCacheOption.OnLoad));
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                    jpegEncoder.Save(fs);

                return fileName;
            }
        }
    }
}