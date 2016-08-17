using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace Gwen.Control
{
	/// <summary>
	/// Base class for multi-column tables.
	/// </summary>
	public class Table : ControlBase
	{
		public delegate TableRow CreateRow();

		private bool m_AutoSizeToContent;
		private bool m_SizeToContents;
		private bool m_AlternateColor;
		private int m_ColumnCount;
		private int m_MaxWidth; // for autosizing, if nonzero - fills last cell up to this size

		private int[] m_ColumnWidth;

		private IEnumerable m_ItemsSource;
		private string m_DisplayMember;
		private string[] m_DisplayMembers;

		private CreateRow m_CreateRow;

		/// <summary>
		/// Column count (default 1).
		/// </summary>
		public int ColumnCount { get { return m_ColumnCount; } set { SetColumnCount(value); Invalidate(); } }

		/// <summary>
		/// Row count.
		/// </summary>
		public int RowCount { get { return Children.Count; } }

		/// <summary>
		/// Adjust the size of the control to fit all rows and columns.
		/// </summary>
		public bool AutoSizeToContent { get { return m_AutoSizeToContent; } set { m_AutoSizeToContent = value; } }

		/// <summary>
		/// Alternate row background colors.
		/// </summary>
		public bool AlternateColor { get { return m_AlternateColor; } set { m_AlternateColor = value; } }

		/// <summary>
		/// Collection of items. If the collection implements the INotifyCollectionChanged interface, items will be added and removed when the collection changes.
		/// </summary>
		public IEnumerable ItemsSource { get { return m_ItemsSource; } set { SetItemsSource(value); } }

		/// <summary>
		/// Property name of the item to display. Table will be one column table.
		/// </summary>
		public string DisplayMember { get { return m_DisplayMember; } set { m_DisplayMember = value; m_DisplayMembers = new string[] { value }; InitCollection(); } }

		/// <summary>
		/// Property names of the item to display. Each name represent a column on the table.
		/// </summary>
		public string[] DisplayMembers { get { return m_DisplayMembers; } set { m_DisplayMembers = value; InitCollection(); } }

		/// <summary>
		/// Returns specific row of the table.
		/// </summary>
		/// <param name="index">Row index.</param>
		/// <returns>Row at the specified index.</returns>
		public TableRow this[int index] { get { return Children[index] as TableRow; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Table"/> class.
		/// </summary>
		/// <param name="parent">Parent control.</param>
		public Table(ControlBase parent, CreateRow createRow = null) : base(parent)
		{
			m_ColumnCount = 1;

			m_ColumnWidth = new int[m_ColumnCount];

			m_AutoSizeToContent = false;
			m_SizeToContents = false;

			if (createRow == null)
				m_CreateRow = CreateRowDefault;
			else
				m_CreateRow = createRow;
		}

		/// <summary>
		/// Set collection of items. If the collection implements the INotifyCollectionChanged interface, items will be added and removed when the collection changes.
		/// </summary>
		/// <param name="itemsSource">Items.</param>
		public void SetItemsSource(IEnumerable itemsSource)
		{
			if (m_ItemsSource == itemsSource)
				return;

			if (m_ItemsSource != null && m_ItemsSource is INotifyCollectionChanged)
			{
				((INotifyCollectionChanged)m_ItemsSource).CollectionChanged -= OnCollectionChanged;
			}

			m_ItemsSource = itemsSource;

			if (m_ItemsSource != null)
			{
				if (m_ItemsSource is INotifyCollectionChanged)
				{
					((INotifyCollectionChanged)m_ItemsSource).CollectionChanged += OnCollectionChanged;
				}

				InitCollection();
			}
			else
			{
				Clear();
			}
		}

		/// <summary>
		/// Sets the number of columns.
		/// </summary>
		/// <param name="columnCount">Number of columns.</param>
		public void SetColumnCount(int columnCount)
		{
			if (m_ColumnCount == columnCount)
				return;

			int[] newColumnWidth = new int[columnCount];

			for (int i = 0; i < m_ColumnCount; i++)
			{
				if (i < columnCount)
				{
					newColumnWidth[i] = m_ColumnWidth[i];
				}
			}

			foreach (TableRow row in Children.OfType<TableRow>())
			{
				row.ColumnCount = columnCount;
			}

			m_ColumnWidth = newColumnWidth;
			m_ColumnCount = columnCount;

			IsDirty = true;
			Invalidate();
		}

		/// <summary>
		/// Sets the column width (in pixels).
		/// </summary>
		/// <param name="columnIndex">Column index.</param>
		/// <param name="width">Column width.</param>
		public void SetColumnWidth(int columnIndex, int width)
		{
			if (columnIndex >= m_ColumnCount)
				throw new ArgumentOutOfRangeException("columnIndex");

			if (m_ColumnWidth[columnIndex] == width)
				return;

			m_ColumnWidth[columnIndex] = width;
			IsDirty = true;
			Invalidate();
		}

		/// <summary>
		/// Gets the column width (in pixels).
		/// </summary>
		/// <param name="columnIndex">Column index.</param>
		/// <returns>Column width.</returns>
		public int GetColumnWidth(int columnIndex)
		{
			if (columnIndex >= m_ColumnCount)
				throw new ArgumentOutOfRangeException("columnIndex");

			return m_ColumnWidth[columnIndex];
		}

		private TableRow CreateRowDefault()
		{
			TableRow row = new TableRow(this);
			row.ColumnCount = m_ColumnCount;
			IsDirty = true;
			Invalidate();
			return row;
		}

		/// <summary>
		/// Adds a new empty row.
		/// </summary>
		/// <returns>Newly created row.</returns>
		public TableRow AddRow()
		{
			return m_CreateRow();
		}

		/// <summary>
		/// Adds a row.
		/// </summary>
		/// <param name="row">Row to add.</param>
		public void AddRow(TableRow row)
		{
			row.Parent = this;
			row.ColumnCount = m_ColumnCount;
			IsDirty = true;
			Invalidate();
		}

		/// <summary>
		/// Adds a new row with specified text in first column.
		/// </summary>
		/// <param name="text">Text to add.</param>
		/// <param name="name">Internal control name.</param>
		/// <param name="userData">User data for newly created row.</param>
		/// <returns>New row.</returns>
		public TableRow AddRow(string text, string name = "", object userData = null)
		{
			var row = m_CreateRow();
			row.SetCellText(0, text);
			row.Name = name;
			row.UserData = userData;
			IsDirty = true;
			Invalidate();
			return row;
		}

		/// <summary>
		/// Adds a new row with specified item.
		/// </summary>
		/// <param name="item">Item to add.</param>
		/// <returns>New row.</returns>
		public TableRow AddRow(object item)
		{
			if (item == null)
				return null;

			TableRow row = m_CreateRow();

			if (m_DisplayMembers == null || m_DisplayMembers.Length == 0)
			{
				string col = item.ToString();
				row.Name = col;
				row.UserData = item;
			}
			else
			{
				string col = GetPropertyValue(item, m_DisplayMembers[0]);
				row.Name = col;
				row.UserData = item;
				row.SetCellText(0, col);

				for (int i = 1; i < m_DisplayMembers.Length; i++)
				{
					row.SetCellText(i, GetPropertyValue(item, m_DisplayMembers[i]));
				}
			}

			IsDirty = true;
			Invalidate();

			return row;
		}

		/// <summary>
		/// Insert a new empty row to specified index.
		/// </summary>
		/// <param name="index">Index where to insert.</param>
		/// <returns>New row.</returns>
		public TableRow InsertRow(int index)
		{
			if (index < 0 || index > RowCount)
				throw new ArgumentOutOfRangeException("index");

			TableRow row = AddRow();

			row.MoveChildToIndex(index);

			return row;
		}

		/// <summary>
		/// Insert a row to specified index.
		/// </summary>
		/// <param name="index">Index where to insert.</param>
		/// <param name="row">Row.</param>
		public void InsertRow(int index, TableRow row)
		{
			if (index < 0 || index > RowCount)
				throw new ArgumentOutOfRangeException("index");

			AddRow(row);

			row.MoveChildToIndex(index);
		}

		/// <summary>
		/// Insert a new row with specified text in first column to specified index.
		/// </summary>
		/// <param name="index">Index where to insert.</param>
		/// <param name="text">Text to add.</param>
		/// <param name="name">Internal control name.</param>
		/// <param name="userData">User data for newly created row.</param>
		/// <returns>New row.</returns>
		public TableRow InsertRow(int index, string text, string name = "", object userData = null)
		{
			if (index < 0 || index > RowCount)
				throw new ArgumentOutOfRangeException("index");

			TableRow row = AddRow(text, name, userData);

			row.MoveChildToIndex(index);

			return row;
		}

		/// <summary>
		/// Insert a new row with specified item to specified index.
		/// </summary>
		/// <param name="index">Index where to insert.</param>
		/// <param name="item">Item to add.</param>
		/// <returns>New row.</returns>
		public TableRow InsertRow(int index, object item)
		{
			if (index < 0 || index > RowCount)
				throw new ArgumentOutOfRangeException("index");

			TableRow row = AddRow(item);

			row.MoveChildToIndex(index);

			return row;
		}

		/// <summary>
		/// Removes a row by reference.
		/// </summary>
		/// <param name="row">Row to remove.</param>
		public void RemoveRow(TableRow row)
		{
			RemoveChild(row, true);
			IsDirty = true;
			Invalidate();
		}

		/// <summary>
		/// Removes a row by index.
		/// </summary>
		/// <param name="idx">Row index.</param>
		public void RemoveRow(int idx)
		{
			var row = Children[idx];
			RemoveRow(row as TableRow);
		}

		/// <summary>
		/// Remove row by item.
		/// </summary>
		/// <param name="item">Item to remove.</param>
		public void RemoveRow(object item)
		{
			TableRow removeRow = null;

			foreach (TableRow row in Children)
			{
				if (row.UserData == item)
					removeRow = row;
			}

			if (removeRow != null)
				RemoveRow(removeRow);
		}

		/// <summary>
		/// Removes all rows.
		/// </summary>
		public void Clear()
		{
			while (RowCount > 0)
				RemoveRow(0);
		}

		/// <summary>
		/// Gets the index of a specified row.
		/// </summary>
		/// <param name="row">Row to search for.</param>
		/// <returns>Row index if found, -1 otherwise.</returns>
		public int GetRowIndex(TableRow row)
		{
			return Children.IndexOf(row);
		}

		/// <summary>
		/// Sizes to fit contents.
		/// </summary>
		public void SizeToContent(int maxWidth = 0)
		{
			m_MaxWidth = maxWidth;
			m_SizeToContents = true;
			Invalidate();
		}

		protected override Size OnMeasure(Size availableSize)
		{
			if (IsDirty && (m_AutoSizeToContent || m_SizeToContents))
			{
				m_SizeToContents = false;

				int height = 0;
				int width = 0;

				int[] columnWidth = new int[m_ColumnCount];

				// Measure cells and determine max column widths
				foreach (TableRow row in Children)
				{
					for (int i = 0; i < ColumnCount; i++)
					{
						ControlBase cell = row.GetCell(i);
						if (cell != null)
						{
							cell.Measure(availableSize);
							columnWidth[i] = Math.Max(columnWidth[i], cell.MeasuredSize.Width);
						}
					}
				}

				// Sum all column widths 
				for (int i = 0; i < ColumnCount; i++)
				{
					width += columnWidth[i];
				}

				// Set column widths to all rows and measure rows
				foreach (TableRow row in Children)
				{
					for (int i = 0; i < ColumnCount; i++)
					{
						if (i < ColumnCount - 1 || m_MaxWidth == 0)
							row.SetColumnWidth(i, columnWidth[i]);
						else
							row.SetColumnWidth(i, columnWidth[i] + Math.Max(0, m_MaxWidth - width));
					}

					row.Measure(availableSize);

					height += row.MeasuredSize.Height;
				}

				IsDirty = false;

				if (m_MaxWidth == 0 || m_MaxWidth < width)
					return new Size(width, height);
				else
					return new Size(m_MaxWidth, height);
			}
			else
			{
				int height = 0;
				int width = 0;
				foreach (TableRow row in Children)
				{
					row.Measure(availableSize);

					width = Math.Max(width, row.MeasuredSize.Width);
					height += row.MeasuredSize.Height;
				}

				return new Size(width, height);
			}
		}

		protected override Size OnArrange(Size finalSize)
		{
			int y = 0;
			int width = 0;
			bool even = false;
			foreach (TableRow row in Children)
			{
				if (m_AlternateColor)
				{
					row.EvenRow = even;
					even = !even;
				}

				row.Arrange(new Rectangle(0, y, finalSize.Width, row.MeasuredSize.Height));
				width = Math.Max(width, row.MeasuredSize.Width);
				y += row.MeasuredSize.Height;
			}

			return new Size(finalSize.Width, y);
		}

		protected override void OnScaleChanged()
		{
			base.OnScaleChanged();

			IsDirty = true;
		}

		private void InitCollection()
		{
			Clear();

			if (m_ItemsSource == null)
				return;

			if (m_DisplayMembers == null || m_DisplayMembers.Length == 0)
				ColumnCount = 1;
			else
				ColumnCount = m_DisplayMembers.Length;

			foreach (object item in m_ItemsSource)
			{
				AddRow(item);
			}
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (var item in e.OldItems)
				{
					RemoveRow(item);
				}
			}
			if (e.NewItems != null)
			{
				int index = e.NewStartingIndex;
				foreach (var item in e.NewItems)
				{
					InsertRow(index++, item);
				}
			}
		}

		private string GetPropertyValue(object obj, string propertyName)
		{
			try
			{
				Type type = obj.GetType();
				PropertyInfo propertyInfo = type.GetProperty(propertyName);
				object property = propertyInfo.GetValue(obj, null);
				return property.ToString();
			}
			catch (Exception)
			{
				return "[NA]";
			}
		}
	}
}
