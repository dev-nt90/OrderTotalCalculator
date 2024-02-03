using OrderTotalCalculator.Contracts.Responses;

namespace OrderTotalCalculator.Api.Middleware
{
	/// <summary>
	/// Middleware for handling FluentValidation.ValidationException and converting validation errors to a standardized JSON response.
	/// </summary>
	public class ValidationMappingMiddleware
	{
		private readonly RequestDelegate next;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationMappingMiddleware"/> class.
		/// </summary>
		/// <param name="next">The next middleware delegate in the request pipeline.</param>
		public ValidationMappingMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		/// <summary>
		/// Invokes the middleware to handle FluentValidation.ValidationException and convert validation errors to a JSON response.
		/// </summary>
		/// <param name="context">The HttpContext for the current request.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch(FluentValidation.ValidationException v)
			{
				context.Response.StatusCode = 400;
				var validationFailureResponse = new ValidationFailureResponse
				{
					Errors = v.Errors.Select(e => new ValidationResponse
					{
						PropertyName = e.PropertyName,
						Message = e.ErrorMessage
					})
				};

				await context.Response.WriteAsJsonAsync(validationFailureResponse);
			}
		}
	}
}
