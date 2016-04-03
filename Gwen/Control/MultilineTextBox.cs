using System;
using System.Collections.Generic;
using System.Linq;
using Gwen.Input;
using Gwen.Control.Internal;

namespace Gwen.Control
{
	[Xml.XmlControl]
	public class MultilineTextBox : ScrollControl
	{
		private readonly Text m_Text;

		private bool m_SelectAll;

		private Point m_CursorPos;
		private Point m_CursorEnd;

		protected Rectangle m_CaretBounds;

		private float m_LastInputTime;

		private List<string> m_TextLines = new List<string>();

		private int m_LineHeight;

		private Point StartPoint
		{
			get
			{
				if (CursorPosition.Y == m_CursorEnd.Y)
				{
					return CursorPosition.X < CursorEnd.X ? CursorPosition : CursorEnd;
				}
				else {
					return CursorPosition.Y < CursorEnd.Y ? CursorPosition : CursorEnd;
				}
			}
		}

		private Point EndPoint
		{
			get
			{
				if (CursorPosition.Y == m_CursorEnd.Y)
				{
					return CursorPosition.X > CursorEnd.X ? CursorPosition : CursorEnd;
				}
				else {
					return CursorPosition.Y > CursorEnd.Y ? CursorPosition : CursorEnd;
				}
			}
		}

		/// <summary>
		/// Indicates whether the text has active selection.
		/// </summary>
		public bool HasSelection { get { return m_CursorPos != m_CursorEnd; } }

		/// <summary>
		/// Invoked when the text has changed.
		/// </summary>
		[Xml.XmlEvent]
		public event GwenEventHandler<EventArgs> TextChanged;

		/// <summary>
		/// Get a point representing where the cursor physically appears on the screen.
		/// Y is line number, X is character position on that line.
		/// </summary>
		public Point CursorPosition
		{
			get
			{
				if (m_TextLines == null || m_TextLines.Count() == 0)
					return new Point(0, 0);

				int Y = m_CursorPos.Y;
				Y = Math.Max(Y, 0);
				Y = Math.Min(Y, m_TextLines.Count() - 1);

				int X = m_CursorPos.X; //X may be beyond the last character, but we will want to draw it at the end of line.
				X = Math.Max(X, 0);
				X = Math.Min(X, m_TextLines[Y].Length);

				return new Point(X, Y);
			}
			set
			{
				m_CursorPos.X = value.X;
				m_CursorPos.Y = value.Y;
				RefreshCursorBounds();
			}
		}

		/// <summary>
		/// Get a point representing where the endpoint of text selection.
		/// Y is line number, X is character position on that line.
		/// </summary>
		public Point CursorEnd
		{
			get
			{
				if (m_TextLines == null || m_TextLines.Count() == 0)
					return new Point(0, 0);

				int Y = m_CursorEnd.Y;
				Y = Math.Max(Y, 0);
				Y = Math.Min(Y, m_TextLines.Count() - 1);

				int X = m_CursorEnd.X; //X may be beyond the last character, but we will want to draw it at the end of line.
				X = Math.Max(X, 0);
				X = Math.Min(X, m_TextLines[Y].Length);

				return new Point(X, Y);
			}
			set
			{
				m_CursorEnd.X = value.X;
				m_CursorEnd.Y = value.Y;
				RefreshCursorBounds();
			}
		}

		/// <summary>
		/// Indicates whether the control will accept Tab characters as input.
		/// </summary>
		[Xml.XmlProperty]
		public bool AcceptTabs { get; set; }

		/// <summary>
		/// Returns the number of lines that are in the Multiline Text Box.
		/// </summary>
		public int TotalLines
		{
			get
			{
				return m_TextLines.Count;
			}
		}

		protected int LineHeight
		{
			get
			{
				if (m_LineHeight == 0 && m_TextLines.Count > 0)
					m_LineHeight = Skin.Renderer.MeasureText(Font, m_TextLines[0]).Height;

				return m_LineHeight;
			}
		}
		/// <summary>
		/// Gets and sets the text to display to the user. Each line is seperated by
		/// an Environment.NetLine character.
		/// </summary>
		[Xml.XmlProperty]
		public string Text
		{
			get
			{
				return String.Join(Environment.NewLine, m_TextLines);
			}
			set
			{
				SetText(value);
			}
		}

