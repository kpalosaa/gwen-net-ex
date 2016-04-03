﻿//#define DEBUG_TEXT_MEASURE

using System;
using Gwen.Control;

namespace Gwen.Control.Internal
{
	/// <summary>
	/// Displays text. Always sized to contents.
	/// </summary>
	public class Text : Base
	{
		private string m_String;
		private Font m_Font;

		private string m_FitToText;

		/// <summary>
		/// Font used to display the text.
		/// </summary>
		/// <remarks>
		/// The font is not being disposed by this class.
		/// </remarks>
		public Font Font { get { return m_Font; } set { m_Font = value; Invalidate(); } }

		/// <summary>
		/// Text to display.
		/// </summary>
		public string String
		{
			get { return m_String; }
			set
			{
				if (value == m_String)
					return;

				m_String = value;
				if (AutoSizeToContents)
					Invalidate();
			}
		}

		/// <summary>
		/// Text color.
		/// </summary>
		public Color TextColor { get; set; }

		/// <summary>
		/// Determines whether the control should be automatically resized to fit the text.
		/// </summary>
		public bool AutoSizeToContents { get; set; }

		/// <summary>
		/// Text length in characters.
		/// </summary>
		public int Length { get { return String.Length; } }

		/// <summary>
		/// Text color override - used by tooltips.
		/// </summary>
		public Color TextColorOverride { get; set; }

		/// <summary>
		/// Text override - used to display different string.
		/// </summary>
		public string TextOverride { get; set; }

		/// <summary>
		/// Set the minimum size of the control to be able to show the text of this property.
		/// </summary>
		public string FitToText { get { return m_FitToText; } set { if (m_FitToText == value) return; m_FitToText = value; Invalidate(); } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Text"/> class.
		/// </summary>
		/// <param name="parent">Parent control.</param>
		public Text(Base parent)
			: base(parent)
		{
			AutoSizeToContents = true;
			m_Font = Skin.DefaultFont;
			m_String = String.Empty;
			m_FitToText = null;
			TextColor = Skin.Colors.Label.Default;
			MouseInputEnabled = false;
			TextColorOverride = new Color(0, 255, 255, 255); // A==0, override disabled
		}

		/// <summary>
		/// Renders the control using specified skin.
		/// </summary>
		/// <param name="skin">Skin to use.</param>
		protected override void Render(Skin.Base skin)
		{
			if (Length == 0 || Font == null) return;

			if (TextColorOverride.A == 0)
				skin.Renderer.DrawColor = TextColor;
			else
				skin.Renderer.DrawColor = TextColorOverride;

			skin.Renderer.RenderText(Font, Point.Zero, TextOverride ?? String);

#if DEBUG_TEXT_MEASURE
			{
				Point lastPos = Point.Zero;

				for (int i = 0; i < m_String.Length + 1; i++)
				{
					String sub = (TextOverride ?? String).Substring(0, i);
					Point p = Skin.Renderer.MeasureText(Font, sub);

					Rectangle rect = new Rectangle();
					rect.Location = lastPos;
					rect.Size = new Size(p.X - lastPos.X, p.Y);
					skin.Renderer.DrawColor = Color.FromArgb(64, 0, 0, 0);
					skin.Renderer.DrawLinedRect(rect);

					lastPos = new Point(rect.Right, 0);
				}
			}
#endif
		}

		protected override Size Measure(Size availableSize)
		{
			if (String == null)
				return Size.Zero;

			if (Font == null)
				throw new InvalidOperationException("Text control font not set.");

			Size size = Size.Zero;

			string text = TextOverride ?? String;

			if (AutoSizeToContents && text.Length == 0)
				size = Skin.Renderer.MeasureText(Font, " ");
			else if (!AutoSizeToContents && !String.IsNullOrWhiteSpace(m_FitToText))
				size = Skin.Renderer.MeasureText(Font, m_FitToText);
			else
				size = Skin.Renderer.MeasureText(Font, text);

			return size;
		}

		protected override Size Arrange(Size finalSize)
		{
			return MeasuredSize;
		}

		/// <summary>
		/// Gets the coordinates of specified character in the text.
		/// </summary>
		/// <param name="index">Character index.</param>
		/// <returns>Character position in local coordinates.</returns>
		public Point GetCharacterPosition(int index)
		{
			if (Length == 0 || index == 0)
			{
				return new Point(0, 0);
			}

			string sub = (TextOverride ?? String).Substring(0, index);
			Size s = Skin.Renderer.MeasureText(Font, sub);

			return new Point(s.Width, 0);
		}

		/// <summary>
		/// Searches for a character closest to given point.
		/// </summary>
		/// <param name="p">Point.</param>
		/// <returns>Character index.</returns>
		public int GetClosestCharacter(Point p)
		{
			int distance = Util.Infinity;
			int c = 0;

			for (int i = 0; i < String.Length + 1; i++)
			{
				Point cp = GetCharacterPosition(i);
				int dist = Math.Abs(cp.X - p.X) + Math.Abs(cp.Y - p.Y); // this isn't proper // [omeg] todo: sqrt

				if (dist > distance)
					continue;

				distance = dist;
				c = i;
			}

			return c;
		}
	}
}
