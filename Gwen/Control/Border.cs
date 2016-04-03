using System;

namespace Gwen.Control
{
	public enum BorderType
	{
		ToolTip,
		StatusBar,
		MenuStrip,
		Selection,
		PanelNormal,
		PanelBright,
		PanelDark,
		PanelHighlight,
		ListBox,
		TreeControl,
		CategoryList
	}

	[Xml.XmlControl]
	public class Border : Base
	{
		private BorderType m_BorderType;

		[Xml.XmlProperty]
		public BorderType BorderType { get { return m_BorderType; } set { if (m_BorderType == value) return; m_BorderType = value; } }

		public Border(Base parent)
			: base(parent)
		{
			m_BorderType = BorderType.PanelNormal;
		}

		protected override void Render(Skin.Base skin)
		{
			skin.DrawBorder(this, m_BorderType);
		}
	}
}
