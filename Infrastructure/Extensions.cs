using Application.DTO.Responses;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Services;
using Infrastructure.Services.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IToken, TokenService>();
            services.AddScoped<IPassword, PasswordService>();
            services.AddScoped<IHash, HashService>();
            services.AddScoped<IVisit, VisitService>();
            services.AddScoped<ILogin, LoginService>();
            services.AddScoped<ISchedule, Services.ScheduleService>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<ICache<GetPatientListResponse>, PatientCache>();
            services.AddScoped<ICache<GetDiseaseResponse>, DiseaseCache>();
            services.AddScoped<ICache<GetSpecializationResponse>, SpecializationCache>();
            services.AddScoped<ICache<GetClientListResponse>, ClientCache>();
            services.AddScoped<ICache<GetServiceResponse>, ServiceCache>();
            services.AddScoped<ICache<GetVetListResponse>, vetCache>();

            services.AddTransient<ISystemSchedule, SystemScheduleService>();

            services.AddScoped<IKlinikaContext, KlinikaContext>();

            return services;
        }
    }
}