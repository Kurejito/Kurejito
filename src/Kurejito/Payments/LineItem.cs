using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kurejito.Payments {
	/// <summary>Represents a single item in a customer's shopping basket.</summary>
	public class LineItem {
		/// <summary>Description of this item - e.g. "20cm copper saucepan", or "Shipping", or "Server rental for March 2008"</summary>
		public string Description { get; set; }
		/// <summary>The total cost of this basket item - including any tax applied to this line item.</summary>
		public decimal LineTotal { get; set; }
		/// <summary>The number of these items being purchased. If null, it means quantity is not applicable - e.g. for shipping</summary>
		public int? Quantity { get; set; }
		/// <summary>The total cost, including tax, of a single one of these items.</summary>
		public decimal? ItemTotal { get; set;  }
		/// <summary>The cost of a single one of these items, excluding any applicable tax.</summary>
		public decimal? ItemValue { get; set; }
		/// <summary>The amount of tax payable on each single item in this line.</summary>
		public decimal? ItemTax { get; set; }

		/// <summary>Create a new simple line-item containing only a description and a line total.</summary>
		/// <param name="description">What is this item? E.g. "Shipping", "Web Order #123"</param>
		/// <param name="lineTotal">The total amount payable for this item.</param>
		public LineItem(string description, decimal lineTotal) {
			this.Description = description;
			this.LineTotal = lineTotal;
		}
	}
}
