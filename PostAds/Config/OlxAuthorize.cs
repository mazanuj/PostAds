using System.Threading.Tasks;
using xNet.Net;

namespace Motorcycle.Config
{
    internal static class OlxAuthorize
    {
        internal static async Task<CookieDictionary> GetPhpSesID(string login, string pass = "1358888t",
            string domain = @"https://ssl.olx.ua/account/?ref%5B0%5D%5Baction%5D=myaccount&ref%5B0%5D%5Bmethod%5D=index")
        {
            return await Task<CookieDictionary>.Factory.StartNew(() =>
            {
                var cookies = new CookieDictionary();

                var request = new HttpRequest {Cookies = cookies};
                request.AddField("login[email]", "slandoecip@mail.ru");
                request.AddField("login[password]", "1358888t");
                var resp = request.Post(domain);

                //var cookieContainer = new CookieContainer();

                //var captchaDictionary = new Dictionary<string, string>
                //{
                //    {"login[email]", "slandoecip@mail.ru"},
                //    {"login[password]", "1358888t"}
                //};

                //var req = Request.POSTRequest(domain, cookieContainer, captchaDictionary, null, domain);
                //var responseString = Response.GetResponseString(req);


                return cookies;
            });
        }
    }
}