		[Xml.XmlProperty]
		public Font Font { get { return m_Text.Font; } set { m_Text.Font = value; m_LineHeight = 0; Invalidate(); } }

		/// <summary>
		/// Initializes a new instance of the <see cref="TextBox"/> class.
		/// </summary>
		/// <param name="parent">Parent control.</param>
		public MultilineTextBox(Base parent) : base(parent)
		{
			Padding = Padding.Three;

			EnableScroll(true, true);
			AutoHideBars = true;

			MouseInputEnabled = true;
			KeyboardInputEnabled = true;

			m_CursorPos = new Point(0, 0);
			m_CursorEnd = new Point(0, 0);
			m_SelectAll = false;

			IsTabable = false;
			AcceptTabs = true;

			m_Text = new Text(this);
			m_Text.AutoSizeToContents = false;
			m_Text.TextColor = Skin.Colors.TextBox.Text;
			m_Text.BoundsChanged += new GwenEventHandler<EventArgs>(ScrollChanged);

			m_TextLines.Add(String.Empty);

			AddAccelerator("Ctrl + C", OnCopy);
			AddAccelerator("Ctrl + X", OnCut);
			AddAccelerator("Ctrl + V", OnPaste);
			AddAccelerator("Ctrl + A", OnSelectAll);

			m_LineHeight = 0;
		}

