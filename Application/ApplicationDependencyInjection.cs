﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Helpers.Commands;
using Application.DTOs.Responses;
using Application.Interfaces.Infrastructure.Commands;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IServiceCollection RegisterCommon(this IServiceCollection services)
        {
            services.AddScoped<ICommandEventRepository, CommandEvent>();
            return services;
        }
    }
}
