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
            cookieContainer.Add(cookieCollection);

            return cookieContainer;
        }
    }
}