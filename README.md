[![GitHub release (latest by date)](https://img.shields.io/github/v/release/xfox111/SimpleOTP)](https://github.com/xfox111/SimpleOTP/releases/latest)
[![GitHub last commit](https://img.shields.io/github/last-commit/xfox111/SimpleOTP?label=Last+update)](https://github.com/XFox111/SimpleOTP/commits/main)

![SimpleOTP](https://raw.githubusercontent.com/XFox111/SimpleOTP/refs/heads/main/assets/banner.svg)

Feature-rich and flexible .NET library for implementation of OTP authenticators and validatiors.

## Features
- Full support for Time-based OTP generation and validation ([RFC 6238][RFC-6238])
- Full support for HMAC-based OTP generation and validation ([RFC 4226][RFC-4226])
- Ability to create `otpauth:` confguration URIs with full compliance with [Usage specification of the otpauth URI format for TOTP and HOTP token generators Internet-Draft][otpauth-ID] by I. Y. Eroglu
- Built-in `otpauth:` URI formatters to comply with different specifications (Apple, Google, IBM, and more)
- Fluent API support
- Supplementary `DependencyInjection` package for easier implementation in ASP.NET
- Continuous support of current and upcoming .NET versions
- And more!

## Download

| Package | Info | Download |
| --- | --- | --- |
| `EugeneFox.SimpleOTP` | [![Nuget](https://img.shields.io/nuget/v/EugeneFox.SimpleOTP)][nuget] / [![Nuget](https://img.shields.io/nuget/dt/EugeneFox.SimpleOTP)][nuget] | [NuGet Gallery][nuget] / [GitHub NuGet Registry][ghnr] |
| `EugeneFox.SimpleOTP.DependencyInjection` | [![Nuget](https://img.shields.io/nuget/v/EugeneFox.SimpleOTP.DependencyInjection)][nuget-di] / [![Nuget](https://img.shields.io/nuget/dt/EugeneFox.SimpleOTP.DependencyInjection)][nuget-di] | [NuGet Gallery][nuget-di] / [GitHub NuGet Registry][ghnr-di] |

Use these commands to install SimpleOTP package in your project:
```bash
# For common projects:
dotnet add package EugeneFox.SimpleOTP
# Or for ASP.NET projects:
dotnet add package EugeneFox.SimpleOTP.DependencyInjection
```

## Usage, examples and docs

Please refer to [project's Wiki](https://github.com/XFox111/SimpleOTP/wiki) for usage examples, API reference and other documentation.

## Contributing
[![GitHub issues](https://img.shields.io/github/issues/xfox111/SimpleOTP)](https://github.com/xfox111/SimpleOTP/issues)
[![CI](https://github.com/XFox111/SimpleOTP/actions/workflows/release-workflow.yml/badge.svg)](https://github.com/XFox111/SimpleOTP/actions/workflows/cd_pipeline.yaml)
[![GitHub repo size](https://img.shields.io/github/repo-size/xfox111/SimpleOTP?label=repo%20size)](https://github.com/xfox111/SimpleOTP)

There are many ways in which you can participate in the project, for example:
- [Submit bugs and feature requests](https://github.com/xfox111/SimpleOTP/issues), and help us verify as they are checked in
- Review [source code changes](https://github.com/xfox111/SimpleOTP/pulls)
- Review documentation and make pull requests for anything from typos to new content

If you are interested in fixing issues and contributing directly to the code base, please refer to the [Contribution Guidelines](https://github.com/XFox111/SimpleOTP/wiki/Contribution-Guidelines)

---

[![Bluesky](https://img.shields.io/badge/%40xfox111.net-BSky?logo=bluesky&logoColor=%230285FF&label=Bluesky&labelColor=white&color=%230285FF)](https://bsky.app/profile/xfox111.net)
[![GitHub](https://img.shields.io/badge/%40xfox111-GitHub?logo=github&logoColor=%23181717&label=GitHub&labelColor=white&color=%23181717)](https://github.com/xfox111)
[![Buy Me a Coffee](https://img.shields.io/badge/%40xfox111-BMC?logo=buymeacoffee&logoColor=black&label=Buy%20me%20a%20coffee&labelColor=white&color=%23FFDD00)](https://buymeacoffee.com/xfox111)

> ©2025 Eugene Fox. Licensed under [MIT license][mit]

[RFC-6238]: https://www.rfc-editor.org/rfc/rfc6238
[RFC-4226]: https://www.rfc-editor.org/rfc/rfc4226
[otpauth-ID]: https://www.ietf.org/archive/id/draft-linuxgemini-otpauth-uri-00.html
[nuget]: https://www.nuget.org/packages/EugeneFox.SimpleOTP
[nuget-di]: https://www.nuget.org/packages/EugeneFox.SimpleOTP.DependencyInjection
[ghnr]: https://github.com/XFox111/SimpleOTP/pkgs/nuget/EugeneFox.SimpleOTP
[ghnr-di]: https://github.com/XFox111/SimpleOTP/pkgs/nuget/EugeneFox.SimpleOTP.DependencyInjection
[mit]: https://github.com/XFox111/SimpleOTP/blob/main/LICENSE
