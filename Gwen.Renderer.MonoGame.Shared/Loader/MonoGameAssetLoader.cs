using System;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gwen.Loader.MonoGame
{
    public class MonoGameAssetLoader : LoaderBase
	{
		private ContentManager m_ContentManager;

		public MonoGameAssetLoader(ContentManager contentManager)
		{
			m_ContentManager = contentManager;
		}

		public override Stream GetTextureStream(string name)
		{
			throw new NotSupportedException();
		}

		public override Stream GetXmlStream(string name)
		{
			throw new NotSupportedException();
		}

		public virtual Texture2D LoadTexture(Texture texture)
		{
			string assetName = GetTextureName(texture);

			try
			{
				return m_ContentManager.Load<Texture2D>(assetName);
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine(String.Format("Texture '{0}' not found.", assetName));
				throw;
			}
		}

		public virtual string GetTextureName(Texture texture)
		{
			string name = texture.Name;

			if (name.Contains(".png") || name.Contains(".jpg"))
				name = name.Substring(0, name.Length - 4);

			return name;
		}

		public virtual SpriteFont LoadFont(Font font)
		{
			string assetName = GetFontName(font);

			try
			{
				return m_ContentManager.Load<SpriteFont>(assetName);
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine(String.Format("Font '{0}' not found.", assetName));
				throw;
			}
		}

		public virtual string GetFontName(Font font)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("{0} {1}", font.FaceName, font.RealSize);

			if (font.Italic && font.Bold)
				sb.Append(" Bold Italic");
			else if (font.Italic)
				sb.Append(" Italic");
			else if (font.Bold)
				sb.Append(" Bold");

			return sb.ToString();
		}
	}
}
