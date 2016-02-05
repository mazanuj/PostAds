using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using NLog;

namespace Motorcycle.POST
{
    internal static class PostMultiString
    {
        internal static string WriteMultipartForm(string boundary, Dictionary<string, string> dataDictionary,
            Dictionary<string, string> fileDictionary)
        {
            var sPostMultiString = string.Empty;
            if (dataDictionary != null)
                sPostMultiString = dataDictionary.Aggregate(sPostMultiString,
                    (current, pair) => current + MultiFormData.GetMultiFormData(pair.Key, pair.Value, boundary));

            if (fileDictionary != null)
                sPostMultiString = fileDictionary.Aggregate(sPostMultiString,
                    (current, pair) =>
                        current +
                        MultiFormData.GetMultiFormDataFile(pair.Key, GetStringFromFile(pair.Value), pair.Value,
                            "image/jpeg", boundary));

            sPostMultiString += "--" + boundary + "--\r\n\r\n";
            return sPostMultiString;
        }

        private static string GetStringFromFile(string filePath)
        {
            if (filePath == string.Empty)
                return string.Empty;

            var sFileContent = string.Empty;
            var fileLength = new FileInfo(filePath).Length;

            if (fileLength >= 614400)
            {
                var image = Image.FromFile(filePath);
                var curW = image.Width;
                var curH = image.Height;

                if (curW > 1280)
                    curW = 1330;

                while (true)
                {
                    curW -= 50;
                    var img = ResizeOrigImg(image, ref curW, ref curH);

                    using (var ms = new MemoryStream((int) fileLength))
                    {
                        img.Save(ms, ImageFormat.Jpeg);
                        if (ms.Length >= 614400) continue;

                        try
                        {
                            var buffer = new byte[4096];
                            ms.Position = 0;
                            while ((ms.Read(buffer, 0, 4096)) != 0)
                                sFileContent += Encoding.Default.GetString(buffer);
                            ms.Close();
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetCurrentClassLogger().Error(ex.Message, ex, "", "");
                        }

                        break;
                    }
                }
            }
            else
            {
                var fStream = File.OpenRead(filePath);
                try
                {
                    var buffer = new byte[4096];
                    while ((fStream.Read(buffer, 0, 4096)) != 0)
                        sFileContent += Encoding.Default.GetString(buffer);
                    fStream.Close();
                }
                catch
                {
                }
            }
            return sFileContent;
        }

        private static Image ResizeOrigImg(Image image, ref int nWidth, ref int nHeight)
        {
            int newWidth, newHeight;
            var coefH = (double) nHeight/image.Height;
            var coefW = (double) nWidth/image.Width;
            if (coefW >= coefH)
            {
                newHeight = (int) (image.Height*coefH);
                newWidth = (int) (image.Width*coefH);
            }
            else
            {
                newHeight = (int) (image.Height*coefW);
                newWidth = (int) (image.Width*coefW);
            }

            Image result = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(result))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(image, 0, 0, newWidth, newHeight);
                g.Dispose();
            }
            return result;
        }
    }
}