using SimpleOTP.Encoding;

namespace SimpleOTP;

// THIS IS THE SECTION OF A PARTIAL CLASS
// Description: Section of OtpSecret class that holds static members
// Base file: OtpSecret.Base.cs

public partial class OtpSecret
{
	/// <summary>
	/// Gets or sets the default encoder for parsing/encoding/serializing secrets.
	/// </summary>
	public static IEncoder DefaultEncoder { get; set; } = Base32Encoder.Instance;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public static implicit operator byte[](OtpSecret secret) => secret._secret;
	public static implicit operator string(OtpSecret secret) => secret.ToString();

	public static explicit operator OtpSecret(byte[] secret) => new(secret);
	public static explicit operator OtpSecret(string secret) => new(secret);

	public static bool operator ==(OtpSecret left, OtpSecret right) => left.Equals(right);
	public static bool operator ==(OtpSecret left, byte[] right) => left.Equals(right);
	public static bool operator ==(OtpSecret left, string right) => left.Equals(right);
	public static bool operator ==(byte[] left, OtpSecret right) => right.Equals(left);
	public static bool operator ==(string left, OtpSecret right) => right.Equals(left);

	public static bool operator !=(OtpSecret left, OtpSecret right) => !(left == right);
	public static bool operator !=(OtpSecret left, byte[] right) => !(left == right);
	public static bool operator !=(OtpSecret left, string right) => !(left == right);
	public static bool operator !=(byte[] left, OtpSecret right) => !(left == right);
	public static bool operator !=(string left, OtpSecret right) => !(left == right);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

	/// <summary>
	/// Creates a copy of the specified <see cref="OtpSecret"/> object.
	/// </summary>
	/// <param name="source">The <see cref="OtpSecret"/> object to copy.</param>
	/// <returns>A copy of the specified <see cref="OtpSecret"/> object.</returns>
	public static OtpSecret CreateCopy(OtpSecret source)
	{
		byte[] bytes = new byte[source._secret.Length];
		Array.Copy(source._secret, bytes, source._secret.Length);
		return new(bytes);
	}

	/// <summary>
	/// Creates a new random <see cref="OtpSecret"/> object with a default length of 20 bytes.
	/// </summary>
	/// <remarks>
	/// 20 bytes (160 bits) is the recommended key length specified by <a href="https://datatracker.ietf.org/doc/html/rfc4226#section-4">RFC 4226</a>.
	/// Minimal recommended length is 16 bytes (128 bits).
	/// </remarks>
	/// <returns>A new random <see cref="OtpSecret"/> object.</returns>
	public static OtpSecret CreateNew() =>
		new();

	/// <summary>
	/// Creates a new random <see cref="OtpSecret"/> object with the specified length.
	/// </summary>
	/// <remarks>
	/// 20 bytes (160 bits) is the recommended key length specified by <a href="https://datatracker.ietf.org/doc/html/rfc4226#section-4">RFC 4226</a>.
	/// Minimal recommended length is 16 bytes (128 bits).
	/// </remarks>
	/// <param name="length">The length of the secret in bytes.</param>
	/// <returns>A new random <see cref="OtpSecret"/> object.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than 1.</exception>
	public static OtpSecret CreateNew(int length) =>
		new(length);

	/// <summary>
	/// Parses a Base32-encoded string into an <see cref="OtpSecret"/> object.
	/// </summary>
	/// <param name="secret">The Base32-encoded string.</param>
	/// <returns>An <see cref="OtpSecret"/> object.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="secret"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentException"><paramref name="secret"/> is empty or contains invalid characters or only whitespace.</exception>
	public static OtpSecret Parse(string secret) =>
		new(secret);

	/// <summary>
	/// Parses a Base32-encoded string into an <see cref="OtpSecret"/> object.
	/// </summary>
	/// <param name="secret">The Base32-encoded string.</param>
	/// <param name="encoder">The encoder.</param>
	/// <returns>An <see cref="OtpSecret"/> object.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="secret"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentException"><paramref name="secret"/> is empty or contains invalid characters or only whitespace.</exception>
	public static OtpSecret Parse(string secret, IEncoder encoder) =>
		new(secret, encoder);

	/// <summary>
	/// Tries to parse a Base32-encoded string into an <see cref="OtpSecret"/> object.
	/// </summary>
	/// <param name="secret">The Base32-encoded string.</param>
	/// <param name="otpSecret">When this method returns, contains the <see cref="OtpSecret"/> object, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
	/// <returns><c>true</c> if <paramref name="secret"/> was converted successfully; otherwise, <c>false</c>.</returns>
	public static bool TryParse(string secret, out OtpSecret? otpSecret) =>
		TryParse(secret, DefaultEncoder, out otpSecret);

	/// <summary>
	/// Tries to parse a Base32-encoded string into an <see cref="OtpSecret"/> object.
	/// </summary>
	/// <param name="secret">The Base32-encoded string.</param>
	/// <param name="encoder">The encoder.</param>
	/// <param name="otpSecret">When this method returns, contains the <see cref="OtpSecret"/> object, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
	/// <returns><c>true</c> if <paramref name="secret"/> was converted successfully; otherwise, <c>false</c>.</returns>
	public static bool TryParse(string secret, IEncoder encoder, out OtpSecret? otpSecret)
	{
		try
		{
			otpSecret = new(secret, encoder);
			return true;
		}
		catch
		{
			otpSecret = null;
			return false;
		}
	}

	/// <summary>
	/// Creates a new <see cref="OtpSecret"/> object from a byte array.
	/// </summary>
	/// <param name="secret">The byte array.</param>
	/// <returns>An <see cref="OtpSecret"/> object.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="secret"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="secret"/> is empty.</exception>
	public static OtpSecret FromBytes(byte[] secret) =>
		new(secret);
}
