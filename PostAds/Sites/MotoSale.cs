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
    using Interfaces;
    using NLog;
    using POST;
    using XmlWorker;

    public class MotoSale : ISitePoster
    {
        private readonly Object locker = new object();

        public PostStatus PostMoto(DicHolder data)
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
                    Log.Warn("Proxy list == null", SiteEnum.MotoSale, ProductEnum.Motorcycle);
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
                    var captcha = CaptchaString.GetCaptchaString(CaptchaXmlWorker.GetCaptchaValues("key"),
                        captchaFileName, CaptchaXmlWorker.GetCaptchaValues("domain"));

                    if (captcha == "ZERO")
                    {
                        Log.Warn(reply + " || Нулевой либо отрицательный баланс", SiteEnum.MotoSale,
                            ProductEnum.Motorcycle);
                        RemoveEntries.Unposted(data.Row, ProductEnum.Motorcycle, SiteEnum.MotoSale);
                        if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                            Log.Debug(reply + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Motorcycle);
                        return PostStatus.CAPTCHA_ERROR;
                    }

                    dataDictionary["fConfirmationCode"] = captcha;

                    //Request
                    respString = string.Empty;
                    var proxyAddress = new ProxyAddressStruct {ProxyAddresses = "localhost"};
                    while (ProxyAddressWorker.ProxyListState)
                    {
                        try
                        {
                            lock (locker)
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
                            lock (locker)
                            {
                                ProxyXmlWorker.RemoveProxyAddressFromFile(proxyAddress.ProxyAddresses);
                            }
                        }
                    }
                } while (respString.Contains("Проверочный код не верен"));
                if (respString == string.Empty) throw new Exception("Not valid proxy addresses");
                if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                    throw new Exception("Нарушение правил добавления");
                if (respString == "Response string empty") throw new Exception("Response string empty");
                if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                {
                    Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                             dataDictionary["mail"] + " или " + dataDictionary["phone"], SiteEnum.MotoSale,
                        ProductEnum.Motorcycle);

                    RemoveEntries.Unposted(data.Row, ProductEnum.Motorcycle, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                        Log.Debug(reply + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Motorcycle);

                    return PostStatus.ERROR;
                }
                if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                {
                    try
                    {
                        while (
                            !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                PasswordXmlWorker.GetPasswordValue()))
                            Thread.Sleep(5000);
                        Log.Info(reply + " successfully posted on Motosale", SiteEnum.MotoSale, ProductEnum.Motorcycle);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(dataDictionary["mail"] + " not confirmed. " + ex.Message, SiteEnum.MotoSale,
                            ProductEnum.Motorcycle);
                    }


                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                        Log.Debug(reply + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Motorcycle);

                    return PostStatus.OK;
                }
                //=====================================================//

                Log.Warn(reply + " unsuccessfully posted on Motosale  (Server error)", SiteEnum.MotoSale,
                    ProductEnum.Motorcycle);
                RemoveEntries.Unposted(data.Row, ProductEnum.Motorcycle, SiteEnum.MotoSale);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                    Log.Debug(reply + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Motorcycle);
                return PostStatus.ERROR;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(string.Format("{0} {1}{2}. {3} unsuccessfully posted on Motosale",
                        ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model"]),
                        data.DataDictionary["manufactured_model"], data.DataDictionary["custom_model"], ex.Message),
                        SiteEnum.MotoSale, ProductEnum.Motorcycle);

                RemoveEntries.Unposted(data.Row, ProductEnum.Motorcycle, SiteEnum.MotoSale);

                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle))
                    LogManager.GetCurrentClassLogger().Debug(string.Format("{0} {1}{2} removed from list (Motosale)",
                        ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model"]),
                        data.DataDictionary["manufactured_model"], data.DataDictionary["custom_model"]),
                        SiteEnum.MotoSale, ProductEnum.Motorcycle);

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
                var reply = string.Format("{0} {1}", ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", dataDictionary["model_zap"]), dataDictionary["type"]);

                if (!ProxyAddressWorker.ProxyListState)
                {
                    Log.Warn("Proxy list == null", SiteEnum.MotoSale, ProductEnum.Spare);
                    RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                        Log.Debug(dataDictionary["header"] + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Spare);
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
                    var captcha = CaptchaString.GetCaptchaString(CaptchaXmlWorker.GetCaptchaValues("key"),
                        captchaFileName, CaptchaXmlWorker.GetCaptchaValues("domain"));

                    if (captcha == "ZERO")
                    {
                        Log.Warn(dataDictionary["header"] + " || Нулевой либо отрицательный баланс", SiteEnum.MotoSale, ProductEnum.Spare);
                        RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.MotoSale);
                        if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                            Log.Debug(dataDictionary["header"] + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Spare);
                        return PostStatus.CAPTCHA_ERROR;
                    }

                    dataDictionary["fConfirmationCode"] = captcha;

                    //Request
                    respString = string.Empty;
                    var proxyAddress = new ProxyAddressStruct {ProxyAddresses = "localhost"};
                    while (ProxyAddressWorker.ProxyListState)
                    {
                        try
                        {
                            lock (locker)
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
                            lock (locker)
                            {
                                ProxyXmlWorker.RemoveProxyAddressFromFile(proxyAddress.ProxyAddresses);
                            }
                        }
                    }
                } while (respString.Contains("Проверочный код не верен"));
                if (respString == string.Empty) throw new Exception("Not valid proxy addresses");
                if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                    throw new Exception("Нарушение правил добавления");
                if (respString == "Response string empty") throw new Exception("Response string empty");
                if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                {
                    Log.Warn(dataDictionary["header"] + " unsuccessfully posted on Motosale || дневной лимит для " +
                             dataDictionary["mail"] + " или " + dataDictionary["phone"], SiteEnum.MotoSale,
                        ProductEnum.Spare);
                    RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                        Log.Debug(dataDictionary["header"] + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Spare);
                    return PostStatus.ERROR;
                }
                if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                {
                    try
                    {
                        while (
                            !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                PasswordXmlWorker.GetPasswordValue()))
                            Thread.Sleep(5000);
                        Log.Info(dataDictionary["header"] + " successfully posted on Motosale", SiteEnum.MotoSale, ProductEnum.Spare);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(dataDictionary["mail"] + " not confirmed." + ex.Message, SiteEnum.MotoSale,
                            ProductEnum.Spare);
                    }

                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                        Log.Debug(dataDictionary["header"] + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Spare);
                    return PostStatus.OK;
                }
                Log.Warn(dataDictionary["header"] + " unsuccessfully posted on Motosale (Server error)", SiteEnum.MotoSale,
                    ProductEnum.Spare);
                RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.MotoSale);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                    Log.Debug(dataDictionary["header"] + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Spare);
                return PostStatus.ERROR;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(data.DataDictionary["header"] + " unsuccessfully posted on Motosale", ex);
                RemoveEntries.Unposted(data.Row, ProductEnum.Spare, SiteEnum.MotoSale);
                if (RemoveEntries.Remove(data.LineNum, ProductEnum.Spare))
                    LogManager.GetCurrentClassLogger()
                        .Debug(data.DataDictionary["header"] + " unsuccessfully posted on Motosale");
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
                var reply = string.Format("{0} {1}", dataDictionary["brand"], dataDictionary["type"]);

                if (!ProxyAddressWorker.ProxyListState)
                {
                    Log.Warn("Proxy list == null", SiteEnum.MotoSale, ProductEnum.Equip);
                    RemoveEntries.Unposted(data.Row, ProductEnum.Equip, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                        Log.Debug(reply + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Equip);
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
                    var captcha = CaptchaString.GetCaptchaString(CaptchaXmlWorker.GetCaptchaValues("key"),
                        captchaFileName, CaptchaXmlWorker.GetCaptchaValues("domain"));

                    if (captcha == "ZERO")
                    {
                        Log.Warn(reply + " || Нулевой либо отрицательный баланс", SiteEnum.MotoSale, ProductEnum.Equip);
                        RemoveEntries.Unposted(data.Row, ProductEnum.Equip, SiteEnum.MotoSale);
                        if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                            Log.Debug(reply + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Equip);
                        return PostStatus.CAPTCHA_ERROR;
                    }

                    dataDictionary["fConfirmationCode"] = captcha;

                    //Request
                    respString = string.Empty;
                    var proxyAddress = new ProxyAddressStruct {ProxyAddresses = "localhost"};
                    while (ProxyAddressWorker.ProxyListState)
                    {
                        try
                        {
                            lock (locker)
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
                            lock (locker)
                            {
                                ProxyXmlWorker.RemoveProxyAddressFromFile(proxyAddress.ProxyAddresses);
                            }
                        }
                    }
                } while (respString.Contains("Проверочный код не верен"));
                if (respString == string.Empty) throw new Exception("Not valid proxy addresses");
                if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                    throw new Exception("Нарушение правил добавления");
                if (respString == "Response string empty") throw new Exception("Response string empty");
                if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                {
                    Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                             dataDictionary["mail"] + " или " + dataDictionary["phone"], SiteEnum.MotoSale,
                        ProductEnum.Equip);
                    RemoveEntries.Unposted(data.Row, ProductEnum.Equip, SiteEnum.MotoSale);
                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                        Log.Debug(reply + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Equip);
                    return PostStatus.ERROR;
                }
                if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                {
                    try
                    {
                        while (
                            !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                PasswordXmlWorker.GetPasswordValue()))
                            Thread.Sleep(5000);
                        Log.Info(reply + " successfully posted on Motosale", SiteEnum.MotoSale, ProductEnum.Equip);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(dataDictionary["mail"] + " not confirmed" + ex.Message, SiteEnum.MotoSale,
                            ProductEnum.Equip);
                    }

                    if (RemoveEntries.Remove(data.LineNum, ProductEnum.Equip))
                        Log.Debug(reply + " removed from list (Motosale)", SiteEnum.MotoSale, ProductEnum.Equip);
                    return PostStatus.OK;
                }
                Log.Warn(reply + " unsuccessfully posted on Motosale (Server error)", SiteEnum.MotoSale,
                    ProductEnum.Equip);
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