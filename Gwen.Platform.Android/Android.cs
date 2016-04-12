using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace Gwen.Platform
{
	public class Android : PlatformBase
	{
		private DateTime m_FirstTime = DateTime.Now;

		/// <summary>
		/// Gets text from clipboard.
		/// </summary>
		/// <returns>Clipboard text.</returns>
		public override string GetClipboardText()
		{
			return String.Empty;
		}

		/// <summary>
		/// Sets the clipboard text.
		/// </summary>
		/// <param name="text">Text to set.</param>
		/// <returns>True if succeeded.</returns>
		public override bool SetClipboardText(string text)
		{
			return false;
		}

		/// <summary>
		/// Gets elapsed time since this class was initalized.
		/// </summary>
		/// <returns>Time interval in seconds.</returns>
		public override float GetTimeInSeconds()
		{
			return (float)((DateTime.Now - m_FirstTime).TotalSeconds);
		}

		/// <summary>
		/// Changes the mouse cursor.
		/// </summary>
		/// <param name="cursor">Cursor type.</param>
		public override void SetCursor(Cursor cursor)
		{
		}

		/// <summary>
		/// Get special folders of the system.
		/// </summary>
		/// <returns>List of folders.</returns>
		public override List<SpecialFolder> GetSpecialFolders()
		{
			List<SpecialFolder> folders = new List<SpecialFolder>();

			try
			{
				folders.Add(new SpecialFolder("Documents", "Libraries", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
				folders.Add(new SpecialFolder("Music", "Libraries", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)));
				folders.Add(new SpecialFolder("Pictures", "Libraries", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)));
				folders.Add(new SpecialFolder("Videos", "Libraries", Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)));
			}
			catch (Exception)
			{

			}

			DriveInfo[] drives = null;
			try
			{
				drives = DriveInfo.GetDrives();
			}
			catch (Exception) { }

			if (drives != null)
			{
				foreach (DriveInfo driveInfo in drives)
				{
					try
					{
						if (driveInfo.IsReady)
						{
							if (String.IsNullOrWhiteSpace(driveInfo.VolumeLabel))
								folders.Add(new SpecialFolder(driveInfo.Name, "Computer", driveInfo.Name));
							else
								folders.Add(new SpecialFolder(String.Format("{0} ({1})", driveInfo.VolumeLabel, driveInfo.Name), "Computer", driveInfo.Name));
						}
					}
					catch (Exception) { }
				}
			}

			return folders;
		}
	}
}
