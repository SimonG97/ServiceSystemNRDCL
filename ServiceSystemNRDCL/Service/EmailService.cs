using Microsoft.Extensions.Options;
using ServiceSystemNRDCL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Service
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"ServiceSystemNRDCL/EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;
        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        //method to send to confirmation mail.
        public async Task SendEmailConfirmation(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}},Confirm Email id!!!", userEmailOptions.PlaceHolder);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmailOptions.PlaceHolder);
            await SendEmail(userEmailOptions);
        }

        //method to send to forgot password mail.
        public async Task SendForgotPasswordEmail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, reset your password!!!", userEmailOptions.PlaceHolder);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPasswordEmail"), userEmailOptions.PlaceHolder);
            await SendEmail(userEmailOptions);
        }
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            //adding subject,body and from fileds in the mail message class
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };

            //adding all the email addresses from User email Options
            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }
            //getting the credentials from smtp model
            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

            //setting up smtp client from smtp model
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential

            };
            mail.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mail);

        }
        //method to get email body from the html file
        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        //method to set up the placeholders in the html file
        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs) {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null) {
                foreach (var placeholder in keyValuePairs) {
                    if (text.Contains(placeholder.Key)) {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }
            return text;
        }
    }
}
