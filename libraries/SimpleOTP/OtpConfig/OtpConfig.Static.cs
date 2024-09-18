using System.Diagnostics.CodeAnalysis;
using SimpleOTP.Encoding;

namespace SimpleOTP;

// THIS IS THE SECTION OF A PARTIAL CLASS
// Description: Section of OtpConfig struct that holds static members
// Base file: OtpConfig.Base.cs

public partial record class OtpConfig
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public static implicit operator Uri(OtpConfig config) => config.ToUri();
	public static implicit operator string(OtpConfig config) => config.ToString();

	public static explicit operator OtpConfig(Uri uri) => ParseUri(uri);
	public static explicit operator OtpConfig(string uri) => ParseUri(uri);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

	/// <summary>
	/// Parses the specified URI into an <see cref="OtpConfig"/> object.
	/// </summary>
	/// <param name="uri">The URI to parse.</param>
	/// <returns>An <see cref="OtpConfig"/> object parsed from the specified URI.</returns>
	/// <exception cref="ArgumentException">Provided URI is not valid (missing required values or has invalid required values).</exception>
	public static OtpConfig ParseUri(Uri uri) =>
		new(uri);

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpConfig"/> class.
	/// </summary>
	/// <param name="uri">The URI of the OTP.</param>
	/// <param name="encoder">The encoder used to decode the secret.</param>
	/// <exception cref="ArgumentException">Provided URI is not valid (missing required values or has invalid required values).</exception>
	public static OtpConfig ParseUri(Uri uri, IEncoder encoder) =>
		new(uri, encoder);

	/// <summary>
	/// Parses the specified URI into an <see cref="OtpConfig"/> object.
	/// </summary>
	/// <param name="uri">The URI to parse.</param>
	/// <returns>An <see cref="OtpConfig"/> object parsed from the specified URI.</returns>
	/// <exception cref="ArgumentException">Provided URI is not valid (missing required values or has invalid required values).</exception>
	/// <exception cref="UriFormatException">Provided URI is not valid (missing required values or has invalid required values).</exception>
	public static OtpConfig ParseUri(string uri) =>
		new(new Uri(uri));

	/// <summary>
	/// Tries to parse the specified URI into an <see cref="OtpConfig"/> object.
	/// </summary>
	/// <param name="uri">The URI to parse.</param>
	/// <param name="config">When this method returns, contains the <see cref="OtpConfig"/> object parsed from the specified URI, if the conversion succeeded, or <c>null</c> if the conversion failed.</param>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryParseUri(Uri uri, [NotNullWhen(true)] out OtpConfig? config)
	{
		try
		{
			config = new(uri);
			return true;
		}
		catch
		{
			config = null;
			return false;
		}
	}

	/// <summary>
	/// Tries to parse the specified URI into an <see cref="OtpConfig"/> object.
	/// </summary>
	/// <param name="uri">The URI to parse.</param>
	/// <param name="config">When this method returns, contains the <see cref="OtpConfig"/> object parsed from the specified URI, if the conversion succeeded, or <c>null</c> if the conversion failed.</param>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryParseUri(string uri, [NotNullWhen(true)] out OtpConfig? config) =>
		TryParseUri(new Uri(uri), out config);

	/// <summary>
	/// Returns if the specified <see cref="OtpConfig"/> object is valid.
	/// </summary>
	/// <param name="config">The <see cref="OtpConfig"/> object to validate.</param>
	/// <param name="error">The error message returned if the <see cref="OtpConfig"/> object is invalid.</param>
	/// <param name="format">The <see cref="OtpUriFormat"/> to use for validation.</param>
	/// <remarks>The <paramref name="format"/> should contain at least one vendor-specific format.</remarks>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public static bool Validate(OtpConfig config, [NotNullWhen(false)] out string? error, OtpUriFormat format = OtpUriFormat.Google)
	{
		List<string> errors = [];

		// Check label presence
		if (string.IsNullOrWhiteSpace(config.Label))
			errors.Add($"- '{nameof(config.Label)}' is required and must be a display name for the account.");

		if ((format.HasFlag(OtpUriFormat.Apple) || format.HasFlag(OtpUriFormat.IIJ)) && config.Type != OtpType.Totp)
			errors.Add($"- '{nameof(config.Type)}' must be '{OtpType.Totp}'.");

		// Vendor-specific formats validation
		if (format.HasFlag(OtpUriFormat.Apple))
		{
			if (string.IsNullOrWhiteSpace(config.Issuer) || Uri.CheckHostName(config.Issuer) != UriHostNameType.Dns)
				errors.Add($"- '{nameof(config.Issuer)}' is required and must be a valid DNS name.");

			if (string.IsNullOrWhiteSpace(config.IssuerLabel))
				errors.Add($"- '{nameof(config.IssuerLabel)}' is required and must be a display name for the issuer.");

			if (((byte[])config.Secret).Length < 20)
				errors.Add($"- '{nameof(config.Secret)}' is required and must be at least 20 bytes long.");
		}

		// All vendors, except Apple reccommend that
		// if issuer label is specified, it should be the same as the issuer
		if (!format.HasFlag(OtpUriFormat.Apple) &&
			!string.IsNullOrWhiteSpace(config.IssuerLabel) && config.IssuerLabel != config.Issuer)
			errors.Add($"- (optional) '{nameof(config.IssuerLabel)}' should be the same as '{nameof(config.Issuer)}'.");

		if (format.HasFlag(OtpUriFormat.Yubico))
		{
			if (config.Type == OtpType.Totp && config.Period is not 15 or 30 or 60)
				errors.Add($"- '{nameof(config.Period)}' must be 15, 30 or 60.");
		}

		// Check for digits value
		if (config.Digits is not 6 or 8)
		{
			// Now it's time for IBM and Yubico to be weird
			if (format.HasFlag(OtpUriFormat.IBM) && config.Digits is not 7 and not 9)
				errors.Add($"- '{nameof(config.Digits)}' must be 6-9.");

			if (format.HasFlag(OtpUriFormat.Yubico) && config.Digits is not 7)
				errors.Add($"- '{nameof(config.Digits)}' must be 6-8.");

			else
				errors.Add($"- '{nameof(config.Digits)}' must be 6 or 8.");
		}

		// Algorithm validation
		if (!config.Algorithm.IsStandard())
		{
			// IIJ can also have an MD5 algorithm
			if (format.HasFlag(OtpUriFormat.IIJ) && config.Algorithm != OtpAlgorithm.MD5)
				errors.Add($"- '{nameof(config.Algorithm)}' must be a standard algorithm, defined by IIJ (SHA1/256/512 or MD5).");
			else
				errors.Add($"- '{nameof(config.Algorithm)}' must be a standard algorithm (SHA1, SHA256 or SHA512).");
		}

		if (errors.Count > 0)
		{
			error = string.Join("\n", errors);
			return false;
		}

		error = null;
		return true;
	}
}
