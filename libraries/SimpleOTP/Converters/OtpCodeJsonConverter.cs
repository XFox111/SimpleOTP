using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleOTP.Converters;

/// <summary>
/// Provides a JSON converter for <see cref="OtpCode"/>.
/// </summary>
public class OtpCodeJsonConverter : JsonConverter<OtpCode>
{
	/// <inheritdoc/>
	public override OtpCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string? code = null;
		DateTimeOffset? expirationTime = null;

		while (reader.Read())
			if (reader.TokenType == JsonTokenType.PropertyName)
			{
				string propertyName = reader.GetString()!;
				reader.Read();

				if (reader.TokenType != JsonTokenType.String)
					continue;

				if (propertyName.Equals("Code", StringComparison.OrdinalIgnoreCase))
					code = reader.GetString();

				if (propertyName.Equals("Expiring", StringComparison.OrdinalIgnoreCase) &&
					reader.TryGetDateTimeOffset(out DateTimeOffset expiring))
					expirationTime = expiring;
			}

		if (code is null)
			throw new JsonException("Missing required property 'Code'.");

		return new OtpCode(code, expirationTime);
	}

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, OtpCode value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteString("Code", value.ToString());

		if (value.ExpirationTime.HasValue)
			writer.WriteString("Expiring", value.ExpirationTime.Value);
		else
			writer.WriteNull("Expiring");

		writer.WriteEndObject();
	}
}
