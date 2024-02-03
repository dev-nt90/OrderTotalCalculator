namespace OrderTotalCalculator.Application.Repositories
{
	public class StateSalesTaxRepository : IStateSalesTaxRepository
	{
		// ENHANCEMENT: we could potentially extend this to a database, allowing us to adjust supported states and their sales taxes dynamically
		private readonly IDictionary<string, float> statesSalesTaxMap = new Dictionary<string, float>()
		{
			{ "GA", 0.06f },
			{ "CA", 0.075f },
			{ "NY", 0.0675f },
			{ "VT", 0.00f }
		};

		public StateSalesTaxRepository() { }

		public float GetSalesTaxRate(string stateCode)
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
