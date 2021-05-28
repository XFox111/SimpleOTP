// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

using SimpleOTP.Enums;
using SimpleOTP.Helpers;
using SimpleOTP.Models;

namespace SimpleOTP
{
	/// <summary>
	/// Service class for generating and validating OTP codes.
	/// </summary>
	public static class OTPService
	{
		/// <summary>
		/// Generates a new OTP code with provided configuration.
		/// </summary>
		/// <remarks>
		/// If you're using HOTP algorithm, save <paramref name="target"/> after calling the function.
		/// </remarks>
		/// <param name="target">OTP configuration object.</param>
		/// <returns><see cref="OTPCode"/> instance with generated code.<br/>
		/// If OTP algorithm is HOTP, <paramref name="target"/> counter is increased by 1.
		/// </returns>
		public static OTPCode GenerateCode(ref OTPConfiguration target) =>
			GenerateCode(ref target, DateTime.UtcNow);

		/// <summary>
		/// Generates a new TOTP code with provided configuration and for specific interval.
		/// </summary>
		/// <remarks>
		/// If you're using HOTP algorithm, save <paramref name="target"/> after calling the function.<br/>
		/// If you're using HOTP algorithm, <paramref name="date"/> will be ignored.
		/// </remarks>
		/// <param name="target">OTP configuration object.</param>
		/// <param name="date"><see cref="DateTime"/> for which the OTP should be generated.</param>
		/// <returns><see cref="OTPCode"/> instance with generated code.<br/>
		/// If OTP algorithm is HOTP, <paramref name="target"/> counter is increased by 1.
		/// </returns>
		public static OTPCode GenerateCode(ref OTPConfiguration target, DateTime date)
		{
			byte[] keyBytes = Base32Encoder.Decode(target.Secret);
			long counter = target.Type == OTPType.HOTP ? target.Counter : GetCurrentCounter(date.ToUniversalTime(), (int)target.Period.TotalSeconds);
			byte[] counterBytes = BitConverter.GetBytes(counter);

			// IDK da fuk is this and at this point I'm too afraid to ask
			if (BitConverter.IsLittleEndian)
				Array.Reverse(counterBytes);

			HMAC hmac = target.Algorithm switch
			{
				Algorithm.SHA256 => new HMACSHA256(keyBytes),
				Algorithm.SHA512 => new HMACSHA512(keyBytes),
				_ => new HMACSHA1(keyBytes)
			};

			byte[] hash = hmac.ComputeHash(counterBytes);

			// Not mine code, so fuck off
			int offset = hash[^1] & 0xf;

			// Convert the 4 bytes into an integer, ignoring the sign.
			int binary =
				((hash[offset] & 0x7f) << 24)
				| (hash[offset + 1] << 16)
				| (hash[offset + 2] << 8)
				| hash[offset + 3];

			OTPCode code = new (binary % (int)Math.Pow(10, target.Digits));

			if (target.Type == OTPType.HOTP)
				target.Counter++;       // Incrementing counter for HMAC OTP type
			else
				code.Expiring = DateTime.UnixEpoch.AddSeconds((counter + 1) * target.Period.TotalSeconds);

			return code;
		}

		/// <summary>
		/// Validates provided HOTP code with provided parameters.
		/// </summary>
		/// <remarks>
		/// Use this method only with HOTP codes.
		/// </remarks>
		/// <param name="otp">HOTP code to validate.</param>
		/// <param name="target">OTP configuration for check codes generation.</param>
		/// <param name="toleranceSpan">Counter span from which OTP codes remain valid.</param>
		/// <param name="resyncCounter">Defines whether method should resync <see cref="OTPConfiguration.Counter"/> of the <paramref name="target"/> or not after successful validation.</param>
		/// <returns><c>True</c> if code is valid, <c>False</c> if it isn't.</returns>
		public static bool ValidateCode(int otp, ref OTPConfiguration target, int toleranceSpan, bool resyncCounter)
		{
			long currentCounter = target.Counter;
			List<(int Code, long Counter)> codes = new ();
			for (long i = currentCounter - toleranceSpan; i <= currentCounter + toleranceSpan; i++)
			{
				OTPConfiguration testTarget = target with { Counter = i };
				codes.Add((GenerateCode(ref testTarget).Code, testTarget.Counter - 1));
			}

			bool isValid = codes.Any(i => i.Code == otp);
			if (isValid && resyncCounter)
				target.Counter = codes.Find(i => i.Code == otp).Counter;
			return isValid;
		}

		/// <summary>
		/// Validates provided TOTP code with provided parameters.
		/// </summary>
		/// <remarks>
		/// Use this method only with Time-based OTP codes.
		/// </remarks>
		/// <param name="otp">OTP code to validate.</param>
		/// <param name="target">OTP configuration for check codes generation.</param>
		/// <param name="toleranceTime">Time span from which OTP codes remain valid.<br/>
		/// Default: 15 seconds.</param>
		/// <returns><c>True</c> if code is valid, <c>False</c> if it isn't.</returns>
		public static bool ValidateCode(int otp, OTPConfiguration target, TimeSpan? toleranceTime = null)
		{
			toleranceTime ??= TimeSpan.FromSeconds(15);
			DateTime now = DateTime.UtcNow;
			List<int> codes = new ();
			for (DateTime time = now - toleranceTime.Value; time <= now + toleranceTime; time += target.Period)
				codes.Add(GenerateCode(ref target, time).Code);

			return codes.Any(i => i == otp);
		}

		private static long GetCurrentCounter(DateTime date, int period) =>
			(long)(date - DateTime.UnixEpoch).TotalSeconds / period;
	}
}