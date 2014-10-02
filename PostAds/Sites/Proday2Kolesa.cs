namespace Motorcycle.Sites
{
    using Captcha;
    using Config.Data;
    using HTTP;
    using Interfaces;
    using NLog;
    using POST;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using XmlWorker;

    internal class Proday2Kolesa : IPostOnSite
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
                            SpareEquipXmlWorker.GetItemSiteIdUsingPlant("p", dataDictionary["model"]),
                            dataDictionary["modification"]);

                        const string url = "http://proday2kolesa.com.ua/index.php";
                        var referer =
                            string.Format(
                                "http://proday2kolesa.com.ua/component/option,{0}/v,{1}/Itemid,{2}/task,edit/category,{3}/",
                                dataDictionary["option"], dataDictionary["vendor"], dataDictionary["Itemid"],
                                dataDictionary["category"]);

                        var cookieContainer = Cookies.GetCookiesContainer(referer);

                        var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary, referer);

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
                        start = responseString.IndexOf("name=\"task\" value=\"", start) +
                                "name=\"task\" value=\"".Length;
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
                        var captcha = CaptchaString.GetCaptchaString(CaptchaXmlWorker.GetCaptchaValues("key"),
                            captchaFileName,
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

                        referer = string.Format(
                            "http://proday2kolesa.com.ua/component/option,{0}/task,publish/id,{1}/error,0/Itemid,{2}/",
                            dataDictionary["option"], id, Itemid);
                        request = Request.POSTRequest(url, cookieContainer, captchaDictionary, null, referer);

                        if (Response.GetResponse(request).StatusCode == HttpStatusCode.OK)
                        {
                            Log.Info(reply + " successfully posted on Proday2kolesa");
                            if (RemoveEntries.Remove(data, ProductEnum.Motorcycle))
                                Log.Info(reply + " removed from list (Proday2Kolesa)");
                            return SitePoster.PostStatus.OK;
                        }
                        Log.Warn(reply + " unsuccessfully posted on Proday2kolesa");
                        request.Abort();
                        RemoveEntries.Unposted(data, ProductEnum.Motorcycle, SiteEnum.Proday2Kolesa);

                        if (RemoveEntries.Remove(data, ProductEnum.Motorcycle))
                            Log.Info(reply + " removed from list (Proday2Kolesa)");

                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(string.Format("{0} {1} unsuccessfully posted on Proday2kolesa",
                                SpareEquipXmlWorker.GetItemSiteIdUsingPlant("p", data.DataDictionary["model"]),
                                data.DataDictionary["modification"]), ex);

                        RemoveEntries.Unposted(data, ProductEnum.Motorcycle, SiteEnum.Proday2Kolesa);

                        if (RemoveEntries.Remove(data, ProductEnum.Motorcycle))
                            LogManager.GetCurrentClassLogger().Info("{0} {1} removed from list (Proday2Kolesa)",
                                SpareEquipXmlWorker.GetItemSiteIdUsingPlant("p", data.DataDictionary["model"]),
                                data.DataDictionary["modification"]);

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
                        var reply = string.Format("{0} {1}",
                            SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pz", dataDictionary["model"]),
                            dataDictionary["modification"]);

                        const string url = "http://proday2kolesa.com.ua/index.php";
                        var referer =
                            string.Format(
                                "http://proday2kolesa.com.ua/component/option,{0}/v,{1}/Itemid,{2}/task,edit/category,{3}/",
                                dataDictionary["option"], dataDictionary["vendor"], dataDictionary["Itemid"],
                                dataDictionary["category"]);

                        var cookieContainer = Cookies.GetCookiesContainer(referer);

                        var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary, referer);

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
                        start = responseString.IndexOf("name=\"task\" value=\"", start) +
                                "name=\"task\" value=\"".Length;
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
                        var captcha = CaptchaString.GetCaptchaString(CaptchaXmlWorker.GetCaptchaValues("key"),
                            captchaFileName,
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

                        referer = string.Format(
                            "http://proday2kolesa.com.ua/component/option,{0}/task,publish/id,{1}/error,0/Itemid,{2}/",
                            dataDictionary["option"], id, Itemid);
                        request = Request.POSTRequest(url, cookieContainer, captchaDictionary, null, referer);

                        if (Response.GetResponse(request).StatusCode == HttpStatusCode.OK)
                        {
                            Log.Info(reply + " successfully posted on Proday2kolesa");
                            if (RemoveEntries.Remove(data, ProductEnum.Spare))
                                Log.Info(reply + " removed from list (Proday2kolesa)");
                            return SitePoster.PostStatus.OK;
                        }
                        Log.Warn(reply + " unsuccessfully posted on Proday2kolesa");
                        request.Abort();

                        RemoveEntries.Unposted(data, ProductEnum.Spare, SiteEnum.Proday2Kolesa);

                        if (RemoveEntries.Remove(data, ProductEnum.Spare))
                            Log.Info(reply + " removed from list (Proday2kolesa)");

                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(string.Format("{0} {1} unsuccessfully posted on Proday2kolesa",
                                SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pz", data.DataDictionary["model"]),
                                data.DataDictionary["modification"]), ex);

                        RemoveEntries.Unposted(data, ProductEnum.Spare, SiteEnum.Proday2Kolesa);

                        if (RemoveEntries.Remove(data, ProductEnum.Spare))
                            LogManager.GetCurrentClassLogger().Info("{0} {1} removed from list (Proday2kolesa)",
                                SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pz", data.DataDictionary["model"]));
                        return SitePoster.PostStatus.ERROR;
                    }
                });
        }

        public async Task<SitePoster.PostStatus> PostEquip(DicHolder data)
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
                            SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pe", dataDictionary["model"]),
                            dataDictionary["modification"]);

                        const string url = "http://proday2kolesa.com.ua/index.php";
                        var referer =
                            string.Format(
                                "http://proday2kolesa.com.ua/component/option,{0}/v,{1}/Itemid,{2}/task,edit/category,{3}/",
                                dataDictionary["option"], dataDictionary["vendor"], dataDictionary["Itemid"],
                                dataDictionary["category"]);

                        var cookieContainer = Cookies.GetCookiesContainer(referer);

                        var request = Request.POSTRequest(url, cookieContainer, dataDictionary, fileDictionary, referer);

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
                        start = responseString.IndexOf("name=\"task\" value=\"", start) +
                                "name=\"task\" value=\"".Length;
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
                        var captcha = CaptchaString.GetCaptchaString(CaptchaXmlWorker.GetCaptchaValues("key"),
                            captchaFileName,
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

                        referer = string.Format(
                            "http://proday2kolesa.com.ua/component/option,{0}/task,publish/id,{1}/error,0/Itemid,{2}/",
                            dataDictionary["option"], id, Itemid);
                        request = Request.POSTRequest(url, cookieContainer, captchaDictionary, null, referer);

                        if (Response.GetResponse(request).StatusCode == HttpStatusCode.OK)
                        {
                            Log.Info(reply + " successfully posted on Proday2kolesa");
                            if (RemoveEntries.Remove(data, ProductEnum.Equip))
                                Log.Info(reply + " removed from list (Proday2kolesa)");
                            return SitePoster.PostStatus.OK;
                        }
                        Log.Warn(reply + " unsuccessfully posted on Proday2kolesa");
                        request.Abort();
                        RemoveEntries.Unposted(data, ProductEnum.Equip, SiteEnum.Proday2Kolesa);
                        if (RemoveEntries.Remove(data, ProductEnum.Equip))
                            Log.Info(reply + " removed from list (Proday2kolesa)");
                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(string.Format("{0} {1} unsuccessfully posted on Proday2kolesa",
                                SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pe", data.DataDictionary["model"]),
                                data.DataDictionary["modification"]), ex);
                        RemoveEntries.Unposted(data, ProductEnum.Equip, SiteEnum.Proday2Kolesa);
                        if (RemoveEntries.Remove(data, ProductEnum.Equip))
                            LogManager.GetCurrentClassLogger().Info("{0} {1} removed from list (Proday2kolesa)",
                                SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pe", data.DataDictionary["model"]),
                                data.DataDictionary["modification"]);
                        return SitePoster.PostStatus.ERROR;
                    }
                });
        }
    }
}