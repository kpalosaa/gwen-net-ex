using System;
using System.Collections.Generic;

namespace Gwen.Platform
{
	public class SpecialFolder
	{
		public SpecialFolder(string name, string category, string path)
		{
			Name = name;
			Category = category;
			Path = path;
		}

		public string Name { get; private set; }
		public string Category { get; private set; }
		public string Path { get; private set; }
	}

	/// <summary>
	/// Platform specific functions.
	/// </summary>
	public abstract class PlatformBase
	{
		/// <summary>
		/// Gets text from clipboard.
		/// </summary>
		/// <returns>Clipboard text.</returns>
		public abstract string GetClipboardText();

		/// <summary>
		/// Sets the clipboard text.
		/// </summary>
		/// <param name="text">Text to set.</param>
		/// <returns>True if succeeded.</returns>
		public abstract bool SetClipboardText(string text);

		/// <summary>
		/// Gets elapsed time since this class was initalized.
		/// </summary>
		/// <returns>Time interval in seconds.</returns>
		public abstract float GetTimeInSeconds();

		/// <summary>
		/// Changes the mouse cursor.
		/// </summary>
		/// <param name="cursor">Cursor type.</param>
		public abstract void SetCursor(Cursor cursor);

		/// <summary>
		/// Get special folders of the system.
		/// </summary>
		/// <returns>List of folders.</returns>
		public abstract List<SpecialFolder> GetSpecialFolders();
	}
}
