// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

namespace SimpleOTP.Enums
{
	/// <summary>
	/// OTP algorithm types.
	/// </summary>
	public enum OTPType
	{
		/// <summary>
		/// Time-based One-Time Password<br/>
		/// <a href="https://datatracker.ietf.org/doc/html/rfc6238">RFC 6238</a>
		/// </summary>
		TOTP = 0,

		/// <summary>
		/// HMAC-based One-Time Password<br/>
		/// /// <a href="https://datatracker.ietf.org/doc/html/rfc4226">RFC 4226</a>
		/// </summary>
		HOTP = 1
	}
}