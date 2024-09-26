using System.Collections.Specialized;

namespace SimpleOTP;

// THIS IS THE BASE OF A PARTIAL CLASS
// List of files
// - OtpConfig.Base.cs				- Base file
// - OtpConfig.Constructors.cs		- Instance constructors
// - OtpConfig.Methods.cs			- Instance methods and serialization
// - OtpConfig.Static.cs			- Static members

/// <summary>
/// Represents the configuration for a One-Time Password (OTP).
/// </summary>
public partial record class OtpConfig
{
	/// <summary>
	/// Gets or sets the type of the OTP.
	/// </summary>
	/// <value>Default is: <see cref="OtpType.Totp"/></value>
	/// <remarks>
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.1">Internet-Draft</a>.<br />
	/// <b>IMPORTANT:</b> Some authenticators do not support <see cref="OtpType.Hotp"/>.
	/// </remarks>
	public OtpType Type { get; set; } = OtpType.Totp;

	/// <summary>
	/// Gets or sets the issuer label prefix of the OTP.
	/// </summary>
	/// <remarks>
	/// <list type="bullet">
	/// <item>Not recommended for use in most cases.</item>
	/// <item>Most authenticators do not support this prefix and mess with the <see cref="Label"/> string.</item>
	/// <item>Required if you intend to use <see cref="OtpUriFormat.Apple"/>. Use this prefix to set the issuer display name.</item>
	/// </list>
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.2">Internet-Draft</a>.
	/// </remarks>
	public string? IssuerLabel { get; set; }

	/// <summary>
	/// Gets or sets the label of the OTP.
	/// </summary>
	/// <remarks>
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.2">Internet-Draft</a>.
	/// </remarks>
	public string Label { get; set; }

	/// <summary>
	/// Gets or sets the secret of the OTP.
	/// </summary>
	/// <value>Default: 160-bit key. Minimal recommended: 128 bits</value>
	/// <remarks>
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.3.1">Internet-Draft</a>
	/// </remarks>
	public OtpSecret Secret { get; set; } = OtpSecret.CreateNew();

	/// <summary>
	/// Gets or sets the hashing algorithm of the OTP.
	/// </summary>
	/// <value>Default: <see cref="OtpAlgorithm.SHA1"/></value>
	/// <remarks>
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.3.2">Internet-Draft</a><br />
	/// <b>IMPORTANT:</b> Some authenticators do not support algorithms other than <see cref="OtpAlgorithm.SHA1"/>.
	/// </remarks>
	public OtpAlgorithm Algorithm { get; set; } = OtpAlgorithm.SHA1;

	/// <summary>
	/// Gets or sets the issuer of the OTP. Optional.
	/// </summary>
	/// <remarks>
	/// <list type="bullet">
	/// <item>Use this property instead of <see cref="IssuerLabel"/>.</item>
	/// <item>Required if you intend to use <see cref="OtpUriFormat.Apple"/>. Use this property to set the issuer domain name.</item>
	/// </list>
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.3.6">Internet-Draft</a>
	/// </remarks>
	public string? Issuer { get; set; }

	/// <summary>
	/// Gets or sets the number of digits of the OTP codes.
	/// </summary>
	/// <value>Default: 6. Recommended: 6 or 8</value>
	/// <remarks>
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.3.3">Internet-Draft</a><br />
	/// <b>IMPORTANT:</b> Some authenticators do not support digits other than 6.
	/// </remarks>
	public int Digits { get; set; } = 6;

	/// <summary>
	/// Gets or sets the counter of the OTP. Required for <see cref="OtpType.Hotp"/>. Ignored for <see cref="OtpType.Totp"/>.
	/// </summary>
	/// <value>Default: 0</value>
	/// <remarks>
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.3.4">Internet-Draft</a><br />
	/// <b>IMPORTANT:</b> Some authenticators do not support <see cref="OtpType.Hotp"/>.
	/// </remarks>
	public long Counter { get; set; } = 0;

	/// <summary>
	/// Gets or sets the period of the OTP in seconds. Optional for <see cref="OtpType.Totp"/>. Ignored for <see cref="OtpType.Hotp"/>.
	/// </summary>
	/// <value>Default: 30</value>
	/// <remarks>
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.3.5">Internet-Draft</a><br />
	/// <b>IMPORTANT:</b> Some authenticators support only periods of 30 seconds.
	/// </remarks>
	public int Period { get; set; } = 30;

	/// <summary>
	/// Gets the custom vendor-specified properties of the current OTP configuration.
	/// </summary>
	/// <remarks>
	/// If set, reserved keys
	/// <c>issuer, digits, counter, secret, period and algorithm</c>
	/// will be removed from the <see cref="CustomProperties"/> upon it's serialization to URI.<br />
	/// <a href="https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html#section-3.3.7">Internet-Draft</a>
	/// </remarks>
	public NameValueCollection CustomProperties { get; } = [];

	// Reserved keys, which are to be removed for CustomProperties
	private static readonly string[] _reservedKeys =
	[
		nameof(Issuer),
		nameof(Digits),
		nameof(Counter),
		nameof(Secret),
		nameof(Period),
		nameof(Algorithm)
	];
}
