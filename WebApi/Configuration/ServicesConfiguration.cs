﻿using System.Text.Json;
using Application;
using Application.Common.Utilities;
using Application.DTOs;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration, string servicesBusConnection)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddUseCases();
            return services;
        }

        public static IConfigurationBuilder AddJsonProvider(this IConfigurationBuilder configuration)
        {
            configuration.AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);
            return configuration;
        }
    }
}