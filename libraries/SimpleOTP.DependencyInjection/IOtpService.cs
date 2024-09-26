namespace SimpleOTP.DependencyInjection;

/// <summary>
/// Provides methods for generating and validating One-Time Passwords.
/// </summary>
public interface IOtpService
{
	/// <summary>
	/// Creates an OTP URI for specified user and secret.
	/// </summary>
	/// <param name="username">The username of the user.</param>
	/// <param name="secret">The secret to use.</param>
	/// <param name="counter">(only for HOTP) The counter to use.</param>
	/// <returns>The generated URI.</returns>
	public Uri CreateUri(string username, OtpSecret secret, long counter = 0);

	/// <summary>
	/// Creates an OTP code for specified user and secret.
	/// </summary>
	/// <param name="secret">The secret to use.</param>
	/// <param name="counter">(only for HOTP) The counter to use.</param>
	public OtpCode GenerateCode(OtpSecret secret, long counter = 0);

	/// <summary>
	/// Validates an OTP code for specified user and secret.
	/// </summary>
	/// <param name="code">The code to validate.</param>
	/// <param name="secret">The secret to use.</param>
	/// <param name="resyncValue">The resync value. Shows how much the code is ahead or behind the current counter value.</param>
	/// <param name="counter">(only for HOTP) The counter to use.</param>
	/// <returns><c>true</c> if the code is valid; otherwise, <c>false</c>.</returns>
	public bool ValidateCode(OtpCode code, OtpSecret secret, out int resyncValue, long counter = 0);
}
