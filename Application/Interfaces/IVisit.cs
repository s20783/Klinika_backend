using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IVisit
    {
        bool IsVisitAbleToCreate(List<Wizytum> wizytaList);

        bool IsWizytaAbleToCancel(DateTime wizytaDate);

        (DateTime, DateTime) GetVisitDates(List<Harmonogram> harmonograms);

        bool IsVisitAbleToReschedule(List<Harmonogram> harmonograms, DateTime startDate);

        decimal GetVisitPrice(List<Usluga> uslugaList);
    }
}