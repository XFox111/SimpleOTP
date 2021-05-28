// ------------------------------------------------------------
// Copyright ©2021 Eugene Fox. All rights reserved.
// Code by Eugene Fox (aka XFox)
//
// Licensed under MIT license (https://opensource.org/licenses/MIT)
// ------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Timers;

using SimpleOTP.Models;

namespace SimpleOTP
{
	/// <summary>
	/// Represents method that will be called when new OTP code is generated.
	/// </summary>
	/// <param name="code">New OTP code instance.</param>
	public delegate void OTPCodeUpdatedEventHandler(OTPCode code);

	/// <summary>
	/// Class used to streamline OTP code generation on client devices.
	/// </summary>
	/// <remarks>
	/// <code>
	/// var factory = new (config);<br/>
	/// factory.CodeUpdated += (newCode) => Console.WriteLine(newCode.Code);
	/// </code>
	/// </remarks>
	public class OTPFactory : INotifyPropertyChanged, IDisposable
	{
		private readonly Timer _timer = new (1000);

		private OTPCode _currentCode;

		/// <summary>
		/// Gets or sets current valid OTP code instance.
		/// </summary>
		public OTPCode CurrentCode
		{
			get => _currentCode;
			set
			{
				if (_currentCode == value)
					return;
				_currentCode = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCode)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeLeft)));
				CodeUpdated?.Invoke(value);
			}
		}

		private OTPConfiguration _configuration;

		/// <summary>
		/// Gets or sets OTP configuration of the current instance.
		/// </summary>
		public OTPConfiguration Configuration
		{
			get => _configuration;
			set
			{
				if (_configuration == value)
					return;
				_configuration = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Configuration)));
			}
		}

		/// <summary>
		/// Gets time left before current OTP code expires.
		/// </summary>
		public TimeSpan? TimeLeft => CurrentCode?.Expiring - DateTime.UtcNow;

		/// <summary>
		/// Event is fired when new OTP code is generated.
		/// </summary>
		public event OTPCodeUpdatedEventHandler CodeUpdated;

		/// <inheritdoc/>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="OTPFactory"/> class.
		/// </summary>
		/// <param name="configuration">OTP configuration for codes producing.</param>
		/// <param name="timerUpdateInterval">Interval for timer updates in milliseconds.</param>
		public OTPFactory(OTPConfiguration configuration, int timerUpdateInterval = 1000)
		{
			Configuration = configuration;
			CurrentCode = OTPService.GenerateCode(ref configuration);

			_timer.Interval = timerUpdateInterval;
			_timer.Elapsed += TimerElapsed;
			_timer.Start();
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_timer.Stop();
			_timer.Elapsed -= TimerElapsed;
			_timer.Dispose();
			GC.SuppressFinalize(this);
		}

		private void TimerElapsed(object sender, ElapsedEventArgs args)
		{
			if (TimeLeft.Value.TotalSeconds <= 0)
				CurrentCode = OTPService.GenerateCode(ref _configuration);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeLeft)));
		}
	}
}