namespace Motorcycle.Sites
{
    using Captcha;
    using Config.Data;
    using HTTP;
    using NLog;
    using POST;
    using System;
    using System.Collections.Generic;
    using XmlWorker;

    public static class Proday2Kolesa
    {
        public static PostStatus PostMoto(DicHolder data)
        {
            try
            {
                var Log = LogManager.GetCurrentClassLogger();
                var dataDictionary = data.DataDictionary;
                var fileDictionary = data.FileDictionary;
                var reply = string.Format("{0} {1}",
                    ManufactureXmlWorker.GetItemSiteIdUsingPlant("p", dataDictionary["model"]),
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

                do
                {
                    #region Find

                    //Find captcha's link
                    var start = responseString.IndexOf("index2.php?option=com_autobb");
                    var end = responseString.IndexOf("\"", start);
                    var captchaUrl = "http://proday2kolesa.com.ua/" +
                                     responseString.Substring(start, end - start);

                    //Find id
                    start = responseString.IndexOf("name=\"id\" value=\"", start) +
                            "name=\"id\" value=\"".Length;
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

                    var req = Request.POSTRequest(url, cookieContainer, captchaDictionary, null, referer);
                    responseString = Response.GetResponseString(req);
                    req.Abort();
                } while (responseString.Contains("Введите секретный код:"));

                Log.Info(reply + " successfully posted on Proday2kolesa", SiteEnum.Proday2Kolesa, ProductEnum.Motorcycle);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                    Log.Debug(reply + " removed from list (Proday2Kolesa)", SiteEnum.Proday2Kolesa, ProductEnum.Motorcycle);
                return PostStatus.OK;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(string.Format("{0} {1} unsuccessfully posted on Proday2kolesa",
                        ManufactureXmlWorker.GetItemSiteIdUsingPlant("p", data.DataDictionary["model"]),
                        data.DataDictionary["modification"]), ex);

                RemoveEntries.Unposted(data.Row, ProductEnum.Motorcycle, SiteEnum.Proday2Kolesa);

                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                    LogManager.GetCurrentClassLogger().Debug("{0} {1} removed from list (Proday2Kolesa)",
                        SpareEquipXmlWorker.GetItemSiteIdUsingPlant("p", data.DataDictionary["model"]),
                        data.DataDictionary["modification"]);

                return PostStatus.ERROR;
            }
        }

        public static PostStatus PostSpare(DicHolder data)
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

                do
                {
                    #region Find

                    //Find captcha's link
                    var start = responseString.IndexOf("index2.php?option=com_autobb");
                    var end = responseString.IndexOf("\"", start);
                    var captchaUrl = "http://proday2kolesa.com.ua/" +
                                     responseString.Substring(start, end - start);

                    //Find id
                    start = responseString.IndexOf("name=\"id\" value=\"", start) +
                            "name=\"id\" value=\"".Length;
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

                    var req = Request.POSTRequest(url, cookieContainer, captchaDictionary, null, referer);
                    responseString = Response.GetResponseString(req);
                } while (responseString.Contains("Введите секретный код:"));

                Log.Info(reply + " successfully posted on Proday2kolesa", SiteEnum.Proday2Kolesa, ProductEnum.Spare);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                    Log.Debug(reply + " removed from list (Proday2kolesa)", SiteEnum.Proday2Kolesa, ProductEnum.Spare);
                return PostStatus.OK;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(string.Format("{0} {1} unsuccessfully posted on Proday2kolesa. {3}",
                        SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pz", data.DataDictionary["model"]),
                        data.DataDictionary["modification"], ex.Message),
                        SiteEnum.Proday2Kolesa, ProductEnum.Spare);

                RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.Proday2Kolesa);

                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                    LogManager.GetCurrentClassLogger().Debug(string.Format("{0} {1} removed from list (Proday2kolesa)",
                        SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pz", data.DataDictionary["model"])),
                        SiteEnum.Proday2Kolesa, ProductEnum.Spare);
                return PostStatus.ERROR;
            }
        }

        public static PostStatus PostEquip(DicHolder data)
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

                do
                {
                    #region Find

                    //Find captcha's link
                    var start = responseString.IndexOf("index2.php?option=com_autobb");
                    var end = responseString.IndexOf("\"", start);
                    var captchaUrl = "http://proday2kolesa.com.ua/" +
                                     responseString.Substring(start, end - start);

                    //Find id
                    start = responseString.IndexOf("name=\"id\" value=\"", start) +
                            "name=\"id\" value=\"".Length;
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

                    var req = Request.POSTRequest(url, cookieContainer, captchaDictionary, null, referer);
                    responseString = Response.GetResponseString(req);
                } while (responseString.Contains("Введите секретный код:"));

                Log.Info(reply + " successfully posted on Proday2kolesa", SiteEnum.Proday2Kolesa, ProductEnum.Equip);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                    Log.Debug(reply + " removed from list (Proday2kolesa)", SiteEnum.Proday2Kolesa, ProductEnum.Equip);
                return PostStatus.OK;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(string.Format("{0} {1} unsuccessfully posted on Proday2kolesa",
                        SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pe", data.DataDictionary["model"]),
                        data.DataDictionary["modification"]), ex);
                RemoveEntries.Unposted(data.Row, ProductEnum.Equip, SiteEnum.Proday2Kolesa);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                    LogManager.GetCurrentClassLogger().Debug("{0} {1} removed from list (Proday2kolesa)",
                        SpareEquipXmlWorker.GetItemSiteIdUsingPlant("pe", data.DataDictionary["model"]),
                        data.DataDictionary["modification"]);
                return PostStatus.ERROR;
            }
        }
    }
}