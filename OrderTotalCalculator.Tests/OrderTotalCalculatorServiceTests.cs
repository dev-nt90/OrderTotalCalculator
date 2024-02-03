using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderTotalCalculator.Application.Models;
using OrderTotalCalculator.Application.Services;

namespace OrderTotalCalculator.Tests
{
	/// <summary>
	/// Test class for the <see cref="OrderTotalCalculatorService"/> with common testing setup provided by the <see cref="BaseTestFixture"/>.
	/// </summary>
	public class OrderTotalCalculatorServiceTests : IClassFixture<BaseTestFixture>
	{
		private readonly IOrderTotalCalculatorService calculatorService;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderTotalCalculatorServiceTests"/> class with the specified <see cref="BaseTestFixture"/>.
		/// </summary>
		/// <param name="fixture">The base test fixture providing common testing setup.</param>
		public OrderTotalCalculatorServiceTests(BaseTestFixture fixture)
		{
			calculatorService = fixture.ServiceProvider.GetRequiredService<IOrderTotalCalculatorService>();
		}

		/// <summary>
		/// Test method to assert that applying a 0% discount does not change the total order amount.
		/// </summary>
		[Fact]
		public async void AssertZeroDiscountDoesNotChangeTotal()
		{
			var incomingOrder = new Order
			{
				State = "VT",
				OrderSubtotal = 123.45m,
				PercentDiscount = 0.00m
			};

			var result = await this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken());

			Assert.NotNull(result);
			Assert.Equal(incomingOrder.OrderSubtotal, result.OrderTotal);
		}

		/// <summary>
		/// Test method to assert that applying a 100% discount results in a total order amount of zero.
		/// </summary>
		[Fact]
		public async void Assert100PercentDiscountEmitsZero()
		{
			var incomingOrder = new Order
			{
				State = "VT",
				OrderSubtotal = 123.45m,
				PercentDiscount = 1.00m
			};

			var result = await this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken());

			Assert.NotNull(result);
			Assert.Equal(0m, result.OrderTotal);
			Assert.Equal(incomingOrder.OrderSubtotal, result.TotalDiscountAmount);
		}

		/// <summary>
		/// Test method to assert that <see cref="ValidationException"/> is thrown when a negative subtotal is encountered.
		/// </summary>
		[Fact]
		public async void AssertNegativeSubtotalThrows()
		{
			var incomingOrder = new Order
			{
				State = "VT",
				OrderSubtotal = -123.45m,
				PercentDiscount = 1.00m
			};

			await Assert.ThrowsAsync<ValidationException>(() => this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken()));
		}

		/// <summary>
		/// Test method to assert that a <see cref="ValidationException"/> is thrown when a zero subtotal is encountered.
		/// </summary>
		[Fact]
		public async void AssertZeroSubtotalThrows()
		{
			var incomingOrder = new Order
			{
				State = "VT",
				OrderSubtotal = 0m,
				PercentDiscount = 1.00m
			};

			await Assert.ThrowsAsync<ValidationException>(() => this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken()));
		}

		/// <summary>
		/// Test method to assert that total discount is correctly calculated and applied.
		/// </summary>
		[Theory]
		[InlineData(12.22, 0.5, 6.11, 6.11)]
		[InlineData(13.45, 0.15, 2.02, 11.43)]
		[InlineData(2_315_331.87, 0.17, 393_606.42, 1_921_725.45)] // large case
		[InlineData(0.01, 0.5001, 0.01, 0.00)] // 50.01% discount on 1 cent
		[InlineData(0.01, 0.4999, 0.00, 0.01)] // 49.99% discount on 1 cent

		public async void AssertTotalDiscountCorrectlyCalculatedAndApplied(
			decimal orderSubtotal,
			decimal discountPercent,
			decimal expectedDiscountAmount,
			decimal expectedOrderTotal)
		{
			// given
			var incomingOrder = new Order
			{
				State = "VT",
				OrderSubtotal = orderSubtotal,
				PercentDiscount = discountPercent
			};

			// when
			var result = await this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken());

			// then
			// calculated...
			Assert.Equal(expectedDiscountAmount, result!.TotalDiscountAmount);

			// ...and applied
			Assert.Equal(expectedOrderTotal, result.OrderTotal); // we can get away with this because VT sales tax is 0%
		}

		/// <summary>
		/// Test method to assert that the total order amount is correctly calculated considering discounts and taxes.
		/// </summary>
		/// <param name="stateCode">The state code for the test case.</param>
		/// <param name="orderSubtotal">The order subtotal for the test case.</param>
		/// <param name="discountPercent">The discount percentage for the test case.</param>
		/// <param name="expectedDiscountAmount">The expected discount amount for the test case.</param>
		/// <param name="expectedOrderTotal">The expected total order amount for the test case.</param>
		/// <param name="expectedTotalSalesTax">The expected total sales tax for the test case.</param>
		/// <param name="expectedDiscountedTotalWithoutSalesTax">The expected discounted total without sales tax for the test case.</param>
		/// <remarks>
		/// TODO: this matrix is pretty ugly and will be hard to maintain, especially as sales taxes change
		/// TODO: fill in test cases for all supported states
		/// </remarks>
		[Theory] 
		[InlineData("VT", 12.22, 0.5, 6.11, 6.11, 0, 6.11)]
		[InlineData("VT", 13.45, 0.15, 2.02, 11.43, 0, 11.43)]
		[InlineData("VT", 2_315_331.87, 0.17, 393_606.42, 1_921_725.45, 0, 1_921_725.45)]
		[InlineData("VT", 0.01, 0.5001, 0.01, 0, 0, 0)]
		[InlineData("VT", 0.01, 0.4999, 0, 0.01, 0, 0.01)]

		[InlineData("GA", 12.22, 0.5, 6.11, 6.48, 0.37, 6.11)]
		[InlineData("GA", 13.45, 0.15, 2.02, 12.12, 0.69, 11.43)]
		[InlineData("GA", 2_315_331.87, 0.17, 393_606.42, 2_037_028.98, 115_303.53, 1_921_725.45)]
		[InlineData("GA", 0.01, 0.5001, 0.01, 0, 0, 0)]
		[InlineData("GA", 0.01, 0.4999, 0, 0.01, 0, 0.01)]

		public async void AssertTotalCorrectlyCalculatedWithDiscountAndTaxes(
			string stateCode,
			decimal orderSubtotal,
			decimal discountPercent,
			decimal expectedDiscountAmount,
			decimal expectedOrderTotal,
			decimal expectedTotalSalesTax,
			decimal expectedDiscountedTotalWithoutSalesTax)
		{
			// given
			var incomingOrder = new Order
			{
				State = stateCode,
				OrderSubtotal = orderSubtotal,
				PercentDiscount = discountPercent
			};

			// when
			var result = await this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken());

			// then
			Assert.Equal(expectedDiscountAmount, result!.TotalDiscountAmount);
			Assert.Equal(expectedOrderTotal, result.OrderTotal);
			Assert.Equal(expectedTotalSalesTax, result.TotalSalesTax);
			Assert.Equal(expectedDiscountedTotalWithoutSalesTax, result.DiscountedOrderTotalMinusSalesTax);
		}
	}
}
