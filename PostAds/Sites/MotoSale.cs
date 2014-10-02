using System.Text;
using System.Threading;
using Motorcycle.Config.Confirm;
using Motorcycle.Config.Proxy;

namespace Motorcycle.Sites
{
    using System;
    using System.Threading.Tasks;
    using Captcha;
    using Config.Data;
    using HTTP;
    using NLog;
    using POST;
    using XmlWorker;
    using Interfaces;

    internal class MotoSale : IPostOnSite
    {
        private readonly Object locker = new object();

        public async Task<SitePoster.PostStatus> PostMoto(DicHolder data)
        {
            return await Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        var Log = LogManager.GetCurrentClassLogger();

                        if (!ProxyAddressWorker.ProxyListState)
                        {
                            Log.Warn("Proxy list == null");
                            RemoveEntries.Unposted(data, ProductEnum.Motorcycle, SiteEnum.MotoSale);
                            return SitePoster.PostStatus.ERROR;
                        }

                        var dataDictionary = data.DataDictionary;
                        var fileDictionary = data.FileDictionary;
                        var reply = string.Format("{0} {1}{2}",
                            ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", dataDictionary["model"]),
                            ManufactureXmlWorker.GetItemNameUsingValue("m", dataDictionary["model"],
                                dataDictionary["manufactured_model"]), dataDictionary["custom_model"]);

                        const string url = "http://www.motosale.com.ua/?add=moto";
                        var cookieContainer = Cookies.GetCookiesContainer(url);

                        var requestImage = Request.GETRequest("http://www.motosale.com.ua/capcha/capcha.php");
                        requestImage.CookieContainer = cookieContainer;

                        var captchaFileName = Response.GetImageFromResponse(requestImage);

                        //Get captcha result
                        var captcha = CaptchaString.GetCaptchaString(
                            CaptchaXmlWorker.GetCaptchaValues("key"),
                            captchaFileName,
                            CaptchaXmlWorker.GetCaptchaValues("domain"));

                        dataDictionary["fConfirmationCode"] = captcha;

                        //Request
                        var respString = string.Empty;
                        var proxyAddress = new ProxyAddressStruct {ProxyAddresses = "localhost"};
                        while (ProxyAddressWorker.ProxyListState)
                        {
                            try
                            {
                                proxyAddress = ProxyAddressWorker.GetValidProxyAddress("moto");
                                respString = Response.GetResponseString(cookieContainer, dataDictionary, fileDictionary,
                                    url, proxyAddress, Encoding.GetEncoding("windows-1251"));
                                if (respString == string.Empty)
                                    respString = "Response string empty";
                                break;
                            }
                            catch
                            {
                                lock (locker)
                                {
                                    ProxyXmlWorker.RemoveProxyAddressFromFile(proxyAddress.ProxyAddresses);
                                }
                            }
                        }
                        if (respString == string.Empty) throw new Exception("Not valid proxy addresses");
                        if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                            throw new Exception("Нарушение правил добавления");
                        if (respString == "Response string empty") throw new Exception("Response string empty");
                        if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                        {
                            Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                                     dataDictionary["mail"] + " или " + dataDictionary["phone"]);
                            RemoveEntries.Unposted(data, ProductEnum.Motorcycle, SiteEnum.MotoSale);
                            return SitePoster.PostStatus.ERROR;
                        }
                        if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                        {
                            Log.Info(reply + " successfully posted on Motosale");

                            while (
                                !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                    "Administr@t0r"))
                                Thread.Sleep(5000);
                            if (RemoveEntries.Remove(data, ProductEnum.Motorcycle))
                                Log.Info(reply + " removed from list (Motosale)");
                            return SitePoster.PostStatus.OK;
                        }
                        //=====================================================//

                        Log.Warn(reply + " unsuccessfully posted on Motosale");
                        RemoveEntries.Unposted(data, ProductEnum.Motorcycle, SiteEnum.MotoSale);
                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(string.Format("{0} {1}{2} unsuccessfully posted on Motosale",
                                ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model"]),
                                data.DataDictionary["manufactured_model"], data.DataDictionary["custom_model"]), ex);
                        RemoveEntries.Unposted(data, ProductEnum.Motorcycle, SiteEnum.MotoSale);
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

                        if (!ProxyAddressWorker.ProxyListState)
                        {
                            Log.Warn("Proxy list == null");
                            RemoveEntries.Unposted(data, ProductEnum.Spare, SiteEnum.MotoSale);
                            return SitePoster.PostStatus.ERROR;
                        }

                        var dataDictionary = data.DataDictionary;
                        var fileDictionary = data.FileDictionary;
                        var reply = string.Format("{0} {1}",
                            ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", dataDictionary["model_zap"]),
                            dataDictionary["type"]);

                        const string url = "http://www.motosale.com.ua/?add=zap";
                        var cookieContainer = Cookies.GetCookiesContainer(url);

                        //Get captcha result
                        var requestImage = Request.GETRequest("http://www.motosale.com.ua/capcha/capcha.php");
                        requestImage.CookieContainer = cookieContainer;

                        var captchaFileName = Response.GetImageFromResponse(requestImage);

                        var captcha = CaptchaString.GetCaptchaString(
                            CaptchaXmlWorker.GetCaptchaValues("key"),
                            captchaFileName,
                            CaptchaXmlWorker.GetCaptchaValues("domain"));

                        dataDictionary["fConfirmationCode"] = captcha;


                        //Get hash result
                        //var requestHash = Request.GETRequest(url);
                        //requestHash.CookieContainer = cookieContainer;
                        //var responseHash = Response.GetResponseString(requestHash);

