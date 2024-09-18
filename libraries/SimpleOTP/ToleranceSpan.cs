namespace SimpleOTP;

/// <summary>
/// Represents a span of tolerance values used in OTP (One-Time Password) validation.
/// </summary>
/// <param name="behind">The number of periods/counter values behind the current value.</param>
/// <param name="ahead">The number of periods/counter values ahead of the current value.</param>
public readonly struct ToleranceSpan(int behind, int ahead) : IEquatable<ToleranceSpan>
{
	/// <summary>
	/// Gets the default recommended <see cref="ToleranceSpan"/> value.
	/// </summary>
	/// <value>The default <see cref="ToleranceSpan"/> value: 1 counter/period ahead and behind.</value>
	public static ToleranceSpan Default { get; } = new(1);

	/// <summary>
	/// Gets the number of tolerance values behind the current value.
	/// </summary>
	public int Behind { get; init; } = behind;

	/// <summary>
	/// Gets the number of tolerance values ahead of the current value.
	/// </summary>
	public int Ahead { get; init; } = ahead;

	/// <summary>
	/// Initializes a new instance of the <see cref="ToleranceSpan"/> struct with the specified tolerance value.
	/// The <see cref="Behind"/> and <see cref="Ahead"/> properties will be set to the same value.
	/// </summary>
	/// <param name="tolerance">The tolerance value to set for both <see cref="Behind"/> and <see cref="Ahead"/>.</param>
	public ToleranceSpan(int tolerance) : this(tolerance, tolerance) { }

	/// <inheritdoc/>
	public bool Equals(ToleranceSpan other) =>
		Behind == other.Behind && Ahead == other.Ahead;

	/// <inheritdoc/>
	public override bool Equals(object? obj) =>
		obj is ToleranceSpan span && Equals(span);

	/// <inheritdoc/>
	public override int GetHashCode() =>
		HashCode.Combine(Behind, Ahead);

	/// <summary>
	/// Returns the string representation of the <see cref="ToleranceSpan"/> struct.
	/// </summary>
	/// <returns>The string representation of the <see cref="ToleranceSpan"/> struct.</returns>
	public override string ToString() =>
		$"(-{Behind}, +{Ahead})";

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public static implicit operator ToleranceSpan(int tolerance) => new(tolerance);
	public static implicit operator ToleranceSpan((int behind, int ahead) tolerance) => new(tolerance.behind, tolerance.ahead);

	public static bool operator ==(ToleranceSpan left, ToleranceSpan right) => left.Equals(right);
	public static bool operator !=(ToleranceSpan left, ToleranceSpan right) => !(left == right);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
