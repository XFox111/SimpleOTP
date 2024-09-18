using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleOTP.Converters;

/// <summary>
/// Provides a JSON converter for <see cref="OtpAlgorithm"/>.
/// </summary>
public class OtpAlgorithmJsonConverter : JsonConverter<OtpAlgorithm>
{
	/// <inheritdoc/>
	public override OtpAlgorithm Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		new(reader.GetString() ?? OtpAlgorithm.SHA1);

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, OtpAlgorithm value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value.ToString());
}
