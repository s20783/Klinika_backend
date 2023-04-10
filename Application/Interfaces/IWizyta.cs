using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IWizyta
    {
        bool IsWizytaAbleToCreate(List<Wizytum> wizytaList);

        bool IsWizytaAbleToCancel(DateTime wizytaDate);

        (DateTime, DateTime) GetWizytaDates(List<Harmonogram> harmonograms);

        bool IsWizytaAbleToReschedule(List<Harmonogram> harmonograms, DateTime startDate);

        decimal GetWizytaCena(List<Usluga> uslugaList);
    }
}