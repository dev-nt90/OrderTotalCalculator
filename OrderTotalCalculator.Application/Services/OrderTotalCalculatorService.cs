using FluentValidation;
using OrderTotalCalculator.Application.Models;
using OrderTotalCalculator.Application.Repositories;

namespace OrderTotalCalculator.Application.Services
{
	/// <summary>
	/// Service responsible for calculating the total amount for orders.
	/// Implements the <see cref="IOrderTotalCalculatorService"/> interface.
	/// </summary>
	public class OrderTotalCalculatorService : IOrderTotalCalculatorService
	{
		private readonly IValidator<Order> orderValidator;
		private readonly IStateSalesTaxRepository stateSalesTaxRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderTotalCalculatorService"/> class.
		/// </summary>
		/// <param name="orderValidator">The validator used for validating orders within the service.</param>
		public OrderTotalCalculatorService(IValidator<Order> orderValidator, IStateSalesTaxRepository stateSalesTaxRepository)
		{
			this.orderValidator = orderValidator;
			this.stateSalesTaxRepository = stateSalesTaxRepository;
		}

		/// <summary>
		/// Given 
		/// (i) state code, (ii) a sub-total, (iii) a discount
		/// 
		/// This method calculates
		/// (i) order total, 
		/// (ii) the total sales tax applied to the order, 
		/// (iii) the sub-total with a discount applied, 
		/// and (iv) the discounted amount.
		/// 
		/// Then pushes the calculated values into a <see cref="Order"/>
		/// </summary>
		/// <param name="order">the <see cref="Order"/></param>
		/// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
		/// <returns>An async task whose result is a nullable <see cref="Order"/></returns>
		public async Task<Order?> CalculateOrderTotalAsync(Order order, CancellationToken cancellationToken)
		{
			await this.orderValidator.ValidateAndThrowAsync(order, cancellationToken);

			var roundedDiscountAmount = Decimal.Round(
				order.OrderSubtotal * order.PercentDiscount,
				2,
				MidpointRounding.AwayFromZero);

			var orderSubTotalWithDiscount = order.OrderSubtotal - roundedDiscountAmount;
			var totalSalesTax = Decimal.Round(orderSubTotalWithDiscount * this.stateSalesTaxRepository.GetSalesTaxRate(order.State), 2, MidpointRounding.AwayFromZero);
			var orderTotal = orderSubTotalWithDiscount + totalSalesTax;

			order.OrderTotal = orderTotal;
			order.TotalSalesTax = totalSalesTax;
			order.DiscountedOrderTotalMinusSalesTax = orderSubTotalWithDiscount;
			order.TotalDiscountAmount = roundedDiscountAmount;

			return order;
		}
	}
}
