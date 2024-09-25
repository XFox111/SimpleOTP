using NUnit.Framework;

namespace SimpleOTP.Tests;

[TestFixture]
public class TotpTests
{
	private const string secret1 = "GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ";
	private const string secret2 = "GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZA";
	private const string secret3 = "GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNA";

	[TestCase(secret1, "SHA1", 59, "94287082", 60)]
	[TestCase(secret2, "SHA256", 59, "46119246", 60)]
	[TestCase(secret3, "SHA512", 59, "90693936", 60)]
	[TestCase(secret1, "SHA1", 1111111109, "07081804", 1111111110)]
	[TestCase(secret2, "SHA256", 1111111109, "68084774", 1111111110)]
	[TestCase(secret3, "SHA512", 1111111109, "25091201", 1111111110)]
	[TestCase(secret1, "SHA1", 1111111111, "14050471", 1111111140)]
	[TestCase(secret2, "SHA256", 1111111111, "67062674", 1111111140)]
	[TestCase(secret3, "SHA512", 1111111111, "99943326", 1111111140)]
	[TestCase(secret1, "SHA1", 1234567890, "89005924", 1234567920)]
	[TestCase(secret2, "SHA256", 1234567890, "91819424", 1234567920)]
	[TestCase(secret3, "SHA512", 1234567890, "93441116", 1234567920)]
	[TestCase(secret1, "SHA1", 2000000000, "69279037", 2000000010)]
	[TestCase(secret2, "SHA256", 2000000000, "90698825", 2000000010)]
	[TestCase(secret3, "SHA512", 2000000000, "38618901", 2000000010)]
	[TestCase(secret1, "SHA1", 20000000000, "65353130", 20000000010)]
	[TestCase(secret2, "SHA256", 20000000000, "77737706", 20000000010)]
	[TestCase(secret3, "SHA512", 20000000000, "47863826", 20000000010)]
	[TestCase(secret1, "SHA1", 20000000000, "353130", 20000000010)]
	[TestCase(secret2, "SHA256", 20000000000, "737706", 20000000010)]
	[TestCase(secret3, "SHA512", 20000000000, "863826", 20000000010)]
	public void ComputeTest(string secret, string algorithm, long timestamp, string expectedOtp, long expectedExpiration)
	{
		using OtpSecret otpSecret = OtpSecret.Parse(secret);
		Totp totp = new(otpSecret, 30, (OtpAlgorithm)algorithm, expectedOtp.Length);
		DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(timestamp);
		OtpCode code = totp.Generate(time);

		Assert.That(code.ToString(), Is.EqualTo(expectedOtp));
		Assert.That(code.ExpirationTime, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(expectedExpiration)));
	}

	[TestCase(59, "287082", 1, true, 0)]
	[TestCase(0, "287082", 1, true, 1)]
	[TestCase(10, "287082", 0, false, 0)]
	[TestCase(20000000000, "353130", 1, true, 0)]
	[TestCase(20000000030, "353130", 0, false, 0)]
	[TestCase(20000000030, "353130", 1, true, -1)]
	[TestCase(20000000060, "353130", 1, false, 0)]
	[TestCase(20000000060, "353130", 2, true, -2)]
	public void ValidateOffsetTest(long timestamp, string code, int toleranceWindow, bool expectedResult, int expectedResync)
	{
		using OtpSecret secret = OtpSecret.Parse(secret1);
		Totp totp = new(secret, 30, code.Length);
		DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(timestamp);

		bool result = totp.Validate((OtpCode)code, toleranceWindow, time, out int resyncValue);

		Assert.That(result, Is.EqualTo(expectedResult));
		Assert.That(resyncValue, Is.EqualTo(expectedResync));
	}
}
