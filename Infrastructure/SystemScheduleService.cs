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
    public class SystemScheduleService : ISystemSchedule
    {
        private readonly ILogger<SystemScheduleService> _logger;
        private readonly IKlinikaContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ISchedule _schedule;
        public SystemScheduleService(ILogger<SystemScheduleService> log, IKlinikaContext klinikaContext, IEmailSender sender, ISchedule schedule)
        {
            _logger = log;
            _context = klinikaContext;
            _emailSender = sender;
            _schedule = schedule;
        }

        public void SendReminderEmail()
        {
            var helperList = from x in _context.Wizyta
                             join y in _context.Harmonograms on x.IdWizyta equals y.IdWizyta
                             join k in _context.Osobas on x.IdOsoba equals k.IdOsoba
                             where x.Status == VisitStatus.Zaplanowana.ToString()
                             group x by new { x.IdWizyta, x.IdOsoba, y.WeterynarzIdOsoba, k.Email }
                              into g
                             select new
                             {
                                 Email = g.Key.Email,
                                 Weterynarz = g.Key.WeterynarzIdOsoba != 0 ? _context.Osobas.Where(i => i.IdOsoba.Equals(g.Key.WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First() : null,
                                 Data = _context.Harmonograms.Where(x => x.IdWizyta.Equals(g.Key.IdWizyta)).Min(x => x.DataRozpoczecia)
                             };

            foreach (var a in helperList)
            {
                if (a.Data.Date == DateTime.Now.Date.AddDays(1))
                {
                    _emailSender.SendReminderEmail(a.Email, a.Data, a.Weterynarz);
                    _logger.LogInformation("Email sent to " + a.Email + " at: " + DateTime.Now.ToString());
                }
            }
        }

        public async Task DeleteWizytaSystemAsync()
        {
            var wizytaList = _context.Wizyta
               .Where(x => x.Status.Equals(VisitStatus.AnulowanaKlient.ToString()) || x.Status.Equals(VisitStatus.AnulowanaKlinika.ToString()))
               .ToList();

            _context.Wizyta.RemoveRange(wizytaList);

            var result = await _context.SaveChangesAsync(new CancellationToken());
            _logger.LogInformation("Records deleted: " + result);
        }

        public void SendVaccinationEmail()
        {
            var helperList = (from x in _context.Szczepienies
                             join z in _context.Szczepionkas on x.IdLek equals z.IdLek
                             join y in _context.Pacjents on x.IdPacjent equals y.IdPacjent
                             join k in _context.Klients on y.IdOsoba equals k.IdOsoba
                             join o in _context.Osobas on k.IdOsoba equals o.IdOsoba
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
                        _emailSender.SendVaccinationEmail(a.Email, (DateTime)dataWaznosci, a.Pacjent);
                        _logger.LogInformation("Email sent to " + a.Email + " at: " + DateTime.Now.ToString());
                    }
                }
            }
        }

        public async Task CreateSchedulesBySystem()
        {
            var endOfCurrentHarmonograms = DateTime.Now.Date;
            if (_context.Harmonograms.Any())
            {
                if(_context.Harmonograms.Max(x => x.DataZakonczenia.Date) > endOfCurrentHarmonograms)
                {
                    endOfCurrentHarmonograms = _context.Harmonograms.Max(x => x.DataZakonczenia.Date);
                }
            }

            for (int i = 1; i <= 7; i++)
            {
                foreach(Weterynarz w in _context.Weterynarzs)
                {
                    _schedule.CreateVetSchedules(_context, endOfCurrentHarmonograms.AddDays(i), w.IdOsoba);
                }
            }

            var result = await _context.SaveChangesAsync(new CancellationToken());
            _logger.LogInformation("Added harmonograms: " + result);
        }
    }
}