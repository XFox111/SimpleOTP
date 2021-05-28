// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

using SimpleOTP.Enums;
using SimpleOTP.Models;

namespace SimpleOTP.Helpers
{
	/// <summary>
	/// Helper class which contains methods to parse OTP auth URIs.
	/// </summary>
	internal static class UriParser
	{
		/// <summary>
		/// Parses OTP Auth URI and returns configuration object for further processing.
		/// URI should be correctly formed.
		/// </summary>
		/// <remarks>
		/// For more information please refer to <a href="https://github.com/google/google-authenticator/wiki/Key-Uri-Format">Key Uri Format</a>.
		/// </remarks>
		/// <param name="uri">OTP Auth URI. Should be correctly formed.
		/// For more information please refer to <a href="https://github.com/google/google-authenticator/wiki/Key-Uri-Format">Key Uri Format</a>.</param>
		/// <returns><see cref="OTPConfiguration"/> configuration object, which contains data for OTP generation.</returns>
		internal static OTPConfiguration ParseUri(Uri uri)
		{
			if (uri.Scheme != "otpauth")
				throw new ArgumentException("Malformed link: Invalid scheme");
			if (!new[] { "hotp", "totp" }.Contains(uri.Host))
				throw new ArgumentException("Malformed link: Invalid OTP type");
			if (string.IsNullOrWhiteSpace(uri.LocalPath[1..]))
				throw new ArgumentException("Malformed link: Invalid label");

			NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
			if (!query.AllKeys.Contains("secret"))
				throw new ArgumentException("Malformed link: No secret provided");
			if (uri.Host == "hotp" && !query.AllKeys.Contains("counter"))
				throw new ArgumentException("Malformed link: No counter provided for HOTP");
			if (!uri.LocalPath[1..].Contains(':') && !query.AllKeys.Contains("issuer"))
				throw new ArgumentException("Malformed link: No issuer provided");

			string[] label = uri.LocalPath[1..].Split(':');
			OTPConfiguration item = new ()
			{
				Type = uri.Host == "totp" ? OTPType.TOTP : OTPType.HOTP,
				IssuerLabel = label.Length > 1 ? label[0] : query["issuer"],
				AccountName = label.Length > 1 ? label[1] : uri.LocalPath[1..],
				Secret = query["secret"],
				Issuer = query["issuer"] ?? label[0],
				Algorithm = query["algorithm"]?.ToUpperInvariant() switch
				{
					"SHA256" => Algorithm.SHA256,
					"SHA512" => Algorithm.SHA512,
					_ => Algorithm.SHA1
				},
				Digits = int.Parse(query["digits"] ?? "6"),
				Counter = int.Parse(query["counter"] ?? "0"),
				Period = TimeSpan.FromSeconds(int.Parse(query["period"] ?? "30"))
			};

			return item;
		}
	}
}