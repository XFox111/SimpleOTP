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
			System.Diagnostics.Debug.WriteLine(config.Id);
			Uri uri = config.GetUri();
			Assert.AreEqual($"otpauth://totp/FoxDev+Studio:eugene@xfox111.net?secret={config.Secret}&issuer=FoxDev+Studio", uri.AbsoluteUri);
		}

		/// <summary>
		/// Get otpauth link from complete configurations (TOTP + HOTP).
		/// </summary>
		[TestMethod("Link generator (full)")]
		public void TestFullLinkGenerator()
		{
			OTPConfiguration config = OTPConfiguration.GetConfiguration("otpauth://totp/FoxDev%20Studio:eugene@xfox111.net?secret=ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH&issuer=FoxDev%20Studio%20Issuer&algorithm=SHA512&digits=8&period=10");
			Assert.AreEqual($"otpauth://totp/FoxDev+Studio:eugene@xfox111.net?secret=ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH&issuer=FoxDev+Studio+Issuer&algorithm=SHA512&digits=8&period=10", config.GetUri().AbsoluteUri);
			config.Type = Enums.OTPType.HOTP;
			Assert.AreEqual($"otpauth://hotp/FoxDev+Studio:eugene@xfox111.net?secret=ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH&issuer=FoxDev+Studio+Issuer&algorithm=SHA512&digits=8&counter=0", config.GetUri().AbsoluteUri);
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