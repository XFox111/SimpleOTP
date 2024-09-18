using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using SimpleOTP.Converters;

namespace SimpleOTP;

// THIS IS THE SECTION OF A PARTIAL CLASS
// Description: Section of OtpSecret class that holds JSON/XML serialization members and attributes
// Base file: OtpSecret.Base.cs

[Serializable]
[JsonConverter(typeof(OtpSecretJsonConverter))]
public partial class OtpSecret
{
	/// <inheritdoc/>
	public XmlSchema? GetSchema() => null;

	/// <inheritdoc/>
	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();

		if (reader.NodeType != XmlNodeType.Element)
			throw new XmlException("Invalid XML element.");

		byte[] secret = DefaultEncoder.GetBytes(reader.ReadElementContentAsString());

#pragma warning disable CS9193 // Argument should be a variable because it is passed to a 'ref readonly' parameter
		Unsafe.AsRef(this) = new OtpSecret(secret);
#pragma warning restore CS9193 // Argument should be a variable because it is passed to a 'ref readonly' parameter
	}

	/// <inheritdoc/>
	public void WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString("encoding", DefaultEncoder.Scheme);
		writer.WriteAttributeString("length", _secret.Length.ToString());
		writer.WriteString(DefaultEncoder.EncodeBytes(_secret));
	}
}
