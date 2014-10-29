using System;
using System.Net;
using System.Text.RegularExpressions;
using Motorcycle.HTTP;
using NLog;
using OpenPop.Pop3;

namespace Motorcycle.Config.Confirm
{
    internal static class PostConfirm
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static bool ConfirmAdv(string hostname, int port, bool useSsl, string username, string password)
        {
            // The client disconnects from the server when being disposed
            using (var client = new Pop3Client())
            {
                while (true)
                {
                    try
                    {
                        // Connect to the server
                        client.Connect(hostname, port, useSsl);

                        // Authenticate ourselves towards the server
                        client.Authenticate(username, password);
                        break;
                    }
                    catch(Exception ex)
                    {
                        Log.Debug(ex.Message, ex);                        
                    }
                }

                // Get the number of messages in the inbox
                var messageCount = client.GetMessageCount();

                for (var i = 1; i <= messageCount; i++)
                {
                    // We want to check the headers of the message before we download
                    // the full message
                    var headers = client.GetMessageHeaders(i);

                    var from = headers.From;
                    var subject = headers.Subject;

                    // Only want to download message if:
                    //  - is from test@xample.com
                    //  - has subject "Some subject"
                    try
                    {
                        if (!@from.HasValidMailAddress || !@from.Address.Equals("admin@motosale.com.ua") ||
                            !subject.Contains("Активируйте свое объявление"))
                            continue;
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex.Message, ex);
                        continue;
                    }

                    // Download the full message
                    var message = client.GetMessage(i);

                    var html = message.FindFirstHtmlVersion();
                    if (html == null) continue;

                    var url = Regex.Match(html.GetBodyAsText(), @"http://www.motosale.com.ua/\?confirm=\w*(?=</a>)").Value;
                    var respString = new WebClient().DownloadString(url);

                    if (respString.Contains("after_confirm=false"))
                    {
                        Log.Warn(string.Format("Confirmation of {0} failed", username), null, null);
                        continue;
                    }
                    Log.Debug(string.Format("Confirmation of {0} success", username));

                    client.DeleteMessage(i);
                    return true;
                }
                return false;
            }
        }
    }
}