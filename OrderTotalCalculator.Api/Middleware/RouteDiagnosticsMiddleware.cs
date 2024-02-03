namespace OrderTotalCalculator.Api.Middleware
{
	/// <summary>
	/// Middleware for logging route data information during request processing.
	/// </summary>
	public class RouteDiagnosticsMiddleware
	{
		private readonly RequestDelegate _next;

		/// <summary>
		/// Initializes a new instance of the <see cref="RouteDiagnosticsMiddleware"/> class.
		/// </summary>
		/// <param name="next">The next middleware delegate in the request pipeline.</param>
		public RouteDiagnosticsMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		/// <summary>
		/// Invokes the middleware to log route data information and continues request processing.
		/// </summary>
		/// <param name="context">The HttpContext for the current request.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task Invoke(HttpContext context)
		{
			var routeData = context.GetRouteData();
			if (routeData != null)
			{
				LogRouteData(routeData);
			}
			await _next(context);
		}

		/// <summary>
		/// Logs route data information to the console.
		/// </summary>
		/// <param name="routeData">The route data to be logged.</param>
		private void LogRouteData(RouteData routeData)
		{
			Console.WriteLine($"Route data: {routeData}");
		}
	}
}
