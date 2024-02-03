using OrderTotalCalculator.Application.Models;

namespace OrderTotalCalculator.Application.Services
{
	/// <summary>
	/// Defines the contract for a service responsible for calculating the total amount for orders.
	/// </summary>
	public interface IOrderTotalCalculatorService
	{
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
		public Task<Order?> CalculateOrderTotalAsync(Order order, CancellationToken cancellationToken);
	}
}
