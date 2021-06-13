// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;

namespace SimpleOTP.Models
{
	/// <summary>
	/// OTP code object model.
	/// </summary>
	public record OTPCode
	{
		/// <summary>
		/// Gets or sets OTP code.
		/// </summary>
		public int Code { get; set; }

		/// <summary>
		/// Gets or sets date-time until the code is valid.
		/// </summary>
		public DateTime? Expiring { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OTPCode"/> class.
		/// </summary>
		public OTPCode()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OTPCode"/> class.<br/>
		/// Use this constructor only for HOTP key. Otherwise, fill out all properties.
		/// </summary>
		/// <param name="code">OTP code.</param>
		public OTPCode(int code) =>
			Code = code;

		/// <summary>
		/// Gets valid 6 digit or more OTP code.
		/// </summary>
		/// <param name="formatter">String formatter. Other variation:
		/// <code>"000 000"</code></param>
		/// <returns>Formatted OTP code string with 6 or more digits.</returns>
		public string GetCode(string formatter = "000000") =>
			Code.ToString(formatter);
	}
}