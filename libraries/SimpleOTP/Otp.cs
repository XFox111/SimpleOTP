using System.Security.Cryptography;

namespace SimpleOTP;

// TODO: Add tests

/// <summary>
/// Represents an abstract class for generating and validating One-Time Passwords (OTP).
/// </summary>
public abstract class Otp
{
	#region Properties

	/// <summary>
	/// Gets or sets the secret key used for generating OTPs.
	/// </summary>
	public OtpSecret Secret { get; set; }

	/// <summary>
	/// Gets or sets the algorithm used for generating OTP codes.
	/// </summary>
	public OtpAlgorithm Algorithm { get; set; } = OtpAlgorithm.SHA1;

	/// <summary>
	/// Gets or sets the number of digits in the OTP code.
	/// </summary>
	/// <value>Default: 6. Recommended: 6-8.</value>
	public int Digits { get; set; } = 6;

	#endregion

	#region Constructors

	/// <summary>
	/// Initializes a new instance of the <see cref="Otp"/> class.
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="algorithm">The algorithm used for generating OTP codes.</param>
	/// <param name="digits">The number of digits in the OTP code.</param>
	public Otp(OtpSecret secret, OtpAlgorithm algorithm, int digits) =>
		(Secret, Algorithm, Digits) = (secret, algorithm, digits);

	/// <summary>
	/// Initializes a new instance of the <see cref="Otp"/> class.
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="algorithm">The algorithm used for generating OTP codes.</param>
	public Otp(OtpSecret secret, OtpAlgorithm algorithm) : this(secret, algorithm, 6) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="Otp"/> class.
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	/// <param name="digits">The number of digits in the OTP code.</param>
	public Otp(OtpSecret secret, int digits) : this(secret, OtpAlgorithm.SHA1, digits) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="Otp"/> class.
	/// </summary>
	/// <param name="secret">The secret key used for generating OTP codes.</param>
	public Otp(OtpSecret secret) : this(secret, OtpAlgorithm.SHA1, 6) { }

	#endregion

	#region Methods

	// Generate

	/// <summary>
	/// Generates an OTP code.
	/// </summary>
	/// <returns>The generated OTP code.</returns>
	public OtpCode Generate() =>
		Generate(GetCounter());

	/// <summary>
	/// Generates an OTP code for the specified counter value.
	/// </summary>
	/// <param name="counter">The counter value to generate the OTP code for.</param>
	/// <returns>The generated OTP code.</returns>
	public virtual OtpCode Generate(long counter) =>
		new(Compute(counter), Digits);

	// Validate

	/// <summary>
	/// Validates an OTP code.
	/// </summary>
	/// <param name="code">The OTP code to validate.</param>
	/// <returns><c>true</c> if the OTP code is valid; otherwise, <c>false</c>.</returns>
	/// <exception cref="InvalidOperationException">
	/// Implementation for the <see cref="Algorithm"/> algorithm was not found.
	/// Use <see cref="HashAlgorithmProviders.AddProvider(OtpAlgorithm)"/> to register an implementation.
	/// </exception>
	public bool Validate(OtpCode code) =>
		Validate(code, (1, 1));

	/// <summary>
	/// Validates an OTP code with tolerance.
	/// </summary>
	/// <param name="code">The OTP code to validate.</param>
	/// <param name="tolerance">The tolerance span for code validation.</param>
	/// <returns><c>true</c> if the OTP code is valid; otherwise, <c>false</c>.</returns>
	/// <exception cref="InvalidOperationException">
	/// Implementation for the <see cref="Algorithm"/> algorithm was not found.
	/// Use <see cref="HashAlgorithmProviders.AddProvider(OtpAlgorithm)"/> to register an implementation.
	/// </exception>
	public bool Validate(OtpCode code, ToleranceSpan tolerance) =>
		Validate(code, tolerance, out _);

	/// <summary>
	/// Validates an OTP code with tolerance and returns the resynchronization value.
	/// </summary>
	/// <param name="code">The OTP code to validate.</param>
	/// <param name="tolerance">The tolerance span for code validation.</param>
	/// <param name="resyncValue">The resynchronization value. Indicates how much given OTP code is ahead or behind the current counter value.</param>
	/// <returns><c>true</c> if the OTP code is valid; otherwise, <c>false</c>.</returns>
	/// <exception cref="InvalidOperationException">
	/// Implementation for the <see cref="Algorithm"/> algorithm was not found.
	/// Use <see cref="HashAlgorithmProviders.AddProvider(OtpAlgorithm)"/> to register an implementation.
	/// </exception>
	public bool Validate(OtpCode code, ToleranceSpan tolerance, out int resyncValue) =>
		Validate(code, tolerance, GetCounter(), out resyncValue);

