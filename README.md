# SimpleOTP
[![GitHub last commit](https://img.shields.io/github/last-commit/xfox111/SimpleOTP)](https://github.com/xfox111/SimpleOTP/commits/master)
[![MIT License](https://img.shields.io/github/license/xfox111/SimpleOTP)](https://opensource.org/licenses/MIT)

[![Twitter Follow](https://img.shields.io/twitter/follow/xfox111?style=social)](https://twitter.com/xfox111)
[![GitHub followers](https://img.shields.io/github/followers/xfox111?label=Follow%20@xfox111&style=social)](https://github.com/xfox111)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-%40xfox111-orange)](https://buymeacoffee.com/xfox111)

.NET library for TOTP/HOTP implementation on server (ASP.NET) or client (Xamarin) side

## Features
- Generate and validate OTP codes
- Support of [TOTP](https://en.wikipedia.org/wiki/Time-based_One-time_password) (RFC 6238) and [HOTP](https://en.wikipedia.org/wiki/HMAC-based_one-time_password) (RFC 4226) algorithms
- Support of HMAC-SHA1, HMAC-SHA256 and HMAC-SHA512 hashing algorithms
- Setup URI parser
- Database-ready configuration models
- Configuration generator for server-side implementation
- QR code generator
- No dependencies

![By Mateusz Adamowski, taken with Canon EOS. - Own work, CC BY-SA 1.0, https://commons.wikimedia.org/w/index.php?curid=142232](https://upload.wikimedia.org/wikipedia/commons/3/33/RSA-SecurID-Tokens.jpg)
##### By Mateusz Adamowski, taken with Canon EOS. - Own work, CC BY-SA 1.0, https://commons.wikimedia.org/w/index.php?curid=142232

## Usage
See more documentation at [project's wiki](https://github.com/xfox111/SimpleOTP/wiki)
### Generate code
```csharp
string sample_config_uri = "otpauth://totp/FoxDev%20Studio:eugene@xfox111.net?secret=ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH&issuer=FoxDev%20Studio";
OTPConfiguration config = OTPConfiguration.GetConfiguration(sample_config_uri);
// OTPConfiguration { Id = af2358b0-3f69-4dd7-9537-32c07d6663aa, Type = TOTP, IssuerLabel = FoxDev Studio, AccountName = eugene@xfox111.net, Secret = ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH, Issuer = FoxDev Studio, Algorithm = SHA1, Digits = 6, Counter = 0, Period = 00:00:30 }


OTPCode code = OTPService.GenerateCode(ref config);
// OTPasswordModel { Code = 350386, Expiring = 23-May-21 06:08:30 PM }
```

### Validate code
```csharp
int codeToValidate = 350386;
bool isValid = OTPService.ValidateTotp(codeToValidate, config, TimeSpan.FromSeconds(30)); // True
```

### Generate setup config
```csharp
OTPConfiguration config = OTPConfiguration.GenerateConfiguration("FoxDev Studio", "eugene@xfox111.net");
// OTPModel { Id = af2358b0-3f69-4dd7-9537-32c07d6663aa, Type = TOTP, IssuerLabel = FoxDev Studio, AccountName = eugene@xfox111.net, Secret = ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH, Issuer = FoxDev Studio, Algorithm = SHA1, Digits = 6, Counter = 0, Period = 00:00:30 }

Uri uri = config.GetUri();	// otpauth://totp/FoxDev%20Studio:eugene@xfox111.net?secret=ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH&issuer=FoxDev%20Studio
string qrCode = config.GetQrImage(300); // data:image/png;base64,...
```

### Streamline code generation for client
```csharp
OTPFactory factory = new (config);

factory.CodeUpdated += (newCode) => Console.WriteLine(newCode);
// OTPCode { Code = 350386, Expiring = 23-May-21 06:08:30 PM }
factory.PropertyChanged += (sender, args) =>
{
	if (args.PropertyName == nameof(factory.TimeLeft))
		Console.WriteLine(factory.TimeLeft);
	else
		Console.WriteLine(factory.Code);
}
...
factory.Dispose();

```

## Download
![Nuget](https://img.shields.io/nuget/v/SimpleOTP)
![Nuget](https://img.shields.io/nuget/dt/SimpleOTP)
- [NuGet Gallery](https://www.nuget.org/packages/SimpleOTP)
- [GitHub Releases](https://github.com/xfox111/SimpleOTP/releases/latest)

## CI/DC status
[![Build Status](https://dev.azure.com/xfox111/GitHub%20CI/_apis/build/status/XFox111.SimpleOTP?branchName=master)](https://dev.azure.com/xfox111/GitHub%20CI/_build/latest?definitionId=13)
![Deployment status](https://vsrm.dev.azure.com/xfox111/_apis/public/Release/badge/e42c572c-a3cd-4aac-bbb1-f720d9ccb5ea/3/15)

![Azure DevOps tests](https://img.shields.io/azure-devops/tests/xfox111/GitHub%2520CI/13?label=Tests)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/xfox111/GitHub%2520CI/13?label=Code+coverage)

## Contributing
[![GitHub issues](https://img.shields.io/github/issues/xfox111/SimpleOTP)](https://github.com/xfox111/SimpleOTP/issues)
[![GitHub repo size](https://img.shields.io/github/repo-size/xfox111/SimpleOTP?label=repo%20size)](https://github.com/xfox111/SimpleOTP)

There are many ways in which you can participate in the project, for example:
- [Submit bugs and feature requests](https://github.com/xfox111/SimpleOTP/issues), and help us verify as they are checked in
- Review [source code changes](https://github.com/xfox111/SimpleOTP/pulls)
- Review documentation and make pull requests for anything from typos to new content

If you are interested in fixing issues and contributing directly to the code base, please see the [Contribution Guidelines](https://github.com/XFox111/SimpleOTP/blob/master/CONTRIBUTING.md), which covers the following:
- [How to deploy the extension on your browser](https://github.com/XFox111/SimpleOTP/blob/master/CONTRIBUTING.md#deploy-test-version-on-your-browser)
- [The development workflow](https://github.com/XFox111/SimpleOTP/blob/master/CONTRIBUTING.md#development-workflow), including debugging and running tests
- [Coding guidelines](https://github.com/XFox111/SimpleOTP/blob/master/CONTRIBUTING.md#coding-guidelines)
- [Submitting pull requests](https://github.com/XFox111/SimpleOTP/blob/master/CONTRIBUTING.md#submitting-pull-requests)
- [Finding an issue to work on](https://github.com/XFox111/SimpleOTP/blob/master/CONTRIBUTING.md#finding-an-issue-to-work-on)
- [Contributing to translations](https://github.com/XFox111/SimpleOTP/blob/master/CONTRIBUTING.md#contributing-to-translations)

## Code of Conduct
This project has adopted the Contributor Covenant. For more information see the [Code of Conduct](https://github.com/XFox111/SimpleOTP/blob/master/CODE_OF_CONDUCT.md)

## Copyrights
> ©2021 Eugene Fox

Licensed under [MIT License](https://opensource.org/licenses/MIT)
