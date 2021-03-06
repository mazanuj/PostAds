﻿using System.IO;

namespace Motorcycle.POST
{
    internal static class MultiFormData
    {
        public static string GetMultiFormData(string key, string value, string boundary)
        {
            var output = "--" + boundary + "\r\n";
            output += "Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n";
            output += value + "\r\n";
            return output;
        }

        public static string GetMultiFormDataFile(string key, string value, string fileName, string fileType,
            string boundary)
        {
            if (File.Exists(fileName))
                fileName = Path.GetFileName(fileName);

            var output = "--" + boundary + "\r\n";
            output += "Content-Disposition: form-data; name=\"" + key + "\"; filename=\"" + fileName + "\"\r\n";
            output += "Content-Type: " + fileType + " \r\n\r\n";
            output += value + "\r\n";
            return output;
        }
    }
}