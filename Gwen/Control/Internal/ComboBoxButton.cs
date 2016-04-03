using System;

namespace Gwen.Control.Internal
{
	/// <summary>
	/// Editable combobox button.
	/// </summary>
	internal class ComboBoxButton : ButtonBase
	{
		private EditableComboBox m_ComboBox;

		/// <summary>
		/// Initializes a new instance of the <see cref="ComboBoxButton"/> class.
		/// </summary>
		/// <param name="parent">Parent control.</param>
		public ComboBoxButton(Base parent, EditableComboBox comboBox)
            : base(parent)
        {
			Width = 15;

			m_ComboBox = comboBox;
		}

		/// <summary>
		/// Renders the control using specified skin.
		/// </summary>
		/// <param name="skin">Skin to use.</param>
		protected override void Render(Skin.Base skin)
		{
			skin.DrawComboBoxArrow(this, IsHovered, IsDepressed, m_ComboBox.IsOpen, m_ComboBox.IsDisabled);
		}
	}
}
