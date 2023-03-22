using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISchedule
    {
        void SendPrzypomnienieEmail();

        Task DeleteWizytaSystemAsync();

        void SendSzczepienieEmail();

        Task CreateHarmonogramsBySystem();
    }
}