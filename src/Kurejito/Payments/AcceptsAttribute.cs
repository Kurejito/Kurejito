using System;
using System.Collections.Generic;
using System.Linq;

namespace Kurejito.Payments {
    /// <summary>
    /// Attribute for decorating Gateway implementations with the supported currency and card combinations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AcceptsAttribute : Attribute {
        protected string Iso3LetterCountryCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptsAttribute"/> class.
        /// </summary>
        /// <param name="iso3LetterCountryCode">The iso3 letter country code.</param>
        /// <param name="commaSeparatedCurrencyCodes">The comma separated currency codes.</param>
        /// <param name="cards">The cards.</param>
        public AcceptsAttribute(string iso3LetterCountryCode, string commaSeparatedCurrencyCodes, params CardType[] cards) {
            if (iso3LetterCountryCode == null) throw new ArgumentNullException("iso3LetterCountryCode");
            if (commaSeparatedCurrencyCodes == null) throw new ArgumentNullException("commaSeparatedCurrencyCodes");
            if (cards == null) throw new ArgumentNullException("cards");
            if (cards.Length < 1) throw new ArgumentOutOfRangeException("cards", @"You must provide at least one card.");
            Iso3LetterCountryCode = iso3LetterCountryCode;
            CurrencyCodes = commaSeparatedCurrencyCodes.Split(',').Select(s => s.Trim()).ToList();
            CardTypes = cards.ToList();
        }

        protected List<CardType> CardTypes { get; set; }

        protected List<string> CurrencyCodes { get; set; }

        /// <summary>
        /// Call this method to determine if a given Type is decorated to accept a certain nation/currency/card combination.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iso3LetterCountryCode">The iso3 letter country code.</param>
        /// <param name="iso3LetterCurrencyCode">The iso3 letter currency code.</param>
        /// <param name="cardType">Type of the card.</param>
        /// <returns></returns>
        public static bool DecoratedToAccept<T>(string iso3LetterCountryCode, string iso3LetterCurrencyCode, CardType cardType) {
            if (iso3LetterCountryCode == null) throw new ArgumentNullException("iso3LetterCountryCode");
            if (iso3LetterCurrencyCode == null) throw new ArgumentNullException("iso3LetterCurrencyCode");

            var acceptsAttributes = GetCustomAttributes(typeof(T), typeof(AcceptsAttribute));

            if (acceptsAttributes.Length < 1)
                return false;
            
            foreach (AcceptsAttribute customAttribute in acceptsAttributes) {
                if(!customAttribute.Iso3LetterCountryCode.Equals(iso3LetterCountryCode, StringComparison.InvariantCultureIgnoreCase))
                    continue;
                if(!customAttribute.CardTypes.Contains(cardType))
                    continue;
                if (customAttribute.CurrencyCodes.Any(s => s.Equals(iso3LetterCurrencyCode, StringComparison.InvariantCultureIgnoreCase)))
                    return true;
            }
            return false;
        }
    }
}