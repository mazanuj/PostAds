using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Motorcycle.POST;

namespace Motorcycle.HTTP
{
    static class Request
    {
        internal static HttpWebRequest POSTRequest(string uri, CookieContainer cookieContainer,
            Dictionary<string, string> dataDictionary, Dictionary<string, string> fileDictionary)
        {
            var request = (HttpWebRequest) WebRequest.Create(uri);
            var boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.CookieContainer = cookieContainer;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.113 Safari/534.30";

            var byteArray =
                Encoding.Default.GetBytes(PostMultiString.WriteMultipartForm(boundary, dataDictionary, fileDictionary));
            request.ContentLength = byteArray.Length;
            request.GetRequestStream().Write(byteArray, 0, byteArray.Length);

            return request;
        }

        internal static HttpWebRequest POSTRequest(string uri, CookieContainer cookieContainer,
            Dictionary<string, string> dataDictionary, Dictionary<string, string> fileDictionary,string referer)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            var boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.CookieContainer = cookieContainer;
            request.Referer = referer;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.113 Safari/534.30";

            var byteArray =
                Encoding.Default.GetBytes(PostMultiString.WriteMultipartForm(boundary, dataDictionary, fileDictionary));
            request.ContentLength = byteArray.Length;
            request.GetRequestStream().Write(byteArray, 0, byteArray.Length);

            return request;
        }

        internal static HttpWebRequest GETRequest(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);

            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.113 Safari/534.30";
            request.Accept = "*/*";
            request.Headers.Add("Accept-Language", "en");
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.Method = "GET";
            request.AllowAutoRedirect = true;

            return request;
        }
    }
}