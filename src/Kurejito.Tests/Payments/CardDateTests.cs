using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Kurejito.Payments;

namespace Kurejito.Tests.Payments {
	public class CardDateTests {

		[Fact]
		public void Verify_CardDate_Is_Implicitly_Assigned_From_M_slash_Y_String() {
		    CardDate cd = "5/8";
		    Assert.Equal("05", cd.TwoDigitMonth);
		    Assert.Equal("08", cd.TwoDigitYear);
		}

		[Fact]
		public void Verify_CardDate_Parses_MMYY_String_Correctly() {
		    var cd = CardDate.Parse("0508");
		    Assert.Equal("05", cd.TwoDigitMonth);
		    Assert.Equal("08", cd.TwoDigitYear);
		}

		[Fact]
		public void Verify_CardDate_Constructed_From_Int_Int_Is_Correct() {
			var cd = new CardDate(5, 8);
			Assert.Equal("05", cd.TwoDigitMonth);
			Assert.Equal("08", cd.TwoDigitYear);
		}

		[Fact]
		public void Verify_Year_Below_Threshold_Is_Interpreted_As_This_Century() {
			var cd = new CardDate(8, 15);
			Assert.Equal(cd.Year, 2015);
		}

		[Fact]
		public void Verify_Year_Below_Threshold_Is_Interpreted_As_Last_Century() {
			var cd = new CardDate(8, 95);
			Assert.Equal(1995, cd.Year);
		}

		[Fact]
		public void Verify_Year_With_Century_Is_Interpreted_Correctly() {
			var cd = new CardDate(8, 2010);
			Assert.Equal(2010, cd.Year);
		}

		[Fact]
		public void Verify_CardDate_Is_Assigned_From_MMYY_String() {
		    CardDate cd = "0508";
		    Assert.Equal("05", cd.TwoDigitMonth);
		    Assert.Equal("08", cd.TwoDigitYear);
		}

		[Fact]
		public void Verify_CardDate_ToString_Works_On_0508() {
			CardDate cd = "0508";
			Assert.Equal("0508", cd.ToString());
		}

		[Fact]
		public void Verify_CardDate_ToString_Works_On_1010() {
			CardDate cd = "1010";
			Assert.Equal("1010", cd.ToString());
		}

		[Fact]
		public void Verify_CardDate_ToString_Works_On_0899() {
			CardDate cd = new CardDate(8, 1999);
			Assert.Equal("0899", cd.ToString());
		}

	}
}
