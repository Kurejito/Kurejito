using System;
using System.Collections.Generic;
using System.Globalization;

namespace System
{
    /// <summary>
    /// Represents a system of money within which <see cref="Money"/>
    /// amounts can be compared and arithmetic can be performed.
    /// </summary>
    public struct Currency : IEquatable<Currency>, IFormatProvider
    {
        private struct CurrencyTableEntry : IEquatable<CurrencyTableEntry>
        {
            internal readonly String Name;
            internal readonly String Iso3LetterCode;
            internal readonly UInt16 IsoNumberCode;
            internal readonly String Symbol;

            internal CurrencyTableEntry(String name,
                                      String iso3LetterCode,
                                      UInt16 isoNumberCode,
                                      String symbol)
            {
                Name = name;
                Iso3LetterCode = iso3LetterCode;
                IsoNumberCode = isoNumberCode;
                Symbol = symbol;
            }

            #region IEquatable<CurrencyTableEntry> Members

            public Boolean Equals(CurrencyTableEntry other)
            {
                return IsoNumberCode == other.IsoNumberCode;
            }

            #endregion
        }

        private static readonly Dictionary<Int32, CurrencyTableEntry> _currencies
            = new Dictionary<Int32, CurrencyTableEntry>();

        private static readonly Dictionary<String, Int32> _codeIndex
            = new Dictionary<String, Int32>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly Dictionary<Int32, Int32> _cultureIdLookup
            = new Dictionary<Int32, Int32>();

        /// <summary>
        /// Lek
        /// </summary>
        public static readonly Currency All;

        /// <summary>
        /// Algerian Dinar
        /// </summary>
        public static readonly Currency Dzd;

        /// <summary>
        /// Argentine Peso
        /// </summary>
        public static readonly Currency Ars;

        /// <summary>
        /// Australian Dollar
        /// </summary>
        public static readonly Currency Aud;

        /// <summary>
        /// Bahamian Dollar
        /// </summary>
        public static readonly Currency Bsd;

        /// <summary>
        /// Bahraini Dinar
        /// </summary>
        public static readonly Currency Bhd;

        /// <summary>
        /// Taka
        /// </summary>
        public static readonly Currency Bdt;

        /// <summary>
        /// Armenian Dram
        /// </summary>
        public static readonly Currency Amd;

        /// <summary>
        /// Barbados Dollar
        /// </summary>
        public static readonly Currency Bbd;

        /// <summary>
        /// Bermudian Dollar 
        /// (customarily known as 
        /// Bermuda Dollar)
        /// </summary>
        public static readonly Currency Bmd;

        /// <summary>
        /// Boliviano
        /// </summary>
        public static readonly Currency Bob;

        /// <summary>
        /// Pula
        /// </summary>
        public static readonly Currency Bwp;

        /// <summary>
        /// Belize Dollar
        /// </summary>
        public static readonly Currency Bzd;

        /// <summary>
        /// Solomon Islands Dollar
        /// </summary>
        public static readonly Currency Sbd;

        /// <summary>
        /// Brunei Dollar
        /// </summary>
        public static readonly Currency Bnd;

        /// <summary>
        /// Kyat
        /// </summary>
        public static readonly Currency Mmk;

        /// <summary>
        /// Burundi Franc
        /// </summary>
        public static readonly Currency Bif;

        /// <summary>
        /// Riel
        /// </summary>
        public static readonly Currency Khr;

        /// <summary>
        /// Canadian Dollar
        /// </summary>
        public static readonly Currency Cad;

        /// <summary>
        /// Cape Verde Escudo
        /// </summary>
        public static readonly Currency Cve;

        /// <summary>
        /// Cayman Islands Dollar
        /// </summary>
        public static readonly Currency Kyd;

        /// <summary>
        /// Sri Lanka Rupee
        /// </summary>
        public static readonly Currency Lkr;

        /// <summary>
        /// Chilean Peso
        /// </summary>
        public static readonly Currency Clp;

        /// <summary>
        /// Yuan Renminbi
        /// </summary>
        public static readonly Currency Cny;

        /// <summary>
        /// Colombian Peso
        /// </summary>
        public static readonly Currency Cop;

        /// <summary>
        /// Comoro Franc
        /// </summary>
        public static readonly Currency Kmf;

        /// <summary>
        /// Costa Rican Colon
        /// </summary>
        public static readonly Currency Crc;

        /// <summary>
        /// Croatian Kuna
        /// </summary>
        public static readonly Currency Hrk;

        /// <summary>
        /// Cuban Peso
        /// </summary>
        public static readonly Currency Cup;

        /// <summary>
        /// Czech Koruna
        /// </summary>
        public static readonly Currency Czk;

        /// <summary>
        /// Danish Krone
        /// </summary>
        public static readonly Currency Dkk;

        /// <summary>
        /// Dominican Peso
        /// </summary>
        public static readonly Currency Dop;

        /// <summary>
        /// El Salvador Colon
        /// </summary>
        public static readonly Currency Svc;

        /// <summary>
        /// Ethiopian Birr
        /// </summary>
        public static readonly Currency Etb;

        /// <summary>
        /// Nakfa
        /// </summary>
        public static readonly Currency Ern;

        /// <summary>
        /// Kroon
        /// </summary>
        public static readonly Currency Eek;

        /// <summary>
        /// Falkland Islands Pound
        /// </summary>
        public static readonly Currency Fkp;

        /// <summary>
        /// Fiji Dollar
        /// </summary>
        public static readonly Currency Fjd;

        /// <summary>
        /// Djibouti Franc
        /// </summary>
        public static readonly Currency Djf;

        /// <summary>
        /// Dalasi
        /// </summary>
        public static readonly Currency Gmd;

        /// <summary>
        /// Gibraltar Pound
        /// </summary>
        public static readonly Currency Gip;

        /// <summary>
        /// Quetzal
        /// </summary>
        public static readonly Currency Gtq;

        /// <summary>
        /// Guinea Franc
        /// </summary>
        public static readonly Currency Gnf;

        /// <summary>
        /// Guyana Dollar
        /// </summary>
        public static readonly Currency Gyd;

        /// <summary>
        /// Gourde
        /// </summary>
        public static readonly Currency Htg;

        /// <summary>
        /// Lempira
        /// </summary>
        public static readonly Currency Hnl;

        /// <summary>
        /// Hong Kong Dollar
        /// </summary>
        public static readonly Currency Hkd;

        /// <summary>
        /// Forint
        /// </summary>
        public static readonly Currency Huf;

        /// <summary>
        /// Iceland Krona
        /// </summary>
        public static readonly Currency Isk;

