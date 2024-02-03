using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTotalCalculator.Application.Repositories
{
	public interface IStateSalesTaxRepository
	{
		
		public float GetSalesTaxRate(string stateCode);

		/// <summary>
		/// Returns a collection of state codes representing the set of states with supported sales tax calculations
		/// </summary>
		/// <returns><see cref="IEnumerable{string}"/></returns>
		public IEnumerable<String> GetSupportedStates();
	}
}
