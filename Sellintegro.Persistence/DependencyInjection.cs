using Microsoft.Extensions.DependencyInjection;
using Sellintegro.Domain.Entities.Tasks.Repositories;
using Sellintegro.Persistence.Repositories;

namespace Sellintegro.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection service)
    {
        service.AddSingleton<ITaskRepository, TaskRepository>();

        return service;
    }
}