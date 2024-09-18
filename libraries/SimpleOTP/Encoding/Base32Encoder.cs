namespace SimpleOTP.Encoding;

/// <summary>
/// Provides methods for encoding and decoding data using the RFC 4648 Base32 standard alphabet.
/// </summary>
public class Base32Encoder : IEncoder
{
	/// <summary>
	/// Gets the singleton instance of the <see cref="Base32Encoder"/> class.
	/// </summary>
	public static Base32Encoder Instance { get; } = new();

	/// <inheritdoc />
	public virtual string Scheme => "base32";

	/// <summary>
	/// Converts a byte array to a Base32 string representation.
	/// </summary>
	/// <param name="bytes">The byte array to convert.</param>
	/// <returns>The Base32 string representation of the byte array.</returns>
	/// <exception cref="ArgumentNullException">Thrown when parameter is null.</exception>
	public string EncodeBytes(byte[] bytes)
	{
		ArgumentNullException.ThrowIfNull(bytes);

		if (bytes.Length < 1)
			return string.Empty;

		char[] outArray = new char[(int)Math.Ceiling(bytes.Length * 8 / 5d)];

		int bitIndex = 0;
		int buffer = 0;
		int filledChars = 0;

		for (int i = 0; i < bytes.Length; i++)
		{
			if (bitIndex >= 5)
			{
				outArray[filledChars++] = ValueToChar(buffer >> (bitIndex - 5) & 0x1F);
				bitIndex -= 5;
			}

			outArray[filledChars++] = ValueToChar(((buffer << (5 - bitIndex)) & 0x1F) | bytes[i] >> (3 + bitIndex));
			buffer = bytes[i];
			bitIndex = 3 + bitIndex;
		}

		// Adding trailing bits
		if (bitIndex > 0)
			outArray[filledChars] = ValueToChar(buffer << (5 - bitIndex) & 0x1F);

		return new string(outArray);
	}

	/// <summary>
	/// Converts a Base32 encoded string to a byte array.
	/// </summary>
	/// <param name="inArray">The Base32 encoded string to convert.</param>
	/// <remarks>Trailing bits are ignored (e.g. AAAR will be treated as AAAQ - 0x00 0x01).</remarks>
	/// <returns>The byte array representation of the Base32 encoded string.</returns>
	/// <exception cref="ArgumentNullException">Thrown when parameter is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="inArray"/> is empty, whitespace, or contains invalid characters.</exception>
	public byte[] GetBytes(string inArray)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(inArray);

		inArray = inArray.TrimEnd('=');
		int buffer = 0x00;
		int bitIndex = 0;
		int filledBytes = 0;
		byte[] outArray = new byte[inArray.Length * 5 / 8];

		for (int i = 0; i < inArray.Length; i++)
		{
			int value = CharToValue(inArray[i]);

			buffer = (buffer << 5) | value;
			bitIndex += 5;

			if (bitIndex >= 8)
			{
				// We have enough bits to fill a byte, flushing it
				outArray[filledBytes++] = (byte)((buffer >> (bitIndex - 8)) & 0xFF);
				bitIndex -= 8;
				buffer &= 0xFF;    // Trimming value to 1 byte to prevent overflow for long strings
			}
		}

		return outArray;
	}

	/// <summary>
	/// Converts a Base32 character to its numeric value.
	/// </summary>
	/// <param name="c">The Base32 character to convert.</param>
	/// <returns>The numeric value of the Base32 character.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="c"/> is not a valid Base32 character.</exception>
	protected virtual int CharToValue(char c) =>
		(int)c switch
		{
			> 0x31 and < 0x38 => c - 0x18,
			> 0x40 and < 0x5B => c - 0x41,
			> 0x60 and < 0x7B => c - 0x61,
			_ => throw new ArgumentException("Character is not a Base32 character.", nameof(c)),
		};

	/// <summary>
	/// Converts a numeric value to its Base32 character.
	/// </summary>
	/// <param name="value">The numeric value to convert.</param>
	/// <returns>The Base32 character corresponding to the numeric value.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is not a valid Base32 value.</exception>
	protected virtual char ValueToChar(int value) =>
		value switch
		{
			< 26 => (char)(value + 0x41),
			< 32 => (char)(value + 0x18),
			_ => throw new ArgumentException("Byte is not a Base32 byte.", nameof(value)),
		};
}
