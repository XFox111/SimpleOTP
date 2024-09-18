namespace SimpleOTP.Fluent;

/// <summary>
/// Provides fluent API for configuring <see cref="OtpConfig"/> objects.
/// </summary>
public static class OtpConfigFluentExtensions
{
	/// <summary>
	/// Sets the <see cref="OtpConfig.Label"/> property.
	/// </summary>
	/// <param name="config">The <see cref="OtpConfig"/> object to configure.</param>
	/// <param name="label">The label of the OTP config.</param>
	/// <returns>The configured <see cref="OtpConfig"/> object.</returns>
	public static OtpConfig WithLabel(this OtpConfig config, string label)
	{
		config.Label = label;
		return config;
	}

	/// <summary>
	/// Sets the <see cref="OtpConfig.Issuer"/> property.
	/// </summary>
	/// <param name="config">The <see cref="OtpConfig"/> object to configure.</param>
	/// <param name="issuer">The issuer of the OTP config.</param>
	/// <returns>The configured <see cref="OtpConfig"/> object.</returns>
	public static OtpConfig WithIssuer(this OtpConfig config, string? issuer)
	{
		config.Issuer = issuer;
		return config;
	}

	/// <summary>
	/// Sets the issuer info, according to Apple specification.
	/// </summary>
	/// <param name="config">The <see cref="OtpConfig"/> object to configure.</param>
	/// <param name="displayName">The display name of the issuer.</param>
	/// <param name="domain">The domain name of the issuer.</param>
	public static OtpConfig WithAppleIssuer(this OtpConfig config, string displayName, string domain)
	{
		config.IssuerLabel = displayName;
		config.Issuer = domain;
		return config;
	}

	/// <summary>
	/// Sets the <see cref="OtpConfig.Secret"/> property with a new secret.
	/// </summary>
	/// <param name="config">The <see cref="OtpConfig"/> object to configure.</param>
	/// <param name="bytesLength">The length of the secret in bytes.</param>
	/// <returns>The configured <see cref="OtpConfig"/> object.</returns>
	public static OtpConfig WithNewSecret(this OtpConfig config, int bytesLength)
	{
		config.Secret = OtpSecret.CreateNew(bytesLength);
		return config;
	}

	/// <summary>
	/// Sets the <see cref="OtpConfig.Secret"/> property with specified secret.
	/// </summary>
	/// <param name="config">The <see cref="OtpConfig"/> object to configure.</param>
	/// <param name="secret">The secret to use.</param>
	/// <returns>The configured <see cref="OtpConfig"/> object.</returns>
	public static OtpConfig WithSecret(this OtpConfig config, OtpSecret secret)
	{
		config.Secret = secret;
		return config;
	}

	/// <summary>
	/// Sets the <see cref="OtpConfig.Algorithm"/> property.
	/// </summary>
	/// <remarks>Not recommended for use, since most implementations do not support custom values.</remarks>
	/// <param name="config">The <see cref="OtpConfig"/> object to configure.</param>
	/// <param name="algorithm">The algorithm to use.</param>
	/// <returns>The configured <see cref="OtpConfig"/> object.</returns>
	public static OtpConfig WithAlgorithm(this OtpConfig config, OtpAlgorithm algorithm)
	{
		config.Algorithm = algorithm;
		return config;
	}

	/// <summary>
	/// Sets the <see cref="OtpConfig.Digits"/> property.
	/// </summary>
	/// <remarks>Not recommended for use, since most implementations do not support custom values.</remarks>
	/// <param name="config">The <see cref="OtpConfig"/> object to configure.</param>
	/// <param name="digits">The number of digits to use.</param>
	/// <returns>The configured <see cref="OtpConfig"/> object.</returns>
	public static OtpConfig WithDigits(this OtpConfig config, int digits)
	{
		config.Digits = digits;
		return config;
	}

	/// <summary>
	/// Adds a custom vendor-specific property to the <see cref="OtpConfig"/>.
	/// </summary>
	/// <remarks>If set, reserved keys
	/// <c>issuer, digits, counter, secret, period and algorithm</c>
	/// will be removed from the <see cref="OtpConfig.CustomProperties"/> upon it's serialization to URI.</remarks>
	/// <param name="config">The <see cref="OtpConfig"/> object to configure.</param>
	/// <param name="key">The key of the property.</param>
	/// <param name="value">The value of the property.</param>
	/// <returns>The configured <see cref="OtpConfig"/> object.</returns>
	public static OtpConfig AddCustomProperty(this OtpConfig config, string key, string value)
	{
		config.CustomProperties.Add(key, value);
		return config;
	}

	/// <summary>
	/// Creates a new <see cref="Otp"/> object from the provided <see cref="OtpConfig"/>
	/// </summary>
	/// <param name="config">The <see cref="OtpConfig"/> object to use.</param>
	/// <returns>A new <see cref="Otp"/> object.</returns>
	public static Otp CreateGenerator(this OtpConfig config) =>
		OtpBuilder.FromConfig(config);
}
