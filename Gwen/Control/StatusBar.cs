using System;

namespace Gwen.Control
{
	/// <summary>
	/// Status bar.
	/// </summary>
	[Xml.XmlControl]
	public class StatusBar : Base
    {
		private Label m_Label;

		[Xml.XmlProperty]
		public string Text { get { return m_Label.Text; } set { m_Label.Text = value; } }

		[Xml.XmlProperty]
		public Color TextColor { get { return m_Label.TextColor; } set { m_Label.TextColor = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public StatusBar(Base parent) : base(parent)
        {
			Height = 26;
			Dock = Dock.Bottom;
            Padding = new Padding(6, 1, 6, 1);

			this.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.VerticalAlignment = VerticalAlignment.Bottom;

			m_Label = new Label(this);
			m_Label.AutoSizeToContents = false;
			m_Label.Alignment = Alignment.Left | Alignment.CenterV;
			m_Label.Dock = Dock.Fill;
		}

		/// <summary>
		/// Adds a control to the bar.
		/// </summary>
		/// <param name="control">Control to add.</param>
		/// <param name="right">Determines whether the control should be added to the right side of the bar.</param>
		public void AddControl(Base control, bool right)
        {
            control.Parent = this;
            control.Dock = right ? Dock.Right : Dock.Left;
			control.VerticalAlignment = VerticalAlignment.Center;
        }

		protected override void OnChildAdded(Base child)
		{
			child.VerticalAlignment = VerticalAlignment.Center;
			if (child.Dock != Dock.Left)
				child.Dock = Dock.Right;

			base.OnChildAdded(child);
		}

		/// <summary>
		/// Renders the control using specified skin.
		/// </summary>
		/// <param name="skin">Skin to use.</param>
		protected override void Render(Skin.Base skin)
        {
            skin.DrawStatusBar(this);
        }
	}
}
