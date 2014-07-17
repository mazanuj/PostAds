using System.IO;
using System.Net;
using System.Text;
using System.Windows.Media.Imaging;

namespace Motorcycle.HTTP
{
    internal static class Response
    {
        internal static string GetResponseString(HttpWebRequest request)
        {
            using (var response = (HttpWebResponse) request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return null;
                using (var reader = new StreamReader(responseStream, Encoding.GetEncoding("Windows-1251")))
                    return reader.ReadToEnd();
            }
        }

        internal static string GetResponseString(HttpWebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return null;
                using (var reader = new StreamReader(responseStream, Encoding.GetEncoding("Windows-1251")))
                    return reader.ReadToEnd();
            }
        }

        internal static CookieCollection GetResponseCookies(HttpWebRequest request)
        {
            using (var response = (HttpWebResponse) request.GetResponse())
                return response.Cookies;
        }

        internal static HttpWebResponse GetResponse(HttpWebRequest request)
        {
            return (HttpWebResponse) request.GetResponse();
        }

        internal static void GetImageFromResponse(HttpWebRequest request)
        {
            using (var response = (HttpWebResponse) request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                if (File.Exists("captcha.jpg"))
                    File.Delete("captcha.jpg");
                
                if (stream == null)
                    return;

                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);

                var jpegEncoder = new JpegBitmapEncoder();
                jpegEncoder.Frames.Add(BitmapFrame.Create(memoryStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad));
                using (var fs = new FileStream("captcha.jpg", FileMode.Create, FileAccess.Write))
                    jpegEncoder.Save(fs);
            }
        }
    }
}