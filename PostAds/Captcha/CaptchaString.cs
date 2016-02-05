using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Motorcycle.Utils;
using NLog;
using xNet.Net;

namespace Motorcycle.Captcha
{
	internal static class CaptchaString
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

	    public static async Task<string> GetBalance(string agKey)
	    {
	        return await Task.Run(() =>
	        {
	            var answ = string.Empty;
	            try
	            {
	                var req = new HttpRequest();
	                answ = req.Get($"http://antigate.com/res.php?key={agKey}&action=getbalance").ToString();
	                return answ;
	            }
	            catch (Exception)
	            {
	            }
	            return answ;
	        });
	    }

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
	            else if (check.StartsWith("ERROR"))
	            {
	                Informer.RaiseOnCaptchaStatusChangedEvent(false);
	                return "ZERO";
	            }
	        } while ((check == "ERROR_NO_SLOT_AVAILABLE"));
	        Informer.RaiseOnCaptchaStatusChangedEvent(true);
	        return check;
	    }

	    internal static string GetCaptchaImage(string url)
		{
			var fileName = CaptchaFileNameGenerator.GetFileName();

			if (File.Exists(fileName))
				File.Delete(fileName);
			var wc = new WebClient();

			wc.DownloadFile(url, fileName);

			return fileName;
		}
	}
}