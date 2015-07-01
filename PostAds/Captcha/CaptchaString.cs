using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Motorcycle.Utils;
using NLog;

namespace Motorcycle.Captcha
{
	internal static class CaptchaString
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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
				if (check == "ERROR_ZERO_BALANCE")
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