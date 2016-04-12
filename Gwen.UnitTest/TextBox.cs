using System;
using Gwen.Control;
using Gwen.Control.Layout;

namespace Gwen.UnitTest
{
	[UnitTest(Category = "Standard", Order = 201)]
	public class TextBox : GUnit
    {
        private readonly Font m_Font1;
        private readonly Font m_Font2;
        private readonly Font m_Font3;

        public TextBox(ControlBase parent)
            : base(parent)
		{
			m_Font1 = Skin.DefaultFont.Copy();
			m_Font1.FaceName = "Courier New"; // fixed width font!

			m_Font2 = Skin.DefaultFont.Copy();
			m_Font2.FaceName = "Times New Roman";
			m_Font2.Size *= 3;

			m_Font3 = Skin.DefaultFont.Copy();
			m_Font3.Size += 5;

			VerticalLayout vlayout = new VerticalLayout(this);
			{
				HorizontalLayout hlayout = new HorizontalLayout(vlayout);
				{
					VerticalLayout vlayout2 = new VerticalLayout(hlayout);
					vlayout2.Width = 200;
					{
						/* Vanilla Textbox */
						{
							Control.TextBox textbox = new Control.TextBox(vlayout2);
							textbox.Margin = Margin.Five;
							textbox.SetText("Type something here");
							textbox.TextChanged += OnEdit;
							textbox.SubmitPressed += OnSubmit;
						}

						{
							Control.TextBoxPassword textbox = new Control.TextBoxPassword(vlayout2);
							textbox.Margin = Margin.Five;
							//textbox.MaskCharacter = '@';
							textbox.SetText("secret");
							textbox.TextChanged += OnEdit;
						}

						{
							Control.TextBox textbox = new Control.TextBox(vlayout2);
							textbox.Margin = Margin.Five;
							textbox.SetText("Select All Text On Focus");
							textbox.SelectAllOnFocus = true;
						}

						{
							Control.TextBox textbox = new Control.TextBox(vlayout2);
							textbox.Margin = Margin.Five;
							textbox.SetText("Different Coloured Text, for some reason");
							textbox.TextColor = Color.Green;
						}

						{
							Control.TextBox textbox = new Control.TextBoxNumeric(vlayout2);
							textbox.Margin = Margin.Five;
							textbox.SetText("200456698");
							textbox.TextColor = Color.Red;
						}
					}

					/* Multiline Textbox */
					{
						Control.MultilineTextBox textbox = new Control.MultilineTextBox(hlayout);
						textbox.Margin = Margin.Five;
						textbox.Size = new Size(500, 150);
						textbox.Font = m_Font1;
						textbox.AcceptTabs = true;
						textbox.SetText("In olden times when wishing still helped one, there lived a king whose daughters were all beautiful,\nbut the youngest was so beautiful that the sun itself, which has seen so much, \nwas astonished whenever it shone in her face. \nClose by the king's castle lay a great dark forest, \nand under an old lime-tree in the forest was a well, and when the day was very warm, \nthe king's child went out into the forest and sat down by the side of the cool fountain, \nand when she was bored she took a golden ball, and threw it up on high and caught it, \nand this ball was her favorite plaything.");
					}
				}

				{
					Control.TextBox textbox = new Control.TextBox(vlayout);
					textbox.Margin = Margin.Five;
					textbox.SetText("In olden times when wishing still helped one, there lived a king whose daughters were all beautiful, but the youngest was so beautiful that the sun itself, which has seen so much, was astonished whenever it shone in her face. Close by the king's castle lay a great dark forest, and under an old lime-tree in the forest was a well, and when the day was very warm, the king's child went out into the forest and sat down by the side of the cool fountain, and when she was bored she took a golden ball, and threw it up on high and caught it, and this ball was her favorite plaything.");
					textbox.TextColor = Color.Black;
					textbox.Font = m_Font3;
				}

				{
					Control.TextBox textbox = new Control.TextBox(vlayout);
					textbox.Margin = Margin.Five;
					textbox.Width = 150;
					textbox.HorizontalAlignment = HorizontalAlignment.Right;
					textbox.SetText("あおい　うみから　やってきた");
					textbox.TextColor = Color.Black;
					textbox.Font = m_Font3;
				}

				{
					Control.TextBox textbox = new Control.TextBox(vlayout);
					textbox.Margin = Margin.Five;
					textbox.HorizontalAlignment = HorizontalAlignment.Left;
					textbox.FitToText = "Fit the text";
					textbox.SetText("FitToText");
					textbox.TextColor = Color.Black;
					textbox.Font = m_Font3;
				}

				{
					Control.TextBox textbox = new Control.TextBox(vlayout);
					textbox.Margin = Margin.Five;
					textbox.HorizontalAlignment = HorizontalAlignment.Left;
					textbox.Width = 200;
					textbox.SetText("Width = 200");
					textbox.TextColor = Color.Black;
					textbox.Font = m_Font3;
				}

				{
					Control.TextBox textbox = new Control.TextBox(vlayout);
					textbox.Margin = Margin.Five;
					textbox.SetText("Different Font");
					textbox.Font = m_Font2;
				}
			}
		}

        public override void Dispose()
        {
            m_Font1.Dispose();
            m_Font2.Dispose();
            m_Font3.Dispose();
            base.Dispose();
        }

		void OnEdit(ControlBase control, EventArgs args)
        {
            Control.TextBox box = control as Control.TextBox;
            UnitPrint(String.Format("TextBox: OnEdit: {0}", box.Text));
        }

		void OnSubmit(ControlBase control, EventArgs args)
        {
            Control.TextBox box = control as Control.TextBox;
            UnitPrint(String.Format("TextBox: OnSubmit: {0}", box.Text));
        }
    }
}
