namespace Motorcycle.Sites
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Config.Data;
    using HTTP;

    using NLog;

    using POST;
    using Interfaces;

    internal class UsedAuto : IPostOnSite
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public async Task<SitePoster.PostStatus> PostMoto(DicHolder data)
        {
            return await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;

                    const string url = "http://usedauto.com.ua/add/add.php";
                    const string urlFile = "http://usedauto.com.ua/add/modules/picturesUpload.php";
                    const string referer = "http://usedauto.com.ua/add/sale.php";

                    var cookieContainer = Cookies.GetCookiesContainer(referer);

                    //Upload fotos
                    var photoId = string.Empty;

                    foreach (
                        var requestFile in
                            fileDictionary
                                .Where(fotoPath => fotoPath.Value != string.Empty)
                                .Select(fotoPath => Request.POSTRequest(urlFile, cookieContainer,
                                    new Dictionary<string, string> { { "photos", "" } },
                                    new Dictionary<string, string> { { "file", fotoPath.Value } })))
                    {
                        requestFile.Referer = referer;
                        var responseFileString = Response.GetResponseString(requestFile);
                        requestFile.Abort();

                        //Get file id
                        var start = responseFileString.IndexOf("value=\"") + "value=\"".Length;
                        var end = responseFileString.IndexOf("\"", start);
                        if (photoId == string.Empty)
                        {
                            photoId = responseFileString.Substring(start, end - start);
                            dataDictionary["main_photo"] = photoId;
                        }
                        else photoId += "," + responseFileString.Substring(start, end - start);
                    }
                    dataDictionary["photos"] = photoId;
                    //==============End upload fotos==============//

                    //Dictionary to NameValueCollection
                    var valueCollection = new NameValueCollection();
                    foreach (var value in dataDictionary)
                        valueCollection.Add(value.Key, value.Value);

                    //Post advert's data            
                    var responseByte = new WebClient().UploadValues(url, "POST", valueCollection);
                    var responseString = Encoding.Default.GetString(responseByte);

                    if (responseString.Contains("redirect"))
                    {
                        Log.Info("UsedAuto OK");
                        return SitePoster.PostStatus.OK;
                    }
                    Log.Error("UsedAuto ERROR");
                    return SitePoster.PostStatus.ERROR;

                    //TaskEx.Delay(1000);
                    //throw new Exception("My Exception!");

                    //Log.Info("UsedAuto ERROR");
                    //return SitePoster.PostStatus.ERROR;
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