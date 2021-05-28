// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleOTP.Models;

namespace SimpleOTP.Test.Helpers
{
	/// <summary>
	/// Unit-tests for OTP URI parser.
	/// </summary>
	[TestClass]
	public class UriParserUnitTest
	{
		private static readonly Guid TestGuid = Guid.NewGuid();

		private readonly string[] uriParts =
		{
			"otpauth",
			"totp|hotp",
			"FoxDev%20Studio",
			"eugene@xfox111.net",
			"secret=ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH",
			"issuer=FoxDev%20Studio%20Issuer",
			"algorithm=SHA1|algorithm=SHA512",
			"digits=8",
			"period=10",
			"counter=10000",
		};

		private readonly OTPConfiguration[] configs =
		{
			new ()
			{
				AccountName = "eugene@xfox111.net",
				Algorithm = Enums.Algorithm.SHA512,
				Digits = 8,
				Type = Enums.OTPType.TOTP,
				Secret = "ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH",
				Issuer = "FoxDev Studio Issuer",
				IssuerLabel = "FoxDev Studio",
				Id = TestGuid,
				Period = TimeSpan.FromSeconds(10),
			},
			new ()
			{
				AccountName = "eugene@xfox111.net",
				Algorithm = Enums.Algorithm.SHA512,
				Digits = 8,
				Type = Enums.OTPType.HOTP,
				Secret = "ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH",
				Issuer = "FoxDev Studio Issuer",
				IssuerLabel = "FoxDev Studio",
				Id = TestGuid,
				Counter = 10000,
			},
			new ()
			{
				AccountName = "eugene@xfox111.net",
				Secret = "ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH",
				Issuer = "FoxDev Studio",
				IssuerLabel = "FoxDev Studio",
				Id = TestGuid,
			},
			new ()
			{
				AccountName = "eugene@xfox111.net",
				Type = Enums.OTPType.HOTP,
				Secret = "ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH",
				Issuer = "FoxDev Studio",
				IssuerLabel = "FoxDev Studio",
				Id = TestGuid,
				Counter = 10000,
			},
		};

		/// <summary>
		/// Test parser with full TOTP URI.
		/// </summary>
		[TestMethod("Valid full URI (TOTP)")]
		public void ParseValidUri_FullFormed_Totp()
		{
			string uri = $"{uriParts[0]}://{uriParts[1].Split('|')[0]}/{uriParts[2]}:{uriParts[3]}?{uriParts[4]}&{uriParts[5]}&{uriParts[6].Split('|')[1]}&{uriParts[7]}&{uriParts[8]}";
			var config = SimpleOTP.Helpers.UriParser.ParseUri(new Uri(uri));
			Assert.IsNotNull(config);
			config.Id = TestGuid;
			Assert.AreEqual(configs[0], config);
		}

		/// <summary>
		/// Test parser with full HOTP URI.
		/// </summary>
		[TestMethod("Valid full URI (HOTP)")]
		public void ParseValidUri_FullFormed_Hotp()
		{
			string uri = $"{uriParts[0]}://{uriParts[1].Split('|')[1]}/{uriParts[2]}:{uriParts[3]}?{uriParts[4]}&{uriParts[5]}&{uriParts[6].Split('|')[1]}&{uriParts[7]}&{uriParts[9]}";
			var config = SimpleOTP.Helpers.UriParser.ParseUri(new Uri(uri));
			Assert.IsNotNull(config);
			config.Id = TestGuid;
			Assert.AreEqual(configs[1], config);
		}

		/// <summary>
		/// Test parser with TOTP URI. Minimal parameter set.
		/// </summary>
		[TestMethod("Valid minimal URI (TOTP)")]
		public void ParseValidUri_Minimal_Totp()
		{
			string uri = $"{uriParts[0]}://{uriParts[1].Split('|')[0]}/{uriParts[2]}:{uriParts[3]}?{uriParts[4]}";
			var config = SimpleOTP.Helpers.UriParser.ParseUri(new Uri(uri));
			Assert.IsNotNull(config);
			config.Id = TestGuid;
			Assert.AreEqual(configs[2], config);

			config = SimpleOTP.Helpers.UriParser.ParseUri(new Uri($"otpauth://totp/eugene@xfox111.net?{uriParts[4]}&{uriParts[5]}&algorithm=SHA256"));
			Assert.IsNotNull(config);
			config.Id = TestGuid;
			Assert.AreEqual(configs[2] with { IssuerLabel = "FoxDev Studio Issuer", Issuer = "FoxDev Studio Issuer", Algorithm = Enums.Algorithm.SHA256 }, config);
		}

		/// <summary>
		/// Test parser with HOTP URI. Minimal parameter set.
		/// </summary>
		[TestMethod("Valid minimal URI (HOTP)")]
		public void ParseValidUri_Minimal_Hotp()
		{
			string uri = $"{uriParts[0]}://{uriParts[1].Split('|')[1]}/{uriParts[2]}:{uriParts[3]}?{uriParts[4]}&{uriParts[9]}";
			var config = SimpleOTP.Helpers.UriParser.ParseUri(new Uri(uri));
			Assert.IsNotNull(config);
			config.Id = TestGuid;
			Assert.AreEqual(configs[3], config);
		}

		/// <summary>
		/// Test parser with invalid OTP URIs.
		/// </summary>
		[TestMethod("Invalid URI")]
		public void ParseInvalidUris()
		{
			string[] uris =
			{
				$"https://{uriParts[1].Split('|')[1]}/{uriParts[2]}:{uriParts[3]}?{uriParts[4]}&{uriParts[9]}",
				$"{uriParts[0]}://otp/{uriParts[2]}:{uriParts[3]}?{uriParts[4]}&{uriParts[9]}",
				$"{uriParts[0]}://{uriParts[2]}:{uriParts[3]}?{uriParts[4]}&{uriParts[9]}",
				$"{uriParts[0]}://{uriParts[1].Split('|')[0]}/{uriParts[3]}?{uriParts[4]}&{uriParts[9]}",
				$"{uriParts[0]}://{uriParts[1].Split('|')[0]}/?{uriParts[4]}&{uriParts[9]}",
				$"{uriParts[0]}://{uriParts[1].Split('|')[0]}/{uriParts[2]}:{uriParts[3]}",
				$"{uriParts[0]}://{uriParts[1].Split('|')[1]}/{uriParts[2]}:{uriParts[3]}?{uriParts[4]}",
			};
			foreach (string uri in uris)
				Assert.ThrowsException<ArgumentException>(() => SimpleOTP.Helpers.UriParser.ParseUri(new Uri(uri)));
		}
	}
}