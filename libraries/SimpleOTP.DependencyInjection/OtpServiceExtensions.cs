using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleOTP.DependencyInjection;

/// <summary>
/// Extension methods for the One-Time Password service.
/// </summary>
public static class OtpServiceExtensions
{
	/// <summary>
	/// Adds the One-Time Password service to the service collection.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="issuerName">The issuer/application/service name.</param>
	/// <returns>A reference to this instance after the operation has completed.</returns>
	public static IServiceCollection AddAuthenticator(this IServiceCollection services, string issuerName) =>
		AddAuthenticator(services, issuerName);

	/// <summary>
	/// Adds the One-Time Password service to the service collection.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="issuerName">The issuer/application/service name.</param>
	/// <param name="configure">The configuration for the One-Time Password service.</param>
	/// <returns>A reference to this instance after the operation has completed.</returns>
	public static IServiceCollection AddAuthenticator(this IServiceCollection services, string issuerName, Action<OtpOptions>? configure = null)
	{
		OtpOptions options = new()
		{
			Issuer = issuerName
		};

		configure?.Invoke(options);
		services.AddTransient<IOtpService, OtpService>(_ => new OtpService(options));
		return services;
	}

	/// <summary>
	/// Adds the One-Time Password service to the service collection.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="configuration">The configuration for the One-Time Password service.</param>
	/// <returns>A reference to this instance after the operation has completed.</returns>
	public static IServiceCollection AddAuthenticator(this IServiceCollection services, IConfiguration configuration) =>
		AddAuthenticator(services, configuration);

	/// <summary>
	/// Adds the One-Time Password service to the service collection.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="configuration">The configuration for the One-Time Password service.</param>
	/// <param name="configure">The configuration for the One-Time Password service.</param>
	/// <returns>A reference to this instance after the operation has completed.</returns>
	public static IServiceCollection AddAuthenticator(this IServiceCollection services, IConfiguration configuration, Action<OtpOptions>? configure = null)
	{
		OtpOptions? options = GetOptionsFromConfiguration(configuration);

		if (options is null)
			return services;

		configure?.Invoke(options);
		services.AddTransient<IOtpService, OtpService>(_ => new OtpService(options));
		return services;
	}

	private static OtpOptions? GetOptionsFromConfiguration(IConfiguration configuration)
	{
		OtpServiceConfig config = new();
		IConfigurationSection configSection = configuration.GetSection("Authenticator");

		if (!configSection.Exists() || string.IsNullOrWhiteSpace(configuration["Authenticator:Issuer"]))
			return null;

		configSection.Bind(config);

		OtpOptions options = new()
		{
			Issuer = config.Issuer,
			Algorithm = config.Algorithm,
			Type = config.Type,
			Digits = config.Digits,
			Period = config.Period,
			IssuerDomain = config.IssuerDomain,
			ToleranceSpan = (config.ToleranceSpan.Behind, config.ToleranceSpan.Ahead),
			UriFormat = config.UriFormat
		};

		options.UriFormat |= config.MinimalUri ? OtpUriFormat.Minimal : OtpUriFormat.Full;

		foreach (KeyValuePair<string, string> pair in config.CustomProperties)
			options.CustomProperties.Add(pair.Key, pair.Value);

		return options;
	}
}
