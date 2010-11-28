using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kurejito.Gateways.DataCash;
using Kurejito.Transport;
using Moq;
using Xunit;

namespace Kurejito.Tests.Gateways.DataCash {
	public class DataCashResponseStatusTests : DataCashTestBase {
		private PaymentResponse Get_Mock_DataCash_Response(int dataCashReturnCode, string dataCashReason) {
			var http = new Mock<IHttpPostTransport>();
			http
				.Setup(h => h.Post(It.IsAny<Uri>(), It.IsAny<string>()))
				.Returns(this.MakeXmlResponse(dataCashReturnCode, dataCashReason));

			var gw = new DataCashPaymentGateway(http.Object, CLIENT_ID, PASSWORD, GATEWAY_URI);
			return (gw.Purchase(TestData.MerchantReference, TestData.Amount, TestData.Currency, TestData.Card));
		}

		private void Verify_Return_Code_Translation_From_DataCash(int returnCode, PaymentStatus expectedStatus, string reason) {

			var response = this.Get_Mock_DataCash_Response(returnCode, reason);
			Assert.Equal(expectedStatus, response.Status);
		}

		[Fact]
		public void Return_Code_1_Converts_To_PaymentStatus_Ok() { Verify_Return_Code_Translation_From_DataCash(1, PaymentStatus.Ok, "Success"); }
		[Fact]
		public void Return_Code_2_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(2, PaymentStatus.Error, "Socket write error"); }
		[Fact]
		public void Return_Code_3_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(3, PaymentStatus.Error, "Timeout"); }
		[Fact]
		public void Return_Code_5_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(5, PaymentStatus.Invalid, "Edit error"); }
		[Fact]
		public void Return_Code_6_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(6, PaymentStatus.Error, "Comms error"); }
		[Fact]
		public void Return_Code_7_Converts_To_PaymentStatus_Declined() { Verify_Return_Code_Translation_From_DataCash(7, PaymentStatus.Declined, "Not authorised"); }
		[Fact]
		public void Return_Code_9_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(9, PaymentStatus.Invalid, "Currency error"); }
		[Fact]
		public void Return_Code_10_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(10, PaymentStatus.Error, "Authentication error"); }
		[Fact]
		public void Return_Code_12_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(12, PaymentStatus.Invalid, "Invalid authorisation code"); }
		[Fact]
		public void Return_Code_13_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(13, PaymentStatus.Invalid, "Type field missing"); }
		[Fact]
		public void Return_Code_14_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(14, PaymentStatus.Error, "Database server error"); }
		[Fact]
		public void Return_Code_15_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(15, PaymentStatus.Invalid, "Invalid type"); }
		[Fact]
		public void Return_Code_19_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(19, PaymentStatus.Invalid, "Cannot fulfill transaction"); }
		[Fact]
		public void Return_Code_20_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(20, PaymentStatus.Invalid, "Duplicate transaction reference"); }
		[Fact]
		public void Return_Code_21_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(21, PaymentStatus.Invalid, "Invalid card type"); }
		[Fact]
		public void Return_Code_22_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(22, PaymentStatus.Invalid, "Invalid reference"); }
		[Fact]
		public void Return_Code_23_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(23, PaymentStatus.Invalid, "Expiry date invalid"); }
		[Fact]
		public void Return_Code_24_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(24, PaymentStatus.Invalid, "Card has already expired"); }
		[Fact]
		public void Return_Code_25_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(25, PaymentStatus.Invalid, "Card number invalid"); }
		[Fact]
		public void Return_Code_26_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(26, PaymentStatus.Invalid, "Card number wrong length"); }
		[Fact]
		public void Return_Code_27_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(27, PaymentStatus.Invalid, "Issue number error"); }
		[Fact]
		public void Return_Code_28_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(28, PaymentStatus.Invalid, "Start date error"); }
		[Fact]
		public void Return_Code_29_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(29, PaymentStatus.Invalid, "Card is not valid yet"); }
		[Fact]
		public void Return_Code_30_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(30, PaymentStatus.Invalid, "Start date after expiry date"); }
		[Fact]
		public void Return_Code_34_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(34, PaymentStatus.Invalid, "Invalid amount"); }
		[Fact]
		public void Return_Code_40_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(40, PaymentStatus.Invalid, "Invalid cheque type"); }
		[Fact]
		public void Return_Code_41_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(41, PaymentStatus.Invalid, "Invalid cheque number"); }
		[Fact]
		public void Return_Code_42_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(42, PaymentStatus.Invalid, "Invalid sort code"); }
		[Fact]
		public void Return_Code_44_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(44, PaymentStatus.Invalid, "Invalid account number"); }
		[Fact]
		public void Return_Code_51_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(51, PaymentStatus.Invalid, "Reference in use"); }
		[Fact]
		public void Return_Code_53_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(53, PaymentStatus.Error, "No free TIDs available for this vTID"); }
		[Fact]
		public void Return_Code_56_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(56, PaymentStatus.Invalid, "Card used too recently"); }
		[Fact]
		public void Return_Code_57_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(57, PaymentStatus.Invalid, "Invalid velocity_check value"); }
		[Fact]
		public void Return_Code_59_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(59, PaymentStatus.Invalid, "This combination of currency, card type and environment is not supported by this vTID"); }
		[Fact]
		public void Return_Code_60_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(60, PaymentStatus.Invalid, "Invalid XML"); }
		[Fact]
		public void Return_Code_61_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(61, PaymentStatus.Error, "Configuration error"); }
		[Fact]
		public void Return_Code_62_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(62, PaymentStatus.Error, "Unsupported protocol"); }
		[Fact]
		public void Return_Code_63_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(63, PaymentStatus.Invalid, "Method not supported by acquirer"); }
		[Fact]
		public void Return_Code_104_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(104, PaymentStatus.Error, "APACS30: WRONG TID"); }
		[Fact]
		public void Return_Code_105_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(105, PaymentStatus.Error, "APACS30: MSG SEQ NUM ERR"); }
		[Fact]
		public void Return_Code_106_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(106, PaymentStatus.Error, "APACS30: WRONG AMOUNT"); }
		[Fact]
		public void Return_Code_190_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(190, PaymentStatus.Error, "No capture method specified"); }
		[Fact]
		public void Return_Code_280_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(280, PaymentStatus.Invalid, "Unknown format of datacash reference"); }
		[Fact]
		public void Return_Code_281_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(281, PaymentStatus.Invalid, "Datacash reference fails Luhn check"); }
		[Fact]
		public void Return_Code_282_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(282, PaymentStatus.Invalid, "Mismatch between historic and current site_id"); }
		[Fact]
		public void Return_Code_283_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(283, PaymentStatus.Invalid, "Mismatch between historic and current modes"); }
		[Fact]
		public void Return_Code_440_Converts_To_PaymentStatus_Error() { Verify_Return_Code_Translation_From_DataCash(440, PaymentStatus.Error, "Payment Gateway Busy"); }
		[Fact]
		public void Return_Code_470_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(470, PaymentStatus.Invalid, "Maestro txns for CNP not supported for clearinghouse"); }
		[Fact]
		public void Return_Code_471_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(471, PaymentStatus.Invalid, "3-D Secure Required"); }
		[Fact]
		public void Return_Code_472_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(472, PaymentStatus.Invalid, "Invalid capturemethod"); }
		[Fact]
		public void Return_Code_473_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(473, PaymentStatus.Invalid, "Invalid transaction type"); }
		[Fact]
		public void Return_Code_480_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(480, PaymentStatus.Invalid, "Invalid value for merchantid"); }
		[Fact]
		public void Return_Code_481_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(481, PaymentStatus.Invalid, "Element merchantid required"); }
		[Fact]
		public void Return_Code_482_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(482, PaymentStatus.Invalid, "Invalid element merchantid"); }
		[Fact]
		public void Return_Code_510_Converts_To_PaymentStatus_Invalid() { Verify_Return_Code_Translation_From_DataCash(510, PaymentStatus.Invalid, "GE Capital: Inappropriate GE Capital card number"); }
		// ReSharper restore InconsistentNaming
	}
}

