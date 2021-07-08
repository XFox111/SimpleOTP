// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleOTP.Models;

namespace SimpleOTP.Test.Models
{
	/// <summary>
	/// Unit-tests for OTP generator configuration model.
	/// </summary>
	[TestClass]
	public class OTPConfigurationUnitTest
	{
		/// <summary>
		/// Get otpauth link from minimal configuration.
		/// </summary>
		[TestMethod("Link generator (short)")]
		public void TestShortLinkGenerator()
		{
			OTPConfiguration config = OTPConfiguration.GenerateConfiguration("FoxDev Studio", "eugene@xfox111.net");
			var testId = config.Id;
			System.Diagnostics.Debug.WriteLine(testId);
			Uri uri = config.GetUri();
			Assert.AreEqual($"otpauth://totp/FoxDev%20Studio:eugene@xfox111.net?secret={config.Secret}&issuer=FoxDev%20Studio", uri.AbsoluteUri);
		}

		/// <summary>
		/// Get otpauth link from complete configurations (TOTP + HOTP).
		/// </summary>
		[TestMethod("Link generator (full)")]
		public void TestFullLinkGenerator()
		{
			OTPConfiguration config = OTPConfiguration.GetConfiguration("otpauth://totp/FoxDev%20Studio:eugene@xfox111.net?secret=ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH&issuer=FoxDev%20Studio%20Issuer&algorithm=SHA512&digits=8&period=10");
			Assert.AreEqual($"otpauth://totp/FoxDev%20Studio:eugene@xfox111.net?secret=ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH&issuer=FoxDev%20Studio%20Issuer&algorithm=SHA512&digits=8&period=10", config.GetUri().AbsoluteUri);
			config.Type = Enums.OTPType.HOTP;
			Assert.AreEqual($"otpauth://hotp/FoxDev%20Studio:eugene@xfox111.net?secret=ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH&issuer=FoxDev%20Studio%20Issuer&algorithm=SHA512&digits=8&counter=0", config.GetUri().AbsoluteUri);
		}

		/// <summary>
		/// Test user-friendly secret formatting.
		/// </summary>
		[TestMethod("Fancy secret")]
		public void GetFormattedSecret()
		{
			OTPConfiguration config = OTPConfiguration.GetConfiguration("ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH", "FoxDev Studio", "eugene@xfox111.net");
			Assert.AreEqual($"ESQV TYRM 2CWZ C3NX 24GR RWIA UUWV HWQH", config.GetFancySecret());
		}

		/// <summary>
		/// Test QR code generation for OTP config.
		/// </summary>
		/// <returns><see cref="Task"/>.</returns>
		[TestMethod("QR code image")]
		public async Task GetQrImage()
		{
			OTPConfiguration config = OTPConfiguration.GetConfiguration("ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH", "FoxDev Studio", "eugene@xfox111.net");
			string imageStr = await config.GetQrImage();
			Assert.AreEqual(
				"data:image/png;base64,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", imageStr);
		}
	}
}