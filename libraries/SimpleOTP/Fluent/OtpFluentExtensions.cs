namespace SimpleOTP.Fluent;

/// <summary>
/// Provides fluent API for configuring <see cref="Otp"/> objects.
/// </summary>
public static class OtpFluentExtensions
{
	/// <summary>
	/// Creates a new <see cref="Otp"/> object from the provided <see cref="OtpConfig"/>
	/// </summary>
	/// <param name="generator">The <see cref="Otp"/> object to configure.</param>
	/// <param name="bytesLength">The length of the secret in bytes.</param>
	/// <returns>The configured <see cref="Otp"/> object.</returns>
	public static Otp WithNewSecret(this Otp generator, int bytesLength)
	{
		generator.Secret = OtpSecret.CreateNew(bytesLength);
		return generator;
	}

	/// <summary>
	/// Creates a new <see cref="Otp"/> object from the provided <see cref="OtpSecret"/>
	/// </summary>
	/// <param name="generator">The <see cref="Otp"/> object to configure.</param>
	/// <param name="secret">The <see cref="OtpSecret"/> to use.</param>
	/// <returns>The configured <see cref="Otp"/> object.</returns>
	public static Otp WithSecret(this Otp generator, OtpSecret secret)
	{
		generator.Secret = secret;
		return generator;
	}

	/// <summary>
	/// Sets the <see cref="Otp.Digits"/> property.
	/// </summary>
	/// <param name="generator">The <see cref="Otp"/> object to configure.</param>
	/// <param name="digits">The number of digits to use in OTP codes.</param>
	/// <returns>The configured <see cref="Otp"/> object.</returns>
	public static Otp WithDigits(this Otp generator, int digits)
	{
		generator.Digits = digits;
		return generator;
	}

	/// <summary>
	/// Sets the <see cref="Otp.Algorithm"/> property.
	/// </summary>
	/// <param name="generator">The <see cref="Otp"/> object to configure.</param>
	/// <param name="algorithm">The algorithm to use.</param>
	/// <returns>The configured <see cref="Otp"/> object.</returns>
	public static Otp WithAlgorithm(this Otp generator, OtpAlgorithm algorithm)
	{
		generator.Algorithm = algorithm;
		return generator;
	}
}
