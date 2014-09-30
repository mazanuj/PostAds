using System;
using Motorcycle.XmlWorker;

namespace Motorcycle.Sites
{
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
        public async Task<SitePoster.PostStatus> PostMoto(DicHolder data)
        {
            return await Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        var Log = LogManager.GetCurrentClassLogger();
                        var dataDictionary = data.DataDictionary;
                        var fileDictionary = data.FileDictionary;
                        var reply = string.Format("{0} {1}",
                            ManufactureXmlWorker.GetItemSiteIdUsingPlant("u", dataDictionary["input[1]"]),
                            dataDictionary["input[153]"]);

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
                                        new Dictionary<string, string> {{"photos", ""}},
                                        new Dictionary<string, string> {{"file", fotoPath.Value}})))
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
                            Log.Info(reply + " successfully posted on UsedAuto");
                            if (RemoveEntries.Remove(data, ProductEnum.Motorcycle))
                                Log.Info(reply + " removed from list");
                            return SitePoster.PostStatus.OK;
                        }
                        Log.Warn(reply + " unsuccessfully posted on UsedAuto");
                        RemoveEntries.Unposted(data, ProductEnum.Motorcycle, SiteEnum.UsedAuto);
                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(string.Format("{0} {1} unsuccessfully posted on UsedAuto",
                                ManufactureXmlWorker.GetItemSiteIdUsingPlant("u", data.DataDictionary["input[1]"]),
                                data.DataDictionary["input[153]"]), ex);
                        RemoveEntries.Unposted(data, ProductEnum.Motorcycle, SiteEnum.UsedAuto);
                        return SitePoster.PostStatus.ERROR;
                    }
                });
        }

        public async Task<SitePoster.PostStatus> PostSpare(DicHolder data)
        {
            return await Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        var Log = LogManager.GetCurrentClassLogger();
                        var dataDictionary = data.DataDictionary;
                        var fileDictionary = data.FileDictionary;
                        var reply = string.Format("{0} {1} successfully posted on UsedAuto",
                            ManufactureXmlWorker.GetItemSiteIdUsingPlant("u", data.DataDictionary["make"]),
                            data.DataDictionary["model"]);

                        const string url = "http://usedauto.com.ua/add/zapchasti.php";

                        var cookieContainer = Cookies.GetCookiesContainer(url);
                        var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary, url);

                        if (Response.GetResponseString(request).Contains("success"))
                        {
                            Log.Info(reply + " successfully posted on UsedAuto");
                            if (RemoveEntries.Remove(data, ProductEnum.Spare))
                                Log.Info(reply + " removed from list");
                            return SitePoster.PostStatus.OK;
                        }
                        Log.Warn(reply + " unsuccessfully posted on UsedAuto");
                        request.Abort();
                        RemoveEntries.Unposted(data, ProductEnum.Spare, SiteEnum.UsedAuto);
                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(string.Format("{0} {1} unsuccessfully posted on UsedAuto",
                                ManufactureXmlWorker.GetItemSiteIdUsingPlant("u", data.DataDictionary["make"]),
                                data.DataDictionary["model"]), ex);
                        RemoveEntries.Unposted(data, ProductEnum.Spare, SiteEnum.UsedAuto);
                        return SitePoster.PostStatus.ERROR;
                    }
                });
        }

        public async Task<SitePoster.PostStatus> PostEquip(DicHolder data)
        {
            return await Task.Factory.StartNew(
                () => SitePoster.PostStatus.OK);
        }
    }
}