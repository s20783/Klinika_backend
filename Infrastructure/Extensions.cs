using Application.DTO.Responses;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Models;
using Infrastructure.Services;
using Infrastructure.Services.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITokenRepository, TokenService>();
            services.AddScoped<IPasswordRepository, PasswordService>();
            services.AddScoped<IHash, HashService>();
            services.AddScoped<IWizytaRepository, WizytaService>();
            services.AddScoped<ILoginRepository, LoginService>();
            services.AddScoped<IHarmonogramRepository, HarmonogramService>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<ICache<GetPacjentListResponse>, PacjentCache>();
            services.AddScoped<ICache<GetChorobaResponse>, ChorobaCache>();
            services.AddScoped<ICache<GetSpecjalizacjaResponse>, SpecjalizacjaCache>();
            services.AddScoped<ICache<GetKlientListResponse>, KlientCache>();
            services.AddScoped<ICache<GetUslugaResponse>, UslugaCache>();
            services.AddScoped<ICache<GetWeterynarzListResponse>, WeterynarzCache>();

            services.AddTransient<ISchedule, ScheduleService>();

            services.AddScoped<IKlinikaContext, KlinikaContext>();

            return services;
        }
    }
}