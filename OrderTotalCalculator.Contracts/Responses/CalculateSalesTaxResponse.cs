using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTotalCalculator.Contracts.Responses
{
	/// <summary>
	/// Represents a response object containing the calculated total amount and related details for an order.
	/// </summary>
	public class CalculateOrderTotalResponse
	{
		public required float OrderTotal { get; init; }
		public required float TotalSalesTax { get; init; }
		public required float DiscountedOrderTotalMinusSalesTax { get; init; }
		public required float TotalDiscountAmount { get; init; }
	}
}
