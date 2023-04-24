using Application.Interfaces;
using Domain;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services
{
    public class VisitService : IVisit
    {
        public bool IsVisitAbleToCreate(List<Wizytum> visits)
        {
            int result = visits.Where(x => ((VisitStatus)Enum.Parse(typeof(VisitStatus), x.Status, true)).Equals(VisitStatus.Zaplanowana)).Count();
            return result <= GlobalValues.MAX_UMOWIONYCH_WIZYT;
        }

        public bool IsWizytaAbleToCancel(DateTime visitDate)
        {
            return visitDate <= DateTime.Now.AddHours(-GlobalValues.GODZINY_DO_ANULOWANIA_WIZYTY_BEZ_KONSEKWENCJI);
        }

        public (DateTime, DateTime) GetVisitDates(List<Harmonogram> harmonograms)
        {
            var result = harmonograms.OrderBy(x => x.DataRozpoczecia).ToList();
            var start = result.First().DataRozpoczecia;
            var end = result.Last().DataZakonczenia;
            return (start, end);
        }

        public bool IsVisitAbleToReschedule(List<Harmonogram> schedules, DateTime startDate)
        {
            schedules = schedules.OrderBy(x => x.DataRozpoczecia).ToList();
            var currentStartDate = startDate;
            for (int i = 0; i < schedules.Count; i++)
            {
                if (!currentStartDate.Equals(schedules.ElementAt(i).DataRozpoczecia))
                {
                    return false;
                }

                currentStartDate = schedules.ElementAt(i).DataZakonczenia;
            }
            return true;
        }

        public decimal GetVisitPrice(List<Usluga> servicesList)
        {
            if(servicesList.Count == 0)
            {
                return 50;
            }

            return servicesList.Sum(x => x.Cena);
        }
    }
}