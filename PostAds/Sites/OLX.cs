using System;
using System.Linq;
using HtmlAgilityPack;
using Motorcycle.Config;
using Motorcycle.Config.Data;
using Motorcycle.Interfaces;
using Newtonsoft.Json.Linq;
using NLog;
using xNet.Net;

namespace Motorcycle.Sites
{
    public class Olx : ISitePoster
    {
        public PostStatus PostMoto(DicHolder data)
        {
            try
            {
                var Log = LogManager.GetCurrentClassLogger();
                var dataDictionary = data.DataDictionary;
                var fileDictionary = data.FileDictionary;
                var reply = dataDictionary["data[title]"];

                const string url = "http://olx.ua/post-new-ad/";
                var urlFile = "http://olx.ua/ajax/upload/upload/?riak_key=&ad_id=&preview=&category=0";

                var cookies = OlxAuthorize.GetPhpSesID().Result; //TODO slandomoto@mail.ru
                using (var req = new HttpRequest())
                {
                    req.Cookies = cookies;

                    var doc = new HtmlDocument();
                    doc.LoadHtml(req.Get(url).ToString());

                    dataDictionary["data[adding_key]"] =
                        doc.DocumentNode.Descendants("input")
                            .First(
                                x => x.Attributes.Contains("name") &&
                                     x.Attributes["name"].Value == "data[adding_key]")
                            .Attributes["value"].Value;
                }

                //Upload fotos
                var riak_key = string.Empty;
                var slot = 1;

                foreach (var json in fileDictionary
                    .Where(fotoPath => fotoPath.Value != string.Empty)
                    .Select(fotoPath =>
                    {
                        string resp;
                        using (var req = new HttpRequest())
                        {
                            req.Cookies = cookies;
                            req.AddFile("file", fotoPath.Value);
                            resp = req.Post(urlFile).ToString();
                        }
                        return resp;
                    }).Select(JObject.Parse))
                {
                    riak_key = json.SelectToken("riak_key").ToString();
                    slot = int.Parse(json.SelectToken("slot").ToString());

                    if (urlFile.Contains("riak_key=&"))
                        urlFile = urlFile.Insert(urlFile.IndexOf("riak_key=") + "riak_key=".Length, riak_key);
                }
                dataDictionary["data[riak_key]"] = riak_key;
                //==============End upload fotos==============//

                using (var req = new HttpRequest())
                {
                    req.IgnoreProtocolErrors = true;
                    req.Cookies = cookies;

                    foreach (var value in dataDictionary)
                        req.AddField(value.Key, value.Value);

                    for (var i = 1; i <= slot; i++)
                        req.AddField("image[]", i);

                    var resp = req.Post(url).ToString();

                    if (resp.Contains("Когда ваше объявление"))
                    {
                        Log.Info(reply + " successfully posted", SiteEnum.Olx, ProductEnum.Motorcycle);
                        RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle);

                        return PostStatus.OK;
                    }
                    Log.Warn($"{reply} unsuccessfully posted", SiteEnum.Olx,
                        ProductEnum.Motorcycle);
                    RemoveEntries.Remove(data, ProductEnum.Motorcycle, SiteEnum.Olx);

                    return PostStatus.ERROR;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(
                        $"{data.DataDictionary["data[title]"]} unsuccessfully posted {ex.Message}", SiteEnum.Olx, ProductEnum.Motorcycle);
                RemoveEntries.Remove(data, ProductEnum.Motorcycle, SiteEnum.Olx);

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
                var reply = dataDictionary["data[title]"];

                const string url = "http://olx.ua/post-new-ad/";
                var urlFile = "http://olx.ua/ajax/upload/upload/?riak_key=&ad_id=&preview=&category=0";

                var cookies = OlxAuthorize.GetPhpSesID().Result; //TODO slandospare@mail.ru
                using (var req = new HttpRequest())
                {
                    req.Cookies = cookies;

                    var doc = new HtmlDocument();
                    doc.LoadHtml(req.Get(url).ToString());

                    dataDictionary["data[adding_key]"] =
                        doc.DocumentNode.Descendants("input")
                            .First(
                                x => x.Attributes.Contains("name") &&
                                     x.Attributes["name"].Value == "data[adding_key]")
                            .Attributes["value"].Value;
                }

                //Upload fotos
                var riak_key = string.Empty;
                var slot = 1;

                foreach (var json in fileDictionary
                    .Where(fotoPath => fotoPath.Value != string.Empty)
                    .Select(fotoPath =>
                    {
                        string resp;
                        using (var req = new HttpRequest())
                        {
                            req.Cookies = cookies;
                            req.AddFile("file", fotoPath.Value);
                            resp = req.Post(urlFile).ToString();
                        }
                        return resp;
                    }).Select(JObject.Parse))
                {
                    riak_key = json.SelectToken("riak_key").ToString();
                    slot = int.Parse(json.SelectToken("slot").ToString());

                    if (urlFile.Contains("riak_key=&"))
                        urlFile = urlFile.Insert(urlFile.IndexOf("riak_key=") + "riak_key=".Length, riak_key);
                }
                dataDictionary["data[riak_key]"] = riak_key;
                //==============End upload fotos==============//

                using (var req = new HttpRequest())
                {
                    req.IgnoreProtocolErrors = true;
                    req.Cookies = cookies;

                    foreach (var value in dataDictionary)
                        req.AddField(value.Key, value.Value);

                    for (var i = 1; i <= slot; i++)
                        req.AddField("image[]", i);

                    var resp = req.Post(url).ToString();

                    if (resp.Contains("Когда ваше объявление"))
                    {
                        Log.Info(reply + " successfully posted", SiteEnum.Olx, ProductEnum.Spare);
                        RemoveEntries.Remove(data.LineNum, ProductEnum.Spare);

                        return PostStatus.OK;
                    }
                    Log.Warn($"{reply} unsuccessfully posted", SiteEnum.Olx,
                        ProductEnum.Spare);
                    RemoveEntries.Remove(data, ProductEnum.Spare, SiteEnum.Olx);

                    return PostStatus.ERROR;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(
                        $"{data.DataDictionary["data[title]"]} unsuccessfully posted {ex.Message}", SiteEnum.Olx, ProductEnum.Spare);
                RemoveEntries.Remove(data, ProductEnum.Spare, SiteEnum.Olx);

                return PostStatus.ERROR;
            }
        }

        public PostStatus PostEquip(DicHolder data)
        {
            try
            {
                var Log = LogManager.GetCurrentClassLogger();
                var dataDictionary = data.DataDictionary;
                var fileDictionary = data.FileDictionary;
                var reply = dataDictionary["data[title]"];

                const string url = "http://olx.ua/post-new-ad/";
                var urlFile = "http://olx.ua/ajax/upload/upload/?riak_key=&ad_id=&preview=&category=0";

                var cookies = OlxAuthorize.GetPhpSesID().Result;
                using (var req = new HttpRequest())
                {
                    req.Cookies = cookies;

                    var doc = new HtmlDocument();
                    doc.LoadHtml(req.Get(url).ToString());

                    dataDictionary["data[adding_key]"] =
                        doc.DocumentNode.Descendants("input")
                            .First(
                                x => x.Attributes.Contains("name") &&
                                     x.Attributes["name"].Value == "data[adding_key]")
                            .Attributes["value"].Value;
                }

                //Upload fotos
                var riak_key = string.Empty;
                var slot = 1;

                foreach (var json in fileDictionary
                    .Where(fotoPath => fotoPath.Value != string.Empty)
                    .Select(fotoPath =>
                    {
                        string resp;
                        using (var req = new HttpRequest())
                        {
                            req.Cookies = cookies;
                            req.AddFile("file", fotoPath.Value);
                            resp = req.Post(urlFile).ToString();
                        }
                        return resp;
                    }).Select(JObject.Parse))
                {
                    riak_key = json.SelectToken("riak_key").ToString();
                    slot = int.Parse(json.SelectToken("slot").ToString());

                    if (urlFile.Contains("riak_key=&"))
                        urlFile = urlFile.Insert(urlFile.IndexOf("riak_key=") + "riak_key=".Length, riak_key);
                }
                dataDictionary["data[riak_key]"] = riak_key;
                //==============End upload fotos==============//

                using (var req = new HttpRequest())
                {
                    req.IgnoreProtocolErrors = true;
                    req.Cookies = cookies;

                    foreach (var value in dataDictionary)
                        req.AddField(value.Key, value.Value);

                    for (var i = 1; i <= slot; i++)
                        req.AddField("image[]", i);

                    var resp = req.Post(url).ToString();

                    if (resp.Contains("Когда ваше объявление"))
                    {
                        Log.Info(reply + " successfully posted", SiteEnum.Olx, ProductEnum.Equip);
                        RemoveEntries.Remove(data.LineNum, ProductEnum.Equip);

                        return PostStatus.OK;
                    }
                    Log.Warn($"{reply} unsuccessfully posted", SiteEnum.Olx,
                        ProductEnum.Equip);
                    RemoveEntries.Remove(data, ProductEnum.Equip, SiteEnum.Olx);

                    return PostStatus.ERROR;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(
                        $"{data.DataDictionary["data[title]"]} unsuccessfully posted {ex.Message}", SiteEnum.Olx, ProductEnum.Equip);
                RemoveEntries.Remove(data, ProductEnum.Equip, SiteEnum.Olx);

                return PostStatus.ERROR;
            }
        }
    }
}