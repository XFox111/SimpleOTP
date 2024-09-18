using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleOTP.Converters;

/// <summary>
/// Provides a JSON converter for <see cref="OtpSecret"/>.
/// </summary>
public class OtpSecretJsonConverter : JsonConverter<OtpSecret>
{
	/// <inheritdoc/>
	public override OtpSecret Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		new(reader.GetString()!);

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, OtpSecret value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value.ToString());
}
