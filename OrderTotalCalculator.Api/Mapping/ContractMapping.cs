using OrderTotalCalculator.Application.Models;
using OrderTotalCalculator.Contracts.Requests;
using OrderTotalCalculator.Contracts.Responses;

namespace OrderTotalCalculator.Api.Mapping
{
	/// <summary>
	/// Utility class containing extension methods for mapping contract-related entities between different types.
	/// </summary>
	/// <remarks>
	/// In a project with data storage concerns, we might create data -> response -> domain and 
	/// request -> domain -> data mappers, but for the scope of this effort such a thing is wholly unnecessary.
	/// </remarks>
	public static class ContractMapping
	{
		/// <summary>
		/// Maps a <see cref="CalculateOrderTotalRequest"/> object to an <see cref="Order"/> object.
		/// </summary>
		/// <param name="request">The request object to be mapped.</param>
		/// <returns>
		/// An instance of <see cref="Order"/> containing the mapped data from the specified request.
		/// </returns>
		public static Order MapToOrder(this CalculateOrderTotalRequest request)
		{
			return new Order
			{
				State = request.State,
				OrderSubtotal = request.OrderSubtotal,
				PercentDiscount = request.PercentDiscount
			};
		}

		/// <summary>
		/// Maps an <see cref="Order"/> object to a <see cref="CalculateOrderTotalResponse"/> object.
		/// </summary>
		/// <param name="order">The order object to be mapped.</param>
		/// <returns>
		/// An instance of <see cref="CalculateOrderTotalResponse"/> containing the mapped data from the specified order.
		/// </returns>
		public static CalculateOrderTotalResponse MapToResponse(this Order order)
		{
			return new CalculateOrderTotalResponse
			{
				OrderTotal = order.OrderTotal,
				TotalSalesTax = order.TotalSalesTax,
				DiscountedOrderTotalMinusSalesTax = order.DiscountedOrderTotalMinusSalesTax,
				TotalDiscountAmount = order.TotalDiscountAmount
			};
		}
	}
}
