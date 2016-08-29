using System;
using System.IO;
using Gwen.Loader;
using Foundation;

namespace Gwen.Platform.iOS
{
	public class BundleLoader : LoaderBase
	{
		private string m_TextureBasePath;
		private string m_XmlBasePath;

		public BundleLoader(string textureBasePath = null, string xmlBasePath = null)
		{
			m_TextureBasePath = textureBasePath;
			m_XmlBasePath = xmlBasePath;
		}

		public override Stream GetTextureStream(string name)
		{
			string path = GetBundleName(m_TextureBasePath, name);

			try
			{
				return File.Open(path, FileMode.Open, FileAccess.Read);
			}
			catch
			{
				throw;
			}
		}

		public override Stream GetXmlStream(string name)
		{
			string path = GetBundleName(m_XmlBasePath, name);

			try
			{
				return File.Open(path, FileMode.Open, FileAccess.Read);
			}
			catch
			{
				throw;
			}
		}

		private string GetBundleName(string basePath, string name)
		{
			string extension = Path.GetExtension(name);
			if (!String.IsNullOrEmpty(extension))
				extension = extension.Substring(1);

			string path;
			if (String.IsNullOrEmpty(basePath))
				path = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(name), extension);
			else
				path = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(name), extension, basePath);

			return path;
		}
	}
}
