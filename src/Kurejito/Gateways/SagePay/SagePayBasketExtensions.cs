using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kurejito.Payments;

namespace Kurejito.Gateways.SagePay {
	public static class SagePayBasketExtensions {
		public static string ToSagePayBasketFormat(this Basket basket) {
			var tokens = new List<string>();
			tokens.Add(basket.Contents.Count.ToString());
			tokens.AddRange(basket.Contents.Select(item => item.ToSagePayLineItemFormat()));
			return (String.Join(":", tokens.ToArray()));
		}

		private const string LINE_ITEM_FORMAT = "{0}:{1}:{2}:{3}:{4}:{5}";
		public static string ToSagePayLineItemFormat(this LineItem item) {
			var desc = (item.Description ?? "(no description supplied)").ToASCIIString();
			var qnty = (item.Quantity.HasValue ? item.Quantity.Value.ToString() : String.Empty);
			var ival = (item.ItemValue.HasValue ? item.ItemValue.Value.ToString("0.00") : String.Empty);
			var itax = (item.ItemTax.HasValue ? item.ItemTax.Value.ToString("0.00") : String.Empty);
			var itot = (item.ItemTotal.HasValue ? item.ItemTotal.Value.ToString("0.00") : String.Empty);
			var ltot = item.LineTotal.ToString("0.00");
			return (String.Format(LINE_ITEM_FORMAT, desc, qnty, ival, itax, itot, ltot));
		}
	}
}
