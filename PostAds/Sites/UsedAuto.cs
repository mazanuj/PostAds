namespace Motorcycle.Sites
{
    using Config.Data;
    using HTTP;
    using Interfaces;
    using XmlWorker;
    using NLog;
    using POST;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using xNet.Net;

    public class UsedAuto : ISitePoster
    {
        public PostStatus PostMoto(DicHolder data)
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

                using (var req = new HttpRequest())
                {
                    req.IgnoreProtocolErrors = true;
                    foreach (var value in dataDictionary)
                        req.AddField(value.Key, value.Value);
                    var resp = req.Post(url);

                    if (resp.StatusCode == HttpStatusCode.OK ||
                        resp.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        Log.Info(reply + " successfully posted", SiteEnum.UsedAuto, ProductEnum.Motorcycle);
                        RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle);

                        return PostStatus.OK;
                    }
                    Log.Warn(string.Format("{0} unsuccessfully posted ({1})", reply, resp.StatusCode), SiteEnum.UsedAuto,
                        ProductEnum.Motorcycle);
                    RemoveEntries.Remove(data, ProductEnum.Motorcycle, SiteEnum.UsedAuto);

                    return PostStatus.ERROR;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(
                        string.Format("{0} {1} unsuccessfully posted {2}",
                            ManufactureXmlWorker.GetItemSiteIdUsingPlant("u", data.DataDictionary["input[1]"]),
                            data.DataDictionary["input[153]"], ex.Message), SiteEnum.UsedAuto, ProductEnum.Motorcycle);
                RemoveEntries.Remove(data, ProductEnum.Motorcycle, SiteEnum.UsedAuto);

                return PostStatus.ERROR;
            }
        }

        public PostStatus PostSpare(DicHolder data)
        {
            try
            {
                var Log = LogManager.GetCurrentClassLogger();
                var dataDictionary = data.DataDictionary;
                var fileDictionary = data.FileDictionary;
                var reply = string.Format("{0} {1}",
                    ManufactureXmlWorker.GetItemSiteIdUsingPlant("u", data.DataDictionary["make"]),
                    data.DataDictionary["model"]);

                const string url = "http://usedauto.com.ua/add/zapchasti.php";

                var cookieContainer = Cookies.GetCookiesContainer(url);
                var respString = Response.GetResponseString(cookieContainer, dataDictionary, fileDictionary, url,
                    Encoding.UTF8);

                if (respString.Contains("success"))
                {
                    Log.Info(reply + " successfully posted", SiteEnum.UsedAuto, ProductEnum.Spare);
                    RemoveEntries.Remove(data.LineNum, ProductEnum.Spare);

                    return PostStatus.OK;
                }
                Log.Warn(reply + " unsuccessfully posted", SiteEnum.UsedAuto, ProductEnum.Spare);
                RemoveEntries.Remove(data, ProductEnum.Spare, SiteEnum.UsedAuto);

                return PostStatus.ERROR;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(
                        string.Format("{0} {1} unsuccessfully posted {2}",
                            ManufactureXmlWorker.GetItemSiteIdUsingPlant("u", data.DataDictionary["make"]),
                            data.DataDictionary["model"], ex.Message), SiteEnum.UsedAuto, ProductEnum.Spare);
                RemoveEntries.Remove(data, ProductEnum.Spare, SiteEnum.UsedAuto);

                return PostStatus.ERROR;
            }
        }

        public PostStatus PostEquip(DicHolder data)
        {
            //this is only stub method
            return PostStatus.OK;
        }
    }
}