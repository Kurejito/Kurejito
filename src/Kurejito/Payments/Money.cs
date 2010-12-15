using System.Runtime.Serialization;

namespace System {
    public struct Money : IEquatable<Money>,
                          IComparable<Money>,
                          IFormattable,
                          IConvertible {
        private const Decimal FractionScale = 1E9M;
        private readonly Currency _currency;
        private readonly Int32 _decimalFraction;
        private readonly Int64 _units;

        public Money(Decimal value) {
            checkValue(value);

            this._units = (Int64) value;
            this._decimalFraction = (Int32) Decimal.Round((value - this._units)*FractionScale);

            if (this._decimalFraction >= FractionScale) {
                this._units += 1;
                this._decimalFraction = this._decimalFraction - (Int32) FractionScale;
            }

            this._currency = Currency.FromCurrentCulture();
        }

        public Money(Decimal value, Currency currency)
            : this(value) {
            this._currency = currency;
        }

        private Money(Int64 units, Int32 fraction, Currency currency) {
            this._units = units;
            this._decimalFraction = fraction;
            this._currency = currency;
        }

        public Currency Currency {
            get { return this._currency; }
        }

        #region Implementation of IEquatable<Money>

        public Boolean Equals(Money other) {
            this.checkCurrencies(other);

            return this._units == other._units &&
                   this._decimalFraction == other._decimalFraction;
        }

        #endregion

        #region Implementation of IComparable<Money>

        public Int32 CompareTo(Money other) {
            this.checkCurrencies(other);

            var unitCompare = this._units.CompareTo(other._units);

            return unitCompare == 0
                       ? this._decimalFraction.CompareTo(other._decimalFraction)
                       : unitCompare;
        }

        #endregion

        #region Implementation of IFormattable

        public String ToString(String format, IFormatProvider formatProvider) {
            return this.computeValue().ToString(format, formatProvider);
        }

        #endregion

        #region Implementation of IConvertible

        public TypeCode GetTypeCode() {
            return TypeCode.Object;
        }

        public Boolean ToBoolean(IFormatProvider provider) {
            return this._units == 0 && this._decimalFraction == 0;
        }

        public Char ToChar(IFormatProvider provider) {
            throw new NotSupportedException();
        }

        public SByte ToSByte(IFormatProvider provider) {
            return (SByte) this.computeValue();
        }

        public Byte ToByte(IFormatProvider provider) {
            return (Byte) this.computeValue();
        }

        public Int16 ToInt16(IFormatProvider provider) {
            return (Int16) this.computeValue();
        }

        public UInt16 ToUInt16(IFormatProvider provider) {
            return (UInt16) this.computeValue();
        }

        public Int32 ToInt32(IFormatProvider provider) {
            return (Int32) this.computeValue();
        }

        public UInt32 ToUInt32(IFormatProvider provider) {
            return (UInt32) this.computeValue();
        }

        public Int64 ToInt64(IFormatProvider provider) {
            return (Int64) this.computeValue();
        }

        public UInt64 ToUInt64(IFormatProvider provider) {
            return (UInt64) this.computeValue();
        }

        public Single ToSingle(IFormatProvider provider) {
            return (Single) this.computeValue();
        }

        public Double ToDouble(IFormatProvider provider) {
            return (Double) this.computeValue();
        }

        public Decimal ToDecimal(IFormatProvider provider) {
            return this.computeValue();
        }

        public DateTime ToDateTime(IFormatProvider provider) {
            throw new NotSupportedException();
        }

        public String ToString(IFormatProvider provider) {
            return ((Decimal) this).ToString(provider);
        }

        public Object ToType(Type conversionType, IFormatProvider provider) {
            throw new NotSupportedException();
        }

        #endregion

        public static implicit operator Money(Byte value) {
            return new Money(value);
        }

        public static implicit operator Money(SByte value) {
            return new Money(value);
        }

        public static implicit operator Money(Single value) {
            return new Money((Decimal) value);
        }

        public static implicit operator Money(Double value) {
            return new Money((Decimal) value);
        }

        public static implicit operator Money(Decimal value) {
            return new Money(value);
        }

        public static implicit operator Decimal(Money value) {
            return value.computeValue();
        }

        public static implicit operator Money(Int16 value) {
            return new Money(value);
        }

        public static implicit operator Money(Int32 value) {
            return new Money(value);
        }

        public static implicit operator Money(Int64 value) {
            return new Money(value);
        }

        public static implicit operator Money(UInt16 value) {
            return new Money(value);
        }

        public static implicit operator Money(UInt32 value) {
            return new Money(value);
        }

        public static implicit operator Money(UInt64 value) {
            return new Money(value);
        }

        public static Money operator -(Money value) {
            return new Money(-value._units, -value._decimalFraction, value._currency);
        }

        public static Money operator +(Money left, Money right) {
            if (left.Currency != right.Currency) {
                throw differentCurrencies();
            }

            var fractionSum = left._decimalFraction + right._decimalFraction;

            Int64 overflow = 0;
            var fractionSign = Math.Sign(fractionSum);
            var absFractionSum = Math.Abs(fractionSum);

            if (absFractionSum >= FractionScale) {
                overflow = fractionSign;
                absFractionSum -= (Int32) FractionScale;
                fractionSum = fractionSign*absFractionSum;
            }

            var newUnits = left._units + right._units + overflow;

            if (fractionSign < 0 && Math.Sign(newUnits) > 0) {
                newUnits -= 1;
                fractionSum = (Int32) FractionScale - absFractionSum;
            }

            return new Money(newUnits,
                             fractionSum,
                             left.Currency);
        }

        public static Money operator -(Money left, Money right) {
            if (left.Currency != right.Currency) {
                throw differentCurrencies();
            }

            return left + -right;
        }

        public static Money operator *(Money left, Decimal right) {
            return ((Decimal) left*right);
        }

        public static Money operator /(Money left, Decimal right) {
            return ((Decimal) left/right);
        }

        public static Boolean operator ==(Money left, Money right) {
            return left.Equals(right);
        }

        public static Boolean operator !=(Money left, Money right) {
            return !left.Equals(right);
        }

        public static Boolean operator >(Money left, Money right) {
            return left.CompareTo(right) > 0;
        }

        public static Boolean operator <(Money left, Money right) {
            return left.CompareTo(right) < 0;
        }

        public static Boolean operator >=(Money left, Money right) {
            return left.CompareTo(right) >= 0;
        }

        public static Boolean operator <=(Money left, Money right) {
            return left.CompareTo(right) <= 0;
        }

        public override Int32 GetHashCode() {
            return 207501131 ^ this._units.GetHashCode() ^ this._currency.GetHashCode();
        }

        public override Boolean Equals(Object obj) {
            if (!(obj is Money)) {
                return false;
            }

            var other = (Money) obj;
            return this.Equals(other);
        }

        public override String ToString() {
            return this.computeValue().ToString("C");
        }

        public String ToString(String format) {
            return this.computeValue().ToString(format);
        }

        private Decimal computeValue() {
            return this._units + this._decimalFraction/FractionScale;
        }

        private static Exception differentCurrencies() {
            return new InvalidOperationException("Money values are in different " +
                                                 "currencies. Convert to the same " +
                                                 "currency before performing " +
                                                 "operations on the values.");
        }

        private static void checkValue(Decimal value) {
            if (value < Int64.MinValue || value > Int64.MaxValue) {
                throw new ArgumentOutOfRangeException("value",
                                                      value,
                                                      "Money value must be between " +
                                                      Int64.MinValue + " and " +
                                                      Int64.MaxValue);
            }
        }

        private void checkCurrencies(Money other) {
            if (other.Currency != this.Currency) {
                throw differentCurrencies();
            }
        }
                          }

