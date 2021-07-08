// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;
using System.Text;

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
		/// Test encoder with byte array.
		/// </summary>
		[TestMethod("Byte array Base32 encoder test")]
		public void EncoderTest()
		{
			byte[] bytes = new byte[new Random().Next(16, 20)];
			new Random().NextBytes(bytes);
			string str = Base32Encoder.Encode(bytes);

			byte[] result = Base32Encoder.Decode(str);
			Assert.AreEqual(bytes.Length, result.Length);
			for (int i = 0; i < bytes.Length; i++)
				Assert.AreEqual(bytes[i], result[i]);
		}

		/// <summary>
		/// Test encoder with string content.
		/// </summary>
		[TestMethod("String Base32 encoder test")]
		public void EncoderStringTest()
		{
			string testStr = "Hello, World!";
			string encodedStr = Base32Encoder.Encode(Encoding.UTF8.GetBytes(testStr));

			byte[] resultBytes = Base32Encoder.Decode(encodedStr);
			string result = Encoding.UTF8.GetString(resultBytes);
			Assert.AreEqual(testStr, result);
		}
	}
}