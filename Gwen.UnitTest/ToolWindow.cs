using System;
using Gwen.Control;
using Gwen.Control.Layout;

namespace Gwen.UnitTest
{
	[UnitTest(Category = "Containers", Order = 301)]
	public class ToolWindow : GUnit
	{
		public ToolWindow(Base parent)
            : base(parent)
        {
			Control.Button button1 = new Control.Button(this);
			button1.Dock = Dock.Top;
			button1.Width = 200;
            button1.Text = "Open a ToolBar";
			button1.Clicked += OpenToolBar;

			Control.Button button2 = new Control.Button(this);
			button2.Dock = Dock.Top;
			button2.Width = 200;
			button2.Text = "Open a tool window";
			button2.Clicked += OpenToolWindow;
		}

		void OpenToolBar(Base control, EventArgs args)
        {
			Control.ToolWindow window = new Control.ToolWindow(GetCanvas());
			window.Padding = Padding.Five;
			window.HorizontalAlignment = HorizontalAlignment.Left;
			window.VerticalAlignment = VerticalAlignment.Top;

			HorizontalLayout layout = new HorizontalLayout(window);

			for (int i = 0; i < 5; i++)
			{
				Control.Button button = new Control.Button(layout);
				button.Size = new Size(36, 36);
				button.UserData = window;
				button.Clicked += Close;
			}
		}

		void OpenToolWindow(Base control, EventArgs args)
		{
			Control.ToolWindow window = new Control.ToolWindow(GetCanvas());
			window.Padding = Padding.Five;
			window.HorizontalAlignment = HorizontalAlignment.Left;
			window.VerticalAlignment = VerticalAlignment.Top;
			window.Vertical = true;

			Control.Layout.GridLayout layout = new Control.Layout.GridLayout(window);
			layout.ColumnCount = 2;

			Control.Button button = new Control.Button(layout);
			button.Size = new Size(100, 40);
			button.UserData = window;
			button.Clicked += Close;

			button = new Control.Button(layout);
			button.Size = new Size(100, 40);
			button.UserData = window;
			button.Clicked += Close;

			button = new Control.Button(layout);
			button.Size = new Size(100, 40);
			button.UserData = window;
			button.Clicked += Close;

			button = new Control.Button(layout);
			button.Size = new Size(100, 40);
			button.UserData = window;
			button.Clicked += Close;
		}

		void Close(Base control, EventArgs args)
		{
			Control.ToolWindow window = control.UserData as Control.ToolWindow;
			window.Close();
			window.Parent.RemoveChild(window, true);
		}
    }
}
