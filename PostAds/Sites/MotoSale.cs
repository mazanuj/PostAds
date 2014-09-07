

namespace Motorcycle.Sites
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Motorcycle.Captcha;
    using Motorcycle.Config.Data;
    using Motorcycle.HTTP;
    using Motorcycle.POST;
    using Motorcycle.XmlWorker;
    using Motorcycle.Interfaces;

    internal class MotoSale : IPostOnSite
    {
        public async Task PostMoto(DicHolder data)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;

                    const string url = "http://www.motosale.com.ua/?add=moto";
                    var cookieContainer = Cookies.GetCookiesContainer(url);

                    var requestImage = Request.GETRequest("http://www.motosale.com.ua/capcha/capcha.php");
                    requestImage.CookieContainer = cookieContainer;

                    Response.GetImageFromResponse(requestImage);

                    //Get captcha result
                    var captcha = CaptchaString.GetCaptchaString(
                        CaptchaXmlWorker.GetCaptchaValues("key"),
                        "captcha.jpg",
                        CaptchaXmlWorker.GetCaptchaValues("domain"));

                    dataDictionary["fConfirmationCode"] = captcha;

                    var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary);
                    request.Referer = url;
                    var responseString = Response.GetResponseString(request);

                    request.Abort();
                });
        }

        public async Task PostSpare(DicHolder data)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;
                });
        }

        public async Task PostEquip(DicHolder data)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;
                });
        }
    }
}