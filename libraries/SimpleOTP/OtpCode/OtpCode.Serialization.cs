using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using SimpleOTP.Converters;

namespace SimpleOTP;

// THIS IS THE SECTION OF A PARTIAL STRUCT
// Description: Section of OtpCode struct that holds JSON/XML serialization members and attributes
// Base file: OtpCode.Base.cs

[Serializable]
[JsonConverter(typeof(OtpCodeJsonConverter))]
public readonly partial struct OtpCode : IXmlSerializable
{
	/// <inheritdoc/>
	public XmlSchema? GetSchema() => null;

	/// <inheritdoc/>
	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();

		if (reader.NodeType != XmlNodeType.Element)
			throw new XmlException("Invalid XML element.");

		DateTimeOffset? expirationTime = null;

		if (reader.HasAttributes && reader.MoveToAttribute("expiring"))
			expirationTime = DateTimeOffset.ParseExact(reader.ReadContentAsString(), "O", null);

		reader.MoveToContent();

		if (reader.NodeType != XmlNodeType.Element)
			throw new XmlException("Invalid XML content.");

		string code = reader.ReadElementContentAsString();

#pragma warning disable CS9195 // Argument should be passed with the in keyword
		Unsafe.AsRef(this) = new OtpCode(code, expirationTime);
#pragma warning restore CS9195 // Argument should be passed with the in keyword
	}

	/// <inheritdoc/>
	public void WriteXml(XmlWriter writer)
	{
		if (ExpirationTime.HasValue)
			writer.WriteAttributeString("expiring", ExpirationTime.Value.ToString("O"));

		writer.WriteString(ToString());
	}
}