	/// <summary>
	/// Validates an OTP code with tolerance and base counter value, and returns the resynchronization value.
	/// </summary>
	/// <param name="code">The OTP code to validate.</param>
	/// <param name="tolerance">The tolerance span for code validation.</param>
	/// <param name="baseCounter">The base counter value.</param>
	/// <param name="resyncValue">The resynchronization value. Indicates how much given OTP code is ahead or behind the current counter value.</param>
	/// <returns><c>true</c> if the OTP code is valid; otherwise, <c>false</c>.</returns>
	/// <exception cref="InvalidOperationException">
	/// Implementation for the <see cref="Algorithm"/> algorithm was not found.
	/// Use <see cref="HashAlgorithmProviders.AddProvider(OtpAlgorithm)"/> to register an implementation.
	/// </exception>
	public bool Validate(OtpCode code, ToleranceSpan tolerance, long baseCounter, out int resyncValue)
	{
		resyncValue = 0;

		using KeyedHashAlgorithm? hashAlgorithm = HashAlgorithmProviders.GetProvider(Algorithm) ??
			throw new InvalidOperationException($"Implementation for the \"{Algorithm}\" algorithm was not found.");

		for (int i = -tolerance.Behind; i <= tolerance.Ahead; i++)
			if (code == Compute(baseCounter + i, hashAlgorithm).ToString($"D{Digits}"))
			{
				resyncValue = i;
				return true;
			}

		return false;
	}

	/// <summary>
	/// Gets the current counter value.
	/// </summary>
	/// <returns>The current counter value.</returns>
	protected abstract long GetCounter();

	/// <summary>
	/// Computes the OTP code for the specified counter value.
	/// </summary>
	/// <param name="counter">The counter value to compute the OTP code for.</param>
	/// <returns>The OTP code for the specified counter value.</returns>
	/// <exception cref="InvalidOperationException">
	/// Implementation for the <see cref="Algorithm"/> algorithm was not found.
	/// Use <see cref="HashAlgorithmProviders.AddProvider(OtpAlgorithm)"/> to register an implementation.
	/// </exception>
	protected int Compute(long counter)
	{
		using KeyedHashAlgorithm? hashAlgorithm = HashAlgorithmProviders.GetProvider(Algorithm) ??
			throw new InvalidOperationException($"Implementation for the \"{Algorithm}\" algorithm was not found.");

		return Compute(counter, hashAlgorithm);
	}

	/// <summary>
	/// Computes the OTP code for the specified counter value using provided hash algorithm.
	/// </summary>
	/// <param name="counter">The counter value to compute the OTP code for.</param>
	/// <param name="hashAlgorithm">The hash algorithm to use for computing the OTP code.</param>
	/// <remarks>You need to dispose of the <paramref name="hashAlgorithm"/> object yourself when you are done using it.</remarks>
	/// <returns>The OTP code for the specified counter value.</returns>
	protected virtual int Compute(long counter, KeyedHashAlgorithm hashAlgorithm)
	{
		byte[] counterBytes = BitConverter.GetBytes(counter);

		// "The HOTP values generated by the HOTP generator are treated as big endian."
		// https://datatracker.ietf.org/doc/html/rfc4226#section-5.2
		if (BitConverter.IsLittleEndian)
			Array.Reverse(counterBytes);

		hashAlgorithm.Key = Secret;

		byte[] hash = hashAlgorithm.ComputeHash(counterBytes);

		// Converting hash to n-digits value
		// See RFC4226 Section 5.4 for more details
		// https://datatracker.ietf.org/doc/html/rfc4226#section-5.4
		int offset = hash[^1] & 0x0F;

		int value =
			(hash[offset + 0] & 0x7F) << 24 |   // Result value should be a 31-bit integer, hence the 0x7F (0111 1111)
			(hash[offset + 1] & 0xFF) << 16 |
			(hash[offset + 2] & 0xFF) << 8 |
			(hash[offset + 3] & 0xFF) << 0;

		return value % (int)Math.Pow(10, Digits);
	}

	#endregion
}
