using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using SimpleOTP.Converters;

namespace SimpleOTP;

// THIS IS THE SECTION OF A PARTIAL CLASS
// Description: Section of OtpConfig struct that holds instance methods
// Base file: OtpConfig.Base.cs

[Serializable]
[JsonConverter(typeof(OtpConfigJsonConverter))]
public partial record class OtpConfig : IXmlSerializable
{
	/// <summary>
	/// Converts the current <see cref="OtpConfig"/> object to a <see cref="Uri"/> object.
	/// </summary>
	/// <remarks>
	/// Uses minimal Google specified formatting (<see cref="OtpUriFormat.Minimal"/> | <see cref="OtpUriFormat.Google"/>).
	/// </remarks>
	/// <returns>A <see cref="Uri"/> object representing the current <see cref="OtpConfig"/> object.</returns>
	public Uri ToUri() =>
		ToUri(OtpUriFormat.Minimal | OtpUriFormat.Google);

	/// <summary>
	/// Converts the current <see cref="OtpConfig"/> object to a <see cref="Uri"/> object.
	/// </summary>
	/// <param name="format">A bitwise combination of the enumeration values that specifies the format of the URI.</param>
	/// <returns>A <see cref="Uri"/> object representing the current <see cref="OtpConfig"/> object.</returns>
	public Uri ToUri(OtpUriFormat format)
	{
		string scheme = format.HasFlag(OtpUriFormat.Apple) ? "apple-otpauth" : "otpauth";
		string label = HttpUtility.UrlEncode(Label).Replace("+", "%20");

		if (!string.IsNullOrWhiteSpace(IssuerLabel))
			label = $"{HttpUtility.UrlEncode(IssuerLabel).Replace("+", "%20")}:{label}";

		UriBuilder uri = new(scheme, Type.ToString().ToLowerInvariant(), -1, label);
		NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);

		query["secret"] = Secret.ToString();

		if (Type == OtpType.Hotp)
			query["counter"] = Counter.ToString();

		if (Issuer is not null)
			query["issuer"] = Issuer;

		if (format.HasFlag(OtpUriFormat.Full) || !Algorithm.Equals(OtpAlgorithm.SHA1))
		{
			if (format.HasFlag(OtpUriFormat.IBM) && Algorithm.IsStandard())
				query["algorithm"] = "Hmac" + Algorithm;
			else
				query["algorithm"] = Algorithm;
		}

		if (format == OtpUriFormat.Full || Digits != 6)
			query["digits"] = Digits.ToString();

		if (format == OtpUriFormat.Full || Period != 30)
			query["period"] = Period.ToString();

		foreach (string key in _reservedKeys)
			CustomProperties.Remove(key);

		query.Add(CustomProperties);

		uri.Query = query.ToString();
		return uri.Uri;
	}

	/// <summary>
	/// Returns if the specified <see cref="OtpConfig"/> object is valid.
	/// </summary>
	/// <param name="error">The error message returned if the <see cref="OtpConfig"/> object is invalid.</param>
	/// <param name="format">The <see cref="OtpUriFormat"/> to use for validation.</param>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public void IsValid([NotNullWhen(false)] out string? error, OtpUriFormat format = OtpUriFormat.Google) =>
		Validate(this, out error, format);

	/// <inheritdoc/>
	public override string ToString() =>
		ToUri().AbsoluteUri;

	/// <inheritdoc/>
	public XmlSchema? GetSchema() => null;

	/// <inheritdoc/>
	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();

		if (reader.NodeType != XmlNodeType.Element)
			throw new XmlException("Invalid XML element.");

#pragma warning disable CS9193 // Argument should be a variable because it is passed to a 'ref readonly' parameter
		Unsafe.AsRef(this) = ParseUri(reader.ReadElementContentAsString());
#pragma warning restore CS9193 // Argument should be a variable because it is passed to a 'ref readonly' parameter
	}

	/// <inheritdoc/>
	public void WriteXml(XmlWriter writer)
	{
		writer.WriteString(ToUri().AbsoluteUri);
	}
}
