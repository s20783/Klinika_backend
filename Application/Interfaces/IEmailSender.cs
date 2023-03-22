using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendHasloEmail(string to, string content);

        Task SendUmowWizytaEmail(string to, DateTime data, string weterynarz);

        Task SendAnulujWizyteEmail(string to, DateTime data);

        Task SendCreateAccountEmail(string to);

        Task SendPrzypomnienieEmail(string to, DateTime data, string weterynarz);

        Task SendSzczepienieEmail(string to, DateTime data, string pacjent);
    }
}