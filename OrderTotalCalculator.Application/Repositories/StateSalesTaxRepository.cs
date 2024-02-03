namespace OrderTotalCalculator.Application.Repositories
{
	public class StateSalesTaxRepository : IStateSalesTaxRepository
	{
		// ENHANCEMENT: we could potentially extend this to a database, allowing us to adjust supported states and their sales taxes dynamically
		private readonly IDictionary<string, decimal> statesSalesTaxMap = new Dictionary<string, decimal>()
		{
			{ "GA", 0.06m },
			{ "CA", 0.075m },
			{ "NY", 0.0675m },
			{ "VT", 0.00m }
		};

		/// <summary>
		/// Initializes a new instance of the <see cref="StateSalesTaxRepository"/> class.
		/// </summary>
		public StateSalesTaxRepository() { }

		/// <summary>
		/// Gets the sales tax rate for a specific state.
		/// </summary>
		/// <param name="stateCode">The state code for which to retrieve the sales tax rate.</param>
		/// <returns>The sales tax rate for the specified state.</returns>
		public decimal GetSalesTaxRate(string stateCode)
		{
			return statesSalesTaxMap[stateCode];
		}

		/// <summary>
		/// Returns a collection of state codes representing the set of states with supported sales tax calculations
		/// </summary>
		/// <returns><see cref="IEnumerable{string}"/></returns>
		public IEnumerable<string> GetSupportedStates()
		{
			return statesSalesTaxMap.Keys;
		}
	}
}
