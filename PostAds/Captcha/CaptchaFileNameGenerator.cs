using System;
using System.IO;
using System.Threading;

namespace Motorcycle.Captcha
{
    internal static class CaptchaFileNameGenerator
    {
        private static int fileCounter;
        private static readonly object locker= new Object() ;

        public static string GetFileName()
        {
            lock (locker)
            {
                if (!Directory.Exists("Captcha"))
                    Directory.CreateDirectory("Captcha");
            }
            
            return string.Format(@"Captcha\captcha{0}.jpg", Interlocked.Increment(ref fileCounter));
        }
    }
}