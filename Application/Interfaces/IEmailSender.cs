using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendPasswordEmail(string to, string content);

        Task SendCreatedVisitEmail(string to, DateTime data, string vet);

        Task SendCancelledVisitEmail(string to, DateTime data);

        Task SendCreateAccountEmail(string to);

        Task SendReminderEmail(string to, DateTime data, string vet);

        Task SendVaccinationEmail(string to, DateTime data, string patient);
    }
}