using System.Collections.Specialized;
using SimpleOTP.Fluent;

namespace SimpleOTP.DependencyInjection;

/// <summary>
/// Provides methods for generating and validating One-Time Passwords.
/// </summary>
/// <param name="configuration">The configuration for the One-Time Password service.</param>
internal class OtpService(OtpOptions configuration) : IOtpService
{
	private readonly string _issuerName = configuration.Issuer;
	private readonly string? _issuerDomain = configuration.IssuerDomain;
	private readonly OtpAlgorithm _algorithm = configuration.Algorithm;
	private readonly OtpType _type = configuration.Type;
	private readonly OtpUriFormat _format = configuration.UriFormat |
		(string.IsNullOrWhiteSpace(configuration.IssuerDomain) ? 0 : OtpUriFormat.Apple);
	private readonly int _digits = configuration.Digits;
	private readonly int _period = configuration.Period;
	private readonly NameValueCollection _customProperties = configuration.CustomProperties;
	private readonly ToleranceSpan _tolerance = configuration.ToleranceSpan;

	/// <summary>
	/// Creates an OTP URI for specified user and secret.
	/// </summary>
	/// <param name="username">The username of the user.</param>
	/// <param name="secret">The secret to use.</param>
	/// <param name="counter">(only for HOTP) The counter to use.</param>
	/// <returns>The generated URI.</returns>
	public Uri CreateUri(string username, OtpSecret secret, long counter = 0)
	{
		OtpConfig config = new(username)
		{
			Algorithm = _algorithm,
			Type = _type,
			Issuer = _issuerName,
			Digits = _digits,
			Period = _period,

			Secret = secret,
			Counter = counter
		};

		if (!string.IsNullOrWhiteSpace(_issuerDomain))
			config.WithAppleIssuer(_issuerName, _issuerDomain);

		config.CustomProperties.Add(_customProperties);

		return config.ToUri(_format);
	}

	/// <summary>
	/// Creates an OTP code for specified user and secret.
	/// </summary>
	/// <param name="secret">The secret to use.</param>
	/// <param name="counter">(only for HOTP) The counter to use.</param>
	/// <returns>The generated code.</returns>
	/// <exception cref="NotSupportedException">The service was not configured properly. Check the "Authenticator:Type" configuration.</exception>
	public OtpCode GenerateCode(OtpSecret secret, long counter = 0)
	{
		using OtpSecret secretClone = OtpSecret.CreateCopy(secret);

		Otp generator = _type switch
		{
			OtpType.Hotp => new Hotp(secret, counter, _algorithm, _digits),
			OtpType.Totp => new Totp(secret, _period, _algorithm, _digits),
			_ => throw new NotSupportedException("The service was not configured properly. Check the \"Authenticator:Type\" configuration.")
		};

		return generator.Generate();
	}

	/// <summary>
	/// Validates an OTP code for specified user and secret.
	/// </summary>
	/// <param name="code">The code to validate.</param>
	/// <param name="secret">The secret to use.</param>
	/// <param name="resyncValue">The resync value. Shows how much the code is ahead or behind the current counter value.</param>
	/// <param name="counter">(only for HOTP) The counter to use.</param>
	/// <returns><c>true</c> if the code is valid; otherwise, <c>false</c>.</returns>
	/// <exception cref="NotSupportedException">The service was not configured properly. Check the "Authenticator:Type" configuration.</exception>
	public bool ValidateCode(OtpCode code, OtpSecret secret, out int resyncValue, long counter = 0)
	{
		using OtpSecret secretClone = OtpSecret.CreateCopy(secret);

		Otp generator = _type switch
		{
			OtpType.Hotp => new Hotp(secret, counter, _algorithm, _digits),
			OtpType.Totp => new Totp(secret, _period, _algorithm, _digits),
			_ => throw new NotSupportedException("The service was not configured properly. Check the \"Authenticator:Type\" configuration.")
		};

		return generator.Validate(code, _tolerance, out resyncValue);
	}
}
