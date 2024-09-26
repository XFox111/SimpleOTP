namespace SimpleOTP.Fluent;

/// <summary>
/// Class used to streamline OTP code configuration on client devices.
/// </summary>
public static class OtpConfigBuilder
{
	/// <summary>
	/// Use TOTP configuration with optional counter period.
	/// </summary>
	/// <param name="accountName">Account name.</param>
	/// <param name="period">Period in seconds.</param>
	/// <returns><see cref="OtpConfig"/> instance.</returns>
	public static OtpConfig UseTotp(string accountName, int period = 30) =>
		new(accountName)
		{
			Type = OtpType.Totp,
			Period = period
		};

	/// <summary>
	/// Use HOTP configuration with optional counter.
	/// </summary>
	/// <param name="accountName">Account name.</param>
	/// <param name="counter">Counter value.</param>
	/// <returns><see cref="OtpConfig"/> instance.</returns>
	public static OtpConfig UseHotp(string accountName, long counter = 0) =>
		new(accountName)
		{
			Type = OtpType.Hotp,
			Counter = counter
		};

	/// <summary>
	/// Use TOTP which satisfies Apple's specification requirements.
	/// </summary>
	/// <param name="accountName">Account name.</param>
	/// <param name="issuerName">Issuer/application/service display name.</param>
	/// <param name="issuerDomain">Issuer/application/service domain name.</param>
	/// <returns><see cref="OtpConfig"/> instance.</returns>
	public static OtpConfig UseApple(string accountName, string issuerName, string issuerDomain) =>
		new(accountName)
		{
			Type = OtpType.Totp,
			Secret = OtpSecret.CreateNew(20),
			Digits = 6,
			IssuerLabel = issuerName,
			Issuer = issuerDomain,
			Label = accountName
		};
}