        /// <summary>
        /// Indian Rupee
        /// </summary>
        public static readonly Currency Inr;

        /// <summary>
        /// Rupiah
        /// </summary>
        public static readonly Currency Idr;

        /// <summary>
        /// Iranian Rial
        /// </summary>
        public static readonly Currency Irr;

        /// <summary>
        /// Iraqi Dinar
        /// </summary>
        public static readonly Currency Iqd;

        /// <summary>
        /// New Israeli Sheqel
        /// </summary>
        public static readonly Currency Ils;

        /// <summary>
        /// Jamaican Dollar
        /// </summary>
        public static readonly Currency Jmd;

        /// <summary>
        /// Yen
        /// </summary>
        public static readonly Currency Jpy;

        /// <summary>
        /// Tenge
        /// </summary>
        public static readonly Currency Kzt;

        /// <summary>
        /// Jordanian Dinar
        /// </summary>
        public static readonly Currency Jod;

        /// <summary>
        /// Kenyan Shilling
        /// </summary>
        public static readonly Currency Kes;

        /// <summary>
        /// North Korean Won
        /// </summary>
        public static readonly Currency Kpw;

        /// <summary>
        /// Won
        /// </summary>
        public static readonly Currency Krw;

        /// <summary>
        /// Kuwaiti Dinar
        /// </summary>
        public static readonly Currency Kwd;

        /// <summary>
        /// Som
        /// </summary>
        public static readonly Currency Kgs;

        /// <summary>
        /// Kip
        /// </summary>
        public static readonly Currency Lak;

        /// <summary>
        /// Lebanese Pound
        /// </summary>
        public static readonly Currency Lbp;

        /// <summary>
        /// Latvian Lats
        /// </summary>
        public static readonly Currency Lvl;

        /// <summary>
        /// Liberian Dollar
        /// </summary>
        public static readonly Currency Lrd;

        /// <summary>
        /// Libyan Dinar
        /// </summary>
        public static readonly Currency Lyd;

        /// <summary>
        /// Lithuanian Litas
        /// </summary>
        public static readonly Currency Ltl;

        /// <summary>
        /// Pataca
        /// </summary>
        public static readonly Currency Mop;

        /// <summary>
        /// Kwacha
        /// </summary>
        public static readonly Currency Mwk;

        /// <summary>
        /// Malaysian Ringgit
        /// </summary>
        public static readonly Currency Myr;

        /// <summary>
        /// Rufiyaa
        /// </summary>
        public static readonly Currency Mvr;

        /// <summary>
        /// Ouguiya
        /// </summary>
        public static readonly Currency Mro;

        /// <summary>
        /// Mauritius Rupee
        /// </summary>
        public static readonly Currency Mur;

        /// <summary>
        /// Mexican Peso
        /// </summary>
        public static readonly Currency Mxn;

        /// <summary>
        /// Tugrik
        /// </summary>
        public static readonly Currency Mnt;

        /// <summary>
        /// Moldovan Leu
        /// </summary>
        public static readonly Currency Mdl;

        /// <summary>
        /// Moroccan Dirham
        /// </summary>
        public static readonly Currency Mad;

        /// <summary>
        /// Rial Omani
        /// </summary>
        public static readonly Currency Omr;

        /// <summary>
        /// Nepalese Rupee
        /// </summary>
        public static readonly Currency Npr;

        /// <summary>
        /// Netherlands Antillian Guilder
        /// </summary>
        public static readonly Currency Ang;

        /// <summary>
        /// Aruban Guilder
        /// </summary>
        public static readonly Currency Awg;

        /// <summary>
        /// Vatu
        /// </summary>
        public static readonly Currency Vuv;

        /// <summary>
        /// New Zealand Dollar
        /// </summary>
        public static readonly Currency Nzd;

        /// <summary>
        /// Cordoba Oro
        /// </summary>
        public static readonly Currency Nio;

        /// <summary>
        /// Naira
        /// </summary>
        public static readonly Currency Ngn;

        /// <summary>
        /// Norwegian Krone
        /// </summary>
        public static readonly Currency Nok;

        /// <summary>
        /// Pakistan Rupee
        /// </summary>
        public static readonly Currency Pkr;

        /// <summary>
        /// Balboa
        /// </summary>
        public static readonly Currency Pab;

        /// <summary>
        /// Kina
        /// </summary>
        public static readonly Currency Pgk;

        /// <summary>
        /// Guarani
        /// </summary>
        public static readonly Currency Pyg;

        /// <summary>
        /// Nuevo Sol
        /// </summary>
        public static readonly Currency Pen;

        /// <summary>
        /// Philippine Peso
        /// </summary>
        public static readonly Currency Php;

        /// <summary>
        /// Guinea-Bissau Peso
        /// </summary>
        public static readonly Currency Gwp;

        /// <summary>
        /// Qatari Rial
        /// </summary>
        public static readonly Currency Qar;

        /// <summary>
        /// Russian Ruble
        /// </summary>
        public static readonly Currency Rub;

        /// <summary>
        /// Rwanda Franc
        /// </summary>
        public static readonly Currency Rwf;

        /// <summary>
        /// Saint Helena Pound
        /// </summary>
        public static readonly Currency Shp;

        /// <summary>
        /// Dobra
        /// </summary>
        public static readonly Currency Std;

        /// <summary>
        /// Saudi Riyal
        /// </summary>
        public static readonly Currency Sar;

        /// <summary>
        /// Seychelles Rupee
        /// </summary>
        public static readonly Currency Scr;

        /// <summary>
        /// Leone
        /// </summary>
        public static readonly Currency Sll;

        /// <summary>
        /// Singapore Dollar
        /// </summary>
        public static readonly Currency Sgd;

        /// <summary>
        /// Slovak Koruna
        /// </summary>
        public static readonly Currency Skk;

        /// <summary>
        /// Dong
        /// </summary>
        public static readonly Currency Vnd;

        /// <summary>
        /// Somali Shilling
        /// </summary>
        public static readonly Currency Sos;

        /// <summary>
        /// Rand
        /// </summary>
        public static readonly Currency Zar;

        /// <summary>
        /// Zimbabwe Dollar
        /// </summary>
        public static readonly Currency Zwd;

        /// <summary>
        /// Lilangeni
        /// </summary>
        public static readonly Currency Szl;

        /// <summary>
        /// Swedish Krona
        /// </summary>
        public static readonly Currency Sek;

        /// <summary>
        /// Swiss Franc
        /// </summary>
        public static readonly Currency Chf;

        /// <summary>
        /// Syrian Pound
        /// </summary>
        public static readonly Currency Syp;

