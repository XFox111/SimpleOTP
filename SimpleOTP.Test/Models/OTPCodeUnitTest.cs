// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleOTP.Models;

namespace SimpleOTP.Test.Models
{
	/// <summary>
	/// Unit-tests for OTP code model.
	/// </summary>
	[TestClass]
	public class OTPCodeUnitTest
	{
		/// <summary>
		/// Get formatted OTP code.
		/// </summary>
		[TestMethod("Code formatting")]
		public void Test_GetFullCode()
		{
			OTPCode code = new ()
			{
				Code = 123,
				Expiring = DateTime.UtcNow.AddSeconds(30),
			};

			Assert.AreEqual("000123", code.GetCode());
			Assert.AreEqual("000 123", code.GetCode("000 000"));
		}
	}
}