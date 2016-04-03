using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Gwen.Control;
using Gwen.Control.Layout;

namespace Gwen.UnitTest
{
    public class UnitTest : Base
    {
        private Control.Base m_LastControl;
        private readonly Control.StatusBar m_StatusBar;
        private readonly Control.ListBox m_TextOutput;
        private readonly Control.CollapsibleList m_List;
        private readonly Base m_Center;
        private readonly Control.LabeledCheckBox m_DebugCheck;

        public double Fps; // set this in your rendering loop
        public string Note; // additional text to display in status bar

        public UnitTest(Base parent) : base(parent)
        {
            Dock = Dock.Fill;

			DockBase dock = new DockBase(this);
			dock.Dock = Dock.Fill;

			m_List = new Control.CollapsibleList(this);

            dock.LeftDock.TabControl.AddPage("Unit tests", m_List);
			dock.LeftDock.Width = 150;

            m_TextOutput = new Control.ListBox(this);
			m_TextOutput.AlternateColor = false;

            dock.BottomDock.TabControl.AddPage("Output", m_TextOutput);
            dock.BottomDock.Height = 200;

            m_StatusBar = new Control.StatusBar(this);
            m_StatusBar.Dock = Dock.Bottom;

			m_DebugCheck = new Control.LabeledCheckBox(m_StatusBar);
			m_DebugCheck.Text = "Debug outlines";
			m_DebugCheck.CheckChanged += DebugCheckChanged;

			m_Center = new Control.Layout.DockLayout(dock);
            m_Center.Dock = Dock.Fill;

			List<Type> tests = typeof(UnitTest).Assembly.GetTypes().Where(t => t.IsDefined(typeof(UnitTestAttribute), false)).ToList();
			tests.Sort((t1, t2) =>
			{
				object[] a1s = t1.GetCustomAttributes(typeof(UnitTestAttribute), false);
				object[] a2s = t2.GetCustomAttributes(typeof(UnitTestAttribute), false);
				if (a1s.Length > 0 && a2s.Length > 0)
				{
					UnitTestAttribute a1 = a1s[0] as UnitTestAttribute;
					UnitTestAttribute a2 = a2s[0] as UnitTestAttribute;
					if (a1.Order == a2.Order)
					{
						if (a1.Category == a2.Category)
							return String.Compare(a1.Name != null ? a1.Name : t1.Name, a2.Name != null ? a2.Name : t2.Name, true);
						else
							return String.Compare(a1.Category, a2.Category, true);
					}
					return a1.Order - a2.Order;
				}

				return 0;
			});
			
			foreach (Type type in tests)
			{
				object[] attribs = type.GetCustomAttributes(typeof(UnitTestAttribute), false);
				if (attribs.Length > 0)
				{
					UnitTestAttribute attrib = attribs[0] as UnitTestAttribute;
					if (attrib != null)
					{
						CollapsibleCategory cat = m_List.FindChildByName(attrib.Category) as CollapsibleCategory;
						if (cat == null)
							cat = m_List.Add(attrib.Category, attrib.Category);
						GUnit test = Activator.CreateInstance(type, m_Center) as GUnit;
						RegisterUnitTest(attrib.Name != null ? attrib.Name : type.Name, cat, test);
					}
				}
			}

			m_StatusBar.SendToBack();
            PrintText("Unit Test started!");
		}

		public void RegisterUnitTest(string name, CollapsibleCategory cat, GUnit test)
        {
            Control.Button btn = cat.Add(name);
            test.Dock = Dock.Fill;
            test.Collapse();
            test.UnitTest = this;
            btn.UserData = test;
            btn.Clicked += OnCategorySelect;
        }

		private void DebugCheckChanged(Base control, EventArgs args)
        {
            if (m_DebugCheck.IsChecked)
                m_Center.DrawDebugOutlines = true;
            else
                m_Center.DrawDebugOutlines = false;
            //Invalidate();
        }

		private void OnCategorySelect(Base control, EventArgs args)
        {
            if (m_LastControl != null)
            {
                m_LastControl.Collapse();
            }
            Base test = control.UserData as Base;
            test.Show();
            m_LastControl = test;
        }

        public void PrintText(string str)
        {
            m_TextOutput.AddRow(str);
            m_TextOutput.ScrollToBottom();
        }

        protected override void Render(Skin.Base skin)
        {
            m_StatusBar.Text = String.Format("GWEN.Net Unit Test - {0:F0} fps. {1}", Fps, Note);

            base.Render(skin);
        }
    }
}
