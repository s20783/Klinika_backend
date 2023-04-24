using Application.Interfaces;
using Domain;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ScheduleService : ISchedule
    {
        private readonly IEmailSender _emailSender;
        public ScheduleService(IEmailSender sender)
        {
            _emailSender = sender;
        }

        public int ScheduleCount(GodzinyPracy godziny)
        {
            var result = godziny.GodzinaZakonczenia.Subtract(godziny.GodzinaRozpoczecia);

            if(result.TotalMinutes % 30 != 0)
            {
                throw new Exception();
            }

            return (int)(result.TotalMinutes/30);
        }

        public void CreateVetSchedules(IKlinikaContext context, DateTime date, int id)
        {
            int dzienRequest = (int)date.DayOfWeek;
            if(context.Urlops.Where(x => x.Dzien.Date.Equals(date.Date) && x.IdOsoba.Equals(id)).Any())
            {
                return;
            }
            var godzinyPracy = context.GodzinyPracies.Where(x => x.DzienTygodnia == dzienRequest && x.IdOsoba.Equals(id)).FirstOrDefault();
            if(godzinyPracy != null)
            {
                var count = ScheduleCount(godzinyPracy);

                for (int i = 0; i < count; i++)
                {
                    var s = godzinyPracy.GodzinaRozpoczecia;
                    context.Harmonograms.Add(new Harmonogram
                    {
                        IdWizyta = null,
                        WeterynarzIdOsoba = id,
                        DataRozpoczecia = date.Date + TimeSpan.FromMinutes((double)s.TotalMinutes + (i * GlobalValues.DLUGOSC_WIZYTY)),
                        DataZakonczenia = date.Date + TimeSpan.FromMinutes((double)s.TotalMinutes + (i * GlobalValues.DLUGOSC_WIZYTY) + GlobalValues.DLUGOSC_WIZYTY)
                    });
                }
            }
        }

        public async Task DeleteSchedules(List<Harmonogram> harmonograms, IKlinikaContext context)
        {
            var grouped = harmonograms.Where(x => x.IdWizyta.HasValue).GroupBy(x => x.IdWizyta).ToList();
            var wizyty = context.Wizyta.Where(x => grouped.Select(x => x.Key.Value).Contains(x.IdWizyta)).ToList();

            foreach (var g in wizyty)
            {
                var startDate = harmonograms.Where(x => x.IdWizyta == g.IdWizyta).OrderBy(x => x.DataRozpoczecia).First().DataRozpoczecia;
                var email = context.Osobas.Where(x => x.IdOsoba == g.IdOsoba).First().Email;
                g.Status = VisitStatus.AnulowanaKlinika.ToString();
                await _emailSender.SendCancelledVisitEmail(email, startDate);
            }

            foreach (Harmonogram h in harmonograms)
            {
                context.Harmonograms.Remove(h);
            }
        }
    }
}