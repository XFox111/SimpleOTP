using System.Collections.Specialized;
using System.Net;
using System.Web;
using SimpleOTP.Encoding;

namespace SimpleOTP;

// THIS IS THE SECTION OF A PARTIAL CLASS
// Description: Section of OtpConfig struct that holds instance constructors
// Base file: OtpConfig.Base.cs

public partial record class OtpConfig
{
	/// <summary>
	/// Initializes a new instance of the <see cref="OtpConfig"/> class.
	/// </summary>
	/// <param name="label">The label of the OTP config.</param>
	public OtpConfig(string label) =>
		Label = label;

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpConfig"/> class.
	/// </summary>
	/// <param name="label">The label of the OTP config.</param>
	/// <param name="issuer">The issuer of the OTP config.</param>
	public OtpConfig(string label, string issuer) =>
		(Label, Issuer) = (label, issuer);

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpConfig"/> class.
	/// </summary>
	/// <param name="label">The label of the OTP config.</param>
	/// <param name="secret">The secret of the OTP config.</param>
	public OtpConfig(string label, OtpSecret secret) =>
		(Label, Secret) = (label, secret);

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpConfig"/> class.
	/// </summary>
	/// <param name="label">The label of the OTP config.</param>
	/// <param name="issuer">The issuer of the OTP config.</param>
	/// <param name="secret">The secret of the OTP config.</param>
	public OtpConfig(string label, string issuer, OtpSecret secret) =>
		(Label, Issuer, Secret) = (label, issuer, secret);

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpConfig"/> class.
	/// </summary>
	/// <param name="uri">The URI of the OTP config.</param>
	/// <exception cref="ArgumentException">Provided URI is not valid (missing required values or has invalid required values).</exception>
	public OtpConfig(Uri uri) : this(uri, OtpSecret.DefaultEncoder) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpConfig"/> class.
	/// </summary>
	/// <param name="uri">The URI of the OTP config.</param>
	/// <param name="encoder">The encoder used to decode the secret.</param>
	/// <exception cref="ArgumentException">Provided URI is not valid (missing required values or has invalid required values).</exception>
	public OtpConfig(Uri uri, IEncoder encoder)
	{
		if (uri.Scheme != "otpauth" && uri.Scheme != "apple-otpauth")
			throw new ArgumentException("Invalid URI scheme. Expected 'otpauth' or 'apple-otpauth'.");

		Type = uri.Host.ToLowerInvariant() switch
		{
			"totp" => OtpType.Totp,
			"hotp" => OtpType.Hotp,
			_ => throw new ArgumentException("Invalid OTP type. Expected 'totp' or 'hotp'.")
		};

		NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);

		Secret = OtpSecret.Parse(query[nameof(Secret)] ?? throw new ArgumentException("Secret is required."), encoder);

		string label = WebUtility.UrlDecode(uri.Segments[^1]);
		string[] labelParts = label.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);
		Label = labelParts.Last();

		if (labelParts.Length > 1)
			IssuerLabel = labelParts[0];

		if (long.TryParse(query[nameof(Counter)], out long counter) || Type != OtpType.Hotp)
			Counter = counter;
		else
			throw new ArgumentException("Counter is required for HOTP algorithm.");

		if (query.Get(nameof(Issuer)) is string issuer)
			Issuer = issuer;

		if (query.Get(nameof(Algorithm)) is string algorithm && !string.IsNullOrWhiteSpace(algorithm))
			Algorithm = (OtpAlgorithm)algorithm;

		if (int.TryParse(query[nameof(Period)], out int period))
			Period = period;

		if (int.TryParse(query[nameof(Digits)], out int digits))
			Digits = digits;

		foreach (string key in _reservedKeys)
			query.Remove(key);

		CustomProperties.Add(query);
	}
}