    namespace System {
        public enum FractionReceivers {
            FirstToLast,
            LastToFirst,
            Random,
        }
    }

    [Serializable]
    public class MoneyAllocationException : Exception {
        private readonly Money _amountToDistribute;
        private readonly Decimal[] _distribution;
        private readonly Money _distributionTotal;

        public MoneyAllocationException(Money amountToDistribute,
                                        Money distributionTotal,
                                        Decimal[] distribution) {
            this._amountToDistribute = amountToDistribute;
            this._distribution = distribution;
            this._distributionTotal = distributionTotal;
        }

        public MoneyAllocationException(Money amountToDistribute,
                                        Money distributionTotal,
                                        Decimal[] distribution,
                                        String message)
            : base(message) {
            this._amountToDistribute = amountToDistribute;
            this._distribution = distribution;
            this._distributionTotal = distributionTotal;
        }

        public MoneyAllocationException(Money amountToDistribute,
                                        Money distributionTotal,
                                        Decimal[] distribution,
                                        String message,
                                        Exception inner)
            : base(message, inner) {
            this._amountToDistribute = amountToDistribute;
            this._distribution = distribution;
            this._distributionTotal = distributionTotal;
        }

        protected MoneyAllocationException(SerializationInfo info,
                                           StreamingContext context)
            : base(info, context) {
            this._amountToDistribute = (Money) info.GetValue("_amountToDistribute",
                                                             typeof (Money));
            this._distributionTotal = (Money) info.GetValue("_distributionTotal",
                                                            typeof (Money));
            this._distribution = (Decimal[]) info.GetValue("_distribution",
                                                           typeof (Decimal[]));
        }

        public Decimal[] Distribution {
            get { return this._distribution; }
        }

        public Money DistributionTotal {
            get { return this._distributionTotal; }
        }

        public Money AmountToDistribute {
            get { return this._amountToDistribute; }
        }
    }

    public enum RoundingPlaces {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine
    }
}