        /// <summary>
        /// Baht
        /// </summary>
        public static readonly Currency Thb;

        /// <summary>
        /// Pa'anga
        /// </summary>
        public static readonly Currency Top;

        /// <summary>
        /// Trinidad and Tobago 
        /// Dollar
        /// </summary>
        public static readonly Currency Ttd;

        /// <summary>
        /// UAE Dirham
        /// </summary>
        public static readonly Currency Aed;

        /// <summary>
        /// Tunisian Dinar
        /// </summary>
        public static readonly Currency Tnd;

        /// <summary>
        /// Manat
        /// </summary>
        public static readonly Currency Tmm;

        /// <summary>
        /// Uganda Shilling
        /// </summary>
        public static readonly Currency Ugx;

        /// <summary>
        /// Denar
        /// </summary>
        public static readonly Currency Mkd;

        /// <summary>
        /// Egyptian Pound
        /// </summary>
        public static readonly Currency Egp;

        /// <summary>
        /// Pound Sterling
        /// </summary>
        public static readonly Currency Gbp;

        /// <summary>
        /// Tanzanian Shilling
        /// </summary>
        public static readonly Currency Tzs;

        /// <summary>
        /// US Dollar
        /// </summary>
        public static readonly Currency Usd;

        /// <summary>
        /// Peso Uruguayo
        /// </summary>
        public static readonly Currency Uyu;

        /// <summary>
        /// Uzbekistan Sum
        /// </summary>
        public static readonly Currency Uzs;

        /// <summary>
        /// Tala
        /// </summary>
        public static readonly Currency Wst;

        /// <summary>
        /// Yemeni Rial
        /// </summary>
        public static readonly Currency Yer;

        /// <summary>
        /// Kwacha
        /// </summary>
        public static readonly Currency Zmk;

        /// <summary>
        /// New Taiwan Dollar
        /// </summary>
        public static readonly Currency Twd;

        /// <summary>
        /// Ghana Cedi
        /// </summary>
        public static readonly Currency Ghs;

        /// <summary>
        /// Bolivar Fuerte
        /// </summary>
        public static readonly Currency Vef;

        /// <summary>
        /// Sudanese Pound
        /// </summary>
        public static readonly Currency Sdg;

        /// <summary>
        /// Serbian Dinar
        /// </summary>
        public static readonly Currency Rsd;

        /// <summary>
        /// Metical
        /// </summary>
        public static readonly Currency Mzn;

        /// <summary>
        /// Azerbaijanian Manat
        /// </summary>
        public static readonly Currency Azn;

        /// <summary>
        /// New Leu
        /// </summary>
        public static readonly Currency Ron;

        /// <summary>
        /// New Turkish Lira
        /// </summary>
        public static readonly Currency Try;

        /// <summary>
        /// CFA Franc BEAC
        /// </summary>
        public static readonly Currency Xaf;

        /// <summary>
        /// East Caribbean Dollar
        /// </summary>
        public static readonly Currency Xcd;

        /// <summary>
        /// CFA Franc BCEAO
        /// </summary>
        public static readonly Currency Xof;

        /// <summary>
        /// CFP Franc
        /// </summary>
        public static readonly Currency Xpf;

        /// <summary>
        /// Bond Markets Units 
        /// European Composite Unit 
        /// (EURCO)
        /// </summary>
        public static readonly Currency Xba;

        /// <summary>
        /// European Monetary 
        /// Unit (E.M.U.-6)
        /// </summary>
        public static readonly Currency Xbb;

        /// <summary>
        /// European Unit of 
        /// Account 9(E.U.A.-9)
        /// </summary>
        public static readonly Currency Xbc;

        /// <summary>
        /// European Unit of 
        /// Account 17(E.U.A.-17)
        /// </summary>
        public static readonly Currency Xbd;

        /// <summary>
        /// Gold
        /// </summary>
        public static readonly Currency Xau;

        /// <summary>
        /// SDR
        /// </summary>
        public static readonly Currency Xdr;

        /// <summary>
        /// Silver
        /// </summary>
        public static readonly Currency Xag;

        /// <summary>
        /// Platinum
        /// </summary>
        public static readonly Currency Xpt;

        /// <summary>
        /// Codes specifically 
        /// reserved for testing 
        /// purposes
        /// </summary>
        public static readonly Currency Xts;

        /// <summary>
        /// Palladium
        /// </summary>
        public static readonly Currency Xpd;

        /// <summary>
        /// Surinam Dollar
        /// </summary>
        public static readonly Currency Srd;

        /// <summary>
        /// Malagasy Ariary
        /// </summary>
        public static readonly Currency Mga;

        /// <summary>
        /// Afghani
        /// </summary>
        public static readonly Currency Afn;

        /// <summary>
        /// Somoni
        /// </summary>
        public static readonly Currency Tjs;

        /// <summary>
        /// Kwanza
        /// </summary>
        public static readonly Currency Aoa;

        /// <summary>
        /// Belarussian Ruble
        /// </summary>
        public static readonly Currency Byr;

        /// <summary>
        /// Bulgarian Lev
        /// </summary>
        public static readonly Currency Bgn;

        /// <summary>
        /// Franc Congolais
        /// </summary>
        public static readonly Currency Cdf;

        /// <summary>
        /// Convertible Marks
        /// </summary>
        public static readonly Currency Bam;

        /// <summary>
        /// Euro
        /// </summary>
        public static readonly Currency Eur;

        /// <summary>
        /// Hryvnia
        /// </summary>
        public static readonly Currency Uah;

        /// <summary>
        /// Lari
        /// </summary>
        public static readonly Currency Gel;

        /// <summary>
        /// Zloty
        /// </summary>
        public static readonly Currency Pln;

        /// <summary>
        /// Brazilian Real
        /// </summary>
        public static readonly Currency Brl;

        /// <summary>
        /// The codes assigned for 
        /// transactions where no 
        /// currency is involved.
        /// </summary>
        public static readonly Currency Xxx;

        static Currency()
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            Dictionary<String, List<Int32>> cultureIdLookup = new Dictionary<String, List<Int32>>();
            Dictionary<String, String> symbolLookup = new Dictionary<String, String>();

            foreach (CultureInfo culture in cultures)
            {
                Int32 lcid = culture.LCID;
                RegionInfo regionInfo = new RegionInfo(lcid);
                String isoSymbol = regionInfo.ISOCurrencySymbol;

                if (!cultureIdLookup.ContainsKey(isoSymbol))
                {
                    cultureIdLookup[isoSymbol] = new List<Int32>();
                }

                cultureIdLookup[isoSymbol].Add(lcid);
                symbolLookup[isoSymbol] = regionInfo.CurrencySymbol;
            }

