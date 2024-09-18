using System.Security.Cryptography;

namespace SimpleOTP;

/// <summary>
/// Provides methods for registering and retrieving <see cref="KeyedHashAlgorithm"/> providers.
/// </summary>
public static class HashAlgorithmProviders
{
	private static readonly Dictionary<OtpAlgorithm, Func<KeyedHashAlgorithm>> _registeredProviders = new()
	{
		{ OtpAlgorithm.SHA1, () => new HMACSHA1() },
		{ OtpAlgorithm.SHA256, () => new HMACSHA256() },
		{ OtpAlgorithm.SHA512, () => new HMACSHA512() },
		{ OtpAlgorithm.MD5, () => new HMACMD5() }
	};

	/// <summary>
	/// Registers a new <see cref="KeyedHashAlgorithm"/> provider.
	/// </summary>
	/// <param name="algorithm">The algorithm to register.</param>
	public static void AddProvider<TAlgorithm>(OtpAlgorithm algorithm)
		where TAlgorithm : KeyedHashAlgorithm, new() =>
		_registeredProviders[algorithm] = () => new TAlgorithm();

	/// <summary>
	/// Retrieves a <see cref="KeyedHashAlgorithm"/> provider.
	/// </summary>
	/// <param name="algorithm">The algorithm to retrieve.</param>
	/// <returns>The <see cref="KeyedHashAlgorithm"/> provider, or <c>null</c> if not found.</returns>
	public static KeyedHashAlgorithm? GetProvider(OtpAlgorithm algorithm)
	{
		if (_registeredProviders.TryGetValue(algorithm, out var provider))
			return provider();

		return null;
	}

	/// <summary>
	/// Removes a <see cref="KeyedHashAlgorithm"/> provider.
	/// </summary>
	/// <param name="algorithm">The algorithm to remove.</param>
	public static void RemoveProvider(OtpAlgorithm algorithm) =>
		_registeredProviders.Remove(algorithm);

	/// <summary>
	/// Determines whether a <see cref="KeyedHashAlgorithm"/> provider is registered.
	/// </summary>
	/// <param name="algorithm">The algorithm to check.</param>
	/// <returns><c>true</c> if the <see cref="KeyedHashAlgorithm"/> provider is registered; otherwise, <c>false</c>.</returns>
	public static bool IsRegistered(OtpAlgorithm algorithm) =>
		_registeredProviders.ContainsKey(algorithm);

	/// <summary>
	/// Removes all registered <see cref="KeyedHashAlgorithm"/> providers.
	/// </summary>
	/// <remarks>This method also clears default providers. Use with caution.</remarks>
	public static void ClearProviders() => _registeredProviders.Clear();
}
