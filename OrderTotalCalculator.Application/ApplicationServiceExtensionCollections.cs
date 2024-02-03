using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderTotalCalculator.Application.Repositories;
using OrderTotalCalculator.Application.Services;

namespace OrderTotalCalculator.Application
{
	/// <summary>
	/// Extension methods for configuring application-related services in the <see cref="IServiceCollection"/>.
	/// </summary>
	public static class ApplicationServiceExtensionCollections
	{
		/// <summary>
		/// Adds application-specific services to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
		/// <returns>The modified <see cref="IServiceCollection"/>.</returns>
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddSingleton<IOrderTotalCalculatorService, OrderTotalCalculatorService>();
			services.AddSingleton<IStateSalesTaxRepository, StateSalesTaxRepository>();

			// find all validators for the "application" assembly without adding them individually
			// pretty neat right?
			services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

			return services;
		}
	}
}
