using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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

                if (!ProxyAddressWorker.ProxyListState)
                {
                    Log.Warn("Proxy list == null");
                    RemoveEntries.Remove(data, ProductEnum.Motorcycle, SiteEnum.MotoSale);
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
                        Log.Warn(dataDictionary["header"] + " || Нулевой либо отрицательный баланс", SiteEnum.MotoSale,
                            ProductEnum.Motorcycle);
                        RemoveEntries.Remove(data, ProductEnum.Motorcycle, SiteEnum.MotoSale);
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
                                fileDictionary, url, Encoding.GetEncoding("windows-1251"), proxyAddress);
                            if (!Regex.IsMatch(respString, @"[а-яА-Я]+?"))
                                continue;
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
                } while (respString.Contains("Проверочный код не верен") || respString.Contains("squid"));
                if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                    throw new Exception("Нарушение правил добавления");
                if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                {
                    Log.Warn(dataDictionary["header"] + " unsuccessfully posted || дневной лимит для " +
                             dataDictionary["mail"] + " или " + dataDictionary["phone"], SiteEnum.MotoSale,
                        ProductEnum.Motorcycle);

                    RemoveEntries.Remove(data, ProductEnum.Motorcycle, SiteEnum.MotoSale);

                    return PostStatus.ERROR;
                }
                if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                {
                    var checker = false;
                    for (var i = 0; i < 12; i++)
                    {
                        try
                        {
                            if (!PostConfirm.ConfirmAdv(dataDictionary["mail"]))
                            {
                                Thread.Sleep(10000);
                                continue;
                            }

                            checker = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log.Debug(dataDictionary["mail"] + " not confirmed " + ex.Message, SiteEnum.MotoSale,
                                ProductEnum.Motorcycle);
                        }
                    }

                    if (checker)
                        Log.Info(dataDictionary["header"] + " successfully posted and confirmed", SiteEnum.MotoSale,
                            ProductEnum.Motorcycle);
                    else
                        Log.Warn(
                            dataDictionary["header"] + " successfully posted BUT NOT confirmed // " +
                            dataDictionary["mail"], SiteEnum.MotoSale,
                            ProductEnum.Motorcycle);

                    RemoveEntries.Remove(data.LineNum, ProductEnum.Motorcycle);
                    return PostStatus.OK;
                }
                //=====================================================//

                Log.Warn(dataDictionary["header"] + " unsuccessfully posted (Server error)", SiteEnum.MotoSale,
                    ProductEnum.Motorcycle);
                RemoveEntries.Remove(data, ProductEnum.Motorcycle, SiteEnum.MotoSale);

                return PostStatus.ERROR;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(data.DataDictionary["header"] + " " + ex.Message, SiteEnum.MotoSale, ProductEnum.Motorcycle);
                RemoveEntries.Remove(data, ProductEnum.Motorcycle, SiteEnum.MotoSale);

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

                if (!ProxyAddressWorker.ProxyListState)
                {
                    Log.Warn("Proxy list == null");
                    RemoveEntries.Remove(data, ProductEnum.Spare, SiteEnum.MotoSale);
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
                        Log.Warn(dataDictionary["header"] + " || Нулевой либо отрицательный баланс", SiteEnum.MotoSale,
                            ProductEnum.Spare);
                        RemoveEntries.Remove(data, ProductEnum.Spare, SiteEnum.MotoSale);
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
                                fileDictionary, url, Encoding.GetEncoding("windows-1251"), proxyAddress);
                            if (!Regex.IsMatch(respString, @"[а-яА-Я]+?"))
                                continue;
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
                } while (respString.Contains("Проверочный код не верен") || respString.Contains("squid"));
                if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                    throw new Exception("Нарушение правил добавления");
                if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                {
                    Log.Warn(dataDictionary["header"] + " unsuccessfully posted || дневной лимит для " +
                             dataDictionary["mail"] + " или " + dataDictionary["phone"], SiteEnum.MotoSale,
                        ProductEnum.Spare);
                    RemoveEntries.Remove(data, ProductEnum.Spare, SiteEnum.MotoSale);

                    return PostStatus.ERROR;
                }
                if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                {
                    var checker = false;
                    for (var i = 0; i < 12; i++)
                    {
                        try
                        {
                            if (!PostConfirm.ConfirmAdv(dataDictionary["mail"]))
                            {
                                Thread.Sleep(10000);
                                continue;
                            }

                            checker = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log.Debug(dataDictionary["mail"] + " not confirmed " + ex.Message, SiteEnum.MotoSale,
                                ProductEnum.Spare);
                        }
                    }

                    if (checker)
                        Log.Info(dataDictionary["header"] + " successfully posted and confirmed", SiteEnum.MotoSale,
                            ProductEnum.Spare);
                    else
                        Log.Warn(
                            dataDictionary["header"] + " successfully posted BUT NOT confirmed // " +
                            dataDictionary["mail"], SiteEnum.MotoSale,
                            ProductEnum.Spare);

                    RemoveEntries.Remove(data.LineNum, ProductEnum.Spare);
                    return PostStatus.OK;
                }
                Log.Warn(dataDictionary["header"] + " unsuccessfully posted (Server error)", SiteEnum.MotoSale,
                    ProductEnum.Spare);

                using (var sw = new StreamWriter("response.txt", true, Encoding.GetEncoding("windows-1251")))
                {
                    sw.WriteLine(respString);
                    sw.WriteLine(new string('=', 50));
                }


                RemoveEntries.Remove(data, ProductEnum.Spare, SiteEnum.MotoSale);

                return PostStatus.ERROR;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(data.DataDictionary["header"] + " " + ex.Message, SiteEnum.MotoSale, ProductEnum.Spare);
                RemoveEntries.Remove(data, ProductEnum.Spare, SiteEnum.MotoSale);
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

                if (!ProxyAddressWorker.ProxyListState)
                {
                    Log.Warn("Proxy list == null");
                    RemoveEntries.Remove(data, ProductEnum.Equip, SiteEnum.MotoSale);
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
                        Log.Warn(dataDictionary["header"] + " || Нулевой либо отрицательный баланс", SiteEnum.MotoSale,
                            ProductEnum.Equip);
                        RemoveEntries.Remove(data, ProductEnum.Equip, SiteEnum.MotoSale);
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
                            if (!Regex.IsMatch(respString, @"[а-яА-Я]+?"))
                                continue;
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
                } while (respString.Contains("Проверочный код не верен") || respString.Contains("squid"));
                if (respString.Contains("Ошибка при добавлении объявления. Не нарушайте правила добавления."))
                    throw new Exception("Нарушение правил добавления");
                if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                {
                    Log.Warn(dataDictionary["header"] + " unsuccessfully posted || дневной лимит для " +
                             dataDictionary["mail"] + " или " + dataDictionary["phone"], SiteEnum.MotoSale,
                        ProductEnum.Equip);
                    RemoveEntries.Remove(data, ProductEnum.Equip, SiteEnum.MotoSale);
                    return PostStatus.ERROR;
                }
                if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                {
                    var checker = false;
                    for (var i = 0; i < 12; i++)
                    {
                        try
                        {
                            if (!PostConfirm.ConfirmAdv(dataDictionary["mail"]))
                            {
                                Thread.Sleep(10000);
                                continue;
                            }

                            checker = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log.Debug(dataDictionary["mail"] + " not confirmed " + ex.Message, SiteEnum.MotoSale,
                                ProductEnum.Equip);
                        }
                    }

                    if (checker)
                        Log.Info(dataDictionary["header"] + " successfully posted and confirmed", SiteEnum.MotoSale,
                            ProductEnum.Equip);
                    else
                        Log.Warn(
                            dataDictionary["header"] + " successfully posted BUT NOT confirmed // " +
                            dataDictionary["mail"], SiteEnum.MotoSale,
                            ProductEnum.Equip);

                    RemoveEntries.Remove(data.LineNum, ProductEnum.Equip);
                    return PostStatus.OK;
                }
                Log.Warn(dataDictionary["header"] + " unsuccessfully posted (Server error)", SiteEnum.MotoSale,
                    ProductEnum.Equip);
                RemoveEntries.Remove(data, ProductEnum.Equip, SiteEnum.MotoSale);
                return PostStatus.ERROR;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger()
                    .Error(data.DataDictionary["header"] + " " + ex.Message, SiteEnum.MotoSale, ProductEnum.Equip);
                RemoveEntries.Remove(data, ProductEnum.Equip, SiteEnum.MotoSale);

                return PostStatus.ERROR;
            }
        }
    }
}