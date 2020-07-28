using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace NETCore.QuartzExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQuartz<JobImplement>(this IServiceCollection services)
            where JobImplement : class, IJob
        {
            services.AddScoped<JobImplement>();
            services.AddScoped<IJobFactory, JobFactoryImpl>();
            services.AddScoped<QuartzSchedulerFactory>();
            return services;
        }

        public static IServiceCollection AddQuartz<JobImplement>(this IServiceCollection services, Action<QuartzOptions> options)
            where JobImplement : class, IJob
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.AddOptions();
            services.Configure(options);
            services.AddScoped<JobImplement>();
            services.AddScoped<IJobFactory, JobFactoryImpl>();
            services.AddScoped<QuartzSchedulerFactory>();

            return services;
        }

        public static IServiceCollection AddQuartz<JobImplement>(this IServiceCollection services, IConfiguration configuration)
            where JobImplement : class, IJob
        {
            services.Configure<QuartzOptions>(configuration);
            services.AddScoped<JobImplement>();
            services.AddScoped<IJobFactory, JobFactoryImpl>();
            services.AddScoped<QuartzSchedulerFactory>();

            return services;
        }
    }
}
