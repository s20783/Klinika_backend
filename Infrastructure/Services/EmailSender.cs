using Application.Interfaces;
using Infrastructure;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendSzczepienieEmail(string to, DateTime data, string pacjent)
        {
            var culture = new System.Globalization.CultureInfo("pl-PL");
            var day = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
            string termin = data.ToShortDateString();

            var body = string.Format(
               "<h2>" +
               "Przypominamy o wykonaniu szczepienia" +
               "</h2>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Data ważności szczepienia: " +
               "{0}" +
               "</p>" +
               "Pacjent: " +
               "{1}" +
               "</p>" +
               "</br>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Klinika PetMed" +
               "</p>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "222 444 555" +
               "</p>", termin, pacjent);

            var email = CreateEmail(to, "Przypomnienie o szczepieniu");
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            await Send(email);
        }


        public async Task SendPrzypomnienieEmail(string to, DateTime data, string weterynarz)
        {
            var culture = new System.Globalization.CultureInfo("pl-PL");
            var day = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
            string termin = data.ToShortDateString() + " (" + day + ") " + data.ToShortTimeString();

            var body = string.Format(
               "<h2>" +
               "Przypominamy o wizycie w klinice PetMed" +
               "</h2>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Termin: " +
               "{0}" +
               "</p>" +
               "Weterynarz: " +
               "{1}" +
               "</p>" +
               "</br>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Klinika PetMed" +
               "</p>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "222 444 555" +
               "</p>", termin, weterynarz);

            var email = CreateEmail(to, "Przypomnienie o wizycie");
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            await Send(email);
        }


        public async Task SendHasloEmail(string to, string createdPassword)
        {
            var body = string.Format(
               "<h2>" +
               "Twoje konto w klinice PetMed zostało utworzone" +
               "</h2>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Twoje hasło to: " +
               "</p>" +
               "<p style='font - family: Arial, Helvetica, sans - serif; color: #00B2EE;'>" +
               "{0}" +
               "</p>" +
               "</br>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Klinika PetMed" +
               "</p>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "222 444 555" +
               "</p>", createdPassword);

            var email = CreateEmail(to, "Twoje konto w serwisie PetMed zostało utworzone");
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            await Send(email);
        }

        public async Task SendUmowWizytaEmail(string to, DateTime data, string weterynarz)
        {
            var culture = new System.Globalization.CultureInfo("pl-PL");
            var day = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
            string termin = data.ToShortDateString() + " (" + day + ") " + data.ToShortTimeString();

            var body = string.Format(
               "<h2>" +
               "Potwiedzenie rezerwacji w klinice PetMed" +
               "</h2>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Termin: " +
               "{0}" +
               "</p>" +
               "Weterynarz: " +
               "{1}" +
               "</p>" +
               "</br>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Klinika PetMed" +
               "</p>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "222 444 555" +
               "</p>", termin, weterynarz);

            var email = CreateEmail(to, "Potwierdzenie wizyty");
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            await Send(email);
        }

        public async Task SendAnulujWizyteEmail(string to, DateTime data)
        {
            var culture = new System.Globalization.CultureInfo("pl-PL");
            var day = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
            string termin = data.ToShortDateString() + " (" + day + ") " + data.ToShortTimeString();

            var body = string.Format(
               "<h2>" +
               "Twoje wizyta w klinice PetMed została anulowana" +
               "</h2>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Termin: " +
               "{0}" +
               "</p>" +
               "</br>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Klinika PetMed" +
               "</p>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "222 444 555" +
               "</p>", termin);

            var email = CreateEmail(to, "Anulowanie wizyty");
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            await Send(email);
        }

        public async Task SendCreateAccountEmail(string to)
        {
            var body = string.Format(
               "<h2>" +
               "Twoje konto w klinice PetMed zostało utworzone" +
               "</h2>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Cieszymy się, że z nami jesteś." +
               "</p>" +
               "</br>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Klinika PetMed" +
               "</p>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "222 444 555" +
               "</p>");

            var email = CreateEmail(to, "Konto zostało utworzone");
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            await Send(email);
        }

        private MimeMessage CreateEmail(string to, string subject)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.Add(new MailboxAddress(to));
            emailMessage.Subject = subject;

            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}