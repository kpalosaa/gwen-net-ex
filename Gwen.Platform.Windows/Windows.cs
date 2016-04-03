using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Gwen.Platform
{
	/// <summary>
	/// Windows platform specific functions.
	/// </summary>
	public class Windows : Base
	{
		private DateTime m_FirstTime = DateTime.Now;

		/// <summary>
		/// Gets text from clipboard.
		/// </summary>
		/// <returns>Clipboard text.</returns>
		public override string GetClipboardText()
		{
			// code from http://forums.getpaint.net/index.php?/topic/13712-trouble-accessing-the-clipboard/page__view__findpost__p__226140
			string ret = String.Empty;
			Thread staThread = new Thread(
				() =>
				{
					try
					{
						if (!Clipboard.ContainsText())
							return;
						ret = Clipboard.GetText();
					}
					catch (Exception)
					{
						return;
					}
				});
			staThread.SetApartmentState(ApartmentState.STA);
			staThread.Start();
			staThread.Join();
			// at this point either you have clipboard data or an exception
			return ret;
		}

		/// <summary>
		/// Sets the clipboard text.
		/// </summary>
		/// <param name="text">Text to set.</param>
		/// <returns>True if succeeded.</returns>
		public override bool SetClipboardText(string text)
		{
			bool ret = false;
			Thread staThread = new Thread(
				() =>
				{
					try
					{
						Clipboard.SetText(text);
						ret = true;
					}
					catch (Exception)
					{
						return;
					}
				});
			staThread.SetApartmentState(ApartmentState.STA);
			staThread.Start();
			staThread.Join();
			// at this point either you have clipboard data or an exception
			return ret;
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
			System.Windows.Forms.Cursor.Current = m_CursorMap[cursor];
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

		private static readonly Dictionary<Cursor, System.Windows.Forms.Cursor> m_CursorMap = new Dictionary<Cursor, System.Windows.Forms.Cursor>
		{
			{ Cursor.Normal, System.Windows.Forms.Cursors.Arrow },
			{ Cursor.Beam, System.Windows.Forms.Cursors.IBeam },
			{ Cursor.SizeNS, System.Windows.Forms.Cursors.SizeNS },
			{ Cursor.SizeWE, System.Windows.Forms.Cursors.SizeWE },
			{ Cursor.SizeNWSE, System.Windows.Forms.Cursors.SizeNWSE },
			{ Cursor.SizeNESW, System.Windows.Forms.Cursors.SizeNESW },
			{ Cursor.SizeAll, System.Windows.Forms.Cursors.SizeAll },
			{ Cursor.No, System.Windows.Forms.Cursors.No },
			{ Cursor.Wait, System.Windows.Forms.Cursors.WaitCursor },
			{ Cursor.Finger, System.Windows.Forms.Cursors.Hand }
		};
	}
}
