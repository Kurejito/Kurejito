using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Diagnostics;

namespace Kurejito.Tests.Encoding {

	public class Base32EncodingTests {
		[Fact]
		public void Base32_Encode_Zero_Returns_Zero() {
			var bytes = new byte[] { 0 };
			Assert.Equal("0", Base32.Encode(bytes));
		}

		[Fact]
		public void Base32_Encode_One_Returns_One() {
			var bytes = new byte[] { 1 };
			Assert.Equal("1", Base32.Encode(bytes));
		}

		[Fact]
		public void Base32_Encode_Sixteen_Returns_g() {
			var bytes = new byte[] { 16 };
			Assert.Equal("g", Base32.Encode(bytes));
		}

		[Fact]
		public void Base32_Encode_31_Returns_g() {
			var bytes = new byte[] { 31 };
			Assert.Equal("z", Base32.Encode(bytes));
		}


		[Fact]
		public void Base32_Encode_MaxValue_Returns_Zero() {

			for(var i = 0; i < 1026; i++) Console.WriteLine("{0} ==> {1}", i, Base32.Encode(i));

			Guid guid;
			guid = new Guid("00000000000000000000000000000000");
			Console.WriteLine(Base32.Encode(guid));

			guid = new Guid("00000000000000000000000000000001");
			Console.WriteLine(Base32.Encode(guid));

			guid = new Guid("00000000000000000000000000000002");
			Console.WriteLine(Base32.Encode(guid));

			guid = new Guid("00000000000000000000000000000003");
			Console.WriteLine(Base32.Encode(guid));

			guid = new Guid("0000000000000000000000000000000f");
			Console.WriteLine(Base32.Encode(guid));

			guid = new Guid("00000000000000000000000000000010");
			Console.WriteLine(Base32.Encode(guid));

			guid = new Guid("ffffffffffffffffffffffffffffffff");
			Console.WriteLine(Base32.Encode(guid));

			guid = new Guid("0000000000000000000000000000ffff");
			Console.WriteLine(Base32.Encode(guid));
			
			guid = new Guid("00000000000000000000000000010000");
			Console.WriteLine(Base32.Encode(guid));

			Console.WriteLine(Base32.Encode(32));

			guid = new Guid("0000000000000000000000000000000f");
			Console.WriteLine(Base32.Encode(guid));

			guid = new Guid("00000000000000000000000000000010");
			Console.WriteLine(Base32.Encode(guid));

			guid = new Guid("000000000000000000000000000000ff");
			Console.WriteLine(Base32.Encode(guid));
		}

	}

	public static class Base32 {

		private static string B32Chars = "0123456789abcdefghjkmnpqrstvwxyz";

		/// <summary>
		/// Converts an array of bytes to a Base32-k string.
		/// </summary>
		public static string Encode(byte[] bytes) {
			var output = new List<char>();
			byte index;
			int hi = 5;
			int currentByte = 0;
			while (currentByte < bytes.Length) {
				// do we need to use the next byte?
				if (hi > 8) {
					// get the last piece from the current byte, shift it to the right
					// and increment the byte counter
					index = (byte)(bytes[currentByte++] >> (hi - 5));
					if (currentByte != bytes.Length) {
						// if we are not at the end, get the first piece from
						// the next byte, clear it and shift it to the left
						index = (byte)(((byte)(bytes[currentByte] << (16 - hi)) >> 3) | index);
					}

					hi -= 3;
				} else if (hi == 8) {
					index = (byte)(bytes[currentByte++] >> 3);
					hi -= 3;
				} else {
					// simply get the stuff from the current byte
					index = (byte)((byte)(bytes[currentByte] << (8 - hi)) >> 3);
					hi += 5;
				}
				output.Add(B32Chars[index]);
			}
			output.Reverse();

			return (String.Join(String.Empty, output.Select(c => c.ToString()).ToArray()));
		}

		public static string Encode(int value) {
			var bytes = new List<byte>();
			while (value > 0) {
				bytes.Add((byte)(value & 0xFF));
				value >>= 8;
			}
			bytes.Reverse();
			return (Encode(bytes.ToArray()));
		}

		public static string Encode(Guid guid) {
			return (Encode(guid.ToByteArray()));
		}
	}
}
