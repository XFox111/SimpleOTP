namespace SimpleOTP.Tests;

[TestFixture]
public class ConfigTests
{
	private static readonly byte[] secretBytes = [0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x21, 0xde, 0xad, 0xbe, 0xef];
	private const string username = "eugene@xfox111.net";
	private const string appName = "Example App";
	private const string issuer = "example.com";


	[TestCase("otpauth://totp/eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=SHA1&digits=6&period=30",
		"SHA1", OtpType.Totp, 6, 30, null)]
	[TestCase("otpauth://totp/Example%20App:eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=SHA1&digits=6&period=30",
		"SHA1", OtpType.Totp, 6, 30, appName)]
	[TestCase("apple-otpauth://totp/Example%20App:eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=SHA1&digits=6&period=30",
		"SHA1", OtpType.Totp, 6, 30, appName)]
	[TestCase("otpauth://totp/Example%20App:eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=SHA256&digits=8&period=60",
		"SHA256", OtpType.Totp, 8, 60, appName)]
	[TestCase("otpauth://totp/eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=SHA512&digits=6&period=30",
		"SHA512", OtpType.Totp, 6, 30, null)]
	[TestCase("otpauth://hotp/eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=SHA512&digits=6&counter=0",
		"SHA512", OtpType.Hotp, 6, 30, null)]
	[TestCase("otpauth://hotp/eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=HmacSHA512&digits=6&counter=0",
		"SHA512", OtpType.Hotp, 6, 30, null)]
	public void ParseTest(string uri, string algorithm, OtpType type, int digits, int period, string? appName)
	{
		OtpConfig config = OtpConfig.ParseUri(uri);

		Assert.That(config.Label, Is.EqualTo(username));
		Assert.That(config.Secret, Is.EqualTo(secretBytes));
		Assert.That(config.Issuer, Is.EqualTo(issuer));
		Assert.That(config.IssuerLabel, Is.EqualTo(appName));
		Assert.That(config.Algorithm, Is.EqualTo(algorithm));
		Assert.That(config.Digits, Is.EqualTo(digits));
		Assert.That(config.Period, Is.EqualTo(period));
		Assert.That(config.Type, Is.EqualTo(type));
	}

	[TestCase(OtpUriFormat.Google | OtpUriFormat.Minimal, false,
		"otpauth://totp/eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com")]
	[TestCase(OtpUriFormat.Google | OtpUriFormat.Full, false,
		"otpauth://totp/eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=SHA1&digits=6&period=30")]
	[TestCase(OtpUriFormat.Apple | OtpUriFormat.Minimal, true,
		"apple-otpauth://totp/Example%20App:eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com")]
	[TestCase(OtpUriFormat.Apple | OtpUriFormat.Full, true,
		"apple-otpauth://totp/Example%20App:eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=SHA1&digits=6&period=30")]
	[TestCase(OtpUriFormat.IBM | OtpUriFormat.Full, false,
		"otpauth://totp/eugene%40xfox111.net?secret=JBSWY3DPEHPK3PXP&issuer=example.com&algorithm=HmacSHA1&digits=6&period=30")]
	public void ToUriTest(OtpUriFormat format, bool appendIssuerLabel, string expectedUri)
	{
		OtpConfig config = new(username)
		{
			Issuer = issuer,
			Secret = OtpSecret.FromBytes(secretBytes)
		};

		if (appendIssuerLabel)
			config.IssuerLabel = appName;

		Uri uri = config.ToUri(format);
		Assert.That(uri.AbsoluteUri, Is.EqualTo(expectedUri));
	}
}
