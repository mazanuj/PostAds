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
                            return SitePoster.PostStatus.ERROR;
                        }

                        var dataDictionary = data.DataDictionary;
                        var fileDictionary = data.FileDictionary;
                        var reply = string.Format("{0} {1}{2}",
                            ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", dataDictionary["model"]),
                            dataDictionary["manufactured_model"], dataDictionary["custom_model"]);

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
                        string respString;
                        var proxyAddress = string.Empty;
                        while (true)
                        {
                            try
                            {
                                respString = string.Empty;
                                //proxyAddress = ProxyAddressWorker.GetValidProxyAddress("moto");
                                if (proxyAddress == null) break;
                                respString = Response.GetResponseString(cookieContainer, dataDictionary, fileDictionary,
                                    url, proxyAddress);
                                break;
                            }
                            catch
                            {
                                lock (locker)
                                {
                                    ProxyXmlWorker.ChangeServerStatus(proxyAddress, "off");
                                }
                            }
                        }
                        if (respString == string.Empty) throw new Exception("Not valid socks5 addresses");
                        if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                        {
                            Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                                     dataDictionary["mail"] + " или " + dataDictionary["phone"]);
                            return SitePoster.PostStatus.ERROR;
                        }
                        if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                        {
                            Log.Info(reply + " successfully posted on Motosale");

                            while (
                                !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, dataDictionary["mail"],
                                    "Administr@t0r"))
                                Thread.Sleep(5000);

                            return SitePoster.PostStatus.OK;
                        }
                        //=====================================================//

                        Log.Warn(reply + " unsuccessfully posted on Motosale");
                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(string.Format("{0} {1}{2} unsuccessfully posted on Motosale",
                                ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model"]),
                                data.DataDictionary["manufactured_model"], data.DataDictionary["custom_model"]), ex);
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

                        //xNet
                        string respString;
                        var proxyAddress = string.Empty;
                        while (true)
                        {
                            try
                            {
                                respString = string.Empty;
                                // proxyAddress = ProxyAddressWorker.GetValidProxyAddress("spare");
                                if (proxyAddress == null) break;
                                respString = Response.GetResponseString(cookieContainer, dataDictionary, fileDictionary,
                                    url, proxyAddress);
                                break;
                            }
                            catch
                            {
                                ProxyXmlWorker.ChangeServerStatus(proxyAddress, "off");
                            }
                        }
                        if (respString == string.Empty) throw new Exception("Not valid socks5 addresses");
                        if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                        {
                            Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                                     dataDictionary["mail"] + " или " + dataDictionary["phone"]);
                            return SitePoster.PostStatus.ERROR;
                        }
                        if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                        {
                            Log.Info(reply + " successfully posted on Motosale");

                            while (
                                !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, "mo-snikers@mail.ru",
                                    "Administr@t0r"))
                                Thread.Sleep(5000);

                            return SitePoster.PostStatus.OK;
                        }
                        Log.Warn(reply + " unsuccessfully posted on Motosale");
                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(string.Format("{0} {1} unsuccessfully posted on Motosale",
                                ManufactureXmlWorker.GetItemSiteIdUsingPlant("m", data.DataDictionary["model_zap"]),
                                data.DataDictionary["type"]), ex);
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

                        //xNet
                        string respString;
                        var proxyAddress = string.Empty;
                        while (true)
                        {
                            try
                            {
                                respString = string.Empty;
                                // proxyAddress = ProxyAddressWorker.GetValidProxyAddress("equip");
                                if (proxyAddress == null) break;
                                respString = Response.GetResponseString(cookieContainer, dataDictionary, fileDictionary,
                                    url, proxyAddress);
                                break;
                            }
                            catch
                            {
                                ProxyXmlWorker.ChangeServerStatus(proxyAddress, "off");
                            }
                        }
                        if (respString == string.Empty) throw new Exception("Not valid socks5 addresses");
                        if (respString.Contains("Вы исчерпали дневной лимит подачи объявлений"))
                        {
                            Log.Warn(reply + " unsuccessfully posted on Motosale || дневной лимит для " +
                                     dataDictionary["mail"] + " или " + dataDictionary["phone"]);
                            return SitePoster.PostStatus.ERROR;
                        }
                        if (respString.Contains("На указанный вами E-mail отправлено письмо"))
                        {
                            Log.Info(reply + " successfully posted on Motosale");

                            while (
                                !PostConfirm.ConfirmAdv("pop.mail.ru", 995, true, "mo-snikers@mail.ru",
                                    "Administr@t0r"))
                                Thread.Sleep(5000);

                            return SitePoster.PostStatus.OK;
                        }
                        Log.Warn(reply + " unsuccessfully posted on Motosale");
                        return SitePoster.PostStatus.ERROR;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger()
                            .Error(
                                string.Format("{0} {1} unsuccessfully posted on Motosale", data.DataDictionary["brand"],
                                    data.DataDictionary["type"]), ex);
                        return SitePoster.PostStatus.ERROR;
                    }
                });
        }
    }
}