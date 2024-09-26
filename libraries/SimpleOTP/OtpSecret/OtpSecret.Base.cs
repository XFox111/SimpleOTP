using System.Numerics;
using System.Security.Cryptography;
using System.Xml.Serialization;
using SimpleOTP.Encoding;

namespace SimpleOTP;

// THIS IS THE BASE OF A PARTIAL CLASS
// List of files
// - OtpSecret.Base.cs			- Base file
// - OtpSecret.Static.cs		- Static members
// - OtpSecret.Serialization.cs	- JSON/XML serialization members and attributes

/// <summary>
/// Represents a one-time password secret.
/// </summary>
public partial class OtpSecret : IEquatable<OtpSecret>, IEquatable<byte[]>, IXmlSerializable, IDisposable
{
	private readonly byte[] _secret;

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpSecret"/> class with a default length of 20 bytes (160 bits).
	/// </summary>
	public OtpSecret() : this(20) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpSecret"/> class with a random secret of the specified length.
	/// </summary>
	/// <remarks>
	/// 20 bytes (160 bits) is the recommended key length specified by <a href="https://datatracker.ietf.org/doc/html/rfc4226#section-4">RFC 4226</a>.
	/// Minimal recommended length is 16 bytes (128 bits).
	/// </remarks>
	/// <param name="length">The length of the secret in bytes.</param>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than 1.</exception>
	public OtpSecret(int length) =>
		_secret = RandomNumberGenerator.GetBytes(length);

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpSecret"/> class from a byte array.
	/// </summary>
	/// <param name="secret">The byte array.</param>
	/// <exception cref="ArgumentNullException"><paramref name="secret"/> is <c>null</c> or empty.</exception>
	public OtpSecret(byte[] secret)
	{
		if (secret is null || secret.Length < 1)
			throw new ArgumentNullException(nameof(secret));

		_secret = secret;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpSecret"/> class from a Base32-encoded string (RFC 4648 §6).
	/// </summary>
	/// <param name="secret">The Base32-encoded string.</param>
	/// <exception cref="ArgumentNullException"><paramref name="secret"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentException"><paramref name="secret"/> is empty or contains invalid characters or only whitespace.</exception>
	public OtpSecret(string secret) : this(secret, DefaultEncoder) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpSecret"/> class from an encoded string.
	/// </summary>
	/// <param name="secret">The encoded string.</param>
	/// <param name="encoder">The encoder.</param>
	/// <exception cref="ArgumentNullException"><paramref name="secret"/> is <c>null</c> or empty.</exception>
	/// <exception cref="ArgumentException"><paramref name="secret"/> is empty or contains invalid characters or only whitespace.</exception>
	public OtpSecret(string secret, IEncoder encoder)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(secret, nameof(secret));
		_secret = encoder.GetBytes(secret);
	}

	/// <summary>
	///	Returns the Base32-encoded string representation of the current <see cref="OtpSecret"/> object.
	/// </summary>
	/// <returns>The Base32-encoded string representation of the current <see cref="OtpSecret"/> object.</returns>
	public override string ToString() =>
		DefaultEncoder.EncodeBytes(_secret);

	/// <summary>
	/// Returns the string representation of the current <see cref="OtpSecret"/> object.
	/// </summary>
	/// <param name="encoder">The encoder.</param>
	/// <returns>The string representation of the current <see cref="OtpSecret"/> object.</returns>
	public string ToString(IEncoder encoder) =>
		encoder.EncodeBytes(_secret);

	/// <inheritdoc/>
	public bool Equals(OtpSecret? other) =>
		other is not null && _secret.SequenceEqual(other._secret);

	/// <inheritdoc/>
	public override bool Equals(object? obj) =>
		obj is OtpSecret other && Equals(other);

	/// <inheritdoc/>
	public bool Equals(byte[]? other) =>
		other is not null && _secret.SequenceEqual(other);

	/// <inheritdoc/>
	public override int GetHashCode() =>
		new BigInteger(_secret ?? []).GetHashCode();

	/// <inheritdoc/>
	public void Dispose()
	{
		Array.Clear(_secret, 0, _secret.Length);
		GC.SuppressFinalize(this);
	}
}
