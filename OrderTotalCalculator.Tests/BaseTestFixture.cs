using Microsoft.Extensions.DependencyInjection;
using OrderTotalCalculator.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTotalCalculator.Tests
{
	public class BaseTestFixture : IClassFixture<BaseTestFixture>
	{
		public IServiceProvider ServiceProvider { get; private set; }

		public BaseTestFixture() 
		{
			var services = new ServiceCollection();

			services.AddApplication();

			this.ServiceProvider = services.BuildServiceProvider();
		}
	}
}
