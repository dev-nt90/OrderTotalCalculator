using Microsoft.AspNetCore.Mvc;
using OrderTotalCalculator.Api.Mapping;
using OrderTotalCalculator.Application.Services;
using OrderTotalCalculator.Contracts.Requests;

namespace OrderTotalCalculator.Api.Controllers
{
	/// <summary>
	/// Controller responsible for handling order-related operations and calculating order totals.
	/// Inherits from <see cref="ControllerBase"/>.
	/// </summary>
	/// <remarks>
	/// ENHANCEMENT: auth
	/// ENHANCEMENT: route constants
	/// ENHANCEMENT: publish to queue instead of letting controller contact service directly
	/// </remarks>
	[Route("api/[controller]")]
	[ApiController]
	public class OrderTotalCalculatorController : ControllerBase
	{
		private readonly IOrderTotalCalculatorService orderTotalCalculatorService;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderTotalCalculatorController"/> class
		/// with the specified order total calculator service.
		/// </summary>
		/// <param name="orderTotalCalculatorService">The service responsible for calculating order totals.</param>
		public OrderTotalCalculatorController(IOrderTotalCalculatorService orderTotalCalculatorService)
		{
			this.orderTotalCalculatorService = orderTotalCalculatorService;
		}

		/// <summary>
		/// Calculates the total amount for an order based on the provided request data.
		/// </summary>
		/// <param name="request">The <see cref="CalculateOrderTotalRequest"/></param>
		/// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
		/// <returns>
		/// An asynchronous operation that represents the HTTP response containing
		/// the calculated order total or an error response if the calculation fails.
		/// </returns>
		[HttpPost]
		public async Task<IActionResult> CalculateOrderTotal(
			[FromBody] CalculateOrderTotalRequest request,
			CancellationToken cancellationToken)
		{
			if (request == null)
			{
				return BadRequest("Request body cannot be empty");
			}

			var order = request.MapToOrder();
			var result = await this.orderTotalCalculatorService.CalculateOrderTotalAsync(order, cancellationToken);

			if (result is null)
			{
				return StatusCode(500, "Internal server error");
			}

			var orderResponse = result.MapToResponse();

			return Ok(orderResponse);
		}
	}
}
