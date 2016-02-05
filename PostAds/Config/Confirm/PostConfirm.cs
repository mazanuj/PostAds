using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MailKit.Net.Pop3;
using MimeKit;
using Motorcycle.XmlWorker;
using NLog;
using xNet.Net;

namespace Motorcycle.Config.Confirm
{
    internal static class PostConfirm
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static bool checker;

        public static bool ConfirmAdv(string username, string password = "")
        {
            if (string.IsNullOrEmpty(password))
                password = PasswordXmlWorker.GetPasswordValue();

            checker = false;
            // The client disconnects from the server when being disposed
            using (var client = new Pop3Client())
            {
                while (true)
                {
                    try
                    {
                        client.Connect("pop.mail.ru", 995, true);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(username, password);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex.Message, ex, "", "");
                    }
                }

                if (client.Count == 0)
                    return checker;

                var headers = client.GetMessageHeaders(0, client.Count());
                for (var i = 0; i < headers.Count; i++)
                {
                    if (!headers[i][HeaderId.Subject].Contains("Сделайте ваше объявление активным")) continue;

                    var message = client.GetMessage(i);
                    var url =
                        Regex.Match(message.Body.ToString(), @"http://www.motosale.com.ua/\?confirm=\w*(?=</a>)")
                            .Value;
                    var respString = new WebClient().DownloadString(url);

                    if (respString.Contains("after_confirm=false"))
                        Log.Warn($"Ads was confirmed EARLIER for {username}", null, null);
                    else if (respString.Contains("after_confirm=true"))
                    {
                        Log.Debug($"Confirmation of {username} success");
                        checker = true;
                    }
                    client.DeleteMessage(i);
                }

                client.Disconnect(true);
                return checker;
            }
        }

        public static async Task ConfirmAllOlxAdv(string username = "slandoecip@mail.ru", string password = "")
        {
            await Task.Factory.StartNew(async () =>
            {
                if (string.IsNullOrEmpty(password))
                    password = PasswordXmlWorker.GetPasswordValue();

                using (var client = new Pop3Client())
                {
                    while (true)
                    {
                        try
                        {
                            client.Connect("pop.mail.ru", 995, true);
                            client.AuthenticationMechanisms.Remove("XOAUTH2");
                            client.Authenticate(username, password);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log.Debug(ex.Message, ex, "", "");
                        }
                    }

                    if (client.Count == 0)
                        return;

                    var headers = client.GetMessageHeaders(0, client.Count());
                    for (var i = 0; i < headers.Count; i++)
                    {
                        if (!headers[i][HeaderId.Subject].Contains("Обновите свои объявления")) continue;

                        var message = client.GetMessage(i);

                        var doc = new HtmlDocument();
                        var body =
                            message.BodyParts.OfType<TextPart>()
                                .FirstOrDefault(x => x.ContentType.IsMimeType("text", "html"));

                        if (body == null)
                            continue;

                        doc.LoadHtml(body.Text);
                        var url =
                            doc.DocumentNode.Descendants("a")
                                .First(
                                    x =>
                                        x.HasAttributes && x.Attributes.Contains("href") &&
                                        x.Attributes["href"].Value.StartsWith(
                                            "https://ssl.olx.ua/obyavlenie/refreshall/?action=refreshall"))
                                .Attributes["href"].Value;

                        var cookies = await OlxAuthorize.GetPhpSesID(username);
                        using (var req = new HttpRequest())
                        {
                            req.Cookies = cookies;
                            var respString = req.Get(url).ToString();

                            //var respString = Encoding.UTF8.GetString(new WebClient().DownloadData(url));

                            if (respString.Contains("Ваши объявления были обновлены и скоро появятся"))
                                Log.Debug("Все ваши объявления на OLX были обновлены");
                            else Log.Warn("Сейчас у вас нет объявлений, которые можно обновить на OLX");
                            client.DeleteMessage(i);
                        }
                    }
                    client.Disconnect(true);
                }
            });
        }
    }
}