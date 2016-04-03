using System;
using Gwen.Control;
using Gwen.Control.Layout;

namespace Gwen.UnitTest
{
	[UnitTest(Category = "Standard", Order = 200)]
	public class Button : GUnit
    {
        public Button(Base parent)
            : base(parent)
        {
			HorizontalLayout hlayout = new HorizontalLayout(this);
			{
				VerticalLayout vlayout = new VerticalLayout(hlayout);
				vlayout.Width = 300;
				{
					Gwen.Control.Button button;

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Text = "Button";

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Text = "Image button (default)";
					button.SetImage("test16.png");

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Text = "Image button (above)";
					button.SetImage("test16.png", ImageAlign.Above);

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Alignment = Alignment.Left | Alignment.CenterV;
					button.Text = "Image button (left)";
					button.SetImage("test16.png");

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Alignment = Alignment.Right | Alignment.CenterV;
					button.Text = "Image button (right)";
					button.SetImage("test16.png");

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Text = "Image button (image left)";
					button.SetImage("test16.png", ImageAlign.Left | ImageAlign.CenterV);

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Text = "Image button (image right)";
					button.SetImage("test16.png", ImageAlign.Right | ImageAlign.CenterV);

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Text = "Image button (image fill)";
					button.SetImage("test16.png", ImageAlign.Fill);

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.SetImage("test16.png");
					button.Size = new Size(20, 20);

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = new Padding(20, 20, 20, 20);
					button.Text = "Toggle me";
					button.IsToggle = true;
					button.Toggled += onToggle;
					button.ToggledOn += onToggleOn;
					button.ToggledOff += onToggleOff;

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Text = "Disabled";
					button.Height = 28;
					button.IsDisabled = true;

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Text = "With Tooltip";
					button.Height = 28;
					button.SetToolTipText("This is tooltip");

					button = new Control.Button(vlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Text = "Autosized";
					button.HorizontalAlignment = HorizontalAlignment.Left;
				}

				{
					Control.Button button = new Control.Button(hlayout);
					button.Margin = Margin.Five;
					button.Padding = Padding.Three;
					button.Text = "Event tester";
					button.Size = new Size(300, 200);
					button.Pressed += onButtonAp;
					button.Clicked += onButtonAc;
					button.Released += onButtonAr;
				}
			}
		}

		private void onButtonAc(Base control, EventArgs args)
        {
            UnitPrint("Button: Clicked");
        }

		private void onButtonAp(Base control, EventArgs args)
        {
            UnitPrint("Button: Pressed");
        }

		private void onButtonAr(Base control, EventArgs args)
        {
            UnitPrint("Button: Released");
        }

		private void onToggle(Base control, EventArgs args)
        {
            UnitPrint("Button: Toggled");
        }

		private void onToggleOn(Base control, EventArgs args)
        {
            UnitPrint("Button: ToggleOn");
        }

		private void onToggleOff(Base control, EventArgs args)
        {
            UnitPrint("Button: ToggledOff");
        }
    }
}
