using System;
using Gwen.Control;
using Gwen.Control.Layout;

namespace Gwen.UnitTest
{
	[UnitTest(Category = "Standard", Order = 202)]
	public class CheckBox : GUnit
    {
        public CheckBox(Base parent)
            : base(parent)
        {
			VerticalLayout layout = new VerticalLayout(this);

			Control.CheckBox check;

			check = new Control.CheckBox(layout);
			check.Margin = Margin.Three;
            check.Checked += OnChecked;
            check.UnChecked += OnUnchecked;
            check.CheckChanged += OnCheckChanged;

			Control.LabeledCheckBox labeled;

			labeled = new Control.LabeledCheckBox(layout);
			labeled.Margin = Margin.Three;
			labeled.Text = "Labeled CheckBox";
            labeled.Checked += OnChecked;
            labeled.UnChecked += OnUnchecked;
			labeled.CheckChanged += OnCheckChanged;

            check = new Control.CheckBox(layout);
			check.Margin = Margin.Three;
			check.IsDisabled = true;
        }

		void OnChecked(Base control, EventArgs args)
        {
            UnitPrint("CheckBox: Checked");
        }

		void OnCheckChanged(Base control, EventArgs args)
        {
            UnitPrint("CheckBox: CheckChanged");
        }

		void OnUnchecked(Base control, EventArgs args)
        {
            UnitPrint("CheckBox: UnChecked");
        }
    }
}
