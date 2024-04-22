﻿using Microsoft.Extensions.DependencyInjection;

namespace Sellintegro.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));

            return services;
        }
    }
}