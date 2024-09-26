using System.Collections.Specialized;

namespace SimpleOTP.DependencyInjection;

/// <summary>
/// Provides options for the One-Time Password service.
/// </summary>
public class OtpOptions
{
	/// <summary>
	/// The name of the issuer.
	/// </summary>
	public required string Issuer { get; set; }

	/// <summary>
	/// The issuer domain.
	/// </summary>
	/// <remarks>
	/// <b>IMPORTANT:</b> Using this property will imply adherence to the Apple specification.
	/// </remarks>
	public string? IssuerDomain { get; set; }

	/// <summary>
	/// The algorithm to use.
	/// </summary>
	public OtpAlgorithm Algorithm { get; set; } = OtpAlgorithm.SHA1;

	/// <summary>
	/// The number of digits in the OTP code.
	/// </summary>
	public int Digits { get; set; } = 6;

	/// <summary>
	/// The number of seconds between each OTP code.
	/// </summary>
	public int Period { get; set; } = 30;

	/// <summary>
	/// The type of One-Time Password to generate.
	/// </summary>
	public OtpType Type { get; set; } = OtpType.Totp;

	/// <summary>
	/// The format of OTP URIs.
	/// </summary>
	public OtpUriFormat UriFormat { get; set; } = OtpUriFormat.Google | OtpUriFormat.Minimal;

	/// <summary>
	/// The tolerance span for the OTP codes validation.
	/// </summary>
	public ToleranceSpan ToleranceSpan { get; set; } = ToleranceSpan.Default;

	/// <summary>
	/// Custom properties to place in OTP URIs.
	/// </summary>
	public NameValueCollection CustomProperties { get; } = [];
}
