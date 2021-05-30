// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleOTP.Models;

namespace SimpleOTP.Test
{
	/// <summary>
	/// OTP factory class unit-tests.
	/// </summary>
	[TestClass]
	public class OTPFactoryUnitTest
	{
		/// <summary>
		/// Complex test of OTP factory.
		/// </summary>
		/// <returns><see cref="Task"/>.</returns>
		[TestMethod("Overall factory test")]
		public async Task TestFactory()
		{
			OTPConfiguration config = OTPConfiguration.GetConfiguration("ESQVTYRM2CWZC3NX24GRRWIAUUWVHWQH", "FoxDev Studio", "eugene@xfox111.net");
			config.Period = TimeSpan.FromSeconds(3);
			using OTPFactory factory = new (config, 1500);
			var testGetConfig = factory.Configuration;
			System.Diagnostics.Debug.WriteLine(testGetConfig);
			var code = factory.CurrentCode;

			factory.Configuration = config;
			factory.CurrentCode = code;

			await Task.Delay(3500);
			Assert.AreNotEqual(code.Code, factory.CurrentCode.Code);
		}
	}
}