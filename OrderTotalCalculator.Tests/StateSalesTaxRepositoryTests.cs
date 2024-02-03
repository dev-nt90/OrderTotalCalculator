using Microsoft.Extensions.DependencyInjection;
using OrderTotalCalculator.Application.Repositories;

namespace OrderTotalCalculator.Tests
{
	/// <summary>
	/// Test class for the <see cref="StateSalesTaxRepository"/> with common testing setup provided by the <see cref="BaseTestFixture"/>.
	/// </summary>
	public class StateSalesTaxRepositoryTests : IClassFixture<BaseTestFixture>
	{
		private readonly IStateSalesTaxRepository stateSalesTaxRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="StateSalesTaxRepositoryTests"/> class with the specified <see cref="BaseTestFixture"/>.
		/// </summary>
		/// <param name="fixture">The base test fixture providing common testing setup.</param>
		public StateSalesTaxRepositoryTests(BaseTestFixture fixture)
		{
			stateSalesTaxRepository = fixture.ServiceProvider.GetRequiredService<IStateSalesTaxRepository>();
		}

		/// <summary>
		/// Test method to assert that the sales tax rate can be retrieved successfully by state code.
		/// </summary>
		/// <param name="stateCode">The state code for which to retrieve the sales tax rate.</param>
		[Theory]
		[InlineData("GA")]
		[InlineData("CA")]
		[InlineData("NY")]
		[InlineData("VT")]
		public void AssertCanFindSalesTaxByStateCode(string stateCode)
		{
			var salesTax = this.stateSalesTaxRepository.GetSalesTaxRate(stateCode);

			Assert.True(salesTax >= 0);
		}

		/// <summary>
		/// Test method to assert that attempting to retrieve the sales tax rate for an invalid state code throws 
		/// a <see cref="KeyNotFoundException"/>.
		/// </summary>
		/// <remarks>
		/// TODO: When migrating state sales tax data to database, this test must be removed or changed
		/// </remarks>
		[Fact]
		public void AssertInvalidStateCodeThrows()
		{
			var garbage = Guid.NewGuid();

			Assert.Throws<KeyNotFoundException>(() => this.stateSalesTaxRepository.GetSalesTaxRate(garbage.ToString()));
		}

		/// <summary>
		/// Test method to assert that the repository can successfully retrieve supported state codes.
		/// </summary>
		[Fact]
		public void AssertCanFindSupportedStateCodes()
		{
			var stateCodes = this.stateSalesTaxRepository.GetSupportedStates();
			Assert.NotEmpty(stateCodes);
		}
	}
}
