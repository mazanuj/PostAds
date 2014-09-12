using System.Net;
using NLog;

namespace Motorcycle.Sites
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Captcha;
    using Config.Data;
    using HTTP;
    using POST;
    using XmlWorker;
    using Interfaces;

    internal class Proday2Kolesa : IPostOnSite
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        public async Task<SitePoster.PostStatus> PostMoto(DicHolder data)
        {
            return await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;

                    const string url = "http://proday2kolesa.com.ua/index.php";
                    var referer =
                        string.Format(
                            "http://proday2kolesa.com.ua/component/option,{0}/v,{1}/Itemid,{2}/task,edit/category,{3}/",
                            dataDictionary["option"], dataDictionary["vendor"], dataDictionary["Itemid"],
                            dataDictionary["category"]);

                    var cookieContainer = Cookies.GetCookiesContainer(referer);

                    var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary);
                    request.Referer = referer;

                    var response = Response.GetResponse(request);
                    cookieContainer.Add(response.Cookies);
                    var responseString = Response.GetResponseString(response);

                    request.Abort();
                    response.Close();

                    #region Find

                    //Find captcha's link
                    var start = responseString.IndexOf("index2.php?option=com_autobb");
                    var end = responseString.IndexOf("\"", start);
                    var captchaUrl = "http://proday2kolesa.com.ua/" + responseString.Substring(start, end - start);

                    //Find id
                    start = responseString.IndexOf("name=\"id\" value=\"", start) + "name=\"id\" value=\"".Length;
                    end = responseString.IndexOf("\"", start);
                    var id = responseString.Substring(start, end - start);

                    //Find Itemid
                    start = responseString.IndexOf("name=\"Itemid\" value=\"", start) +
                            "name=\"Itemid\" value=\"".Length;
                    end = responseString.IndexOf("\"", start);
                    var Itemid = responseString.Substring(start, end - start);

                    //Find option
                    start = responseString.IndexOf("name=\"option\" value=\"", start) +
                            "name=\"option\" value=\"".Length;
                    end = responseString.IndexOf("\"", start);
                    var option = responseString.Substring(start, end - start);

                    //Find task
                    start = responseString.IndexOf("name=\"task\" value=\"", start) + "name=\"task\" value=\"".Length;
                    end = responseString.IndexOf("\"", start);
                    var task = responseString.Substring(start, end - start);

                    //Find simage_id
                    start = responseString.IndexOf("name=\"simage_id\" value=\"", start) +
                            "name=\"simage_id\" value=\"".Length;
                    end = responseString.IndexOf("\"", start);
                    var simage_id = responseString.Substring(start, end - start);

                    #endregion

                    //Get captcha result
                    var captchaFileName = CaptchaString.GetCaptchaImage(captchaUrl);
                    var captcha = CaptchaString.GetCaptchaString(CaptchaXmlWorker.GetCaptchaValues("key"), captchaFileName,
                        CaptchaXmlWorker.GetCaptchaValues("domain"));

                    //Send captcha request
                    var captchaDictionary = new Dictionary<string, string>
                    {
                        {"secure_code", captcha},
                        {"id", id},
                        {"Itemid", Itemid},
                        {"option", option},
                        {"task", task},
                        {"simage_id", simage_id}
                    };

                    request = Request.POSTRequest(url, cookieContainer, captchaDictionary, null);
                    request.Referer =
                        string.Format(
                            "http://proday2kolesa.com.ua/component/option,{0}/task,publish/id,{1}/error,0/Itemid,{2}/",
                            dataDictionary["option"], id, Itemid);

                    //responseString = Response.GetResponseString(request);
                    var test = Response.GetResponse(request);
                    var b = test.StatusCode;
                    request.Abort();

                    if (b == HttpStatusCode.OK)
                    {
                        //log.Info("Proday2kolesa OK");
                        return SitePoster.PostStatus.OK;
                    }
                    return SitePoster.PostStatus.ERROR;
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