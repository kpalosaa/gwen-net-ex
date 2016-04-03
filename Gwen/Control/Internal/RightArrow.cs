using System;
using Gwen.Control;

namespace Gwen.Control.Internal
{
    /// <summary>
    /// Submenu indicator.
    /// </summary>
    public class RightArrow : Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightArrow"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public RightArrow(Base parent)
            : base(parent)
        {
            MouseInputEnabled = false;

			this.Size = new Size(15, 15);
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
            skin.DrawMenuRightArrow(this);
        }
    }
}
