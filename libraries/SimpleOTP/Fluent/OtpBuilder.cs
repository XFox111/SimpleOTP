namespace SimpleOTP.Fluent;

/// <summary>
/// Class used to streamline OTP code generation on client devices.
/// </summary>
public static class OtpBuilder
{
	/// <summary>
	/// Use TOTP generator with optional counter period.
	/// </summary>
	/// <param name="period">Period in seconds.</param>
	/// <returns><see cref="Otp"/> instance.</returns>
	public static Otp UseTotp(int period = 30) =>
		new Totp(OtpSecret.CreateNew(), period);

	/// <summary>
	/// Use HOTP generator with optional counter value.
	/// </summary>
	/// <param name="counter">Counter value.</param>
	/// <returns><see cref="Otp"/> instance.</returns>
	public static Otp UseHotp(long counter = 0) =>
		new Hotp(OtpSecret.CreateNew(), counter);

	/// <summary>
	/// Creates <see cref="Otp"/> instance from <see cref="OtpConfig"/> object.
	/// </summary>
	/// <param name="config"><see cref="OtpConfig"/> object.</param>
	/// <returns><see cref="Otp"/> instance.</returns>
	public static Otp FromConfig(OtpConfig config)
	{
		Otp generator = config.Type == OtpType.Totp ?
			new Totp(config.Secret, config.Period) :
			new Hotp(config.Secret, config.Counter);

		generator.Algorithm = config.Algorithm;
		generator.Digits = config.Digits;

		return generator;
	}
}
