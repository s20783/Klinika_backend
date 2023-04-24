using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISchedule
    {
        int ScheduleCount(GodzinyPracy godziny);

        Task DeleteSchedules(List<Harmonogram> schedules, IKlinikaContext context);

        void CreateVetSchedules(IKlinikaContext context, DateTime date, int id);
    }
}