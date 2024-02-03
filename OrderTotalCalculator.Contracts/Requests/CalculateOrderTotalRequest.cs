using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTotalCalculator.Contracts.Requests
{
	/// <summary>
	/// Represents a request object for calculating the total amount of an order.
	/// </summary>
	public class CalculateOrderTotalRequest
	{
		public required String State { get; init; } // state code, not object state
		public required float OrderSubtotal { get; init; }
		public required float PercentDiscount { get; init; }
	}
}
