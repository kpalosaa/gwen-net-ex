using System;
using Gwen.Control;
using Gwen.Control.Layout;

namespace Gwen.UnitTest
{
    public class GUnit : Base
    {
        public UnitTest UnitTest;

        public GUnit(Base parent) : base(parent)
        {
			this.IsVirtualControl = true;
        }

        public void UnitPrint(string str)
        {
            if (UnitTest != null)
                UnitTest.PrintText(str);
        }
	}
}
