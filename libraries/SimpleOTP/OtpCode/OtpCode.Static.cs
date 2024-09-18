namespace SimpleOTP;

// THIS IS THE SECTION OF A PARTIAL STRUCT
// Description: Section of OtpCode struct that holds static members
// Base file: OtpCode.Base.cs

public readonly partial struct OtpCode
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public static implicit operator string(OtpCode code) => code.ToString();
	public static implicit operator OtpCode(string code) => new(code);

	public static bool operator ==(OtpCode left, OtpCode right) => left.Equals(right);
	public static bool operator ==(string left, OtpCode right) => left.Equals(right._value);
	public static bool operator ==(OtpCode left, string right) => left._value.Equals(right);

	public static bool operator !=(OtpCode left, OtpCode right) => !(left == right);
	public static bool operator !=(string left, OtpCode right) => !(left == right);
	public static bool operator !=(OtpCode left, string right) => !(left == right);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

	/// <summary>
	/// Parses the specified <see cref="string"/> into an <see cref="OtpCode"/> object.
	/// </summary>
	/// <param name="code">The string to parse.</param>
	/// <returns>An <see cref="OtpCode"/> object.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="code"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="code"/> is not a valid numeric code.</exception>
	public static OtpCode Parse(string code) =>
		new(code);

	/// <summary>
	/// Tries to parse the specified <see cref="string"/> into an <see cref="OtpCode"/> object.
	/// </summary>
	/// <param name="code">The string to parse.</param>
	/// <param name="result">The parsed <see cref="OtpCode"/> object.</param>
	/// <returns><see langword="true"/> if <paramref name="code"/> was parsed successfully; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse(string code, out OtpCode result)
	{
		try
		{
			result = new(code);
			return true;
		}
		catch
		{
			result = default;
			return false;
		}
	}
}
