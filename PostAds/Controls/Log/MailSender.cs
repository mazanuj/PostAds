using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using NLog;

namespace Motorcycle.Controls.Log
{
    internal static class MailSender
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        internal static void SendEmail(string to, string from, string subject, string boby)
        {
            try
            {
                var client = new SmtpClient("smtp.yandex.ru", 25)
                {
                    Credentials = new NetworkCredential("postads", "postads2407"),
                    EnableSsl = false
                };

                var message = new MailMessage(from, to, subject, boby);

                var file = string.Format(@"{0}logs\current.log", AppDomain.CurrentDomain.BaseDirectory);
                if (File.Exists(file))
                {
                    // Create  the file attachment for this e-mail message.
                    var data = new Attachment(file, MediaTypeNames.Application.Octet);
                    // Add the file attachment to this e-mail message.
                    message.Attachments.Add(data);
                }
                client.SendCompleted += SendCompletedCallback;

                //client.Send(message);
                client.SendAsync(message, "Sending log");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, null, null);
            }
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            Log.Info("Message send complete");
        }
    }
}