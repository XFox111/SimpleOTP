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
	/// Unit-tests for key generator.
	/// </summary>
	[TestClass]
	public class SecretGeneratorUnitTest
	{
		/// <summary>
		/// Overall test of key generator.
		/// </summary>
		[TestMethod("Overall generator tests")]
		public void Test_Generator()
		{
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => SecretGenerator.GenerateSecret(64));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => SecretGenerator.GenerateSecret(256));

			string key = SecretGenerator.GenerateSecret();
			Assert.IsFalse(string.IsNullOrWhiteSpace(key));

			key = SecretGenerator.GenerateSecret(128);
			Assert.IsFalse(string.IsNullOrWhiteSpace(key));
		}
	}
}