                        //var start = responseHash.IndexOf("name=\"insert\" value=\"") + "name=\"insert\" value=\"".Length;
                        //var stop = responseHash.IndexOf("\"", start);
                        //var hash = responseHash.Substring(start, stop - start);

                        //dataDictionary["insert"] = hash;

                        //Request
                        var respString = string.Empty;
                        var proxyAddress = new ProxyAddressStruct {ProxyAddresses = "localhost"};
                        while (ProxyAddressWorker.ProxyListState)
                        {
                            try
                            {
                                proxyAddress = ProxyAddressWorker.GetValidProxyAddress("spare");
                                respString = Response.GetResponseString(cookieContainer, dataDictionary, fileDictionary,
                                    url, proxyAddress, Encoding.GetEncoding("windows-1251"));
                                if (respString == string.Empty)
                                    respString = "Response string empty";

                                break;
                            }
                            catch
                            {
                                lock (locker)
                                {
                                    ProxyXmlWorker.RemoveProxyAddressFromFile(proxyAddress.ProxyAddresses);
                                }
                            }
                        }
                        if (respString == string.Empty) throw new Exception("Not valid proxy addresses");
                        if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                            throw new Exception("Нарушение правил добавления");
                        if (respString == "Response string empty") throw new Exception("Response string empty");
                        if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                        {
                            Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                                     dataDictionary["mail"] + " или " + dataDictionary["phone"]);
                            RemoveEntries.Unposted(data, ProductEnum.Spare, SiteEnum.MotoSale);
                            return SitePoster.PostStatus.ERROR;
                        }
                        if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                        {
                            Log.Info(reply + " successfully posted on Motosale");

                            while (
                                !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                    "Administr@t0r"))
                                Thread.Sleep(5000);
                            if (RemoveEntries.Remove(data, ProductEnum.Motorcycle))
                                Log.Info(reply + " removed from list (Motosale)");
                            return SitePoster.PostStatus.OK;
                        }
                        Log.Warn(reply + " unsuccessfully posted on Motosale");
                        RemoveEntries.Unposted(data, ProductEnum.Spare, SiteEnum.MotoSale);
                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(string.Format("{0} {1} unsuccessfully posted on Motosale",
                                ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model_zap"]),
                                data.DataDictionary["type"]), ex);
                        RemoveEntries.Unposted(data, ProductEnum.Spare, SiteEnum.MotoSale);
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

                        if (!ProxyAddressWorker.ProxyListState)
                        {
                            Log.Warn("Proxy list == null");
                            RemoveEntries.Unposted(data, ProductEnum.Equip, SiteEnum.MotoSale);
                            return SitePoster.PostStatus.ERROR;
                        }

                        var dataDictionary = data.DataDictionary;
                        var fileDictionary = data.FileDictionary;
                        var reply = string.Format("{0} {1}", dataDictionary["brand"], dataDictionary["type"]);

                        const string url = "http://www.motosale.com.ua/?add=equ";
                        var cookieContainer = Cookies.GetCookiesContainer(url);

                        //Get captcha result
                        var requestImage = Request.GETRequest("http://www.motosale.com.ua/capcha/capcha.php");
                        requestImage.CookieContainer = cookieContainer;

                        var captchaFileName = Response.GetImageFromResponse(requestImage);

                        var captcha = CaptchaString.GetCaptchaString(
                            CaptchaXmlWorker.GetCaptchaValues("key"),
                            captchaFileName,
                            CaptchaXmlWorker.GetCaptchaValues("domain"));

                        dataDictionary["fConfirmationCode"] = captcha;

                        //Request
                        var respString = string.Empty;
                        var proxyAddress = new ProxyAddressStruct {ProxyAddresses = "localhost"};
                        while (ProxyAddressWorker.ProxyListState)
                        {
                            try
                            {
                                proxyAddress = ProxyAddressWorker.GetValidProxyAddress("equip");
                                respString = Response.GetResponseString(cookieContainer, dataDictionary, fileDictionary,
                                    url, proxyAddress, Encoding.GetEncoding("windows-1251"));
                                if (respString == string.Empty)
                                    respString = "Response string empty";

                                break;
                            }
                            catch
                            {
                                lock (locker)
                                {
                                    ProxyXmlWorker.RemoveProxyAddressFromFile(proxyAddress.ProxyAddresses);
                                }
                            }
                        }
                        if (respString == string.Empty) throw new Exception("Not valid proxy addresses");
                        if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                            throw new Exception("Нарушение правил добавления");
                        if (respString == "Response string empty") throw new Exception("Response string empty");
                        if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                        {
                            Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                                     dataDictionary["mail"] + " или " + dataDictionary["phone"]);
                            RemoveEntries.Unposted(data, ProductEnum.Equip, SiteEnum.MotoSale);
                            return SitePoster.PostStatus.ERROR;
                        }
                        if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                        {
                            Log.Info(reply + " successfully posted on Motosale");

                            while (
                                !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                    "Administr@t0r"))
                                Thread.Sleep(5000);
                            if (RemoveEntries.Remove(data, ProductEnum.Motorcycle))
                                Log.Info(reply + " removed from list (Motosale)");
                            return SitePoster.PostStatus.OK;
                        }
                        Log.Warn(reply + " unsuccessfully posted on Motosale");
                        RemoveEntries.Unposted(data, ProductEnum.Equip, SiteEnum.MotoSale);
                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(
                                string.Format("{0} {1} unsuccessfully posted on Motosale", data.DataDictionary["brand"],
                                    data.DataDictionary["type"]), ex);
                        RemoveEntries.Unposted(data, ProductEnum.Equip, SiteEnum.MotoSale);
                        return SitePoster.PostStatus.ERROR;
                    }
                });
        }
    }
}