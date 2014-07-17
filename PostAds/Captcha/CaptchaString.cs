using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace Motorcycle.Captcha
{
    internal static class CaptchaString
    {
        internal static string GetCaptchaString(string key, string filePath, string domain)
        {
            string check;
            var dataDictionary = new Dictionary<string, string>
            {
                {"method", "post"},
                {"key", key}
            };

            var fileDictionary = new Dictionary<string, string> {{"file", filePath}};

            do
            {
                check = SolveCaptcha.GetText(dataDictionary, fileDictionary, domain, 5000);
                if (check == "ERROR_NO_SLOT_AVAILABLE")
                    Thread.Sleep(2000);
            } while ((check == "ERROR_NO_SLOT_AVAILABLE"));
            return check;
        }

        internal static void GetCaptchaImage(string url)
        {
            if (File.Exists("captcha.jpg"))
                File.Delete("captcha.jpg");
            var wc = new WebClient();
            wc.DownloadFile(url, "captcha.jpg");
        }
    }
}