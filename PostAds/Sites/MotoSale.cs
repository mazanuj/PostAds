using System.Text;
using System.Threading;
using Motorcycle.Config.Confirm;
using Motorcycle.Config.Proxy;

namespace Motorcycle.Sites
{
    using System;
    using Captcha;
    using Config.Data;
    using HTTP;
    using NLog;
    using POST;
    using XmlWorker;

    public static class MotoSale
    {
        private static readonly Object Locker = new object();

        public static PostStatus PostMoto(DicHolder data)
        {
            try
            {
                var Log = LogManager.GetCurrentClassLogger();
                var dataDictionary = data.DataDictionary;
                var fileDictionary = data.FileDictionary;
                var reply = string.Format("{0} {1}{2}",
                    ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", dataDictionary["model"]),
                    ManufactureXmlWorker.GetItemNameUsingValue("m", dataDictionary["model"],
                        dataDictionary["manufactured_model"]), dataDictionary["custom_model"]);

                if (!ProxyAddressWorker.ProxyListState)
                {
                    Log.Warn("Proxy list == null");
                    RemoveEntries.Unposted(data.Row, ProductEnum.Motorcycle, SiteEnum.MotoSale);
                    return PostStatus.ERROR;
                }

                string respString;
                const string url = "http://www.motosale.com.ua/?add=moto";
                do
                {
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
                    respString = string.Empty;
                    var proxyAddress = new ProxyAddressStruct { ProxyAddresses = "localhost" };
                    while (ProxyAddressWorker.ProxyListState)
                    {
                        try
                        {
                            lock (Locker)
                            {
                                proxyAddress = ProxyAddressWorker.GetValidProxyAddress("moto");
                            }

                            respString = Response.GetResponseString(cookieContainer, dataDictionary,
                                fileDictionary,
                                url, Encoding.GetEncoding("windows-1251"), proxyAddress);
                            if (respString == string.Empty)
                                respString = "Response string empty";
                            break;
                        }
                        catch
                        {
                            lock (Locker)
                            {
                                ProxyXmlWorker.RemoveProxyAddressFromFile(proxyAddress.ProxyAddresses);
                            }
                        }
                    }
                } while (respString.Contains("Введите код подтверждения:"));
                if (respString == string.Empty) throw new Exception("Not valid proxy addresses");
                if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                    throw new Exception("Нарушение правил добавления");
                if (respString == "Response string empty") throw new Exception("Response string empty");
                if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                {
                    Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                             dataDictionary["mail"] + " или " + dataDictionary["phone"]);

                    RemoveEntries.Unposted(data.Row, ProductEnum.Motorcycle, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                        Log.Debug(reply + " removed from list (Motosale)");

                    return PostStatus.ERROR;
                }
                if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                {
                    Log.Info(reply + " successfully posted on Motosale");
                    try
                    {
                        while (
                            !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                PasswordXmlWorker.GetPasswordValue()))
                            Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(dataDictionary["mail"] + " not confirmed", ex);
                    }


                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                        Log.Debug(reply + " removed from list (Motosale)");

                    return PostStatus.OK;
                }
                //=====================================================//

                Log.Warn(reply + " unsuccessfully posted on Motosale");
                RemoveEntries.Unposted(data.Row, ProductEnum.Motorcycle, SiteEnum.MotoSale);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                    Log.Debug(reply + " removed from list (Motosale)");
                return PostStatus.ERROR;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(string.Format("{0} {1}{2} unsuccessfully posted on Motosale",
                        ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model"]),
                        data.DataDictionary["manufactured_model"], data.DataDictionary["custom_model"]), ex);

                RemoveEntries.Unposted(data.Row, ProductEnum.Motorcycle, SiteEnum.MotoSale);

                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                    LogManager.GetCurrentClassLogger().Debug("{0} {1}{2} removed from list (Motosale)",
                        ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model"]),
                        data.DataDictionary["manufactured_model"], data.DataDictionary["custom_model"]);

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
                    ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", dataDictionary["model_zap"]),
                    dataDictionary["type"]);

                if (!ProxyAddressWorker.ProxyListState)
                {
                    Log.Warn("Proxy list == null");
                    RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                        Log.Debug(reply + " removed from list (Motosale)");
                    return PostStatus.ERROR;
                }

