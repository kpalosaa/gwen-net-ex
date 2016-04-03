﻿using System;
using Gwen.Control;

namespace Gwen.Control.Internal
{
    /// <summary>
    /// Numeric up arrow.
    /// </summary>
    public class UpDownButton_Up : ButtonBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="UpDownButton_Up"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public UpDownButton_Up(Base parent)
            : base(parent)
        {
			Width = 7;
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawNumericUpDownButton(this, IsDepressed, true);
        }
    }
}
