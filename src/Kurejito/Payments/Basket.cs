using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kurejito.Payments {
	/// <summary>Represents a 'shopping basket', storing the items which a customer is purchasing.</summary>
	/// <remarks>For certain providers, a shopping basket is required.</remarks>
	public class Basket {
		private IList<LineItem> items = new List<LineItem>();
		public IList<LineItem> Contents {
			get { return items; }
		}
		/// <summary>Create a new basket containing the specified items.</summary>
		public Basket(params LineItem[] items) {
			this.items = new List<LineItem>(items);
		}
	}
}