                string respString;
                const string url = "http://www.motosale.com.ua/?add=zap";
                do
                {
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
                    respString = string.Empty;
                    var proxyAddress = new ProxyAddressStruct { ProxyAddresses = "localhost" };
                    while (ProxyAddressWorker.ProxyListState)
                    {
                        try
                        {
                            lock (Locker)
                            {
                                proxyAddress = ProxyAddressWorker.GetValidProxyAddress("spare");
                            }

                            respString = Response.GetResponseString(cookieContainer, dataDictionary,
                                fileDictionary,
                                url, Encoding.GetEncoding("windows-1251"), proxyAddress);
                            if (respString == string.Empty)
                                respString = "Response string empty";
                            break;
                        }
                        catch
                        {
                            lock (Locker)
                            {
                                ProxyXmlWorker.RemoveProxyAddressFromFile(proxyAddress.ProxyAddresses);
                            }
                        }
                    }
                } while (respString.Contains("Введите код подтверждения:"));
                if (respString == string.Empty) throw new Exception("Not valid proxy addresses");
                if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                    throw new Exception("Нарушение правил добавления");
                if (respString == "Response string empty") throw new Exception("Response string empty");
                if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                {
                    Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                             dataDictionary["mail"] + " или " + dataDictionary["phone"]);
                    RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                        Log.Debug(reply + " removed from list (Motosale)");
                    return PostStatus.ERROR;
                }
                if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                {
                    Log.Info(reply + " successfully posted on Motosale");

                    try
                    {
                        while (
                            !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                PasswordXmlWorker.GetPasswordValue()))
                            Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(dataDictionary["mail"] + " not confirmed", ex);
                    }

                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                        Log.Debug(reply + " removed from list (Motosale)");
                    return PostStatus.OK;
                }
                Log.Warn(reply + " unsuccessfully posted on Motosale");
                RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.MotoSale);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                    Log.Debug(reply + " removed from list (Motosale)");
                return PostStatus.ERROR;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(string.Format("{0} {1} unsuccessfully posted on Motosale",
                        ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model_zap"]),
                        data.DataDictionary["type"]), ex);
                RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.MotoSale);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                    LogManager.GetCurrentClassLogger()
                        .Debug("{0} {1} removed from list (Motosale)",
                            ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model_zap"]),
                            data.DataDictionary["type"]);
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
                var reply = string.Format("{0} {1}", dataDictionary["brand"], dataDictionary["type"]);

                if (!ProxyAddressWorker.ProxyListState)
                {
                    Log.Warn("Proxy list == null");
                    RemoveEntries.Unposted(data.Row, ProductEnum.Equip, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                        Log.Debug(reply + " removed from list (Motosale)");
                    return PostStatus.ERROR;
                }

                string respString;
                const string url = "http://www.motosale.com.ua/?add=equ";
                do
                {
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
                    respString = string.Empty;
                    var proxyAddress = new ProxyAddressStruct { ProxyAddresses = "localhost" };
                    while (ProxyAddressWorker.ProxyListState)
                    {
                        try
                        {
                            lock (Locker)
                            {
                                proxyAddress = ProxyAddressWorker.GetValidProxyAddress("equip");
                            }

                            respString = Response.GetResponseString(cookieContainer, dataDictionary,
                                fileDictionary,
                                url, Encoding.GetEncoding("windows-1251"), proxyAddress);
                            if (respString == string.Empty)
                                respString = "Response string empty";
                            break;
                        }
                        catch
                        {
                            lock (Locker)
                            {
                                ProxyXmlWorker.RemoveProxyAddressFromFile(proxyAddress.ProxyAddresses);
                            }
                        }
                    }
                } while (respString.Contains("Введите код подтверждения:"));
                if (respString == string.Empty) throw new Exception("Not valid proxy addresses");
                if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                    throw new Exception("Нарушение правил добавления");
                if (respString == "Response string empty") throw new Exception("Response string empty");
                if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                {
                    Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                             dataDictionary["mail"] + " или " + dataDictionary["phone"]);
                    RemoveEntries.Unposted(data.Row, ProductEnum.Equip, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                        Log.Debug(reply + " removed from list (Motosale)");
                    return PostStatus.ERROR;
                }
                if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                {
                    Log.Info(reply + " successfully posted on Motosale");

                    try
                    {
                        while (
                            !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                PasswordXmlWorker.GetPasswordValue()))
                            Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(dataDictionary["mail"] + " not confirmed", ex);
                    }

                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                        Log.Debug(reply + " removed from list (Motosale)");
                    return PostStatus.OK;
                }
                Log.Warn(reply + " unsuccessfully posted on Motosale");
                RemoveEntries.Unposted(data.Row, ProductEnum.Equip, SiteEnum.MotoSale);
                return PostStatus.ERROR;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(
                        string.Format("{0} {1} unsuccessfully posted on Motosale", data.DataDictionary["brand"],
                            data.DataDictionary["type"]), ex);

                RemoveEntries.Unposted(data.Row, ProductEnum.Equip, SiteEnum.MotoSale);

                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                    LogManager.GetCurrentClassLogger()
                        .Debug("{0} {1} removed from list (Motosale)", data.DataDictionary["brand"],
                            data.DataDictionary["type"]);

                return PostStatus.ERROR;
            }
        }
    }
}