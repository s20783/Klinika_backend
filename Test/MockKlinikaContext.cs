using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    public class MockKlinikaContext
    {
        public static Mock<IKlinikaContext> GetMockDbContext()
        {
            var myDbMoq = new Mock<IKlinikaContext>();
            myDbMoq.Setup(p => p.Osobas).Returns(GetQueryableMockDbSet(MockData.GetOsobaList()));
            myDbMoq.Setup(p => p.Klients).Returns(GetQueryableMockDbSet(MockData.GetKlientList()));
            myDbMoq.Setup(p => p.KlientZnizkas).Returns(GetQueryableMockDbSet(MockData.GetKlientZnizkaList()));
            myDbMoq.Setup(p => p.Pacjents).Returns(GetQueryableMockDbSet(MockData.GetPacjentList()));
            myDbMoq.Setup(p => p.Weterynarzs).Returns(GetQueryableMockDbSet(MockData.GetWeterynarzList()));
            myDbMoq.Setup(p => p.GodzinyPracies).Returns(GetQueryableMockDbSet(MockData.GetGodzinyPracyList()));
            myDbMoq.Setup(p => p.WeterynarzSpecjalizacjas).Returns(GetQueryableMockDbSet(MockData.GetWeterynarzSpecjalizacjaList()));
            myDbMoq.Setup(p => p.Specjalizacjas).Returns(GetQueryableMockDbSet(MockData.GetSpecjalizacjaList()));
            myDbMoq.Setup(p => p.Leks).Returns(GetQueryableMockDbSet(MockData.GetLekList()));
            myDbMoq.Setup(p => p.LekWMagazynies).Returns(GetQueryableMockDbSet(MockData.GetLekWMagazynieList()));
            myDbMoq.Setup(p => p.Chorobas).Returns(GetQueryableMockDbSet(MockData.GetChorobaList()));
            myDbMoq.Setup(p => p.ChorobaLeks).Returns(GetQueryableMockDbSet(MockData.GetChorobaLekList()));
            myDbMoq.Setup(p => p.Szczepionkas).Returns(GetQueryableMockDbSet(MockData.GetSzczepionkaList()));
            myDbMoq.Setup(p => p.Szczepienies).Returns(GetQueryableMockDbSet(MockData.GetSzczepienieList()));
            myDbMoq.Setup(p => p.Uslugas).Returns(GetQueryableMockDbSet(MockData.GetUslugaList()));
            myDbMoq.Setup(p => p.WizytaUslugas).Returns(GetQueryableMockDbSet(MockData.GetWizytaUslugaList()));
            myDbMoq.Setup(p => p.Znizkas).Returns(GetQueryableMockDbSet(MockData.GetZnizkaList()));
            myDbMoq.Setup(p => p.Harmonograms).Returns(GetQueryableMockDbSet(MockData.GetHarmonogramList()));
            myDbMoq.Setup(p => p.Wizyta).Returns(GetQueryableMockDbSet(MockData.GetWizytaList()));
            myDbMoq.Setup(p => p.Urlops).Returns(GetQueryableMockDbSet(MockData.GetUrlopList()));
            myDbMoq.Setup(p => p.WizytaChorobas).Returns(GetQueryableMockDbSet(MockData.GetWizytaChorobaList()));
            myDbMoq.Setup(p => p.Recepta).Returns(GetQueryableMockDbSet(MockData.GetReceptaList()));
            myDbMoq.Setup(p => p.ReceptaLeks).Returns(GetQueryableMockDbSet(MockData.GetReceptaLekList()));
            myDbMoq.Setup(p => p.WizytaLeks).Returns(GetQueryableMockDbSet(MockData.GetWizytaLekList()));

            return myDbMoq;
        }

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>((entity) => sourceList.Remove(entity));

            return dbSet.Object;
        }
    }
}