using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Motorcycle.Config;
using MotorcycleWPF.Captcha;
using MotorcycleWPF.HTTP;
using MotorcycleWPF.POST;

namespace MotorcycleWPF.Sites
{
    static class MotoSale
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
            var captcha = CaptchaString.GetCaptchaString(GetSettings.GetCaptcha("key"), "captcha.jpg", GetSettings.GetCaptcha("domain"));

            dataDictionary["fConfirmationCode"] = captcha;

            var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary);
            request.Referer = url;

// ReSharper disable once UnusedVariable
            var responseString = Response.GetResponseString(request);

            request.Abort();                        
        }
    }
}