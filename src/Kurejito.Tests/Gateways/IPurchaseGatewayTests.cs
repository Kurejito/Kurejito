using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Kurejito.Payments;

namespace Kurejito.Tests.Gateways {
	/// <summary>Test fixtures covering the purchase capabilities of a gateway should implement this interface
	/// and provide valid tests for each of the test cases defined by the interface.</summary>
	public interface IPurchaseGatewayTests {
		/// <summary>Verify that a successful purchase with valid card data returns the correct (OK) payment result.</summary>
		void Successful_Purchase_Returns_Correct_PaymentStatus();
	}
}
