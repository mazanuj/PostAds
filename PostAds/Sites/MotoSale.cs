namespace Motorcycle.Sites
{
    using System;
    using System.Threading.Tasks;
    using Captcha;
    using Config.Data;
    using HTTP;

    using NLog;

    using POST;
    using XmlWorker;
    using Interfaces;

    internal class MotoSale : IPostOnSite
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public async Task<SitePoster.PostStatus> PostMoto(DicHolder data)
        {
           return await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;

                    const string url = "http://www.motosale.com.ua/?add=moto";
                    var cookieContainer = Cookies.GetCookiesContainer(url);

                    var requestImage = Request.GETRequest("http://www.motosale.com.ua/capcha/capcha.php");
                    requestImage.CookieContainer = cookieContainer;

                    var captchaFileName = Response.GetImageFromResponse(requestImage);

                    //Get captcha result
                    var captcha = CaptchaString.GetCaptchaString(
                        CaptchaXmlWorker.GetCaptchaValues("key"),
                        captchaFileName,
                        CaptchaXmlWorker.GetCaptchaValues("domain"));

                    dataDictionary["fConfirmationCode"] = captcha;

                    var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary);
                    request.Referer = url;
                    var responseString = Response.GetResponseString(request);
                    request.Abort();

                    if (responseString.Contains("На указанный вами E-mail отправлено письмо"))
                    {
                        Log.Info("MotoSale OK");
                        return SitePoster.PostStatus.OK;
                    }
                    Log.Error("MotoSale ERROR");
                    return SitePoster.PostStatus.ERROR;

                    //TaskEx.Delay(8500);
                    //throw new Exception("Exception in MotoSale");
                    //Log.Info("MotoSale OK");
                    //return SitePoster.PostStatus.OK;
                });
        }

        public async Task<SitePoster.PostStatus> PostSpare(DicHolder data)
        {
            return await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;

                    const string url = "http://www.motosale.com.ua/?add=zap";
                    var cookieContainer = Cookies.GetCookiesContainer(url);

                    //Get captcha result
                    var requestImage = Request.GETRequest("http://www.motosale.com.ua/capcha/capcha.php");
                    requestImage.CookieContainer = cookieContainer;

                    var captchaFileName = Response.GetImageFromResponse(requestImage);
                    
                    var captcha = CaptchaString.GetCaptchaString(
                        CaptchaXmlWorker.GetCaptchaValues("key"),
                        captchaFileName,
                        CaptchaXmlWorker.GetCaptchaValues("domain"));

                    //Get hash result
                    var requestHash = Request.GETRequest(url);
                    requestHash.CookieContainer = cookieContainer;
                    var responseHash = Response.GetResponseString(requestHash);

                    var start = responseHash.IndexOf("name=\"insert\" value=\"") + "name=\"insert\" value=\"".Length;
                    var stop = responseHash.IndexOf("\"", start);
                    var hash = responseHash.Substring(start, stop - start);

                    dataDictionary["fConfirmationCode"] = captcha;
                    dataDictionary["insert"] = hash;

                    var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary);
                    request.Referer = url;
                    var responseString = Response.GetResponseString(request);
                    request.Abort();

                    return responseString.Contains("На указанный вами E-mail отправлено письмо") ? SitePoster.PostStatus.OK : SitePoster.PostStatus.ERROR;
                });
        }

        public async Task<SitePoster.PostStatus> PostEquip(DicHolder data)
        {
           return await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;

                    return SitePoster.PostStatus.OK;
                });
        }
    }
}