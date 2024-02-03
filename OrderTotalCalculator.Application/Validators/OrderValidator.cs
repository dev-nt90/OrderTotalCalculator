using FluentValidation;
using OrderTotalCalculator.Application.Models;
using OrderTotalCalculator.Application.Repositories;

namespace OrderTotalCalculator.Application.Validators
{
	/// <summary>
	/// Validator class for the <see cref="Order"/> entity, responsible for defining validation rules.
	/// </summary>
	public class OrderValidator : AbstractValidator<Order>
	{
		private readonly IStateSalesTaxRepository stateSalesTaxRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderValidator"/> class.
		/// </summary>
		/// <param name="orderTotalCalculatorService">The service used for order total calculations.</param>
		public OrderValidator(IStateSalesTaxRepository stateSalesTaxRepository)
		{ 
			this.stateSalesTaxRepository = stateSalesTaxRepository;

			// validate we are applying sales tax from a supported state
			RuleFor(x => x.State).Must(ValidateStateIsSupported).WithMessage("Unknown State");
			RuleFor(x => x.OrderSubtotal).GreaterThan(0.00f).WithMessage("Order Total Below or Equal To Zero");
			
			// TODO: free allowed?
			// TODO: discount repped as range from 0 to 1 or 0 to 100?
			RuleFor(x => x.PercentDiscount).LessThanOrEqualTo(1.00f); 
		}

		/// <summary>
		/// Validates whether a given state code is supported for sales tax calculations.
		/// </summary>
		/// <param name="stateCode">The state code to be validated.</param>
		/// <returns>
		///   <c>true</c> if the state code is supported; otherwise, <c>false</c>.
		/// </returns>
		private bool ValidateStateIsSupported(string stateCode)
		{
			// while we _could_ jam these ops together into one condition, I think it would be messy to debug
			if(string.IsNullOrWhiteSpace(stateCode)) 
			{  
				return false; 
			}

			// ENHANCEMENT: make async to scale
			return this.stateSalesTaxRepository
				.GetSupportedStates()
				.Any(s => s.Equals(stateCode, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}
