using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Motorcycle.POST
{
    internal static class PostMultiString
    {
        internal static string WriteMultipartForm(string boundary, Dictionary<string, string> dataDictionary, Dictionary<string, string> fileDictionary)
        {
            var sPostMultiString = string.Empty;
            if (dataDictionary != null)
                sPostMultiString = dataDictionary.Aggregate(sPostMultiString, (current, data) => current + MultiFormData.GetMultiFormData(data.Key, data.Value, boundary));            

            if (fileDictionary != null)
                sPostMultiString += fileDictionary.Aggregate(sPostMultiString, (current, file) => current + MultiFormData.GetMultiFormDataFile(file.Key, GetStringFromFile(file.Value), file.Value, "image/jpeg", boundary));

            sPostMultiString += "--" + boundary + "--\r\n\r\n";
            return sPostMultiString;
        }
        private static string GetStringFromFile(string filePath)
        {
            if (filePath == string.Empty)
                return string.Empty;

            var sFileContent = string.Empty;
            Stream fStream = File.OpenRead(filePath);
            try
            {
                var buffer = new byte[4096];
                while ((fStream.Read(buffer, 0, 4096)) != 0)
                    sFileContent += Encoding.Default.GetString(buffer);
                fStream.Close();
            }
            catch (Exception)
            {
            }
            return sFileContent;
        }
    }
}