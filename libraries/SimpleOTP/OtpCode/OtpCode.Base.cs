namespace SimpleOTP;

// THIS IS THE BASE OF A PARTIAL STRUCT
// List of files
// - OtpCode.Base.cs			- Base file
// - OtpCode.Static.cs			- Static members
// - OtpCode.Serialization.cs	- JSON/XML serialization members and attributes

/// <summary>
/// Represents a one-time password (OTP) code.
/// </summary>
public readonly partial struct OtpCode : IEquatable<OtpCode>, IEquatable<string>
{
	private readonly int _value;
	private readonly int _digits;

	/// <summary>
	/// Gets a value indicating whether the OTP code can expire (<c>true</c> for TOTP, <c>false</c> for HOTP).
	/// </summary>
	public readonly bool CanExpire => ExpirationTime is not null;

	/// <summary>
	/// Gets the expiration time of the OTP code (TOTP only).
	/// </summary>
	public readonly DateTimeOffset? ExpirationTime { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpCode"/> struct with the specified value with no expiration time.
	/// </summary>
	/// <param name="code">The value of the OTP code.</param>
	/// <param name="digits">The number of digits in the OTP code.</param>
	/// <exception cref="ArgumentNullException"><paramref name="code"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="code"/> is not a valid numeric code.</exception>
	public OtpCode(int code, int digits) : this(code, digits, null) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpCode"/> struct with the specified value and the expiration time.
	/// </summary>
	/// <param name="code">The value of the OTP code.</param>
	/// <param name="digits">The number of digits in the OTP code.</param>
	/// <param name="expirationTime">The expiration time of the OTP code (TOTP only).</param>
	/// <exception cref="ArgumentNullException"><paramref name="code"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="code"/> is not a valid numeric code.</exception>
	public OtpCode(int code, int digits, DateTimeOffset? expirationTime = null)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(code);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(digits);

		_value = code;
		_digits = digits;
		ExpirationTime = expirationTime;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpCode"/> struct with the specified value with no expiration time.
	/// </summary>
	/// <param name="code">The value of the OTP code.</param>
	/// <exception cref="ArgumentNullException"><paramref name="code"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="code"/> is not a valid numeric code.</exception>
	public OtpCode(string code) : this(code, null) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpCode"/> struct with the specified value and the expiration time.
	/// </summary>
	/// <param name="code">The value of the OTP code.</param>
	/// <param name="expirationTime">The expiration time of the OTP code (TOTP only).</param>
	/// <exception cref="ArgumentNullException"><paramref name="code"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="code"/> is not a valid numeric code.</exception>
	public OtpCode(string code, DateTimeOffset? expirationTime = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);

		if (!int.TryParse(code, out var value))
			throw new ArgumentException($"'{code}' is not a valid numeric code.", nameof(code));

		_value = value;
		_digits = code.Length;
		ExpirationTime = expirationTime;
	}

	/// <summary>
	/// Returns a string representation of the OTP code.
	/// </summary>
	/// <returns>A string representation of the OTP code.</returns>
	public override readonly string ToString() =>
		_value.ToString($"D{_digits}");

	/// <summary>
	/// Returns a string representation of the OTP code.
	/// </summary>
	/// <param name="format">The format to use.</param>
	/// <returns>The string representation of the OTP code.</returns>
	public readonly string ToString(string? format) =>
		_value.ToString(format);

	/// <inheritdoc/>
	public bool Equals(OtpCode other) =>
		ToString() == other.ToString();

	/// <inheritdoc/>
	public override bool Equals(object? obj) =>
		obj is OtpCode code && Equals(code);

	/// <inheritdoc/>
	public bool Equals(string? other) =>
		ToString() == other;

	/// <inheritdoc/>
	public override int GetHashCode() =>
		_value.GetHashCode();
}
