// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;
using System.Linq;

namespace SimpleOTP.Helpers
{
	/// <summary>
	/// Helper class which contains methods for encoding and decoding Base32 bytes.
	/// </summary>
	internal static class Base32Encoder
	{
		// Standard RFC 4648 Base32 alphabet
		private const string AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

		/// <summary>
		/// Encode byte array to Base32 string.
		/// </summary>
		/// <param name="data">Byte array to encode.</param>
		/// <returns>Base32 string.</returns>
		internal static string Encode(byte[] data)
		{
			string binary = string.Empty;
			foreach (byte b in data)
				binary += Convert.ToString(b, 2).PadLeft(8, '0');   // Getting binary sequence to split into 5 digits

			int numberOfBlocks = (binary.Length / 5) + Math.Clamp(binary.Length % 5, 0, 1);
			string[] sequence = Enumerable.Range(0, numberOfBlocks)
				.Select(i => binary.Substring(i * 5, Math.Min(5, binary.Length - (i * 5))).PadRight(5, '0'))
				.ToArray();   // Splitting sequence on groups of 5

			string output = string.Empty;
			foreach (string str in sequence)
				output += AllowedCharacters[Convert.ToInt32(str, 2)];

			output = output.PadRight(output.Length + (output.Length % 8), '=');

			return output;
		}

		/// <summary>
		/// Decode Base32 string into byte array.
		/// </summary>
		/// <param name="base32str">Base32-encoded string.</param>
		/// <returns>Initial byte array.</returns>
		internal static byte[] Decode(string base32str)
		{
			base32str = base32str.Replace("=", string.Empty);   // Removing padding

			string[] quintets = base32str.Select(i => Convert.ToString(AllowedCharacters.IndexOf(i), 2).PadLeft(5, '0')).ToArray();     // Getting quintets
			string binary = string.Join(null, quintets);

			byte[] output = Enumerable.Range(0, binary.Length / 8).Select(i => Convert.ToByte(binary.Substring(i * 8, 8), 2)).ToArray();

			return output;
		}
	}
}