            #region Currency table loading
            _currencies[008] = new CurrencyTableEntry("Lek", "ALL", 008, lookupString("ALL", symbolLookup));
            _currencies[012] = new CurrencyTableEntry("Algerian Dinar", "DZD", 012, lookupString("DZD", symbolLookup));
            _currencies[032] = new CurrencyTableEntry("Argentine Peso", "ARS", 032, lookupString("ARS", symbolLookup));
            _currencies[036] = new CurrencyTableEntry("Australian Dollar", "AUD", 036, lookupString("AUD", symbolLookup));
            _currencies[044] = new CurrencyTableEntry("Bahamian Dollar", "BSD", 044, lookupString("BSD", symbolLookup));
            _currencies[048] = new CurrencyTableEntry("Bahraini Dinar", "BHD", 048, lookupString("BHD", symbolLookup));
            _currencies[050] = new CurrencyTableEntry("Taka", "BDT", 050, lookupString("BDT", symbolLookup));
            _currencies[051] = new CurrencyTableEntry("Armenian Dram", "AMD", 051, lookupString("AMD", symbolLookup));
            _currencies[052] = new CurrencyTableEntry("Barbados Dollar", "BBD", 052, lookupString("BBD", symbolLookup));
            _currencies[060] = new CurrencyTableEntry("Bermudian Dollar (customarily known as Bermuda Dollar)", "BMD", 060, lookupString("BMD", symbolLookup));
            _currencies[068] = new CurrencyTableEntry("Boliviano", "BOB", 068, lookupString("BOB", symbolLookup));
            _currencies[072] = new CurrencyTableEntry("Pula", "BWP", 072, lookupString("BWP", symbolLookup));
            _currencies[084] = new CurrencyTableEntry("Belize Dollar", "BZD", 084, lookupString("BZD", symbolLookup));
            _currencies[090] = new CurrencyTableEntry("Solomon Islands Dollar", "SBD", 090, lookupString("SBD", symbolLookup));
            _currencies[096] = new CurrencyTableEntry("Brunei Dollar", "BND", 096, lookupString("BND", symbolLookup));
            _currencies[104] = new CurrencyTableEntry("Kyat", "MMK", 104, lookupString("MMK", symbolLookup));
            _currencies[108] = new CurrencyTableEntry("Burundi Franc", "BIF", 108, lookupString("BIF", symbolLookup));
            _currencies[116] = new CurrencyTableEntry("Riel", "KHR", 116, lookupString("KHR", symbolLookup));
            _currencies[124] = new CurrencyTableEntry("Canadian Dollar", "CAD", 124, lookupString("CAD", symbolLookup));
            _currencies[132] = new CurrencyTableEntry("Cape Verde Escudo", "CVE", 132, lookupString("CVE", symbolLookup));
            _currencies[136] = new CurrencyTableEntry("Cayman Islands Dollar", "KYD", 136, lookupString("KYD", symbolLookup));
            _currencies[144] = new CurrencyTableEntry("Sri Lanka Rupee", "LKR", 144, lookupString("LKR", symbolLookup));
            _currencies[152] = new CurrencyTableEntry("Chilean Peso", "CLP", 152, lookupString("CLP", symbolLookup));
            _currencies[156] = new CurrencyTableEntry("Yuan Renminbi", "CNY", 156, lookupString("CNY", symbolLookup));
            _currencies[170] = new CurrencyTableEntry("Colombian Peso", "COP", 170, lookupString("COP", symbolLookup));
            _currencies[174] = new CurrencyTableEntry("Comoro Franc", "KMF", 174, lookupString("KMF", symbolLookup));
            _currencies[188] = new CurrencyTableEntry("Costa Rican Colon", "CRC", 188, lookupString("CRC", symbolLookup));
            _currencies[191] = new CurrencyTableEntry("Croatian Kuna", "HRK", 191, lookupString("HRK", symbolLookup));
            _currencies[192] = new CurrencyTableEntry("Cuban Peso", "CUP", 192, lookupString("CUP", symbolLookup));
            _currencies[203] = new CurrencyTableEntry("Czech Koruna", "CZK", 203, lookupString("CZK", symbolLookup));
            _currencies[208] = new CurrencyTableEntry("Danish Krone", "DKK", 208, lookupString("DKK", symbolLookup));
            _currencies[214] = new CurrencyTableEntry("Dominican Peso", "DOP", 214, lookupString("DOP", symbolLookup));
            _currencies[222] = new CurrencyTableEntry("El Salvador Colon", "SVC", 222, lookupString("SVC", symbolLookup));
            _currencies[230] = new CurrencyTableEntry("Ethiopian Birr", "ETB", 230, lookupString("ETB", symbolLookup));
            _currencies[232] = new CurrencyTableEntry("Nakfa", "ERN", 232, lookupString("ERN", symbolLookup));
            _currencies[233] = new CurrencyTableEntry("Kroon", "EEK", 233, lookupString("EEK", symbolLookup));
            _currencies[238] = new CurrencyTableEntry("Falkland Islands Pound", "FKP", 238, lookupString("FKP", symbolLookup));
            _currencies[242] = new CurrencyTableEntry("Fiji Dollar", "FJD", 242, lookupString("FJD", symbolLookup));
            _currencies[262] = new CurrencyTableEntry("Djibouti Franc", "DJF", 262, lookupString("DJF", symbolLookup));
            _currencies[270] = new CurrencyTableEntry("Dalasi", "GMD", 270, lookupString("GMD", symbolLookup));
            _currencies[292] = new CurrencyTableEntry("Gibraltar Pound", "GIP", 292, lookupString("GIP", symbolLookup));
            _currencies[320] = new CurrencyTableEntry("Quetzal", "GTQ", 320, lookupString("GTQ", symbolLookup));
            _currencies[324] = new CurrencyTableEntry("Guinea Franc", "GNF", 324, lookupString("GNF", symbolLookup));
            _currencies[328] = new CurrencyTableEntry("Guyana Dollar", "GYD", 328, lookupString("GYD", symbolLookup));
            _currencies[332] = new CurrencyTableEntry("Gourde", "HTG", 332, lookupString("HTG", symbolLookup));
            _currencies[340] = new CurrencyTableEntry("Lempira", "HNL", 340, lookupString("HNL", symbolLookup));
            _currencies[344] = new CurrencyTableEntry("Hong Kong Dollar", "HKD", 344, lookupString("HKD", symbolLookup));
            _currencies[348] = new CurrencyTableEntry("Forint", "HUF", 348, lookupString("HUF", symbolLookup));
            _currencies[352] = new CurrencyTableEntry("Iceland Krona", "ISK", 352, lookupString("ISK", symbolLookup));
            _currencies[356] = new CurrencyTableEntry("Indian Rupee", "INR", 356, lookupString("INR", symbolLookup));
            _currencies[360] = new CurrencyTableEntry("Rupiah", "IDR", 360, lookupString("IDR", symbolLookup));
            _currencies[364] = new CurrencyTableEntry("Iranian Rial", "IRR", 364, lookupString("IRR", symbolLookup));
            _currencies[368] = new CurrencyTableEntry("Iraqi Dinar", "IQD", 368, lookupString("IQD", symbolLookup));
            _currencies[376] = new CurrencyTableEntry("New Israeli Sheqel", "ILS", 376, lookupString("ILS", symbolLookup));
            _currencies[388] = new CurrencyTableEntry("Jamaican Dollar", "JMD", 388, lookupString("JMD", symbolLookup));
            _currencies[392] = new CurrencyTableEntry("Yen", "JPY", 392, lookupString("JPY", symbolLookup));
            _currencies[398] = new CurrencyTableEntry("Tenge", "KZT", 398, lookupString("KZT", symbolLookup));
            _currencies[400] = new CurrencyTableEntry("Jordanian Dinar", "JOD", 400, lookupString("JOD", symbolLookup));
            _currencies[404] = new CurrencyTableEntry("Kenyan Shilling", "KES", 404, lookupString("KES", symbolLookup));
            _currencies[408] = new CurrencyTableEntry("North Korean Won", "KPW", 408, lookupString("KPW", symbolLookup));
            _currencies[410] = new CurrencyTableEntry("Won", "KRW", 410, lookupString("KRW", symbolLookup));
            _currencies[414] = new CurrencyTableEntry("Kuwaiti Dinar", "KWD", 414, lookupString("KWD", symbolLookup));
            _currencies[417] = new CurrencyTableEntry("Som", "KGS", 417, lookupString("KGS", symbolLookup));
            _currencies[418] = new CurrencyTableEntry("Kip", "LAK", 418, lookupString("LAK", symbolLookup));
            _currencies[422] = new CurrencyTableEntry("Lebanese Pound", "LBP", 422, lookupString("LBP", symbolLookup));
            _currencies[428] = new CurrencyTableEntry("Latvian Lats", "LVL", 428, lookupString("LVL", symbolLookup));
            _currencies[430] = new CurrencyTableEntry("Liberian Dollar", "LRD", 430, lookupString("LRD", symbolLookup));
            _currencies[434] = new CurrencyTableEntry("Libyan Dinar", "LYD", 434, lookupString("LYD", symbolLookup));
            _currencies[440] = new CurrencyTableEntry("Lithuanian Litas", "LTL", 440, lookupString("LTL", symbolLookup));
            _currencies[446] = new CurrencyTableEntry("Pataca", "MOP", 446, lookupString("MOP", symbolLookup));
            _currencies[454] = new CurrencyTableEntry("Kwacha", "MWK", 454, lookupString("MWK", symbolLookup));
            _currencies[458] = new CurrencyTableEntry("Malaysian Ringgit", "MYR", 458, lookupString("MYR", symbolLookup));
            _currencies[462] = new CurrencyTableEntry("Rufiyaa", "MVR", 462, lookupString("MVR", symbolLookup));
            _currencies[478] = new CurrencyTableEntry("Ouguiya", "MRO", 478, lookupString("MRO", symbolLookup));
            _currencies[480] = new CurrencyTableEntry("Mauritius Rupee", "MUR", 480, lookupString("MUR", symbolLookup));
            _currencies[484] = new CurrencyTableEntry("Mexican Peso", "MXN", 484, lookupString("MXN", symbolLookup));
            _currencies[496] = new CurrencyTableEntry("Tugrik", "MNT", 496, lookupString("MNT", symbolLookup));
            _currencies[498] = new CurrencyTableEntry("Moldovan Leu", "MDL", 498, lookupString("MDL", symbolLookup));
            _currencies[504] = new CurrencyTableEntry("Moroccan Dirham", "MAD", 504, lookupString("MAD", symbolLookup));
            _currencies[512] = new CurrencyTableEntry("Rial Omani", "OMR", 512, lookupString("OMR", symbolLookup));
            _currencies[524] = new CurrencyTableEntry("Nepalese Rupee", "NPR", 524, lookupString("NPR", symbolLookup));
            _currencies[532] = new CurrencyTableEntry("Netherlands Antillian Guilder", "ANG", 532, lookupString("ANG", symbolLookup));
            _currencies[533] = new CurrencyTableEntry("Aruban Guilder", "AWG", 533, lookupString("AWG", symbolLookup));
            _currencies[548] = new CurrencyTableEntry("Vatu", "VUV", 548, lookupString("VUV", symbolLookup));
            _currencies[554] = new CurrencyTableEntry("New Zealand Dollar", "NZD", 554, lookupString("NZD", symbolLookup));
            _currencies[558] = new CurrencyTableEntry("Cordoba Oro", "NIO", 558, lookupString("NIO", symbolLookup));
            _currencies[566] = new CurrencyTableEntry("Naira", "NGN", 566, lookupString("NGN", symbolLookup));
            _currencies[578] = new CurrencyTableEntry("Norwegian Krone", "NOK", 578, lookupString("NOK", symbolLookup));
            _currencies[586] = new CurrencyTableEntry("Pakistan Rupee", "PKR", 586, lookupString("PKR", symbolLookup));
            _currencies[590] = new CurrencyTableEntry("Balboa", "PAB", 590, lookupString("PAB", symbolLookup));
            _currencies[598] = new CurrencyTableEntry("Kina", "PGK", 598, lookupString("PGK", symbolLookup));
            _currencies[600] = new CurrencyTableEntry("Guarani", "PYG", 600, lookupString("PYG", symbolLookup));
            _currencies[604] = new CurrencyTableEntry("Nuevo Sol", "PEN", 604, lookupString("PEN", symbolLookup));
            _currencies[608] = new CurrencyTableEntry("Philippine Peso", "PHP", 608, lookupString("PHP", symbolLookup));
            _currencies[624] = new CurrencyTableEntry("Guinea-Bissau Peso", "GWP", 624, lookupString("GWP", symbolLookup));
            _currencies[634] = new CurrencyTableEntry("Qatari Rial", "QAR", 634, lookupString("QAR", symbolLookup));
            _currencies[643] = new CurrencyTableEntry("Russian Ruble", "RUB", 643, lookupString("RUB", symbolLookup));
            _currencies[646] = new CurrencyTableEntry("Rwanda Franc", "RWF", 646, lookupString("RWF", symbolLookup));
            _currencies[654] = new CurrencyTableEntry("Saint Helena Pound", "SHP", 654, lookupString("SHP", symbolLookup));
            _currencies[678] = new CurrencyTableEntry("Dobra", "STD", 678, lookupString("STD", symbolLookup));
            _currencies[682] = new CurrencyTableEntry("Saudi Riyal", "SAR", 682, lookupString("SAR", symbolLookup));
            _currencies[690] = new CurrencyTableEntry("Seychelles Rupee", "SCR", 690, lookupString("SCR", symbolLookup));
            _currencies[694] = new CurrencyTableEntry("Leone", "SLL", 694, lookupString("SLL", symbolLookup));
            _currencies[702] = new CurrencyTableEntry("Singapore Dollar", "SGD", 702, lookupString("SGD", symbolLookup));
            _currencies[703] = new CurrencyTableEntry("Slovak Koruna", "SKK", 703, lookupString("SKK", symbolLookup));
            _currencies[704] = new CurrencyTableEntry("Dong", "VND", 704, lookupString("VND", symbolLookup));
            _currencies[706] = new CurrencyTableEntry("Somali Shilling", "SOS", 706, lookupString("SOS", symbolLookup));
            _currencies[710] = new CurrencyTableEntry("Rand", "ZAR", 710, lookupString("ZAR", symbolLookup));
            _currencies[716] = new CurrencyTableEntry("Zimbabwe Dollar", "ZWD", 716, lookupString("ZWD", symbolLookup));
            _currencies[748] = new CurrencyTableEntry("Lilangeni", "SZL", 748, lookupString("SZL", symbolLookup));
            _currencies[752] = new CurrencyTableEntry("Swedish Krona", "SEK", 752, lookupString("SEK", symbolLookup));
            _currencies[756] = new CurrencyTableEntry("Swiss Franc", "CHF", 756, lookupString("CHF", symbolLookup));
            _currencies[760] = new CurrencyTableEntry("Syrian Pound", "SYP", 760, lookupString("SYP", symbolLookup));
            _currencies[764] = new CurrencyTableEntry("Baht", "THB", 764, lookupString("THB", symbolLookup));
            _currencies[776] = new CurrencyTableEntry("Pa'anga", "TOP", 776, lookupString("TOP", symbolLookup));
            _currencies[780] = new CurrencyTableEntry("Trinidad and Tobago Dollar", "TTD", 780, lookupString("TTD", symbolLookup));
            _currencies[784] = new CurrencyTableEntry("UAE Dirham", "AED", 784, lookupString("AED", symbolLookup));
            _currencies[788] = new CurrencyTableEntry("Tunisian Dinar", "TND", 788, lookupString("TND", symbolLookup));
            _currencies[795] = new CurrencyTableEntry("Manat", "TMM", 795, lookupString("TMM", symbolLookup));
            _currencies[800] = new CurrencyTableEntry("Uganda Shilling", "UGX", 800, lookupString("UGX", symbolLookup));
            _currencies[807] = new CurrencyTableEntry("Denar", "MKD", 807, lookupString("MKD", symbolLookup));
            _currencies[818] = new CurrencyTableEntry("Egyptian Pound", "EGP", 818, lookupString("EGP", symbolLookup));
            _currencies[826] = new CurrencyTableEntry("Pound Sterling", "GBP", 826, lookupString("GBP", symbolLookup));
            _currencies[834] = new CurrencyTableEntry("Tanzanian Shilling", "TZS", 834, lookupString("TZS", symbolLookup));
            _currencies[840] = new CurrencyTableEntry("US Dollar", "USD", 840, lookupString("USD", symbolLookup));
            _currencies[858] = new CurrencyTableEntry("Peso Uruguayo", "UYU", 858, lookupString("UYU", symbolLookup));
            _currencies[860] = new CurrencyTableEntry("Uzbekistan Sum", "UZS", 860, lookupString("UZS", symbolLookup));
            _currencies[882] = new CurrencyTableEntry("Tala", "WST", 882, lookupString("WST", symbolLookup));
            _currencies[886] = new CurrencyTableEntry("Yemeni Rial", "YER", 886, lookupString("YER", symbolLookup));
            _currencies[894] = new CurrencyTableEntry("Kwacha", "ZMK", 894, lookupString("ZMK", symbolLookup));
            _currencies[901] = new CurrencyTableEntry("New Taiwan Dollar", "TWD", 901, lookupString("TWD", symbolLookup));
            _currencies[936] = new CurrencyTableEntry("Ghana Cedi", "GHS", 936, lookupString("GHS", symbolLookup));
            _currencies[937] = new CurrencyTableEntry("Bolivar Fuerte", "VEF", 937, lookupString("VEF", symbolLookup));
            _currencies[938] = new CurrencyTableEntry("Sudanese Pound", "SDG", 938, lookupString("SDG", symbolLookup));
            _currencies[941] = new CurrencyTableEntry("Serbian Dinar", "RSD", 941, lookupString("RSD", symbolLookup));
            _currencies[943] = new CurrencyTableEntry("Metical", "MZN", 943, lookupString("MZN", symbolLookup));
            _currencies[944] = new CurrencyTableEntry("Azerbaijanian Manat", "AZN", 944, lookupString("AZN", symbolLookup));
            _currencies[946] = new CurrencyTableEntry("New Leu", "RON", 946, lookupString("RON", symbolLookup));
            _currencies[949] = new CurrencyTableEntry("New Turkish Lira", "TRY", 949, lookupString("TRY", symbolLookup));
            _currencies[950] = new CurrencyTableEntry("CFA Franc BEAC", "XAF", 950, lookupString("XAF", symbolLookup));
            _currencies[951] = new CurrencyTableEntry("East Caribbean Dollar", "XCD", 951, lookupString("XCD", symbolLookup));
            _currencies[952] = new CurrencyTableEntry("CFA Franc BCEAO", "XOF", 952, lookupString("XOF", symbolLookup));
            _currencies[953] = new CurrencyTableEntry("CFP Franc", "XPF", 953, lookupString("XPF", symbolLookup));
            _currencies[955] = new CurrencyTableEntry("Bond Markets Units European Composite Unit (EURCO)", "XBA", 955, lookupString("XBA", symbolLookup));
            _currencies[956] = new CurrencyTableEntry("European Monetary Unit (E.M.U.-6)", "XBB", 956, lookupString("XBB", symbolLookup));
            _currencies[957] = new CurrencyTableEntry("European Unit of Account 9(E.U.A.-9)", "XBC", 957, lookupString("XBC", symbolLookup));
            _currencies[958] = new CurrencyTableEntry("European Unit of Account 17(E.U.A.-17)", "XBD", 958, lookupString("XBD", symbolLookup));
            _currencies[959] = new CurrencyTableEntry("Gold", "XAU", 959, lookupString("XAU", symbolLookup));
            _currencies[960] = new CurrencyTableEntry("SDR", "XDR", 960, lookupString("XDR", symbolLookup));
            _currencies[961] = new CurrencyTableEntry("Silver", "XAG", 961, lookupString("XAG", symbolLookup));
            _currencies[962] = new CurrencyTableEntry("Platinum", "XPT", 962, lookupString("XPT", symbolLookup));
            _currencies[963] = new CurrencyTableEntry("Codes specifically reserved for testing purposes", "XTS", 963, lookupString("XTS", symbolLookup));
            _currencies[964] = new CurrencyTableEntry("Palladium", "XPD", 964, lookupString("XPD", symbolLookup));
            _currencies[968] = new CurrencyTableEntry("Surinam Dollar", "SRD", 968, lookupString("SRD", symbolLookup));
            _currencies[969] = new CurrencyTableEntry("Malagasy Ariary", "MGA", 969, lookupString("MGA", symbolLookup));
            _currencies[971] = new CurrencyTableEntry("Afghani", "AFN", 971, lookupString("AFN", symbolLookup));
            _currencies[972] = new CurrencyTableEntry("Somoni", "TJS", 972, lookupString("TJS", symbolLookup));
            _currencies[973] = new CurrencyTableEntry("Kwanza", "AOA", 973, lookupString("AOA", symbolLookup));
            _currencies[974] = new CurrencyTableEntry("Belarussian Ruble", "BYR", 974, lookupString("BYR", symbolLookup));
            _currencies[975] = new CurrencyTableEntry("Bulgarian Lev", "BGN", 975, lookupString("BGN", symbolLookup));
            _currencies[976] = new CurrencyTableEntry("Franc Congolais", "CDF", 976, lookupString("CDF", symbolLookup));
            _currencies[977] = new CurrencyTableEntry("Convertible Marks", "BAM", 977, lookupString("BAM", symbolLookup));
            _currencies[978] = new CurrencyTableEntry("Euro", "EUR", 978, lookupString("EUR", symbolLookup));
            _currencies[980] = new CurrencyTableEntry("Hryvnia", "UAH", 980, lookupString("UAH", symbolLookup));
            _currencies[981] = new CurrencyTableEntry("Lari", "GEL", 981, lookupString("GEL", symbolLookup));
            _currencies[985] = new CurrencyTableEntry("Zloty", "PLN", 985, lookupString("PLN", symbolLookup));
            _currencies[986] = new CurrencyTableEntry("Brazilian Real", "BRL", 986, lookupString("BRL", symbolLookup));
            _currencies[999] = new CurrencyTableEntry("The codes assigned for transactions where no currency is involved are:", "XXX", 999, lookupString("XXX", symbolLookup));
            #endregion

