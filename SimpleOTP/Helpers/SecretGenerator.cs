// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;

namespace SimpleOTP.Helpers
{
	/// <summary>
	/// Helper class for OTP secret generation.
	/// </summary>
	public static class SecretGenerator
	{
		/// <summary>
		/// Generate OTP secret key.
		/// </summary>
		/// <param name="length">Length of the key in bits<br/>
		/// It should belong to [128-160] bit span<br/>
		/// Default is: 160 bits.</param>
		/// <remarks>Number of bits will be rounded down to the nearest number which divides by 8.</remarks>
		/// <returns>Base32 encoded alphanumeric string with length form 16 to 20 characters.</returns>
		public static string GenerateSecret(int length = 160)
		{
			if (length > 160 || length < 128)
				throw new ArgumentOutOfRangeException(nameof(length), "Invalid key length. It should belong to [128-160] bits span");

			byte[] key = new byte[length / 8];
			new Random().NextBytes(key);

			return Base32Encoder.Encode(key);
		}
	}
}