// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

using SimpleOTP.Enums;
using SimpleOTP.Helpers;

namespace SimpleOTP.Models
{
	/// <summary>
	/// OTP generator configuration object.
	/// </summary>
	public record OTPConfiguration
	{
		/// <summary>
		/// Gets or sets unique identifier of current configuration instance.
		/// </summary>
		public Guid Id { get; set; } = Guid.NewGuid();

		/// <summary>
		/// Gets or sets oTP algorithm type.
		/// </summary>
		public OTPType Type { get; set; } = OTPType.TOTP;

		/// <summary>
		/// Gets or sets name of config issuer/service.
		/// </summary>
		public string IssuerLabel { get; set; }

		/// <summary>
		/// Gets or sets username or email of current config.
		/// </summary>
		public string AccountName { get; set; }

		/// <summary>
		/// Gets or sets secret key for OTP code generation.
		/// </summary>
		public string Secret { get; set; }

		/// <summary>
		/// Gets or sets internal issuer name for additional identification. Currently should be the same with <see cref="IssuerLabel"/>.
		/// </summary>
		public string Issuer { get; set; }

		/// <summary>
		/// Gets or sets oTP hashing algorithm.
		/// </summary>
		public Algorithm Algorithm { get; set; } = Algorithm.SHA1;

		/// <summary>
		/// Gets or sets number of digits of OTP code.
		/// </summary>
		public int Digits { get; set; } = 6;

		/// <summary>
		/// Gets or sets counter for HOTP generation. Update each time password has been generated.<br/>
		/// HOTP only.
		/// </summary>
		public long Counter { get; set; } = 0;

		/// <summary>
		/// Gets or sets time of OTP validity interval. Used to calculate TOTP counter.
		/// TOTP only.
		/// </summary>
		public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(30);

		/// <summary>
		/// Generate a new OTP configuration to send it to client.
		/// </summary>
		/// <remarks>
		/// <list type="table">
		/// <listheader>Algorithm parameters:</listheader>
		/// <item>
		/// <term>OTP algorithm</term>
		/// <description>Time-based OTP</description>
		/// </item>
		/// <item>
		/// <term>Key length</term>
		/// <description>160 bit (20 characters)</description>
		/// </item>
		/// <item>
		/// <term>Hashing algorithm</term>
		/// <description>HMAC-SHA-1</description>
		/// </item>
		/// <item>
		/// <term>Number of digits</term>
		/// <description>6</description>
		/// </item>
		/// <item>
		/// <term>Period</term>
		/// <description>30 seconds</description>
		/// </item>
		/// </list>
		/// </remarks>
		/// <param name="issuer">Name of your application/service.</param>
		/// <param name="accountName">Username/email of the user.</param>
		/// <returns>Valid <see cref="OTPConfiguration"/> configuraion.</returns>
		public static OTPConfiguration GenerateConfiguration(string issuer, string accountName) =>
			new ()
			{
				Issuer = issuer,
				IssuerLabel = issuer,
				AccountName = accountName,
				Secret = SecretGenerator.GenerateSecret()
			};

		/// <summary>
		/// Load OTP configuraiton with default parameters.
		/// </summary>
		/// <remarks>
		/// <list type="table">
		/// <listheader>Algorithm parameters:</listheader>
		/// <item>
		/// <term>OTP algorithm</term>
		/// <description>Time-based OTP</description>
		/// </item>
		/// <item>
		/// <term>Hashing algorithm</term>
		/// <description>HMAC-SHA-1</description>
		/// </item>
		/// <item>
		/// <term>Number of digits</term>
		/// <description>6</description>
		/// </item>
		/// <item>
		/// <term>Period</term>
		/// <description>30 seconds</description>
		/// </item>
		/// </list>
		/// </remarks>
		/// <param name="secret">OTP generator secret key (Base32 encoded string).</param>
		/// <param name="issuer">Name of your application/service.</param>
		/// <param name="accountName">Username/email of the user.</param>
		/// <returns>Valid <see cref="OTPConfiguration"/> configuraion.</returns>
		public static OTPConfiguration GetConfiguration(string secret, string issuer, string accountName) =>
			new ()
			{
				Issuer = issuer,
				IssuerLabel = issuer,
				AccountName = accountName,
				Secret = secret
			};

		/// <summary>
		/// Loads OTP configuration from OTP AUTH URI.
		/// </summary>
		/// <remarks>
		/// For more information please refer to <a href="https://github.com/google/google-authenticator/wiki/Key-Uri-Format">Key Uri Format</a>.
		/// </remarks>
		/// <param name="uri">OTP Auth URI. Should be correctly formed.</param>
		/// <returns>Valid <see cref="OTPConfiguration"/> configuraion.</returns>
		public static OTPConfiguration GetConfiguration(Uri uri) =>
			Helpers.UriParser.ParseUri(uri);

		/// <summary>
		/// Loads OTP configuration from OTP AUTH URI.
		/// </summary>
		/// <remarks>
		/// For more information please refer to <a href="https://github.com/google/google-authenticator/wiki/Key-Uri-Format">Key Uri Format</a>.
		/// </remarks>
		/// <param name="uri">OTP Auth URI. Should be correctly formed.</param>
		/// <returns>Valid <see cref="OTPConfiguration"/> configuraion.</returns>
		public static OTPConfiguration GetConfiguration(string uri) =>
			GetConfiguration(new Uri(uri));

		/// <summary>
		/// Gets URI from current configuration to reuse it somewhere else.
		/// </summary>
		/// <returns>Valid OTP AUTH URI.</returns>
		public Uri GetUri()
		{
			string path = $"otpauth://{Type}/{HttpUtility.UrlEncode(IssuerLabel)}";
			if (!string.IsNullOrWhiteSpace(AccountName))
				path += $":{AccountName}";
			path += $"?secret={Secret}&issuer={HttpUtility.UrlEncode(Issuer)}";
			if (Algorithm != Algorithm.SHA1)
				path += $"&algorithm={Algorithm}";
			if (Digits != 6)
				path += $"&digits={Digits}";
			if (Type == OTPType.HOTP)
				path += $"&counter={Counter}";
			if (Type == OTPType.TOTP && Period.TotalSeconds != 30)
				path += $"&period={(int)Period.TotalSeconds}";

			return new Uri(path);
		}

		/// <summary>
		/// Returns secret key separated with whitespaces on groups of 4.
		/// </summary>
		/// <returns>Formatted secret key string.</returns>
		public string GetFancySecret()
		{
			string secret = Secret;
			for (int k = 0; 4 + (5 * k) < secret.Length; k++)
				secret = secret.Insert(4 + (5 * k), " ");
			return secret;
		}

		/// <summary>
		/// Generates QR code image for current configuration with Google Chart API.
		/// </summary>
		/// <param name="qrCodeSize">QR code image size in pixels.</param>
		/// <param name="requestTimeout">Web request timeout in seconds.</param>
		/// <returns>string-encoded PNG image.</returns>
		public async Task<string> GetQrImage(int qrCodeSize = 300, int requestTimeout = 30)
		{
			HttpClient client = new () { Timeout = TimeSpan.FromSeconds(requestTimeout) };
			HttpResponseMessage response = client.GetAsync($"https://chart.googleapis.com/chart?cht=qr&chs={qrCodeSize}x{qrCodeSize}&chl={HttpUtility.UrlEncode(GetUri().AbsoluteUri)}").Result;

			if (!response.IsSuccessStatusCode)
				throw new HttpRequestException($"Response status code indicates that request has failed (Response code: {response.StatusCode})");

			byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
			string imageString = @$"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";

			return imageString;
		}
	}
}