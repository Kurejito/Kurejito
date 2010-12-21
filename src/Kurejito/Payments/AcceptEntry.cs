using System;
using System.Collections.Generic;

namespace Kurejito.Payments {
    public class AcceptEntry {
        public AcceptEntry(Currency currency, params CardType[] cardTypes) {
            if(cardTypes == null || cardTypes.Length < 1)
                throw new ArgumentException(@"You must provide at least one CardType.","cardTypes");
            this.Currency = currency;
            this.CardTypes = cardTypes;
        }

        public CardType[] CardTypes { get; private set; }
        public Currency Currency { get; private set; }
    }
}