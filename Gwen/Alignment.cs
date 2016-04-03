using System;
using Gwen.Control;

namespace Gwen
{
	public enum Alignment
	{
		Left = Base.InternalFlags.AlignHLeft,
		CenterH = Base.InternalFlags.AlignHCenter,
		Right = Base.InternalFlags.AlignHRight,
		Top = Base.InternalFlags.AlignVTop,
		CenterV = Base.InternalFlags.AlignVCenter,
		Bottom = Base.InternalFlags.AlignVBottom,
		Center = CenterV | CenterH
	}

	public enum HorizontalAlignment
	{
		Left = Base.InternalFlags.AlignHLeft,
		Center = Base.InternalFlags.AlignHCenter,
		Right = Base.InternalFlags.AlignHRight,
		Stretch = Base.InternalFlags.AlignHStretch
	}

	public enum VerticalAlignment
	{
		Top = Base.InternalFlags.AlignVTop,
		Center = Base.InternalFlags.AlignVCenter,
		Bottom = Base.InternalFlags.AlignVBottom,
		Stretch = Base.InternalFlags.AlignVStretch
	}
}
