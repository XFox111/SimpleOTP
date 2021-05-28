// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleOTP.Helpers;

namespace SimpleOTP.Test.Helpers
{
	/// <summary>
	/// Unit-tests for Base32 encoder.
	/// </summary>
	[TestClass]
	public class Base32UnitTest
	{
		/// <summary>
		/// Test overall work of the encoder.
		/// </summary>
		[TestMethod("Overall Base32 encoder test")]
		public void EncoderTest()
		{
			// byte[] bytes = new byte[new Random().Next(128, 161)];    // FIXME: See SimpleOTP.Helpers.Base32Encoder.Encode()
			byte[] bytes = new byte[160];
			new Random().NextBytes(bytes);
			string str = Base32Encoder.Encode(bytes);

			bytes = Base32Encoder.Decode(str);
			string result = Base32Encoder.Encode(bytes);
			Assert.AreEqual(str, result);
		}
	}
}