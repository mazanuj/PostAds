﻿namespace Motorcycle.Sites
{
    using System.Threading;

    class CaptchaFileNameGenerator
    {
        private static int fileCounter;

        public static string GetFileName()
        {
            return string.Format("captcha{0}.jpg", Interlocked.Increment(ref fileCounter));
        }
    }
}
