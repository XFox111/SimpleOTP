// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleOTP.Models;

namespace SimpleOTP.Test
{
	/// <summary>
	/// Unit-tests for OTP generator.
	/// </summary>
	[TestClass]
	public class OTPServiceUnitTest
	{
		private readonly DateTime time = new (2021, 5, 28, 10, 47, 50, DateTimeKind.Utc);
		private OTPConfiguration totpConfig = OTPConfiguration.GetConfiguration(new Uri("otpauth://totp/FoxDev%20Studio:eugene@xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=FoxDev%20Studio"));
		private OTPConfiguration hotpConfig = OTPConfiguration.GetConfiguration(new Uri("otpauth://hotp/FoxDev%20Studio:eugene@xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=FoxDev%20Studio&counter=10000"));

		/// <summary>
		/// Test time-based OTP generator with pre-calculated code.
		/// </summary>
		[TestMethod("TOTP code generation")]
		public void GenerateCode_Totp()
		{
			var config = totpConfig with { };
			var code = OTPService.GenerateCode(ref config, time);
			Assert.AreEqual(160102, code.Code);

			config.Algorithm = Enums.Algorithm.SHA256;
			code = OTPService.GenerateCode(ref config, time);
			Assert.AreEqual(195671, code.Code);

			config.Algorithm = Enums.Algorithm.SHA512;
			code = OTPService.GenerateCode(ref config, time);
			Assert.AreEqual(293657, code.Code);
		}

		/// <summary>
		/// Test time-based OTP generator with customly formatted secret.
		/// </summary>
		[TestMethod("Secret format test")]
		public void FormatTest()
		{
			Console.Write("Uppercase space-separated: ");
			var config = totpConfig with { Secret = "JBSW Y3DP EHPK 3PXP" };
			var code = OTPService.GenerateCode(ref config, time);
			Assert.AreEqual(160102, code.Code);
			Console.WriteLine("Passed.");

			Console.Write("Lowercase space-separated: ");
			config = totpConfig with { Secret = "jbsw y3dp ehpk 3pxp" };
			code = OTPService.GenerateCode(ref config, time);
			Assert.AreEqual(160102, code.Code);
			Console.WriteLine("Passed.");

			Console.Write("Lowercase: ");
			config = totpConfig with { Secret = "jbswy3dpehpk3pxp" };
			code = OTPService.GenerateCode(ref config, time);
			Assert.AreEqual(160102, code.Code);
			Console.WriteLine("Passed.");
		}

		/// <summary>
		/// Test HOTP generator with pre-calculated code.
		/// </summary>
		[TestMethod("HOTP code generation")]
		public void GenerateCode_Hotp()
		{
			var code = OTPService.GenerateCode(ref hotpConfig);
			Assert.AreEqual(457608, code.Code);
			Assert.AreEqual(10001, hotpConfig.Counter);
		}

		/// <summary>
		/// Test time-based OTP validator with series of codes.
		/// </summary>
		[TestMethod("TOTP code validation")]
		public void ValidateCode_Totp()
		{
			int[] codes =
			{
				OTPService.GenerateCode(ref totpConfig, DateTime.UtcNow.AddSeconds(-45)).Code,
				OTPService.GenerateCode(ref totpConfig, DateTime.UtcNow.AddSeconds(-15)).Code,
				OTPService.GenerateCode(ref totpConfig, DateTime.UtcNow.AddSeconds(0)).Code,
				OTPService.GenerateCode(ref totpConfig, DateTime.UtcNow.AddSeconds(15)).Code,
				OTPService.GenerateCode(ref totpConfig, DateTime.UtcNow.AddSeconds(45)).Code,
			};
			Assert.IsFalse(OTPService.ValidateTotp(codes[0], totpConfig));
			Assert.IsTrue(OTPService.ValidateTotp(codes[1], totpConfig));
			Assert.IsTrue(OTPService.ValidateTotp(codes[2], totpConfig));
			Assert.IsTrue(OTPService.ValidateTotp(codes[3], totpConfig));
			Assert.IsFalse(OTPService.ValidateTotp(codes[4], totpConfig));

			Assert.IsTrue(OTPService.ValidateTotp(codes[0], totpConfig, TimeSpan.FromSeconds(60)));

			Assert.ThrowsException<ArgumentException>(() => OTPService.ValidateTotp(0, hotpConfig));
		}

		/// <summary>
		/// Test HOTP validator with series of codes.
		/// </summary>
		[TestMethod("HOTP code validation")]
		public void ValidateCode_Hotp()
		{
			hotpConfig.Counter = 10000;
			int[] codes =
			{
				OTPService.GenerateCode(ref hotpConfig).Code,
				OTPService.GenerateCode(ref hotpConfig).Code,
				OTPService.GenerateCode(ref hotpConfig).Code,
				OTPService.GenerateCode(ref hotpConfig).Code,
				OTPService.GenerateCode(ref hotpConfig).Code,
			};

			hotpConfig.Counter = 10002;
			Assert.IsFalse(OTPService.ValidateHotp(codes[0], ref hotpConfig, 1, true));
			Assert.AreEqual(10002, hotpConfig.Counter);
			Assert.IsTrue(OTPService.ValidateHotp(codes[1], ref hotpConfig, 1, true));
			Assert.AreEqual(10001, hotpConfig.Counter);
			hotpConfig.Counter = 10002;
			Assert.IsTrue(OTPService.ValidateHotp(codes[2], ref hotpConfig, 1, true));
			Assert.AreEqual(10002, hotpConfig.Counter);
			hotpConfig.Counter = 10002;
			Assert.IsTrue(OTPService.ValidateHotp(codes[3], ref hotpConfig, 1, true));
			Assert.AreEqual(10003, hotpConfig.Counter);
			hotpConfig.Counter = 10002;
			Assert.IsFalse(OTPService.ValidateHotp(codes[4], ref hotpConfig, 1, true));
			Assert.AreEqual(10002, hotpConfig.Counter);

			Assert.ThrowsException<ArgumentException>(() => OTPService.ValidateHotp(0, ref totpConfig, 1, true));
		}
	}
}