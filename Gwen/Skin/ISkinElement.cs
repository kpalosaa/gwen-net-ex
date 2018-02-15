using System;

namespace Gwen.Skin.Texturing
{
	public interface ISkinElement
	{
		void Draw(Renderer.RendererBase render, Rectangle r);

		void Draw(Renderer.RendererBase render, Rectangle r, Color col);
	}
}
