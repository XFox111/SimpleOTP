namespace SimpleOTP;

/// <summary>
/// Represents the type of One-Time Password (OTP).
/// </summary>
public enum OtpType
{
	/// <summary>
	/// Time-based One-Time Password (TOTP).
	/// </summary>
	/// <remarks>
	/// <a href="https://tools.ietf.org/html/rfc6238">RFC 6238</a>
	/// </remarks>
	Totp = 0,
	/// <summary>
	/// HMAC-based One-Time Password (HOTP).
	/// </summary>
	/// <remarks>
	/// <a href="https://tools.ietf.org/html/rfc4226">RFC 4226</a>
	/// </remarks>
	Hotp = 1
}
