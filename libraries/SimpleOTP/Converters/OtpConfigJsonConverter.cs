using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleOTP.Converters;

/// <summary>
/// Provides a JSON converter for <see cref="OtpConfig"/>.
/// </summary>
public class OtpConfigJsonConverter : JsonConverter<OtpConfig>
{
	/// <inheritdoc/>
	public override OtpConfig? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		OtpConfig.ParseUri(reader.GetString()!);

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, OtpConfig value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value.ToUri().AbsoluteUri);
}
