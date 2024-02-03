using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderTotalCalculator.Application.Models;
using OrderTotalCalculator.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTotalCalculator.Tests
{
	public class OrderTotalCalculatorServiceTests : IClassFixture<BaseTestFixture>
	{
		private readonly IOrderTotalCalculatorService calculatorService;

		public OrderTotalCalculatorServiceTests(BaseTestFixture fixture) 
		{
			calculatorService = fixture.ServiceProvider.GetRequiredService<IOrderTotalCalculatorService>();
		}

		[Fact]
		public async void AssertZeroDiscountDoesNotChangeTotal()
		{
			var incomingOrder = new Order
			{
				State = "VT",
				OrderSubtotal = 123.45f,
				PercentDiscount = 0.00f
			};

			var result = await this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken());

			Assert.NotNull(result);
			Assert.Equal(incomingOrder.OrderSubtotal, result.OrderTotal);
		}

		[Fact]
		public async void Assert100PercentDiscountEmitsZero()
		{
			var incomingOrder = new Order
			{
				State = "VT",
				OrderSubtotal = 123.45f,
				PercentDiscount = 1.00f
			};

			var result = await this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken());

			Assert.NotNull(result);
			Assert.Equal(0f, result.OrderTotal);
			Assert.Equal(incomingOrder.OrderSubtotal, result.TotalDiscountAmount);
		}

		[Fact]
		public async void AssertNegativeSubtotalThrows()
		{
			var incomingOrder = new Order
			{
				State = "VT",
				OrderSubtotal = -123.45f,
				PercentDiscount = 1.00f
			};

			//var result = await this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken());

			await Assert.ThrowsAsync<ValidationException>(() => this.calculatorService.CalculateOrderTotalAsync(incomingOrder, new CancellationToken()));
		}
	}
}
