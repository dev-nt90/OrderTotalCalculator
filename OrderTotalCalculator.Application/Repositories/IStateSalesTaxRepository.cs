namespace OrderTotalCalculator.Application.Repositories
{
	public interface IStateSalesTaxRepository
	{
		/// <summary>
		/// Gets the sales tax rate for a specific state.
		/// </summary>
		/// <param name="stateCode">The state code for which to retrieve the sales tax rate.</param>
		/// <returns>The sales tax rate for the specified state.</returns>
		public decimal GetSalesTaxRate(string stateCode);

		/// <summary>
		/// Returns a collection of state codes representing the set of states with supported sales tax calculations
		/// </summary>
		/// <returns><see cref="IEnumerable{string}"/></returns>
		public IEnumerable<String> GetSupportedStates();
	}
}
