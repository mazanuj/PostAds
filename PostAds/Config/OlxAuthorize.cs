using System.Threading.Tasks;
using Motorcycle.XmlWorker;
using xNet.Net;

namespace Motorcycle.Config
{
    internal static class OlxAuthorize
    {
        internal static async Task<CookieDictionary> GetPhpSesID(string login = "slandoecip@mail.ru", string password = "",
            string domain = @"https://ssl.olx.ua/account/?ref%5B0%5D%5Baction%5D=myaccount&ref%5B0%5D%5Bmethod%5D=index")
        {
            if (string.IsNullOrEmpty(password))
                password = PasswordXmlWorker.GetPasswordValue();

            return await Task<CookieDictionary>.Factory.StartNew(() =>
            {
                var cookies = new CookieDictionary();

                var request = new HttpRequest {Cookies = cookies};
                request.AddField("login[email]", login);
                request.AddField("login[password]", password);

                request.Post(domain).None();

                return cookies;
            });
        }
    }
}