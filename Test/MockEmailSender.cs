using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class MockEmailSender : IEmailSender
    {
        public Task SendCancelledVisitEmail(string to, DateTime data)
        {
            return Task.CompletedTask;
        }

        public Task SendCreateAccountEmail(string to)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordEmail(string to, string content)
        {
            return Task.CompletedTask;
        }

        public Task SendReminderEmail(string to, DateTime data, string weterynarz)
        {
            return Task.CompletedTask;
        }

        public Task SendVaccinationEmail(string to, DateTime data, string pacjent)
        {
            return Task.CompletedTask;
        }

        public Task SendCreatedVisitEmail(string to, DateTime data, string weterynarz)
        {
            return Task.CompletedTask;
        }
    }
}
