// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

namespace SimpleOTP.Enums
{
	/// <summary>
	/// Available OTP encryption algorithms.
	/// </summary>
	public enum Algorithm
	{
		/// <summary>
		/// HMAC-SHA1 hasing algorithm (default)<br/>
		/// <a href="https://datatracker.ietf.org/doc/html/rfc3174">RFC 3174</a>
		/// </summary>
		SHA1 = 0,

		/// <summary>
		/// HMAC-SHA256 hasing algorithm<br/>
		/// <a href="https://datatracker.ietf.org/doc/html/rfc4634">RFC 4634</a>
		/// </summary>
		SHA256 = 1,

		/// <summary>
		/// HMAC-SHA512 hasing algorithm<br/>
		/// <a href="https://datatracker.ietf.org/doc/html/rfc4634">RFC 4634</a>
		/// </summary>
		SHA512 = 2
	}
}