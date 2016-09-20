using System;
using Gwen.Control;
using Gwen.Control.Layout;

namespace Gwen.UnitTest
{
	[UnitTest(Category = "Standard", Order = 208)]
	public class Menu : GUnit
    {
		private Control.Menu m_ContextMenu;

		public Menu(ControlBase parent)
			: base(parent)
		{
			/* Menu Strip */
			{
				Control.MenuStrip menu = new Control.MenuStrip(this);
				menu.Dock = Dock.Top;

				/* File */
				{
					Control.MenuItem root = menu.AddItem("File");
					root.AddItem("Load", "test16.png", "Ctrl+L").Selected += MenuItemSelect;
					root.AddItem("Save", String.Empty, "Ctrl+S").Selected += MenuItemSelect;
					root.AddItem("Save As..", String.Empty, "Ctrl+A").Selected += MenuItemSelect;
					root.AddItem("Quit", String.Empty, "Ctrl+Q").Selected += MenuItemSelect;
				}

				/* Russian */
				{
					Control.MenuItem pRoot = menu.AddItem("\u043F\u0438\u0440\u0430\u0442\u0441\u0442\u0432\u043E");
					pRoot.AddItem("\u5355\u5143\u6D4B\u8BD5").Selected += MenuItemSelect;
					pRoot.AddItem("\u0111\u01A1n v\u1ECB th\u1EED nghi\u1EC7m", "test16.png").Selected += MenuItemSelect;
				}

				/* Embdedded Menu Items */
				{
					Control.MenuItem pRoot = menu.AddItem("Submenu");

					Control.MenuItem pCheckable = pRoot.AddItem("Checkable");
					pCheckable.IsCheckable = true;
					pCheckable.IsCheckable = true;

					{
						Control.MenuItem pRootB = pRoot.AddItem("Two");
						pRootB.AddItem("Two.One");
						pRootB.AddItem("Two.Two");
						pRootB.AddItem("Two.Three");
						pRootB.AddItem("Two.Four");
						pRootB.AddItem("Two.Five");
						pRootB.AddItem("Two.Six");
						pRootB.AddItem("Two.Seven");
						pRootB.AddItem("Two.Eight");
						pRootB.AddItem("Two.Nine", "test16.png");
					}

					pRoot.AddItem("Three");
					pRoot.AddItem("Four");
					pRoot.AddItem("Five");

					{
						Control.MenuItem pRootB = pRoot.AddItem("Six");
						pRootB.AddItem("Six.One");
						pRootB.AddItem("Six.Two");
						pRootB.AddItem("Six.Three");
						pRootB.AddItem("Six.Four");
						pRootB.AddItem("Six.Five", "test16.png");

						{
							Control.MenuItem pRootC = pRootB.AddItem("Six.Six");
							pRootC.AddItem("Sheep");
							pRootC.AddItem("Goose");
							{
								Control.MenuItem pRootD = pRootC.AddItem("Camel");
								pRootD.AddItem("Eyes");
								pRootD.AddItem("Nose");
								{
									Control.MenuItem pRootE = pRootD.AddItem("Hair");
									pRootE.AddItem("Blonde");
									pRootE.AddItem("Black");
									{
										Control.MenuItem pRootF = pRootE.AddItem("Red");
										pRootF.AddItem("Light");
										pRootF.AddItem("Medium");
										pRootF.AddItem("Dark");
									}
									pRootE.AddItem("Brown");
								}
								pRootD.AddItem("Ears");
							}
							pRootC.AddItem("Duck");
						}

						pRootB.AddItem("Six.Seven");
						pRootB.AddItem("Six.Eight");
						pRootB.AddItem("Six.Nine");
					}

					pRoot.AddItem("Seven");
				}
			}

			/* Context Menu Strip */
			{
				Control.Label lblClickMe = new Control.Label(this);
				lblClickMe.Dock = Dock.Fill;
				lblClickMe.VerticalAlignment = VerticalAlignment.Center;
				lblClickMe.Text = "Right Click Me";

				m_ContextMenu = new Control.Menu(this);
				m_ContextMenu.AddItem("Test");
				m_ContextMenu.AddItem("Clickable").Clicked += (sender2, args2) =>
				{
					UnitPrint("Clickable item was clicked");
				};

				lblClickMe.RightClicked += (sender, args) =>
				{
					m_ContextMenu.Position = this.CanvasPosToLocal(new Point(args.X, args.Y));
					m_ContextMenu.Show();
				};
			}
		}

		void MenuItemSelect(ControlBase control, EventArgs args)
        {
            MenuItem item = control as MenuItem;
            UnitPrint(String.Format("Menu item selected: {0}", item.Text));
        }
    }
}
