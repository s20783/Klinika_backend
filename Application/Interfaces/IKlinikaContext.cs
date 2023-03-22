using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Application.Interfaces
{
    public interface IKlinikaContext
    {
        public DbSet<Choroba> Chorobas { get; set; }
        public DbSet<ChorobaLek> ChorobaLeks { get; set; }
        public DbSet<GodzinyPracy> GodzinyPracies { get; set; }
        public DbSet<Harmonogram> Harmonograms { get; set; }
        public DbSet<Klient> Klients { get; set; }
        public DbSet<KlientZnizka> KlientZnizkas { get; set; }
        public DbSet<Lek> Leks { get; set; }
        public DbSet<LekWMagazynie> LekWMagazynies { get; set; }
        public DbSet<Osoba> Osobas { get; }
        public DbSet<Pacjent> Pacjents { get; }
        public DbSet<Powiadomienie> Powiadomienies { get; set; }
        public DbSet<ReceptaLek> ReceptaLeks { get; set; }
        public DbSet<Receptum> Recepta { get; set; }
        public DbSet<Specjalizacja> Specjalizacjas { get; set; }
        public DbSet<Szczepienie> Szczepienies { get; set; }
        public DbSet<Szczepionka> Szczepionkas { get; set; }
        public DbSet<Urlop> Urlops { get; set; }
        public DbSet<Usluga> Uslugas { get; set; }
        public DbSet<Weterynarz> Weterynarzs { get; set; }
        public DbSet<WeterynarzSpecjalizacja> WeterynarzSpecjalizacjas { get; set; }
        public DbSet<WizytaLek> WizytaLeks { get; set; }
        public DbSet<WizytaChoroba> WizytaChorobas { get; set; }
        public DbSet<WizytaUsluga> WizytaUslugas { get; set; }
        public DbSet<Wizytum> Wizyta { get; set; }
        public DbSet<Znizka> Znizkas { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
