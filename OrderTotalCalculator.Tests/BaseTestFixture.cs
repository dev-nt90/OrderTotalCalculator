using Microsoft.Extensions.DependencyInjection;
using OrderTotalCalculator.Application;

namespace OrderTotalCalculator.Tests
{
	/// <summary>
	/// Base test fixture class used for setting up common testing resources and configurations.
	/// </summary>
	public class BaseTestFixture : IClassFixture<BaseTestFixture>
	{
		public IServiceProvider ServiceProvider { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseTestFixture"/> class.
		/// </summary>
		public BaseTestFixture()
		{
			var services = new ServiceCollection();

			services.AddApplication();

			this.ServiceProvider = services.BuildServiceProvider();
		}
	}
}
