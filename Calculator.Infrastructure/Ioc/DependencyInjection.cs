using Calculator.Application.UseCases;
using Calculator.Domain.Interfaces;
using Calculator.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Domain implementations
        services.AddScoped<ICalculator, BasicCalculator>();

        // Application use cases
        services.AddScoped<ICalculateUseCase, CalculateUseCase>();

        return services;
    }
}
