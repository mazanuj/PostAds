using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using MailKit.Net.Pop3;
using MimeKit;
using NLog;

namespace Motorcycle.Config.Confirm
{
    internal static class PostConfirm
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static bool checker;

        public static bool ConfirmAdv(string username, string password = "1358888t")
        {
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
                        Log.Debug(ex.Message, ex);
                    }
                }

                var headers = client.GetMessageHeaders(0, client.Count());
                for (var i = 0; i < headers.Count; i++)
                {
                    if (headers[i][HeaderId.Subject] != "Активируйте свое объявление.") continue;

                    var message = client.GetMessage(i);
                    var url =
                        Regex.Match(message.Body.ToString(), @"http://www.motosale.com.ua/\?confirm=\w*(?=</a>)")
                            .Value;
                    var respString = new WebClient().DownloadString(url);

                    if (respString.Contains("after_confirm=false"))
                        Log.Warn(string.Format("Ads was confirmed for {0}", username), null, null);
                    else if (respString.Contains("after_confirm=true"))
                    {
                        Log.Debug(string.Format("Confirmation of {0} success", username));
                        checker = true;
                    }
                    client.DeleteMessage(i);
                }

                client.Disconnect(true);

                return checker;
            }
        }
    }
}