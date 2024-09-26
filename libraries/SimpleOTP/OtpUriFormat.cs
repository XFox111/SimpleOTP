namespace SimpleOTP;

/// <summary>
/// Bitwise flags for specifying the format of One-Time Password (OTP) URIs.
/// </summary>
public enum OtpUriFormat : ushort
{
	/// <summary>
	/// Represents a minimal URI format - only non-default properties are included.
	/// </summary>
	/// <remarks>
	/// This is the default format.
	/// </remarks>
	Minimal = 0b_0000_0001,

	/// <summary>
	/// Represents a full URI format - all properties are included.
	/// </summary>
	Full = 0b_0000_0010,

	/// <summary>
	/// Represents a Google URI format.
	/// </summary>
	/// <remarks>
	/// This is the default format.<br />
	/// <a href="https://github.com/google/google-authenticator/wiki/Key-Uri-Format">Google Authenticator. Key Uri Format</a>
	/// </remarks>
	Google = 0b_0001_0000,

	/// <summary>
	/// Represents an Apple URI format.
	/// </summary>
	/// <remarks>
	/// <a href="https://developer.apple.com/documentation/authenticationservices/securing_logins_with_icloud_keychain_verification_codes">
	/// Apple. Securing Logins with iCloud Keychain Verification Codes
	/// </a>
	/// </remarks>
	Apple = 0b_0010_0000,

	/// <summary>
	/// Represents an IBM URI format.
	/// </summary>
	/// <remarks>
	/// <a href="https://www.ibm.com/docs/en/sva/9.0.6?topic=authentication-configuring-totp-one-time-password-mechanism">
	/// IBM. Authentication Configuring TOTP One-Time Password Mechanism
	/// </a>
	/// </remarks>
	IBM = 0b_0100_0000,

	/// <summary>
	/// Represents a Yubico URI format.
	/// </summary>
	/// <remarks>
	/// <a href="https://docs.yubico.com/yesdk/users-manual/application-oath/uri-string-format.html">
	/// Yubico. URI String Format
	/// </a>
	/// </remarks>
	Yubico = 0b_1000_0000,

	/// <summary>
	/// Represents an IIJ URI format.
	/// </summary>
	/// <remarks>
	/// <a href="https://www1.auth.iij.jp/smartkey/en/uri_v1.html">Internet Initiative Japan. URI format</a>
	/// </remarks>
	IIJ = 0b_0001_0000_0000
}
