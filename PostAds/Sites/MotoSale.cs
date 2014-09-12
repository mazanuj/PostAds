namespace Motorcycle.Sites
{
    using System.Threading.Tasks;
    using Captcha;
    using Config.Data;
    using HTTP;
    using POST;
    using XmlWorker;
    using Interfaces;

    internal class MotoSale : IPostOnSite
    {
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

                    return responseString.Contains("На указанный вами E-mail отправлено письмо") ? SitePoster.PostStatus.OK : SitePoster.PostStatus.ERROR;
                });
        }

        public async Task<SitePoster.PostStatus> PostSpare(DicHolder data)
        {
            return await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;

                    return SitePoster.PostStatus.OK;
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