namespace YummyConsole
{
	/// <summary>
	/// The coordinate space in which to operate.
	/// </summary>
	public enum Space
	{
		/// <summary>
		/// Values represents the local offset.
		/// </summary>
		Local,
		/// <summary>
		/// Values represents the global value, as if it didn't have a parent.
		/// </summary>
		Global,
	}
}