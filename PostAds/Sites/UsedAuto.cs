using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Motorcycle.Config.Data;
using Motorcycle.HTTP;
using Motorcycle.POST;

namespace Motorcycle.Sites
{
    internal class UsedAuto : IPostOnSite
    {
        public void PostMoto(DicHolder data)
        {
            var dataDictionary = data.DataDictionary;
            var fileDictionary = data.FileDictionary;

            const string url = "http://usedauto.com.ua/add/add.php";
            const string urlFile = "http://usedauto.com.ua/add/modules/picturesUpload.php";
            const string referer = "http://usedauto.com.ua/add/sale.php";

            var cookieContainer = Cookies.GetCookiesContainer(referer);

            var requestFile = Request.POSTRequest(urlFile, cookieContainer, new Dictionary<string, string> {{"photos", ""}}, fileDictionary);
            requestFile.Referer = referer;
            var responseFileString = Response.GetResponseString(requestFile);
            requestFile.Abort();

            //Get file id
            var start = responseFileString.IndexOf("value=\"") + "value=\"".Length;
            var end = responseFileString.IndexOf("\"", start);
            var photoId = responseFileString.Substring(start, end - start);
            dataDictionary["photos"] = photoId;
            dataDictionary["main_photo"] = photoId;

            //Dictionary to NameValueCollection
            var valueCollection = new NameValueCollection();
            foreach (var value in dataDictionary)
                valueCollection.Add(value.Key, value.Value);

            //Post advert's data            
            var responseByte = new WebClient().UploadValues(url,"POST", valueCollection);
            var responseString = Encoding.Default.GetString(responseByte);
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