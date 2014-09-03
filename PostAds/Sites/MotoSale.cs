using System.Collections.Generic;
using Motorcycle.Captcha;
using Motorcycle.Config.Data;
using Motorcycle.HTTP;
using Motorcycle.POST;
using Motorcycle.XmlWorker;

namespace Motorcycle.Sites
{
    internal class MotoSale : IPostOnSite
    {
        public void PostMoto(DicHolder data)
        {
            var dataDictionary = data.DataDictionary;
            var fileDictionary = data.FileDictionary;

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

        public void PostSpare(DicHolder data)
        {
            var dataDictionary = data.DataDictionary;
            var fileDictionary = data.FileDictionary;
        }

        public void PostEquip(DicHolder data)
        {
            var dataDictionary = data.DataDictionary;
            var fileDictionary = data.FileDictionary;
        }
    }
}