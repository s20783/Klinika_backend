using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IHarmonogram
    {
        int HarmonogramCount(GodzinyPracy godziny);

        Task DeleteHarmonograms(List<Harmonogram> harmonograms, IKlinikaContext context);

        void CreateWeterynarzHarmonograms(IKlinikaContext context, DateTime date, int id);
    }
}