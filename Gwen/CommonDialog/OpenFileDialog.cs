using System;
using System.IO;
using Gwen.Control;

namespace Gwen.CommonDialog
{
	/// <summary>
	/// Dialog for selecting an existing file.
	/// </summary>
	public class OpenFileDialog : FileDialog
	{
		public OpenFileDialog(Base parent)
			: base(parent)
		{
		}

		protected override void OnCreated()
		{
			base.OnCreated();

			Title = "Open File";
			OkButtonText = "Open";
			EnableNewFolder = false;
		}

		protected override void OnItemSelected(string path)
		{
			if (File.Exists(path))
			{
				SetCurrentItem(Path.GetFileName(path));
			}
		}

		protected override bool IsSubmittedNameOk(string path)
		{
			if (Directory.Exists(path))
			{
				SetPath(path);
			}
			else if (File.Exists(path))
			{
				return true;
			}

			return false;
		}

		protected override bool ValidateFileName(string path)
		{
			return File.Exists(path);
		}
	}
}
