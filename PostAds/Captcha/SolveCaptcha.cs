using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Motorcycle.HTTP;

namespace Motorcycle.Captcha
{
    internal static class SolveCaptcha
    {
        internal static string GetText(Dictionary<string, string> dataDictionary, Dictionary<string, string> fileDictionary, string domain, int delay)
        {
            var request = Request.POSTRequest("http://" + domain + "/in.php", new CookieContainer(), dataDictionary, fileDictionary);
            var response = Response.GetResponseString(request);
            request.Abort();

            if (response.StartsWith("ERROR"))
                return response;

            var pars = response.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            var captchaId = pars[1];

            for (var i = 0; i < 30; i++)
            {
                Thread.Sleep(delay);
                var request2 = Request.GETRequest(string.Format("http://{0}/res.php?key={1}&action=get&id={2}", domain, dataDictionary["key"], captchaId));
                var response2 = Response.GetResponseString(request2);

                if (response2 == "CAPCHA_NOT_READY") continue;
                var pars2 = response2.Split('|');

                if (pars2[0] == "OK")
                    return pars2[1];
            }
            return captchaId;
        }

// ReSharper disable once UnusedMember.Global
        internal static string FalseCaptcha(string kapchaKey, string captchaId, string domain)
        {
            var request = Request.GETRequest("http://" + domain + "/res.php?key=" + kapchaKey + "&action=reportbad&id=" + captchaId);
            return Response.GetResponseString(request);
        }
    }
}