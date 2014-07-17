using System.Collections.Generic;
using Motorcycle.Captcha;
using Motorcycle.Config;
using Motorcycle.HTTP;
using Motorcycle.POST;

namespace Motorcycle.Sites
{
    static class Proday2Kolesa
    {
        internal static void PostAdvert(Dictionary<string,string> data2kolesa, Dictionary<string,string> fileDictionary)
        {            
            const string url = "http://proday2kolesa.com.ua/index.php";
            var referer = string.Format("http://proday2kolesa.com.ua/component/option,{0}/v,{1}/Itemid,{2}/task,edit/category,{3}/",
                    data2kolesa["option"], data2kolesa["vendor"], data2kolesa["Itemid"], data2kolesa["category"]);

            var cookieContainer = Cookies.GetCookiesContainer(referer);

            var request = Request.POSTRequest(url, cookieContainer, data2kolesa, fileDictionary);
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
            start = responseString.IndexOf("name=\"Itemid\" value=\"", start) + "name=\"Itemid\" value=\"".Length;
            end = responseString.IndexOf("\"", start);
            var Itemid = responseString.Substring(start, end - start);

            //Find option
            start = responseString.IndexOf("name=\"option\" value=\"", start) + "name=\"option\" value=\"".Length;
            end = responseString.IndexOf("\"", start);
            var option = responseString.Substring(start, end - start);

            //Find task
            start = responseString.IndexOf("name=\"task\" value=\"", start) + "name=\"task\" value=\"".Length;
            end = responseString.IndexOf("\"", start);
            var task = responseString.Substring(start, end - start);

            //Find simage_id
            start = responseString.IndexOf("name=\"simage_id\" value=\"", start) + "name=\"simage_id\" value=\"".Length;
            end = responseString.IndexOf("\"", start);
            var simage_id = responseString.Substring(start, end - start);

            #endregion

            //Get captcha result
            CaptchaString.GetCaptchaImage(captchaUrl);
            var captcha = CaptchaString.GetCaptchaString(GetSettings.GetCaptcha("key"), "captcha.jpg", GetSettings.GetCaptcha("domain"));

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
            request.Referer = string.Format("http://proday2kolesa.com.ua/component/option,{0}/task,publish/id,{1}/error,0/Itemid,{2}/",
                    data2kolesa["option"], id, Itemid);
// ReSharper disable once RedundantAssignment
            responseString = Response.GetResponseString(request);
            request.Abort();
        }        
    }
}