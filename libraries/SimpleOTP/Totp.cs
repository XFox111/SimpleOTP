namespace SimpleOTP;

/// <summary>
/// Represents a Time-based One-Time Password (TOTP) generator.
/// </summary>
public class Totp : Otp
{
	/// <summary>
	/// Gets or sets the time period (in seconds) for which each generated OTP is valid.
	/// </summary>
	/// <remarks>Also used to calculate the current counter value.</remarks>
	public int Period { get; set; } = 30;

	#region Constructors

	/// <summary>
	/// Initializes a new instance of the <see cref="Totp"/> class with the specified secret key.
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	public Totp(OtpSecret secret) : base(secret) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="Totp"/> class with the specified secret key and number of OTP code digits.
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="period">The time period (in seconds) for which each generated OTP is valid.</param>
	public Totp(OtpSecret secret, int period) : base(secret) =>
		Period = period;

	/// <summary>
	/// Initializes a new instance of the <see cref="Totp"/> class with the specified secret key, number of OTP code digits, and time period.
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="period">The time period (in seconds) for which each generated OTP is valid.</param>
	/// <param name="digits">The number of digits in the OTP code.</param>
	public Totp(OtpSecret secret, int period, int digits) : base(secret, digits) =>
		Period = period;

	/// <summary>
	/// Initializes a new instance of the <see cref="Totp"/> class with the specified secret key, hash algorithm, and time period.
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="period">The time period (in seconds) for which each generated OTP is valid.</param>
	/// <param name="algorithm">The algorithm used for generating OTP codes.</param>
	public Totp(OtpSecret secret, int period, OtpAlgorithm algorithm) : base(secret, algorithm) =>
		Period = period;

	/// <summary>
	/// Initializes a new instance of the <see cref="Totp"/> class with the specified secret key, hash algorithm, number of digits, and time period.
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="period">The time period (in seconds) for which each generated OTP is valid.</param>
	/// <param name="algorithm">The algorithm used for generating OTP codes.</param>
	/// <param name="digits">The number of digits in the OTP code.</param>
	public Totp(OtpSecret secret, int period, OtpAlgorithm algorithm, int digits) : base(secret, algorithm, digits) =>
		Period = period;

	#endregion

	/// <summary>
	/// Generates an OTP based on the specified counter value.
	/// </summary>
	/// <param name="counter">The counter value to use for OTP generation.</param>
	/// <returns>An instance of <see cref="OtpCode"/> representing the generated OTP.</returns>
	public override OtpCode Generate(long counter) =>
		new(Compute(counter), Digits, DateTime.UnixEpoch.AddSeconds((counter + 1) * Period));

	/// <summary>
	/// Generates an OTP based on the specified date and time.
	/// </summary>
	/// <param name="date">The date and time to use for OTP generation.</param>
	/// <returns>An instance of <see cref="OtpCode"/> representing the generated OTP.</returns>
	public OtpCode Generate(DateTimeOffset date) =>
		Generate(date.ToUnixTimeSeconds() / Period);

	/// <summary>
	/// Gets the current counter value based on the current UTC time and the configured time period.
	/// </summary>
	/// <returns>The current counter value.</returns>
	protected override long GetCounter() =>
		DateTimeOffset.UtcNow.ToUnixTimeSeconds() / Period;
}
