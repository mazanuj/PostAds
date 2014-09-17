using Motorcycle.HTTP;
using OpenPop.Pop3;

namespace Motorcycle.Config.Confirm
{
    static class PostConfirm
    {
        public static void GetMails(string hostname, int port, bool useSsl, string username, string password)
        {
            // The client disconnects from the server when being disposed
            using (var client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

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
                    if (!@from.HasValidMailAddress || !@from.Address.Equals("admin@motosale.com.ua") ||
                        !subject.Contains("Активируйте свое объявление")) continue;

                    // Download the full message
                    var message = client.GetMessage(i);

                    var html = message.FindFirstHtmlVersion();
                    if (html == null) continue;

                    var text = html.GetBodyAsText();

                    var start = text.IndexOf("<a href='");
                    if (start == -1) continue;

                    start += "<a href='".Length;
                    var stop = text.IndexOf("' target=", start);

                    var url = text.Substring(start, stop - start);
                    var req = Request.GETRequest(url);
                    var resp = Response.GetResponse(req);
                    


                    var respString = Response.GetResponseString(req);

                    //TODO -> LOG

                    client.DeleteMessage(i);
                }
            }
        }
    }
}
