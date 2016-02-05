using System.IO;
using System.Threading;

namespace Motorcycle.Captcha
{
	internal static class CaptchaFileNameGenerator
	{
		private static int fileCounter;
		private static readonly object locker = new object();

		public static string GetFileName()
		{
			lock (locker)
			{
				if (!Directory.Exists("Captcha"))
					Directory.CreateDirectory("Captcha");
			}

			return $@"Captcha\captcha{Interlocked.Increment(ref fileCounter)}.jpg";
		}
	}
}