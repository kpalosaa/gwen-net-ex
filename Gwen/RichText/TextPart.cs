using System;
using System.Collections.Generic;

namespace Gwen.RichText
{
	public class TextPart : Part
	{
		private string m_Text;
		private Color? m_Color;
		private Font m_Font;

		public string Text { get { return m_Text; } }
		public Color? Color { get { return m_Color; } }
		public Font Font { get { return m_Font; } protected set { m_Font = value; } }

		public TextPart(string text)
		{
			m_Text = text;
			m_Color = null;
		}

		public TextPart(string text, Color color)
		{
			m_Text = text;
			m_Color = color;
		}

		public override string[] Split(ref Font font)
		{
			m_Font = font;

			return StringSplit(m_Text);
		}

		protected string[] StringSplit(string str)
		{
			List<string> strs = new List<string>();
			int len = str.Length;
			int index = 0;
			int i;

			while (index < len)
			{
				i = str.IndexOf(' ', index);
				if (i == index)
				{
					strs.Add(" ");
					while (index < len && str[index] == ' ')
						index++;
				}
				else if (i != -1)
				{
					strs.Add(str.Substring(index, i - index + 1));
					index = i + 1;
				}
				else
				{
					strs.Add(str.Substring(index));
					break;
				}
			}

			return strs.ToArray();
		}
	}
}
