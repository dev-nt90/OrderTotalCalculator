using OrderTotalCalculator.Api.Middleware;
using OrderTotalCalculator.Application;

namespace SalesTaxCalculatorApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddApplication();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.UseMiddleware<ValidationExceptionMappingMiddleware>();

			app.MapControllers();

			app.Run();
		}
	}
}
