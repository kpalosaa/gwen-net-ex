using System;
using System.Collections.Generic;

namespace Gwen.RichText
{
	internal abstract class LineBreaker
	{
		private Renderer.Base m_Renderer;
		private Font m_DefaultFont;

		public Renderer.Base Renderer { get { return m_Renderer; } }
		public Font DefaultFont { get { return m_DefaultFont; } }

		public LineBreaker(Renderer.Base renderer, Font defaultFont)
		{
			m_Renderer = renderer;
			m_DefaultFont = defaultFont;
		}

		public abstract List<TextBlock> LineBreak(Paragraph paragraph, int width);
	}
}
