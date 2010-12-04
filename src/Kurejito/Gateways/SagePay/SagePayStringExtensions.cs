using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kurejito.Gateways.SagePay {
	public static class SagePayStringExtensions {
		/// <summary>Returns the supplied string with accented and high-order Unicode characters converted to their basic ASCII equivalents</summary>
		/// <param name="unicodeString"></param>
		/// <returns></returns>
		public static string ToASCIIString(this string unicodeString) {
			var normalized = unicodeString.Normalize(NormalizationForm.FormKD);
			var stripper = Encoding.GetEncoding(Encoding.ASCII.CodePage, new EncoderReplacementFallback(String.Empty), new DecoderReplacementFallback(String.Empty));
			var bytes = stripper.GetBytes(normalized);
			var asciiString = Encoding.ASCII.GetString(bytes);
			return (asciiString);
		}

		/// <summary>
		/// Determines whether the specified string ('haystack') contains any of the specified character tokens ('needles')
		/// </summary>
		/// <param name="haystack">The string that we're looking in.</param>
		/// <param name="needles">The characters that we're looking for.</param>
		/// <returns><c>true</c> if the specified string contains any of the specified characters; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>For non-English speakers, parameters are named with reference to the English idiom 'looking for a needle in a haystack'</remarks>
		public static bool ContainsAny(this string haystack, char[] needles) {
			if (needles == null) return (false);
			if (haystack.ToCharArray().Any(c => haystack.Contains(c))) return (true);
			return (false);
		}

		private static readonly char[] ToxicFragments = new char[] { (char)0x000a, (char)0x000d, ':' };
		private static readonly char[] ChocolateChips = "!\"#$%&'()*+,-./0123456789;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ ¨ª¯²³´¸¹º¼½¾ÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜÝàáâãäåçèéêëìíîïñòóôõöùúûüýÿĀāĂăĄąĆćĈĉĊċČčĎďĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĨĩĪīĬĭĮįİĲĳĴĵĶķĹĺĻļĽľĿŀŃńŅņŇňŉŌōŎŏŐőŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſƠơƯưǄǅǆǇǈǉǊǋǌǍǎǏǐǑǒǓǔǕǖǗǘǙǚǛǜǞǟǠǡǦǧǨǩǪǫǬǭǰǱǲǳǴǵǸǹǺǻȀȁȂȃȄȅȆȇȈȉȊȋȌȍȎȏȐȑȒȓȔȕȖȗȘșȚțȞȟȦȧȨȩȪȫȬȭȮȯȰȱȲȳʰʲʳʷʸ˘˙˚˛˜˝ˡˢˣͺ;΄΅".ToCharArray();

		/// <summary>Determines whether this string is 'alphanumeric' according to the SagePay payment gateway validation tests.</summary>
		/// <param name="unicodeString">A unicode string.</param>
		/// <returns><c>true</c> if the specified unicode string can be safely passed to SagePay's API in a field specified as being 'alphanumeric'; otherwise, <c>false</c>.</returns>
		/// <remarks>The SagePay documentation refers to fields that must be 'alphanumeric', but as of VSP v2.23 these fields will actually accept
		/// a much wider range of characters including apostrophes and hyphens (often found in customers' given names).
		/// </remarks>
		public static bool IsConsideredAlphanumericBySagePay(this string unicodeString) {
			// Anything containing toxic fragments is unacceptable
			if (unicodeString.ContainsAny(ToxicFragments)) return (false);

			// As long as there's no poison, we'll accept anything with at least one chocolate chip in it.
			if (unicodeString.ContainsAny(ChocolateChips)) return (true);

			// But if there's no chocolate chips, then it's invalid, whether it's toxic or not.
			return (false);
		}

		/// <summary>
		/// Convert this dictionary contents to a byte array suitable for submitting as an HTTP POST.
		/// </summary>
		/// <param name="postValues">A byte array containing the form-encoded contents of this dictionary.</param>
		/// <returns></returns>
		public static byte[] ToHttpPostData(this Dictionary<string, string> postValues) {
			var postData = postValues.Select(pair => String.Format("{0}={1}", pair.Key, HttpUtility.UrlEncode(pair.Value))).Join("&");
			var encoding = new ASCIIEncoding();
			return (encoding.GetBytes(postData));
		}

		/// <summary>
		/// Convert each element in <paramref name="tokens"/> to it's ToString() representation
		/// and join the resulting strings into a single string using the specified separator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="tokens">The items that .</param>
		/// <param name="separator">The separator.</param>
		/// <returns>A string containing string representation of each input token joined by the specified separator.</returns>
		public static string Join<T>(this IEnumerable<T> tokens, string separator) {
			return (String.Join(separator, tokens.Select(bit => bit.ToString()).ToArray()));
		}

		/// <summary>
		/// Converts a byte array into a continuous string of uppercase hex digit pairs - e.g. converts { 0x01, 0x02, 0xAA, 0xFF } into a "0102AAFF"
		/// </summary>
		/// <param name="bytes">An array of bytes, such as { 0x01, 0x02, 0xAA, 0xFF }</param>
		/// <returns>A continuous string of uppercase hex digit pairs, such as "0102AAFF"</returns>
		public static string ToHexString(this byte[] bytes) {
			return (bytes.Select(b => b.ToString("x2")).ToArray().Join(String.Empty)).ToUpper();
		}
	}
}