            foreach (CurrencyTableEntry currency in _currencies.Values)
            {
                String iso3LetterCode = currency.Iso3LetterCode;
                List<Int32> lcids;

                if (cultureIdLookup.TryGetValue(iso3LetterCode, out lcids))
                {
                    foreach (Int32 lcid in lcids)
                    {
                        _cultureIdLookup[lcid] = currency.IsoNumberCode;
                    }
                }

                _codeIndex[iso3LetterCode] = currency.IsoNumberCode;
            }

            All = new Currency(008);
            Dzd = new Currency(012);
            Ars = new Currency(032);
            Aud = new Currency(036);
            Bsd = new Currency(044);
            Bhd = new Currency(048);
            Bdt = new Currency(050);
            Amd = new Currency(051);
            Bbd = new Currency(052);
            Bmd = new Currency(060);
            Bob = new Currency(068);
            Bwp = new Currency(072);
            Bzd = new Currency(084);
            Sbd = new Currency(090);
            Bnd = new Currency(096);
            Mmk = new Currency(104);
            Bif = new Currency(108);
            Khr = new Currency(116);
            Cad = new Currency(124);
            Cve = new Currency(132);
            Kyd = new Currency(136);
            Lkr = new Currency(144);
            Clp = new Currency(152);
            Cny = new Currency(156);
            Cop = new Currency(170);
            Kmf = new Currency(174);
            Crc = new Currency(188);
            Hrk = new Currency(191);
            Cup = new Currency(192);
            Czk = new Currency(203);
            Dkk = new Currency(208);
            Dop = new Currency(214);
            Svc = new Currency(222);
            Etb = new Currency(230);
            Ern = new Currency(232);
            Eek = new Currency(233);
            Fkp = new Currency(238);
            Fjd = new Currency(242);
            Djf = new Currency(262);
            Gmd = new Currency(270);
            Gip = new Currency(292);
            Gtq = new Currency(320);
            Gnf = new Currency(324);
            Gyd = new Currency(328);
            Htg = new Currency(332);
            Hnl = new Currency(340);
            Hkd = new Currency(344);
            Huf = new Currency(348);
            Isk = new Currency(352);
            Inr = new Currency(356);
            Idr = new Currency(360);
            Irr = new Currency(364);
            Iqd = new Currency(368);
            Ils = new Currency(376);
            Jmd = new Currency(388);
            Jpy = new Currency(392);
            Kzt = new Currency(398);
            Jod = new Currency(400);
            Kes = new Currency(404);
            Kpw = new Currency(408);
            Krw = new Currency(410);
            Kwd = new Currency(414);
            Kgs = new Currency(417);
            Lak = new Currency(418);
            Lbp = new Currency(422);
            Lvl = new Currency(428);
            Lrd = new Currency(430);
            Lyd = new Currency(434);
            Ltl = new Currency(440);
            Mop = new Currency(446);
            Mwk = new Currency(454);
            Myr = new Currency(458);
            Mvr = new Currency(462);
            Mro = new Currency(478);
            Mur = new Currency(480);
            Mxn = new Currency(484);
            Mnt = new Currency(496);
            Mdl = new Currency(498);
            Mad = new Currency(504);
            Omr = new Currency(512);
            Npr = new Currency(524);
            Ang = new Currency(532);
            Awg = new Currency(533);
            Vuv = new Currency(548);
            Nzd = new Currency(554);
            Nio = new Currency(558);
            Ngn = new Currency(566);
            Nok = new Currency(578);
            Pkr = new Currency(586);
            Pab = new Currency(590);
            Pgk = new Currency(598);
            Pyg = new Currency(600);
            Pen = new Currency(604);
            Php = new Currency(608);
            Gwp = new Currency(624);
            Qar = new Currency(634);
            Rub = new Currency(643);
            Rwf = new Currency(646);
            Shp = new Currency(654);
            Std = new Currency(678);
            Sar = new Currency(682);
            Scr = new Currency(690);
            Sll = new Currency(694);
            Sgd = new Currency(702);
            Skk = new Currency(703);
            Vnd = new Currency(704);
            Sos = new Currency(706);
            Zar = new Currency(710);
            Zwd = new Currency(716);
            Szl = new Currency(748);
            Sek = new Currency(752);
            Chf = new Currency(756);
            Syp = new Currency(760);
            Thb = new Currency(764);
            Top = new Currency(776);
            Ttd = new Currency(780);
            Aed = new Currency(784);
            Tnd = new Currency(788);
            Tmm = new Currency(795);
            Ugx = new Currency(800);
            Mkd = new Currency(807);
            Egp = new Currency(818);
            Gbp = new Currency(826);
            Tzs = new Currency(834);
            Usd = new Currency(840);
            Uyu = new Currency(858);
            Uzs = new Currency(860);
            Wst = new Currency(882);
            Yer = new Currency(886);
            Zmk = new Currency(894);
            Twd = new Currency(901);
            Ghs = new Currency(936);
            Vef = new Currency(937);
            Sdg = new Currency(938);
            Rsd = new Currency(941);
            Mzn = new Currency(943);
            Azn = new Currency(944);
            Ron = new Currency(946);
            Try = new Currency(949);
            Xaf = new Currency(950);
            Xcd = new Currency(951);
            Xof = new Currency(952);
            Xpf = new Currency(953);
            Xba = new Currency(955);
            Xbb = new Currency(956);
            Xbc = new Currency(957);
            Xbd = new Currency(958);
            Xau = new Currency(959);
            Xdr = new Currency(960);
            Xag = new Currency(961);
            Xpt = new Currency(962);
            Xts = new Currency(963);
            Xpd = new Currency(964);
            Srd = new Currency(968);
            Mga = new Currency(969);
            Afn = new Currency(971);
            Tjs = new Currency(972);
            Aoa = new Currency(973);
            Byr = new Currency(974);
            Bgn = new Currency(975);
            Cdf = new Currency(976);
            Bam = new Currency(977);
            Eur = new Currency(978);
            Uah = new Currency(980);
            Gel = new Currency(981);
            Pln = new Currency(985);
            Brl = new Currency(986);
            Xxx = new Currency(999);
        }