		/// <summary>
		/// Sets the label text.
		/// </summary>
		/// <param name="str">Text to set.</param>
		/// <param name="doEvents">Determines whether to invoke "text changed" event.</param>
		public void SetText(string str, bool doEvents = true)
		{
			m_TextLines = new List<string>(str.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n'));

			UpdateText();
			RefreshCursorBounds();

			if (doEvents)
				OnTextChanged();
		}

		/// <summary>
		/// Inserts text at current cursor position, erasing selection if any.
		/// </summary>
		/// <param name="text">Text to insert.</param>
		public void InsertText(string text)
		{
			if (HasSelection)
			{
				EraseSelection();
			}

			if (text.Contains("\r") || text.Contains("\n"))
			{
				string[] newLines = text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');

				Point cursorPos = CursorPosition;
				string oldLineStart = m_TextLines[cursorPos.Y].Substring(0, cursorPos.X);
				string oldLineEnd = m_TextLines[cursorPos.Y].Substring(cursorPos.X);

				if (newLines.Length > 0)
				{
					m_TextLines[cursorPos.Y] = oldLineStart + newLines[0]; // First line
					for (int i = 1; i < newLines.Length - 1; i++)
					{
						m_TextLines.Insert(cursorPos.Y + i, newLines[i]); // Middle lines
					}
					m_TextLines.Insert(cursorPos.Y + newLines.Length - 1, newLines[newLines.Length - 1] + oldLineEnd); // Last line

					m_CursorPos.X = newLines[newLines.Length - 1].Length;
					m_CursorPos.Y = cursorPos.Y + newLines.Length - 1;
					m_CursorEnd = m_CursorPos;
				}
			}
			else
			{
				string str = m_TextLines[m_CursorPos.Y];
				str = str.Insert(CursorPosition.X, text);
				m_TextLines[m_CursorPos.Y] = str;

				m_CursorPos.X = CursorPosition.X + text.Length;
				m_CursorEnd = m_CursorPos;
			}

			UpdateText();
			OnTextChanged();
			RefreshCursorBounds();
		}

		/// <summary>
		/// Handler invoked on mouse click (left) event.
		/// </summary>
		/// <param name="x">X coordinate.</param>
		/// <param name="y">Y coordinate.</param>
		/// <param name="down">If set to <c>true</c> mouse button is down.</param>
		protected override void OnMouseClickedLeft(int x, int y, bool down)
		{
			base.OnMouseClickedLeft(x, y, down);
			if (m_SelectAll)
			{
				OnSelectAll(this, EventArgs.Empty);
				//m_SelectAll = false;
				return;
			}

			Point coords = GetClosestCharacter(x, y);

			if (down)
			{
				CursorPosition = coords;

				if (!Input.InputHandler.IsShiftDown)
					CursorEnd = coords;

				InputHandler.MouseFocus = this;
			}
			else {
				if (InputHandler.MouseFocus == this)
				{
					CursorPosition = coords;
					InputHandler.MouseFocus = null;
				}
			}

			RefreshCursorBounds();
		}

		/// <summary>
		/// Handler invoked on mouse double click (left) event.
		/// </summary>
		/// <param name="x">X coordinate.</param>
		/// <param name="y">Y coordinate.</param>
		protected override void OnMouseDoubleClickedLeft(int x, int y)
		{
			//base.OnMouseDoubleClickedLeft(x, y);
			OnSelectAll(this, EventArgs.Empty);
		}

		/// <summary>
		/// Handler invoked on mouse moved event.
		/// </summary>
		/// <param name="x">X coordinate.</param>
		/// <param name="y">Y coordinate.</param>
		/// <param name="dx">X change.</param>
		/// <param name="dy">Y change.</param>
		protected override void OnMouseMoved(int x, int y, int dx, int dy)
		{
			base.OnMouseMoved(x, y, dx, dy);
			if (InputHandler.MouseFocus != this) return;

			Point c = GetClosestCharacter(x, y);

			CursorPosition = c;

			RefreshCursorBounds();
		}

		/// <summary>
		/// Handler for character input event.
		/// </summary>
		/// <param name="chr">Character typed.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnChar(char chr)
		{
			//base.OnChar(chr);
			if (chr == '\t' && !AcceptTabs) return false;

			InsertText(chr.ToString());
			return true;
		}

		/// <summary>
		/// Handler for Paste event.
		/// </summary>
		/// <param name="from">Source control.</param>
		protected override void OnPaste(Base from, EventArgs args)
		{
			base.OnPaste(from, args);
			InsertText(Platform.Platform.GetClipboardText());
		}

		/// <summary>
		/// Handler for Copy event.
		/// </summary>
		/// <param name="from">Source control.</param>
		protected override void OnCopy(Base from, EventArgs args)
		{
			if (!HasSelection) return;
			base.OnCopy(from, args);

			Platform.Platform.SetClipboardText(GetSelection());
		}

		/// <summary>
		/// Handler for Cut event.
		/// </summary>
		/// <param name="from">Source control.</param>
		protected override void OnCut(Base from, EventArgs args)
		{
			if (!HasSelection) return;
			base.OnCut(from, args);

			Platform.Platform.SetClipboardText(GetSelection());
			EraseSelection();
		}


		/// <summary>
		/// Handler for Select All event.
		/// </summary>
		/// <param name="from">Source control.</param>
		protected override void OnSelectAll(Base from, EventArgs args)
		{
			//base.OnSelectAll(from);
			m_CursorEnd = new Point(0, 0);
			m_CursorPos = new Point(m_TextLines.Last().Length, m_TextLines.Count());

			RefreshCursorBounds();
		}

		/// <summary>
		/// Handler for Return keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyReturn(bool down)
		{
			if (down) return true;

			//Split current string, putting the rhs on a new line
			string CurrentLine = m_TextLines[m_CursorPos.Y];
			string lhs = CurrentLine.Substring(0, CursorPosition.X);
			string rhs = CurrentLine.Substring(CursorPosition.X);

			m_TextLines[m_CursorPos.Y] = lhs;
			m_TextLines.Insert(m_CursorPos.Y + 1, rhs);

			OnKeyDown(true);
			OnKeyHome(true);

			if (m_CursorPos.Y == TotalLines - 1)
			{
				ScrollToBottom();
			}

			UpdateText();
			OnTextChanged();
			RefreshCursorBounds();

			return true;
		}

		/// <summary>
		/// Handler for Backspace keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyBackspace(bool down)
		{
			if (!down) return true;

			if (HasSelection)
			{
				EraseSelection();
				return true;
			}

			if (m_CursorPos.X == 0)
			{
				if (m_CursorPos.Y == 0)
				{
					return true; //Nothing left to delete
				}
				else {
					string lhs = m_TextLines[m_CursorPos.Y - 1];
					string rhs = m_TextLines[m_CursorPos.Y];
					m_TextLines.RemoveAt(m_CursorPos.Y);
					OnKeyUp(true);
					OnKeyEnd(true);
					m_TextLines[m_CursorPos.Y] = lhs + rhs;
				}
			}
			else {
				string CurrentLine = m_TextLines[m_CursorPos.Y];
				string lhs = CurrentLine.Substring(0, CursorPosition.X - 1);
				string rhs = CurrentLine.Substring(CursorPosition.X);
				m_TextLines[m_CursorPos.Y] = lhs + rhs;
				OnKeyLeft(true);
			}

			UpdateText();
			OnTextChanged();
			RefreshCursorBounds();

			return true;
		}

		/// <summary>
		/// Handler for Delete keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyDelete(bool down)
		{
			if (!down) return true;

			if (HasSelection)
			{
				EraseSelection();
				return true;
			}

			if (m_CursorPos.X == m_TextLines[m_CursorPos.Y].Length)
			{
				if (m_CursorPos.Y == m_TextLines.Count - 1)
				{
					return true; //Nothing left to delete
				}
				else {
					string lhs = m_TextLines[m_CursorPos.Y];
					string rhs = m_TextLines[m_CursorPos.Y + 1];
					m_TextLines.RemoveAt(m_CursorPos.Y + 1);
					OnKeyEnd(true);
					m_TextLines[m_CursorPos.Y] = lhs + rhs;
				}
			}
			else {
				string CurrentLine = m_TextLines[m_CursorPos.Y];
				string lhs = CurrentLine.Substring(0, CursorPosition.X);
				string rhs = CurrentLine.Substring(CursorPosition.X + 1);
				m_TextLines[m_CursorPos.Y] = lhs + rhs;
			}

			UpdateText();
			OnTextChanged();
			RefreshCursorBounds();

			return true;
		}

		/// <summary>
		/// Handler for Up Arrow keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyUp(bool down)
		{
			if (!down) return true;

			if (m_CursorPos.Y > 0)
			{
				m_CursorPos.Y -= 1;
			}

			if (!Input.InputHandler.IsShiftDown)
			{
				m_CursorEnd = m_CursorPos;
			}

			RefreshCursorBounds();

			return true;
		}

		/// <summary>
		/// Handler for Down Arrow keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyDown(bool down)
		{
			if (!down) return true;

			if (m_CursorPos.Y < TotalLines - 1)
			{
				m_CursorPos.Y += 1;
			}

			if (!Input.InputHandler.IsShiftDown)
			{
				m_CursorEnd = m_CursorPos;
			}

			RefreshCursorBounds();

			return true;
		}

		/// <summary>
		/// Handler for Left Arrow keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyLeft(bool down)
		{
			if (!down) return true;

			if (m_CursorPos.X > 0)
			{
				m_CursorPos.X = Math.Min(m_CursorPos.X - 1, m_TextLines[m_CursorPos.Y].Length);
			}
			else {
				if (m_CursorPos.Y > 0)
				{
					OnKeyUp(down);
					OnKeyEnd(down);
				}
			}

			if (!Input.InputHandler.IsShiftDown)
			{
				m_CursorEnd = m_CursorPos;
			}

			RefreshCursorBounds();

			return true;
		}

		/// <summary>
		/// Handler for Right Arrow keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyRight(bool down)
		{
			if (!down) return true;

			if (m_CursorPos.X < m_TextLines[m_CursorPos.Y].Length)
			{
				m_CursorPos.X = Math.Min(m_CursorPos.X + 1, m_TextLines[m_CursorPos.Y].Length);
			}
			else {
				if (m_CursorPos.Y < m_TextLines.Count - 1)
				{
					OnKeyDown(down);
					OnKeyHome(down);
				}
			}

			if (!Input.InputHandler.IsShiftDown)
			{
				m_CursorEnd = m_CursorPos;
			}

			RefreshCursorBounds();

			return true;
		}

		/// <summary>
		/// Handler for Home Key keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyHome(bool down)
		{
			if (!down) return true;

			m_CursorPos.X = 0;

			if (!Input.InputHandler.IsShiftDown)
			{
				m_CursorEnd = m_CursorPos;
			}

			RefreshCursorBounds();

			return true;
		}

		/// <summary>
		/// Handler for End Key keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyEnd(bool down)
		{
			if (!down) return true;

			m_CursorPos.X = m_TextLines[m_CursorPos.Y].Length;

			if (!Input.InputHandler.IsShiftDown)
			{
				m_CursorEnd = m_CursorPos;
			}

			UpdateText();
			RefreshCursorBounds();

			return true;
		}

		/// <summary>
		/// Handler for Tab Key keyboard event.
		/// </summary>
		/// <param name="down">Indicates whether the key was pressed or released.</param>
		/// <returns>
		/// True if handled.
		/// </returns>
		protected override bool OnKeyTab(bool down)
		{
			if (!AcceptTabs) return base.OnKeyTab(down);
			if (!down) return false;

			OnChar('\t');
			return true;
		}

		/// <summary>
		/// Returns currently selected text.
		/// </summary>
		/// <returns>Current selection.</returns>
		public string GetSelection()
		{
			if (!HasSelection) return String.Empty;

			string str = String.Empty;

			if (StartPoint.Y == EndPoint.Y)
			{
				int start = StartPoint.X;
				int end = EndPoint.X;

				str = m_TextLines[m_CursorPos.Y];
				str = str.Substring(start, end - start);
			}
			else {
				str = m_TextLines[StartPoint.Y].Substring(StartPoint.X) + Environment.NewLine; //Copy start
				for (int i = 1; i < EndPoint.Y - StartPoint.Y; i++)
				{
					str += m_TextLines[StartPoint.Y + i] + Environment.NewLine; //Copy middle
				}
				str += m_TextLines[EndPoint.Y].Substring(0, EndPoint.X); //Copy end
			}

			return str;
		}

		//[halfofastaple] TODO Implement this and use it. The end user can work around not having it, but it is terribly convenient.
		//	See the delete key handler for help. Eventually, the delete key should use this.
		///// <summary>
		///// Deletes text.
		///// </summary>
		///// <param name="startPos">Starting cursor position.</param>
		///// <param name="length">Length in characters.</param>
		//public void DeleteText(Point StartPos, int length) {
		//    /* Single Line Delete */
		//    if (StartPos.X + length <= m_TextLines[StartPos.Y].Length) {
		//        string str = m_TextLines[StartPos.Y];
		//        str = str.Remove(StartPos.X, length);
		//        m_TextLines[StartPos.Y] = str;

		//        if (CursorPosition.X > StartPos.X) {
		//            m_CursorPos.X = CursorPosition.X - length;
		//        }

		//        m_CursorEnd = m_CursorPos;
		//    /* Multiline Delete */
		//    } else {

		//    }
		//}

		/// <summary>
		/// Deletes selected text.
		/// </summary>
		public void EraseSelection()
		{
			if (StartPoint.Y == EndPoint.Y)
			{
				int start = StartPoint.X;
				int end = EndPoint.X;

				m_TextLines[StartPoint.Y] = m_TextLines[StartPoint.Y].Remove(start, end - start);
			}
			else {
				Point startPoint = StartPoint;
				Point endPoint = EndPoint;

				/* Remove Start */
				if (startPoint.X < m_TextLines[startPoint.Y].Length)
				{
					m_TextLines[startPoint.Y] = m_TextLines[startPoint.Y].Remove(startPoint.X);
				}

				/* Remove Middle */
				for (int i = 1; i < endPoint.Y - startPoint.Y; i++)
				{
					m_TextLines.RemoveAt(startPoint.Y + 1);
				}

				/* Remove End */
				if (endPoint.X < m_TextLines[startPoint.Y + 1].Length)
				{
					m_TextLines[startPoint.Y] += m_TextLines[startPoint.Y + 1].Substring(endPoint.X);
				}
				m_TextLines.RemoveAt(startPoint.Y + 1);
			}

			// Move the cursor to the start of the selection, 
			// since the end is probably outside of the string now.
			m_CursorPos = StartPoint;
			m_CursorEnd = StartPoint;

			UpdateText();
			OnTextChanged();
			RefreshCursorBounds();
		}

		/// <summary>
		/// Refreshes the cursor location and selected area when the inner panel scrolls
		/// </summary>
		/// <param name="control">The inner panel the text is embedded in</param>
		private void ScrollChanged(Base control, EventArgs args)
		{
			RefreshCursorBounds(false);
		}

		/// <summary>
		/// Handler for text changed event.
		/// </summary>
		protected void OnTextChanged()
		{
			if (TextChanged != null)
				TextChanged.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Invalidates the control.
		/// </summary>
		/// <remarks>
		/// Causes layout, repaint, invalidates cached texture.
		/// </remarks>
		private void UpdateText()
		{
			if (m_Text != null)
			{
				m_Text.String = Text;
			}

			Invalidate();
			return;
		}

		/// <summary>
		/// Renders the control using specified skin.
		/// </summary>
		/// <param name="skin">Skin to use.</param>
		protected override void Render(Skin.Base skin)
		{
			base.Render(skin);

			if (ShouldDrawBackground)
				skin.DrawTextBox(this);

			if (!HasFocus) return;

			Rectangle oldClipRegion = skin.Renderer.ClipRegion;

			skin.Renderer.SetClipRegion(this.Container.Bounds);

			int verticalSize = this.LineHeight;

			// Draw selection.. if selected..
			if (m_CursorPos != m_CursorEnd)
			{
				if (StartPoint.Y == EndPoint.Y)
				{
					Point pA = GetCharacterPosition(StartPoint);
					Point pB = GetCharacterPosition(EndPoint);

					Rectangle SelectionBounds = new Rectangle();
					SelectionBounds.X = Math.Min(pA.X, pB.X);
					SelectionBounds.Y = pA.Y;
					SelectionBounds.Width = Math.Max(pA.X, pB.X) - SelectionBounds.X;
					SelectionBounds.Height = verticalSize;

					skin.Renderer.DrawColor = Skin.Colors.TextBox.Background_Selected;
					skin.Renderer.DrawFilledRect(SelectionBounds);
				}
				else {
					/* Start */
					Point pA = GetCharacterPosition(StartPoint);
					Point pB = GetCharacterPosition(new Point(m_TextLines[StartPoint.Y].Length, StartPoint.Y));

					Rectangle SelectionBounds = new Rectangle();
					SelectionBounds.X = Math.Min(pA.X, pB.X);
					SelectionBounds.Y = pA.Y;
					SelectionBounds.Width = Math.Max(pA.X, pB.X) - SelectionBounds.X;
					SelectionBounds.Height = verticalSize;

					skin.Renderer.DrawColor = Skin.Colors.TextBox.Background_Selected;
					skin.Renderer.DrawFilledRect(SelectionBounds);

					/* Middle */
					for (int i = 1; i < EndPoint.Y - StartPoint.Y; i++)
					{
						pA = GetCharacterPosition(new Point(0, StartPoint.Y + i));
						pB = GetCharacterPosition(new Point(m_TextLines[StartPoint.Y + i].Length, StartPoint.Y + i));

						SelectionBounds = new Rectangle();
						SelectionBounds.X = Math.Min(pA.X, pB.X);
						SelectionBounds.Y = pA.Y;
						SelectionBounds.Width = Math.Max(pA.X, pB.X) - SelectionBounds.X;
						SelectionBounds.Height = verticalSize;

						skin.Renderer.DrawColor = Skin.Colors.TextBox.Background_Selected;
						skin.Renderer.DrawFilledRect(SelectionBounds);
					}

					/* End */
					pA = GetCharacterPosition(new Point(0, EndPoint.Y));
					pB = GetCharacterPosition(EndPoint);

					SelectionBounds = new Rectangle();
					SelectionBounds.X = Math.Min(pA.X, pB.X);
					SelectionBounds.Y = pA.Y;
					SelectionBounds.Width = Math.Max(pA.X, pB.X) - SelectionBounds.X;
					SelectionBounds.Height = verticalSize;

					skin.Renderer.DrawColor = Skin.Colors.TextBox.Background_Selected;
					skin.Renderer.DrawFilledRect(SelectionBounds);
				}
			}

			// Draw caret
			float time = Platform.Platform.GetTimeInSeconds() - m_LastInputTime;

			if ((time % 1.0f) <= 0.5f)
			{
				skin.Renderer.DrawColor = Skin.Colors.TextBox.Caret;
				skin.Renderer.DrawFilledRect(m_CaretBounds);
			}

			skin.Renderer.ClipRegion = oldClipRegion;
		}

		private Point GetCharacterPosition(Point cursorPosition)
		{
			if (m_TextLines.Count == 0)
			{
				return new Point(0, 0);
			}
			string currLine = m_TextLines[cursorPosition.Y].Substring(0, Math.Min(cursorPosition.X, m_TextLines[cursorPosition.Y].Length));

			string sub = "";
			for (int i = 0; i < cursorPosition.Y; i++)
			{
				sub += m_TextLines[i] + "\n";
			}

			Point p = new Point(Skin.Renderer.MeasureText(Font, currLine).Width, Skin.Renderer.MeasureText(Font, sub).Height);

			return new Point(p.X + m_Text.ActualLeft + Padding.Left, p.Y + m_Text.ActualTop + Padding.Top);
		}

		/// <summary>
		/// Returns index of the character closest to specified point (in canvas coordinates).
		/// </summary>
		/// <param name="px"></param>
		/// <param name="py"></param>
		/// <returns></returns>
		protected Point GetClosestCharacter(int px, int py)
		{
			Point p = m_Text.CanvasPosToLocal(new Point(px, py));
			Size cp;
			double distance = Double.MaxValue;
			Point best = new Point(0, 0);
			string sub = String.Empty;

			/* Find the appropriate Y row (always pick whichever y the mouse currently is on) */
			sub = m_TextLines[0];
			for (int y = 1; y < m_TextLines.Count; y++)
			{
				cp = Skin.Renderer.MeasureText(Font, sub);
				if (p.Y < cp.Height)
					break;
				else
					best.Y = y;
				sub += Environment.NewLine + m_TextLines[y];
			}

			/* Find the best X row, closest char */
			sub = String.Empty;
			distance = Double.MaxValue;
			cp = Size.Zero;
			for (int x = 0; x <= m_TextLines[best.Y].Length; x++)
			{
				double xDist = Math.Abs(cp.Width - p.X);
				if (xDist < distance)
				{
					distance = xDist;
					best.X = x;
				}

				if (x < m_TextLines[best.Y].Length)
					sub += m_TextLines[best.Y][x];
				else
					sub += " ";

				cp = Skin.Renderer.MeasureText(Font, sub);
			}

			return best;
		}

		protected void RefreshCursorBounds(bool makeCaretVisible = true)
		{
			m_LastInputTime = Platform.Platform.GetTimeInSeconds();

			if (makeCaretVisible)
				MakeCaretVisible();

			Point pA = GetCharacterPosition(CursorPosition);

			m_CaretBounds.X = pA.X;
			m_CaretBounds.Y = pA.Y;

			m_CaretBounds.Width = 1;
			m_CaretBounds.Height = this.LineHeight;

			Redraw();
		}

		protected virtual void MakeCaretVisible()
		{
			Size viewSize = ViewableContentSize;
			Point caretPos = GetCharacterPosition(CursorPosition);

			caretPos.X -= Padding.Left + m_Text.ActualLeft;
			caretPos.Y -= Padding.Top + m_Text.ActualTop;

			EnsureVisible(new Rectangle(caretPos.X, caretPos.Y, 5, LineHeight), new Size(viewSize.Width / 5, 0));
		}
	}
}
