using System;
using System.IO;
using Gwen.Loader;
using Android.App;

namespace Gwen.Platform.Android
{
	public class AssetLoader : LoaderBase
	{
		private string m_TextureBasePath;
		private string m_XmlBasePath;

		public AssetLoader(string textureBasePath = null, string xmlBasePath = null)
		{
			m_TextureBasePath = textureBasePath;
			m_XmlBasePath = xmlBasePath;
		}

		public override Stream GetTextureStream(string name)
		{
			return GetStream(m_TextureBasePath, name);
		}

		public override Stream GetXmlStream(string name)
		{
			return GetStream(m_XmlBasePath, name);
		}

		private Stream GetStream(string basePath, string name)
		{
			string path;
			if (String.IsNullOrEmpty(basePath))
				path = name;
			else
				path = Path.Combine(basePath, name);

			return Application.Context.Assets.Open(path);
		}
	}
}
