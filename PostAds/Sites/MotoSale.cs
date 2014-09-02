using System.Collections.Generic;
using Motorcycle.Captcha;
using Motorcycle.HTTP;
using Motorcycle.POST;
using Motorcycle.XmlWorker;

namespace Motorcycle.Sites
{
    internal static class MotoSale
    {
        internal static void PostAdvert(Dictionary<string, string> dataDictionary,
            Dictionary<string, string> fileDictionary)
        {
            const string url = "http://www.motosale.com.ua/?add=moto";
            var cookieContainer = Cookies.GetCookiesContainer(url);

            var requestImage = Request.GETRequest("http://www.motosale.com.ua/capcha/capcha.php");
            requestImage.CookieContainer = cookieContainer;

            Response.GetImageFromResponse(requestImage);

            //Get captcha result
            var captcha = CaptchaString.GetCaptchaString(CaptchaXmlWorker.GetCaptchaValues("key"), "captcha.jpg",
                CaptchaXmlWorker.GetCaptchaValues("domain"));

            dataDictionary["fConfirmationCode"] = captcha;

            var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary);
            request.Referer = url;
            var responseString = Response.GetResponseString(request);

            request.Abort();
        }
    }
}