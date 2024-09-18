namespace SimpleOTP;

/// <summary>
/// Represents a HOTP (HMAC-based One-Time Password) generator.
/// </summary>
public class Hotp : Otp
{
	/// <summary>
	/// Gets or sets the counter value used for generating OTP codes.
	/// </summary>
	public long Counter { get; set; } = 0;

	/// <summary>
	/// Initializes a new instance of the <see cref="Hotp"/> class
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	public Hotp(OtpSecret secret) : base(secret) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="Hotp"/> class
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="counter">The counter value used for generating OTP codes.</param>
	public Hotp(OtpSecret secret, long counter) : base(secret) =>
		Counter = counter;

	/// <summary>
	/// Initializes a new instance of the <see cref="Hotp"/> class
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="counter">The counter value used for generating OTP codes.</param>
	/// <param name="digits">The number of digits in the OTP code.</param>
	public Hotp(OtpSecret secret, long counter, int digits) : base(secret, digits) =>
		Counter = counter;

	/// <summary>
	/// Initializes a new instance of the <see cref="Hotp"/> class
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="counter">The counter value used for generating OTP codes.</param>
	/// <param name="algorithm">The algorithm used for generating OTP codes.</param>
	public Hotp(OtpSecret secret, long counter, OtpAlgorithm algorithm) : base(secret, algorithm) =>
		Counter = counter;

	/// <summary>
	/// Initializes a new instance of the <see cref="Hotp"/> class
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="counter">The counter value used for generating OTP codes.</param>
	/// <param name="algorithm">The algorithm used for generating OTP codes.</param>
	/// <param name="digits">The number of digits in the OTP code.</param>
	public Hotp(OtpSecret secret, long counter, OtpAlgorithm algorithm, int digits) : base(secret, algorithm, digits) =>
		Counter = counter;

	/// <summary>
	/// Gets the current counter value.
	/// </summary>
	/// <returns>The current counter value.</returns>
	protected override long GetCounter() => Counter;
}
