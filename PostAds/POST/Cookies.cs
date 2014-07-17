using System.Net;
using Motorcycle.HTTP;

namespace Motorcycle.POST
{
    static class Cookies
    {
        internal static CookieContainer GetCookiesContainer(string path)
        {
            var cookieContainer = new CookieContainer();

            var request = Request.GETRequest(path);
            request.CookieContainer = cookieContainer;

            var cookieCollection = Response.GetResponseCookies(request);
            if (path.Contains("motosale"))
            {
                var cookie = new Cookie("b", "b", "/", "www.motosale.com.ua");
                cookieCollection.Add(cookie);
            }
            cookieContainer.Add(cookieCollection);

            return cookieContainer;
        }
    }
}