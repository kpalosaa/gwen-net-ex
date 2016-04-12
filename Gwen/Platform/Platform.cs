using System;
using System.Collections.Generic;

namespace Gwen.Platform
{
	public class Platform
	{
		private static PlatformBase m_Platform = null;

		/// <summary>
		/// Gets text from clipboard.
		/// </summary>
		/// <returns>Clipboard text.</returns>
		public static string GetClipboardText()
		{
			System.Diagnostics.Debug.Assert(m_Platform != null);

			return m_Platform.GetClipboardText();
		}

		/// <summary>
		/// Sets the clipboard text.
		/// </summary>
		/// <param name="text">Text to set.</param>
		/// <returns>True if succeeded.</returns>
		public static bool SetClipboardText(string text)
		{
			System.Diagnostics.Debug.Assert(m_Platform != null);

			return m_Platform.SetClipboardText(text);
		}


		/// <summary>
		/// Gets elapsed time since this class was initalized.
		/// </summary>
		/// <returns>Time interval in seconds.</returns>
		public static float GetTimeInSeconds()
		{
			System.Diagnostics.Debug.Assert(m_Platform != null);

			return m_Platform.GetTimeInSeconds();
		}

		/// <summary>
		/// Changes the mouse cursor.
		/// </summary>
		/// <param name="cursor">Cursor type.</param>
		public static void SetCursor(Cursor cursor)
		{
			System.Diagnostics.Debug.Assert(m_Platform != null);

			m_Platform.SetCursor(cursor);
		}

		/// <summary>
		/// Get special folders of the system.
		/// </summary>
		/// <returns>List of folders.</returns>
		public static List<SpecialFolder> GetSpecialFolders()
		{
			System.Diagnostics.Debug.Assert(m_Platform != null);

			return m_Platform.GetSpecialFolders();
		}

		/// <summary>
		/// Set the current platform.
		/// </summary>
		/// <param name="platform">Platform.</param>
		public static void Init(PlatformBase platform)
		{
			m_Platform = platform;
		}
	}
}
