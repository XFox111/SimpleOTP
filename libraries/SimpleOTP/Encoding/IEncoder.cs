namespace SimpleOTP.Encoding;

/// <summary>
/// Provides methods for encoding and decoding data using the RFC 4648 Base32 "Extended Hex" alphabet.
/// </summary>
public interface IEncoder
{
	/// <summary>
	/// Gets the encoding scheme used by the encoder (e.g. <c>base32</c> or <c>base32hex</c>).
	/// </summary>
	public string Scheme { get; }

	/// <summary>
	/// Converts a byte array to a Base32 string representation.
	/// </summary>
	/// <param name="data">The byte array to convert.</param>
	/// <returns>The Base32 string representation of the byte array.</returns>
	public string EncodeBytes(byte[] data);

	/// <summary>
	/// Converts a Base32 encoded string to a byte array.
	/// </summary>
	/// <param name="data">The Base32 encoded string to convert.</param>
	/// <returns>The byte array representation of the Base32 encoded string.</returns>
	public byte[] GetBytes(string data);
}
