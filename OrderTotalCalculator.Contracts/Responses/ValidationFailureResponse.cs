using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTotalCalculator.Contracts.Responses
{
	/// <summary>
	/// Represents a response object containing a collection of validation errors.
	/// </summary>
	public class ValidationFailureResponse
	{
		public IEnumerable<ValidationResponse> Errors { get; init; }
	}
}
