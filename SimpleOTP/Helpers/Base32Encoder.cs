// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System.Collections.Generic;

namespace SimpleOTP.Helpers
{
	/// <summary>
	/// Helper class which contains methods for encoding and decoding Base32 bytes.
	/// </summary>
	internal static class Base32Encoder
	{
		private const string AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

		/// <summary>
		/// Encode byte array to Base32 string.
		/// </summary>
		/// <param name="data">Byte array to encode.</param>
		/// <returns>Base32 string.</returns>
		internal static string Encode(byte[] data)
		{
			// FIXME: Encoder works correctly only with 160-bit keys
			string output = string.Empty;
			for (int bitIndex = 0; bitIndex < data.Length * 8; bitIndex += 5)
			{
				int dualbyte = data[bitIndex / 8] << 8;
				if ((bitIndex / 8) + 1 < data.Length)
					dualbyte |= data[(bitIndex / 8) + 1];
				dualbyte = 0x1f & (dualbyte >> (16 - (bitIndex % 8) - 5));
				output += AllowedCharacters[dualbyte];
			}

			return output;
		}

		/// <summary>
		/// Decode Base32 string into byte array.
		/// </summary>
		/// <param name="base32str">Base32-encoded string.</param>
		/// <returns>Initial byte array.</returns>
		internal static byte[] Decode(string base32str)
		{
			List<byte> output = new ();
			char[] bytes = base32str.ToCharArray();
			for (var bitIndex = 0; bitIndex < base32str.Length * 5; bitIndex += 8)
			{
				var dualbyte = AllowedCharacters.IndexOf(bytes[bitIndex / 5]) << 10;
				if ((bitIndex / 5) + 1 < bytes.Length)
					dualbyte |= AllowedCharacters.IndexOf(bytes[(bitIndex / 5) + 1]) << 5;
				if ((bitIndex / 5) + 2 < bytes.Length)
					dualbyte |= AllowedCharacters.IndexOf(bytes[(bitIndex / 5) + 2]);

				dualbyte = 0xff & (dualbyte >> (15 - (bitIndex % 5) - 8));
				output.Add((byte)dualbyte);
			}

			return output.ToArray();
		}
	}
}