using System;
using System.IO;
using Gwen.Control;

namespace Gwen.CommonDialog
{
	/// <summary>
	/// Dialog for selecting an existing directory.
	/// </summary>
	public class FolderBrowserDialog : FileDialog
	{
		public FolderBrowserDialog(Base parent)
			: base(parent)
		{
		}

		protected override void OnCreated()
		{
			base.OnCreated();

			FoldersOnly = true;
			Title = "Select Folder";
			OkButtonText = "Select";
		}

		protected override void OnItemSelected(string path)
		{
			if (Directory.Exists(path))
			{
				SetCurrentItem(Path.GetFileName(path));
			}
		}

		protected override bool IsSubmittedNameOk(string path)
		{
			if (Directory.Exists(path))
			{
				SetPath(path);
				return true;
			}

			return false;
		}

		protected override bool ValidateFileName(string path)
		{
			return Directory.Exists(path);
		}
	}
}
