namespace SimpleOTP.DependencyInjection;

/// <summary>
/// Configuration for the One-Time Password service.
/// </summary>
public class OtpServiceConfig
{
	/// <summary>
	/// The name of the issuer.
	/// </summary>
	public string Issuer { get; set; } = null!;

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
	public OtpUriFormat UriFormat { get; set; } = OtpUriFormat.Google;

	/// <summary>
	/// Whether to use minimal URI formatting (only required, or altered properties are included), or full URI formatting.
	/// </summary>
	public bool MinimalUri { get; set; } = true;

	/// <summary>
	/// The tolerance span for the OTP codes validation.
	/// </summary>
	public ToleranceSpanConfig ToleranceSpan { get; set; } = new();

	/// <summary>
	/// Custom properties to place in OTP URIs.
	/// </summary>
	public Dictionary<string, string> CustomProperties { get; } = [];
}

/// <summary>
/// Configuration for the tolerance span.
/// </summary>
public class ToleranceSpanConfig
{
	/// <summary>
	/// The number of periods/counter values behind the current value.
	/// </summary>
	public int Behind { get; set; } = ToleranceSpan.Default.Behind;

	/// <summary>
	/// The number of periods/counter values ahead of the current value.
	/// </summary>
	public int Ahead { get; set; } = ToleranceSpan.Default.Ahead;
}
