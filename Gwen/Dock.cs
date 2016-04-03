using System;
using Gwen.Control;

namespace Gwen
{
	public enum Dock
	{
		None = Base.InternalFlags.DockNone,
		Left = Base.InternalFlags.DockLeft,
		Top = Base.InternalFlags.DockTop,
		Right = Base.InternalFlags.DockRight,
		Bottom = Base.InternalFlags.DockBottom,
		Fill = Base.InternalFlags.DockFill,
	}
}