        /// <summary>
        /// Creates a <see cref="Currency"/> instance from the 
        /// <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="Currency"/> which corresponds
        /// to the current culture.
        /// </returns>
        public static Currency FromCurrentCulture()
        {
            return FromCultureInfo(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Creates a <see cref="Currency"/> instance from the 
        /// given <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="cultureInfo">
        /// The <see cref="CultureInfo"/> from which to create the currency.
        /// </param>
        /// <returns>
        /// The <see cref="Currency"/> which corresponds
        /// to <paramref name="cultureInfo"/>.
        /// </returns>
        public static Currency FromCultureInfo(CultureInfo cultureInfo)
        {
            Int32 currencyId;

            if (_cultureIdLookup.TryGetValue(cultureInfo.LCID, out currencyId))
            {
                return new Currency(currencyId);
            }

            throw new InvalidOperationException("Unknown culture: " + cultureInfo);
        }

        /// <summary>
        /// Creates a <see cref="Currency"/> instance from its
        /// 3-letter ISO 4217 code.
        /// </summary>
        /// <param name="code">The ISO 4217 3-letter currency code.</param>
        /// <returns>
        /// The <see cref="Currency"/> which corresponds
        /// to the ISO 4217 3-letter <paramref name="code"/>.
        /// </returns>
        public static Currency FromIso3LetterCode(String code)
        {
            return new Currency(code);
        }

        public static Boolean operator ==(Currency left, Currency right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(Currency left, Currency right)
        {
            return !left.Equals(right);
        }

        private readonly Int32 _id;

        public Currency(Int32 isoCurrencyCode)
        {
            if (!_currencies.ContainsKey(isoCurrencyCode))
            {
                throw new ArgumentOutOfRangeException("isoCurrencyCode",
                                                      isoCurrencyCode,
                                                      "The value isn't a valid " +
                                                      "ISO 4217 numeric currency code.");
            }

            _id = isoCurrencyCode;
        }

        public Currency(String iso3LetterCode)
        {
            Int32 currencyId;

            if (_codeIndex.TryGetValue(iso3LetterCode, out currencyId))
            {
                _id = currencyId;
                return;
            }

            throw new InvalidOperationException("Unknown currency code: " +
                                                iso3LetterCode);
        }

        public override Int32 GetHashCode()
        {
            return 609502847 ^ _id.GetHashCode();
        }

        public override Boolean Equals(Object obj)
        {
            if (!(obj is Currency))
            {
                return false;
            }

            Currency other = (Currency)obj;
            return Equals(other);
        }

        public override String ToString()
        {
            return String.Format("{0} ({1})", Name, Iso3LetterCode);
        }

        #region IEquatable<Currency> Members

        public Boolean Equals(Currency other)
        {
            return _id == other._id;
        }

        #endregion

        #region IFormatProvider Members

        public Object GetFormat(Type formatType)
        {
            return formatType == typeof(NumberFormatInfo)
                        ? new CultureInfo(_id).NumberFormat
                        : null;
        }

        #endregion

        public String Name
        {
            get
            {
                CurrencyTableEntry entry = getEntry(_id);

                return entry.Name;
            }
        }

        public String Symbol
        {
            get
            {
                CurrencyTableEntry entry = getEntry(_id);

                return entry.Symbol;
            }
        }

        public String Iso3LetterCode
        {
            get
            {
                CurrencyTableEntry entry = getEntry(_id);

                return entry.Iso3LetterCode;
            }
        }

        public Int32 IsoNumericCode
        {
            get
            {
                CurrencyTableEntry entry = getEntry(_id);

                return entry.IsoNumberCode;
            }
        }

        private static CurrencyTableEntry getEntry(Int32 id)
        {
            CurrencyTableEntry entry;

            if (!_currencies.TryGetValue(id, out entry))
            {
                throw new InvalidOperationException("Unknown currency: " + id);
            }

            return entry;
        }

        private static String lookupString(String key, IDictionary<String, String> table)
        {
            String value;

            return !table.TryGetValue(key, out value) ? null : value;
        }
    }
}
