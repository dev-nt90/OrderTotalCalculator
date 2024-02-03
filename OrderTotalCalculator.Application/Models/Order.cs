using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTotalCalculator.Application.Models
{
	/// <summary>
	/// Represents an order entity in the system.
	/// </summary>
	public class Order
	{
		// properties for request
		public required String State { get; init; }
		public required float OrderSubtotal { get; init; }
		public required float PercentDiscount { get; init; }

		// properties for response
		public float OrderTotal { get; set; }
		public float TotalSalesTax { get; set; }
		public float DiscountedOrderTotalMinusSalesTax { get; set; }
		public float TotalDiscountAmount { get; set; }
	}
}
