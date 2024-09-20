using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using SimpleOTP.Converters;

namespace SimpleOTP;

/// <summary>
/// Represents the hashing algorithm used for One-Time Passwords.
/// </summary>
[Serializable]
[JsonConverter(typeof(OtpAlgorithmJsonConverter))]
public readonly partial struct OtpAlgorithm : IEquatable<OtpAlgorithm>, IEquatable<string>, IXmlSerializable
{
	/// <summary>
	/// The HMAC-SHA1 hashing algorithm.
	/// </summary>
	public static OtpAlgorithm SHA1 { get; } = new("SHA1");

	/// <summary>
	/// The HMAC-SHA256 hashing algorithm.
	/// </summary>
	public static OtpAlgorithm SHA256 { get; } = new("SHA256");

	/// <summary>
	/// The HMAC-SHA512 hashing algorithm.
	/// </summary>
	public static OtpAlgorithm SHA512 { get; } = new("SHA512");

	/// <summary>
	/// The HMAC-MD5 hashing algorithm.
	/// </summary>
	/// <remarks>
	/// This is not a standard algorithm, but it is defined by IIJ specification and recognized by default.<br />
	/// <a href="https://www1.auth.iij.jp/smartkey/en/uri_v1.html">Internet Initiative Japan. URI format</a>
	/// </remarks>
	public static OtpAlgorithm MD5 { get; } = new("MD5");

	private readonly string _value;

	/// <summary>
	/// Initializes a new instance of the <see cref="OtpAlgorithm"/> struct.
	/// </summary>
	/// <param name="value">The algorithm to use.</param>
	/// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty or whitespace.</exception>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	public OtpAlgorithm(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value);

		if (StandardAlgorithmsRegex().IsMatch(value))
			_value = StandardAlgorithmsRegex().Match(value).Value.ToUpperInvariant();
		else
			_value = value.ToUpperInvariant();
	}

	/// <inheritdoc/>
	public bool Equals(OtpAlgorithm other) =>
		_value == other._value;

	/// <inheritdoc/>
	public bool Equals(string? other)
	{
		if (string.IsNullOrWhiteSpace(other))
			return _value is null;
		if (_value is null)
			return false;

		return Equals(new OtpAlgorithm(other));
	}

	/// <inheritdoc/>
	public override bool Equals(object? obj) =>
		obj is OtpAlgorithm algorithm && Equals(algorithm);

	/// <summary>
	/// Determines whether the specified <see cref="OtpAlgorithm"/> is standard HMAC SHA algorithm (SHA-1, SHA-256 or SHA-512).
	/// </summary>
	/// <returns><see langword="true"/> if the specified <see cref="OtpAlgorithm"/> is standard; otherwise, <see langword="false"/>.</returns>
	public bool IsStandard() =>
		IsStandard(_value);

	/// <inheritdoc/>
	public override int GetHashCode() => _value.GetHashCode();

	/// <summary>
	/// Returns the string representation of the <see cref="OtpAlgorithm"/> struct.
	/// </summary>
	/// <returns>The string representation of the <see cref="OtpAlgorithm"/> struct.</returns>
	public override string ToString() => _value;

	/// <inheritdoc/>
	public XmlSchema? GetSchema() => null;

	/// <inheritdoc/>
	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();

		if (reader.NodeType != XmlNodeType.Element)
			throw new XmlException("Invalid XML element.");

		string algorithm = reader.ReadElementContentAsString();

#pragma warning disable CS9195 // Argument should be passed with the in keyword
		Unsafe.AsRef(this) = new OtpAlgorithm(algorithm);
#pragma warning restore CS9195 // Argument should be passed with the in keyword
	}

	/// <inheritdoc/>
	public void WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString("standard", IsStandard().ToString().ToLowerInvariant());
		writer.WriteString(_value);
	}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public static bool operator ==(OtpAlgorithm left, OtpAlgorithm right) => left.Equals(right);
	public static bool operator ==(string left, OtpAlgorithm right) => right.Equals(left);
	public static bool operator ==(OtpAlgorithm left, string right) => left.Equals(right);

	public static bool operator !=(OtpAlgorithm left, OtpAlgorithm right) => !(left == right);
	public static bool operator !=(string left, OtpAlgorithm right) => !(left == right);
	public static bool operator !=(OtpAlgorithm left, string right) => !(left == right);

	public static implicit operator string(OtpAlgorithm algorithm) => algorithm._value;
	public static explicit operator OtpAlgorithm(string value) => new(value);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

	/// <summary>
	/// Determines whether the specified <see cref="OtpAlgorithm"/> is standard HMAC SHA algorithm (SHA-1, SHA-256 or SHA-512).
	/// </summary>
	/// <param name="algorithm">The algorithm to check.</param>
	/// <returns><see langword="true"/> if the specified <see cref="OtpAlgorithm"/> is standard; otherwise, <see langword="false"/>.</returns>
	public static bool IsStandard(string algorithm) =>
		StandardAlgorithmsRegex().IsMatch(algorithm);

	/// <summary>
	/// Determines whether the specified <see cref="OtpAlgorithm"/> is standard HMAC SHA algorithm (SHA-1, SHA-256 or SHA-512).
	/// </summary>
	/// <param name="algorithm">The algorithm to check.</param>
	/// <returns><see langword="true"/> if the specified <see cref="OtpAlgorithm"/> is standard; otherwise, <see langword="false"/>.</returns>
	public static bool IsStandard(OtpAlgorithm algorithm) =>
		IsStandard(algorithm._value);

	[GeneratedRegex(@"(?<=Hmac)?SHA(1|256|512)", RegexOptions.IgnoreCase, "")]
	private static partial Regex StandardAlgorithmsRegex();
}
