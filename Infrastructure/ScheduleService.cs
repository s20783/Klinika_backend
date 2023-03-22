using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ScheduleService : ISchedule
    {
        private readonly ILogger<ScheduleService> logger;
        private readonly IKlinikaContext context;
        private readonly IEmailSender emailSender;
        private readonly IHarmonogramRepository harmonogram;
        public ScheduleService(ILogger<ScheduleService> log, IKlinikaContext klinikaContext, IEmailSender sender, IHarmonogramRepository _harmonogram)
        {
            logger = log;
            context = klinikaContext;
            emailSender = sender;
            harmonogram = _harmonogram;
        }

        public void SendPrzypomnienieEmail()
        {
            var helperList = from x in context.Wizyta
                             join y in context.Harmonograms on x.IdWizyta equals y.IdWizyta
                             join k in context.Osobas on x.IdOsoba equals k.IdOsoba
                             where x.Status == WizytaStatus.Zaplanowana.ToString()
                             group x by new { x.IdWizyta, x.IdOsoba, y.WeterynarzIdOsoba, k.Email }
                              into g
                             select new
                             {
                                 Email = g.Key.Email,
                                 Weterynarz = g.Key.WeterynarzIdOsoba != 0 ? context.Osobas.Where(i => i.IdOsoba.Equals(g.Key.WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First() : null,
                                 Data = context.Harmonograms.Where(x => x.IdWizyta.Equals(g.Key.IdWizyta)).Min(x => x.DataRozpoczecia)
                             };

            foreach (var a in helperList)
            {
                if (a.Data.Date == DateTime.Now.Date.AddDays(1))
                {
                    emailSender.SendPrzypomnienieEmail(a.Email, a.Data, a.Weterynarz);
                    logger.LogInformation("Email sent to " + a.Email + " at: " + DateTime.Now.ToString());
                }
            }
        }

        public async Task DeleteWizytaSystemAsync()
        {
            var wizytaList = context.Wizyta
               .Where(x => x.Status.Equals(WizytaStatus.AnulowanaKlient.ToString()) || x.Status.Equals(WizytaStatus.AnulowanaKlinika.ToString()))
               .ToList();

            context.Wizyta.RemoveRange(wizytaList);

            var result = await context.SaveChangesAsync(new CancellationToken());
            logger.LogInformation("Records deleted: " + result);
        }

        public void SendSzczepienieEmail()
        {
            var helperList = (from x in context.Szczepienies
                             join z in context.Szczepionkas on x.IdLek equals z.IdLek
                             join y in context.Pacjents on x.IdPacjent equals y.IdPacjent
                             join k in context.Klients on y.IdOsoba equals k.IdOsoba
                             join o in context.Osobas on k.IdOsoba equals o.IdOsoba
                             select new
                             {
                                 Email = o.Email,
                                 Data = x.Data,
                                 OkresWaznosci = z.OkresWaznosci,
                                 Pacjent = y.Nazwa
                             }).ToList();

            foreach (var a in helperList)
            {
                if(a.OkresWaznosci != null)
                {
                    var dataWaznosci = a.Data.AddTicks((long)a.OkresWaznosci);
                    if(dataWaznosci < DateTime.Now.AddMonths(1) && dataWaznosci > DateTime.Now)
                    {
                        emailSender.SendSzczepienieEmail(a.Email, (DateTime)dataWaznosci, a.Pacjent);
                        logger.LogInformation("Email sent to " + a.Email + " at: " + DateTime.Now.ToString());
                    }
                }
            }
        }

        public async Task CreateHarmonogramsBySystem()
        {
            var endOfCurrentHarmonograms = context.Harmonograms.Max(x => x.DataZakonczenia).Date;
            if(endOfCurrentHarmonograms < DateTime.Now)
            {
                endOfCurrentHarmonograms = DateTime.Now.Date;
            }

            for (int i = 1; i <= 7; i++)
            {
                foreach(Weterynarz w in context.Weterynarzs)
                {
                    harmonogram.CreateWeterynarzHarmonograms(context, endOfCurrentHarmonograms.AddDays(i), w.IdOsoba);
                }
            }

            var result = await context.SaveChangesAsync(new CancellationToken());
            logger.LogInformation("Added harmonograms: " + result);
        }
    }
}