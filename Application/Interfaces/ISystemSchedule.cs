using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISystemSchedule
    {
        void SendReminderEmail();

        Task DeleteWizytaSystemAsync();

        void SendVaccinationEmail();

        Task CreateSchedulesBySystem();
    }
}