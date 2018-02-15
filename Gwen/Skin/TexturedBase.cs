using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Gwen.Control;
using Gwen.Skin.Texturing;
using Single = Gwen.Skin.Texturing.Single;

namespace Gwen.Skin
{
	#region UI element textures
	public struct SkinTextures
	{
		public ISkinElement StatusBar;
		public ISkinElement Selection;
		public ISkinElement Shadow;
		public ISkinElement Tooltip;

		public struct _ToolWindow
		{
			public struct _H
			{
				public ISkinElement DragBar;
				public ISkinElement Client;
			}

			public struct _V
			{
				public ISkinElement DragBar;
				public ISkinElement Client;
			}

			public _H H;
			public _V V;
		}

		public _ToolWindow ToolWindow;

		public struct _Panel
		{
			public ISkinElement Normal;
			public ISkinElement Bright;
			public ISkinElement Dark;
			public ISkinElement Highlight;
		}

		public struct _Window
		{
			public struct _Normal
			{
				public ISkinElement TitleBar;
				public ISkinElement Client;
			}

			public struct _Inactive
			{
				public ISkinElement TitleBar;
				public ISkinElement Client;
			}

			public _Normal Normal;
			public _Inactive Inactive;

			public ISkinElement Close;
			public ISkinElement Close_Hover;
			public ISkinElement Close_Down;
			public ISkinElement Close_Disabled;
		}

		public struct _CheckBox
		{
			public struct _Active
			{
				public ISkinElement Normal;
				public ISkinElement Checked;
			}
			public struct _Disabled
			{
				public ISkinElement Normal;
				public ISkinElement Checked;
			}

			public _Active Active;
			public _Disabled Disabled;
		}

		public struct _RadioButton
		{
			public struct _Active
			{
				public ISkinElement Normal;
				public ISkinElement Checked;
			}
			public struct _Disabled
			{
				public ISkinElement Normal;
				public ISkinElement Checked;
			}

			public _Active Active;
			public _Disabled Disabled;
		}

		public struct _TextBox
		{
			public ISkinElement Normal;
			public ISkinElement Focus;
			public ISkinElement Disabled;
		}

		public struct _Tree
		{
			public ISkinElement Background;
			public ISkinElement Minus;
			public ISkinElement Plus;
		}

		public struct _ProgressBar
		{
			public ISkinElement Back;
			public ISkinElement Front;
		}

		public struct _Scroller
		{
			public ISkinElement TrackV;
			public ISkinElement TrackH;
			public ISkinElement ButtonV_Normal;
			public ISkinElement ButtonV_Hover;
			public ISkinElement ButtonV_Down;
			public ISkinElement ButtonV_Disabled;
			public ISkinElement ButtonH_Normal;
			public ISkinElement ButtonH_Hover;
			public ISkinElement ButtonH_Down;
			public ISkinElement ButtonH_Disabled;

			public struct _Button
			{
				public ISkinElement[] Normal;
				public ISkinElement[] Hover;
				public ISkinElement[] Down;
				public ISkinElement[] Disabled;
			}

			public _Button Button;
		}

		public struct _Menu
		{
			public ISkinElement RightArrow;
			public ISkinElement Check;

			public ISkinElement Strip;
			public ISkinElement Background;
			public ISkinElement BackgroundWithMargin;
			public ISkinElement Hover;
		}

		public struct _Input
		{
			public struct _Button
			{
				public ISkinElement Normal;
				public ISkinElement Hovered;
				public ISkinElement Disabled;
				public ISkinElement Pressed;
			}

			public struct _ComboBox
			{
				public ISkinElement Normal;
				public ISkinElement Hover;
				public ISkinElement Down;
				public ISkinElement Disabled;

				public struct _Button
				{
					public ISkinElement Normal;
					public ISkinElement Hover;
					public ISkinElement Down;
					public ISkinElement Disabled;
				}

				public _Button Button;
			}

			public struct _Slider
			{
				public struct _H
				{
					public ISkinElement Normal;
					public ISkinElement Hover;
					public ISkinElement Down;
					public ISkinElement Disabled;
				}

				public struct _V
				{
					public ISkinElement Normal;
					public ISkinElement Hover;
					public ISkinElement Down;
					public ISkinElement Disabled;
				}

				public _H H;
				public _V V;
			}

			public struct _ListBox
			{
				public ISkinElement Background;
				public ISkinElement Hovered;
				public ISkinElement EvenLine;
				public ISkinElement OddLine;
				public ISkinElement EvenLineSelected;
				public ISkinElement OddLineSelected;
			}

			public struct _UpDown
			{
				public struct _Up
				{
					public ISkinElement Normal;
					public ISkinElement Hover;
					public ISkinElement Down;
					public ISkinElement Disabled;
				}

				public struct _Down
				{
					public ISkinElement Normal;
					public ISkinElement Hover;
					public ISkinElement Down;
					public ISkinElement Disabled;
				}

				public _Up Up;
				public _Down Down;
			}

			public _Button Button;
			public _ComboBox ComboBox;
			public _Slider Slider;
			public _ListBox ListBox;
			public _UpDown UpDown;
		}

		public struct _Tab
		{
			public struct _Bottom
			{
				public ISkinElement Inactive;
				public ISkinElement Active;
			}

			public struct _Top
			{
				public ISkinElement Inactive;
				public ISkinElement Active;
			}

			public struct _Left
			{
				public ISkinElement Inactive;
				public ISkinElement Active;
			}

			public struct _Right
			{
				public ISkinElement Inactive;
				public ISkinElement Active;
			}

			public _Bottom Bottom;
			public _Top Top;
			public _Left Left;
			public _Right Right;

			public ISkinElement Control;
			public ISkinElement HeaderBar;
		}

		public struct _CategoryList
		{
			public struct _Inner
			{
				public ISkinElement Header;
				public ISkinElement Client;
			}

			public ISkinElement Outer;
			public _Inner Inner;
			public ISkinElement Header;
		}

		public struct _Page
		{
			public struct _Navigation
			{
				public ISkinElement Title;
				public ISkinElement Client;
			}

			public _Navigation Navigation;
			public ISkinElement Content;
		}

		public struct _NavigationBar
		{
			public struct _Button
			{
				public ISkinElement Back;
				public ISkinElement Options;
				public ISkinElement Menu;
			}

			public _Button Button;
		}

		public _Panel Panel;
		public _Window Window;
		public _CheckBox CheckBox;
		public _RadioButton RadioButton;
		public _TextBox TextBox;
		public _Tree Tree;
		public _ProgressBar ProgressBar;
		public _Scroller Scroller;
		public _Menu Menu;
		public _Input Input;
		public _Tab Tab;
		public _CategoryList CategoryList;
	}
	#endregion

	/// <summary>
	/// Base textured skin.
	/// </summary>
	public class TexturedBase : Skin.SkinBase
	{
		protected SkinTextures Textures;

		private readonly Texture m_Texture;

		/// <summary>
		/// Initializes a new instance of the <see cref="TexturedBase"/> class.
		/// </summary>
		/// <param name="renderer">Renderer to use.</param>
		/// <param name="textureName">Name of the skin texture map.</param>
		/// <param name="xmlName">Name of the skin definition file.</param>
		public TexturedBase(Renderer.RendererBase renderer, string textureName, string xmlName)
			: base(renderer)
		{
			m_Texture = new Texture(Renderer);
			m_Texture.Load(textureName);

			ReadXml(Loader.LoaderBase.Loader.GetXmlStream(xmlName));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TexturedBase"/> class.
		/// </summary>
		/// <param name="renderer">Renderer to use.</param>
		/// <param name="textureData">Stream of the skin texture map.</param>
		/// <param name="xmlStream">Stream of the skin definition file.</param>
		public TexturedBase(Renderer.RendererBase renderer, Stream textureData, Stream xmlStream)
			: base(renderer)
		{
			m_Texture = new Texture(Renderer);
			m_Texture.LoadStream(textureData);

			ReadXml(xmlStream);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public override void Dispose()
		{
			m_Texture.Dispose();
			base.Dispose();
		}

		#region Initialization

		private void ReadXml(Stream xmlStream)
		{
			Dictionary<string, ISkinElement> skinElements = new Dictionary<string, ISkinElement>();
			Dictionary<string, Point> colors = new Dictionary<string, Point>();

			string name = null;
			Rectangle rect = new Rectangle();
			Margin margin = new Margin();
			Point pos = new Point();

			XDocument doc = XDocument.Load(xmlStream);

			foreach (var rootElement in doc.Elements())
			{
				switch (rootElement.Name.LocalName)
				{
					case "Skin":
						foreach (var skinElement in rootElement.Elements())
						{
							switch (skinElement.Name.LocalName)
							{
								case "Single":
									foreach (var attrib in skinElement.Attributes())
									{
										switch (attrib.Name.LocalName)
										{
											case "Name": name = attrib.Value; break;
											case "Rect": rect = Xml.XmlHelper.Parse<Rectangle>(attrib.Value); break;
											default: throw new System.Xml.XmlException($"Unknown attribute '{attrib.Name.LocalName}'.");
										}
									}

									skinElements[name] = new Single(m_Texture, rect.X, rect.Y, rect.Width, rect.Height);
									break;

								case "Bordered":
									foreach (var attrib in skinElement.Attributes())
									{
										switch (attrib.Name.LocalName)
										{
											case "Name": name = attrib.Value; break;
											case "Rect": rect = Xml.XmlHelper.Parse<Rectangle>(attrib.Value); break;
											case "Margin": margin = Xml.XmlHelper.Parse<Margin>(attrib.Value); break;
											default: throw new System.Xml.XmlException($"Unknown attribute '{attrib.Name.LocalName}'.");
										}
									}

									skinElements[name] = new Bordered(m_Texture, rect.X, rect.Y, rect.Width, rect.Height, margin);
									break;

								case "Color":
									foreach (var attrib in skinElement.Attributes())
									{
										switch (attrib.Name.LocalName)
										{
											case "Name": name = attrib.Value; break;
											case "Pos": pos = Xml.XmlHelper.Parse<Point>(attrib.Value); break;
											default: throw new System.Xml.XmlException($"Unknown attribute '{attrib.Name.LocalName}'.");
										}
									}

									colors[name] = pos;
									break;

								default:
									throw new System.Xml.XmlException($"Unknown element '{skinElement.Name.LocalName}'.");
							}
						}
						break;

					default:
						throw new System.Xml.XmlException($"Unknown element '{rootElement.Name.LocalName}'.");
				}
			}

			xmlStream.Dispose();

			InitializeColors(colors);
			InitializeTextures(skinElements);
		}

		private Color ReadPixel(Dictionary<string, Point> colors, string key, Color def)
		{
			if (colors.TryGetValue(key, out Point pos))
			{
				return Renderer.PixelColor(m_Texture, (uint)pos.X, (uint)pos.Y, def);
			}

			return def;
		}

		private void InitializeColors(Dictionary<string, Point> colors)
		{
			Colors.Window.TitleActive   = ReadPixel(colors, "Window.TitleActive", Color.Red);
			Colors.Window.TitleInactive = ReadPixel(colors, "Window.TitleInactive", Color.Yellow);

			Colors.Button.Normal   = ReadPixel(colors, "Button.Normal", Color.Yellow);
			Colors.Button.Hover    = ReadPixel(colors, "Button.Hover", Color.Yellow);
			Colors.Button.Down     = ReadPixel(colors, "Button.Down", Color.Yellow);
			Colors.Button.Disabled = ReadPixel(colors, "Button.Disabled", Color.Yellow);

			Colors.Tab.Active.Normal     = ReadPixel(colors, "Tab.Active.Normal", Color.Yellow);
			Colors.Tab.Active.Hover      = ReadPixel(colors, "Tab.Active.Hover", Color.Yellow);
			Colors.Tab.Active.Down       = ReadPixel(colors, "Tab.Active.Down", Color.Yellow);
			Colors.Tab.Active.Disabled   = ReadPixel(colors, "Tab.Active.Disabled", Color.Yellow);
			Colors.Tab.Inactive.Normal   = ReadPixel(colors, "Tab.Inactive.Normal", Color.Yellow);
			Colors.Tab.Inactive.Hover    = ReadPixel(colors, "Tab.Inactive.Hover", Color.Yellow);
			Colors.Tab.Inactive.Down     = ReadPixel(colors, "Tab.Inactive.Down", Color.Yellow);
			Colors.Tab.Inactive.Disabled = ReadPixel(colors, "Tab.Inactive.Disabled", Color.Yellow);

			Colors.Label.Default   = ReadPixel(colors, "Label.Default", Color.Yellow);
			Colors.Label.Bright    = ReadPixel(colors, "Label.Bright", Color.Yellow);
			Colors.Label.Dark      = ReadPixel(colors, "Label.Dark", Color.Yellow);
			Colors.Label.Highlight = ReadPixel(colors, "Label.Highlight", Color.Yellow);

			Colors.TextBox.Text                = ReadPixel(colors, "TextBox.Text", Color.Yellow);
			Colors.TextBox.Background_Selected = ReadPixel(colors, "TextBox.Background_Selected", Color.Yellow);
			Colors.TextBox.Caret               = ReadPixel(colors, "TextBox.Caret", Color.Yellow);

			Colors.ListBox.Text_Normal   = ReadPixel(colors, "ListBox.Text_Normal", Color.Yellow);
			Colors.ListBox.Text_Selected = ReadPixel(colors, "ListBox.Text_Selected", Color.Yellow);

			Colors.Tree.Lines    = ReadPixel(colors, "Tree.Lines", Color.Yellow);
			Colors.Tree.Normal   = ReadPixel(colors, "Tree.Normal", Color.Yellow);
			Colors.Tree.Hover    = ReadPixel(colors, "Tree.Hover", Color.Yellow);
			Colors.Tree.Selected = ReadPixel(colors, "Tree.Selected", Color.Yellow);

			Colors.Properties.Line_Normal     = ReadPixel(colors, "Properties.Line_Normal", Color.Yellow);
			Colors.Properties.Line_Selected   = ReadPixel(colors, "Properties.Line_Selected", Color.Yellow);
			Colors.Properties.Line_Hover      = ReadPixel(colors, "Properties.Line_Hover", Color.Yellow);
			Colors.Properties.Title           = ReadPixel(colors, "Properties.Title", Color.Yellow);
			Colors.Properties.Column_Normal   = ReadPixel(colors, "Properties.Column_Normal", Color.Yellow);
			Colors.Properties.Column_Selected = ReadPixel(colors, "Properties.Column_Selected", Color.Yellow);
			Colors.Properties.Column_Hover    = ReadPixel(colors, "Properties.Column_Hover", Color.Yellow);
			Colors.Properties.Border          = ReadPixel(colors, "Properties.Border", Color.Yellow);
			Colors.Properties.Label_Normal    = ReadPixel(colors, "Properties.Label_Normal", Color.Yellow);
			Colors.Properties.Label_Selected  = ReadPixel(colors, "Properties.Label_Selected", Color.Yellow);
			Colors.Properties.Label_Hover     = ReadPixel(colors, "Properties.Label_Hover", Color.Yellow);

			Colors.ModalBackground = ReadPixel(colors, "ModalBackground", Color.Yellow);

			Colors.TooltipText = ReadPixel(colors, "TooltipText", Color.Yellow);

			Colors.Category.Header                  = ReadPixel(colors, "Category.Header", Color.Yellow);
			Colors.Category.Header_Closed           = ReadPixel(colors, "Category.Header_Closed", Color.Yellow);
			Colors.Category.Line.Text               = ReadPixel(colors, "Category.Line.Text", Color.Yellow);
			Colors.Category.Line.Text_Hover         = ReadPixel(colors, "Category.Line.Text_Hover", Color.Yellow);
			Colors.Category.Line.Text_Selected      = ReadPixel(colors, "Category.Line.Text_Selected", Color.Yellow);
			Colors.Category.Line.Button             = ReadPixel(colors, "Category.Line.Button", Color.Yellow);
			Colors.Category.Line.Button_Hover       = ReadPixel(colors, "Category.Line.Button_Hover", Color.Yellow);
			Colors.Category.Line.Button_Selected    = ReadPixel(colors, "Category.Line.Button_Selected", Color.Yellow);
			Colors.Category.LineAlt.Text            = ReadPixel(colors, "Category.LineAlt.Text", Color.Yellow);
			Colors.Category.LineAlt.Text_Hover      = ReadPixel(colors, "Category.LineAlt.Text_Hover", Color.Yellow);
			Colors.Category.LineAlt.Text_Selected   = ReadPixel(colors, "Category.LineAlt.Text_Selected", Color.Yellow);
			Colors.Category.LineAlt.Button          = ReadPixel(colors, "Category.LineAlt.Button", Color.Yellow);
			Colors.Category.LineAlt.Button_Hover    = ReadPixel(colors, "Category.LineAlt.Button_Hover", Color.Yellow);
			Colors.Category.LineAlt.Button_Selected = ReadPixel(colors, "Category.LineAlt.Button_Selected", Color.Yellow);

			Colors.GroupBox.Dark = ReadPixel(colors, "GroupBox.Dark", Color.Yellow);
			Colors.GroupBox.Light = ReadPixel(colors, "GroupBox.Light", Color.Yellow);
		}

		private ISkinElement GetTexture(Dictionary<string, ISkinElement> skinElements, string key)
		{
			if (skinElements.TryGetValue(key, out ISkinElement skinElement))
			{
				return skinElement;
			}

			return new Single(m_Texture, 0, 0, 1, 1);
		}

		private void InitializeTextures(Dictionary<string, ISkinElement> skinElements)
		{
			Textures.Shadow    = GetTexture(skinElements, "Shadow");
			Textures.Tooltip   = GetTexture(skinElements, "Tooltip");
			Textures.StatusBar = GetTexture(skinElements, "StatusBar");
			Textures.Selection = GetTexture(skinElements, "Selection");

			Textures.Panel.Normal    = GetTexture(skinElements, "Panel.Normal");
			Textures.Panel.Bright    = GetTexture(skinElements, "Panel.Bright");
			Textures.Panel.Dark      = GetTexture(skinElements, "Panel.Dark");
			Textures.Panel.Highlight = GetTexture(skinElements, "Panel.Highlight");

			Textures.Window.Normal.TitleBar   = GetTexture(skinElements, "Window.Normal.TitleBar");
			Textures.Window.Normal.Client     = GetTexture(skinElements, "Window.Normal.Client");
			Textures.Window.Inactive.TitleBar = GetTexture(skinElements, "Window.Inactive.TitleBar");
			Textures.Window.Inactive.Client   = GetTexture(skinElements, "Window.Inactive.Client");

			Textures.ToolWindow.H.DragBar = GetTexture(skinElements, "ToolWindow.H.DragBar");
			Textures.ToolWindow.H.Client  = GetTexture(skinElements, "ToolWindow.H.Client");
			Textures.ToolWindow.V.DragBar = GetTexture(skinElements, "ToolWindow.V.DragBar");
			Textures.ToolWindow.V.Client  = GetTexture(skinElements, "ToolWindow.V.Client");

			Textures.CheckBox.Active.Checked  = GetTexture(skinElements, "CheckBox.Active.Checked");
			Textures.CheckBox.Active.Normal   = GetTexture(skinElements, "CheckBox.Active.Normal");
			Textures.CheckBox.Disabled.Normal = GetTexture(skinElements, "CheckBox.Disabled.Normal");
			Textures.CheckBox.Disabled.Normal = GetTexture(skinElements, "CheckBox.Disabled.Normal");

			Textures.RadioButton.Active.Checked  = GetTexture(skinElements, "RadioButton.Active.Checked");
			Textures.RadioButton.Active.Normal   = GetTexture(skinElements, "RadioButton.Active.Normal");
			Textures.RadioButton.Disabled.Normal = GetTexture(skinElements, "RadioButton.Disabled.Normal");
			Textures.RadioButton.Disabled.Normal = GetTexture(skinElements, "RadioButton.Disabled.Normal");

			Textures.TextBox.Normal   = GetTexture(skinElements, "TextBox.Normal");
			Textures.TextBox.Focus    = GetTexture(skinElements, "TextBox.Focus");
			Textures.TextBox.Disabled = GetTexture(skinElements, "TextBox.Disabled");

			Textures.Menu.Strip                = GetTexture(skinElements, "Menu.Strip");
			Textures.Menu.BackgroundWithMargin = GetTexture(skinElements, "Menu.BackgroundWithMargin");
			Textures.Menu.Background           = GetTexture(skinElements, "Menu.Background");
			Textures.Menu.Hover                = GetTexture(skinElements, "Menu.Hover");
			Textures.Menu.RightArrow           = GetTexture(skinElements, "Menu.RightArrow");
			Textures.Menu.Check                = GetTexture(skinElements, "Menu.Check");

			Textures.Tab.Control         = GetTexture(skinElements, "Tab.Control");
			Textures.Tab.Bottom.Active   = GetTexture(skinElements, "Tab.Bottom.Active");
			Textures.Tab.Bottom.Inactive = GetTexture(skinElements, "Tab.Bottom.Inactive");
			Textures.Tab.Top.Active      = GetTexture(skinElements, "Tab.Top.Active");
			Textures.Tab.Top.Inactive    = GetTexture(skinElements, "Tab.Top.Inactive");
			Textures.Tab.Left.Active     = GetTexture(skinElements, "Tab.Left.Active");
			Textures.Tab.Left.Inactive   = GetTexture(skinElements, "Tab.Left.Inactive");
			Textures.Tab.Right.Active    = GetTexture(skinElements, "Tab.Right.Active");
			Textures.Tab.Right.Inactive  = GetTexture(skinElements, "Tab.Right.Inactive");
			Textures.Tab.HeaderBar       = GetTexture(skinElements, "Tab.HeaderBar");

			Textures.Window.Close          = GetTexture(skinElements, "Window.Close");
			Textures.Window.Close_Hover    = GetTexture(skinElements, "Window.Close_Hover");
			Textures.Window.Close_Down     = GetTexture(skinElements, "Window.Close_Down");
			Textures.Window.Close_Disabled = GetTexture(skinElements, "Window.Close_Disabled");

			Textures.Scroller.TrackV           = GetTexture(skinElements, "Scroller.TrackV");
			Textures.Scroller.ButtonV_Normal   = GetTexture(skinElements, "Scroller.ButtonV_Normal");
			Textures.Scroller.ButtonV_Hover    = GetTexture(skinElements, "Scroller.ButtonV_Hover");
			Textures.Scroller.ButtonV_Down     = GetTexture(skinElements, "Scroller.ButtonV_Down");
			Textures.Scroller.ButtonV_Disabled = GetTexture(skinElements, "Scroller.ButtonV_Disabled");
			Textures.Scroller.TrackH           = GetTexture(skinElements, "Scroller.TrackH");
			Textures.Scroller.ButtonH_Normal   = GetTexture(skinElements, "Scroller.ButtonH_Normal");
			Textures.Scroller.ButtonH_Hover    = GetTexture(skinElements, "Scroller.ButtonH_Hover");
			Textures.Scroller.ButtonH_Down     = GetTexture(skinElements, "Scroller.ButtonH_Down");
			Textures.Scroller.ButtonH_Disabled = GetTexture(skinElements, "Scroller.ButtonH_Disabled");

			Textures.Tree.Background = GetTexture(skinElements, "Tree.Background");
			Textures.Tree.Plus       = GetTexture(skinElements, "Tree.Plus");
			Textures.Tree.Minus      = GetTexture(skinElements, "Tree.Minus");

			Textures.Input.Button.Normal   = GetTexture(skinElements, "Input.Button.Normal");
			Textures.Input.Button.Hovered  = GetTexture(skinElements, "Input.Button.Hovered");
			Textures.Input.Button.Disabled = GetTexture(skinElements, "Input.Button.Disabled");
			Textures.Input.Button.Pressed  = GetTexture(skinElements, "Input.Button.Pressed");

			Textures.Scroller.Button.Normal = new ISkinElement[]
			{
				GetTexture(skinElements, "Scroller.Button.Left.Normal"),
				GetTexture(skinElements, "Scroller.Button.Top.Normal"),
				GetTexture(skinElements, "Scroller.Button.Right.Normal"),
				GetTexture(skinElements, "Scroller.Button.Bottom.Normal")
			};
			Textures.Scroller.Button.Disabled = new ISkinElement[]
			{
				GetTexture(skinElements, "Scroller.Button.Left.Disabled"),
				GetTexture(skinElements, "Scroller.Button.Top.Disabled"),
				GetTexture(skinElements, "Scroller.Button.Right.Disabled"),
				GetTexture(skinElements, "Scroller.Button.Bottom.Disabled")
			};
			Textures.Scroller.Button.Hover = new ISkinElement[]
			{
				GetTexture(skinElements, "Scroller.Button.Left.Hover"),
				GetTexture(skinElements, "Scroller.Button.Top.Hover"),
				GetTexture(skinElements, "Scroller.Button.Right.Hover"),
				GetTexture(skinElements, "Scroller.Button.Bottom.Hover")
			};
			Textures.Scroller.Button.Down = new ISkinElement[]
			{
				GetTexture(skinElements, "Scroller.Button.Left.Down"),
				GetTexture(skinElements, "Scroller.Button.Top.Down"),
				GetTexture(skinElements, "Scroller.Button.Right.Down"),
				GetTexture(skinElements, "Scroller.Button.Bottom.Down")
			};

			Textures.Input.ListBox.Background       = GetTexture(skinElements, "Input.ListBox.Background");
			Textures.Input.ListBox.Hovered          = GetTexture(skinElements, "Input.ListBox.Hovered");
			Textures.Input.ListBox.EvenLine         = GetTexture(skinElements, "Input.ListBox.EvenLine");
			Textures.Input.ListBox.OddLine          = GetTexture(skinElements, "Input.ListBox.OddLine");
			Textures.Input.ListBox.EvenLineSelected = GetTexture(skinElements, "Input.ListBox.EvenLineSelected");
			Textures.Input.ListBox.OddLineSelected  = GetTexture(skinElements, "Input.ListBox.OddLineSelected");

			Textures.Input.ComboBox.Normal   = GetTexture(skinElements, "Input.ComboBox.Normal");
			Textures.Input.ComboBox.Hover    = GetTexture(skinElements, "Input.ComboBox.Hover");
			Textures.Input.ComboBox.Down     = GetTexture(skinElements, "Input.ComboBox.Down");
			Textures.Input.ComboBox.Disabled = GetTexture(skinElements, "Input.ComboBox.Disabled");

			Textures.Input.ComboBox.Button.Normal   = GetTexture(skinElements, "Input.ComboBox.Button.Normal");
			Textures.Input.ComboBox.Button.Hover    = GetTexture(skinElements, "Input.ComboBox.Button.Hover");
			Textures.Input.ComboBox.Button.Down     = GetTexture(skinElements, "Input.ComboBox.Button.Down");
			Textures.Input.ComboBox.Button.Disabled = GetTexture(skinElements, "Input.ComboBox.Button.Disabled");

			Textures.Input.UpDown.Up.Normal     = GetTexture(skinElements, "Input.UpDown.Up.Normal");
			Textures.Input.UpDown.Up.Hover      = GetTexture(skinElements, "Input.UpDown.Up.Hover");
			Textures.Input.UpDown.Up.Down       = GetTexture(skinElements, "Input.UpDown.Up.Down");
			Textures.Input.UpDown.Up.Disabled   = GetTexture(skinElements, "Input.UpDown.Up.Disabled");
			Textures.Input.UpDown.Down.Normal   = GetTexture(skinElements, "Input.UpDown.Down.Normal");
			Textures.Input.UpDown.Down.Hover    = GetTexture(skinElements, "Input.UpDown.Down.Hover");
			Textures.Input.UpDown.Down.Down     = GetTexture(skinElements, "Input.UpDown.Down.Down");
			Textures.Input.UpDown.Down.Disabled = GetTexture(skinElements, "Input.UpDown.Down.Disabled");

			Textures.ProgressBar.Back  = GetTexture(skinElements, "ProgressBar.Back");
			Textures.ProgressBar.Front = GetTexture(skinElements, "ProgressBar.Front");

			Textures.Input.Slider.H.Normal   = GetTexture(skinElements, "Input.Slider.H.Normal");
			Textures.Input.Slider.H.Hover    = GetTexture(skinElements, "Input.Slider.H.Hover");
			Textures.Input.Slider.H.Down     = GetTexture(skinElements, "Input.Slider.H.Down");
			Textures.Input.Slider.H.Disabled = GetTexture(skinElements, "Input.Slider.H.Disabled");

			Textures.Input.Slider.V.Normal   = GetTexture(skinElements, "Input.Slider.V.Normal");
			Textures.Input.Slider.V.Hover    = GetTexture(skinElements, "Input.Slider.V.Hover");
			Textures.Input.Slider.V.Down     = GetTexture(skinElements, "Input.Slider.V.Down");
			Textures.Input.Slider.V.Disabled = GetTexture(skinElements, "Input.Slider.V.Disabled");

			Textures.CategoryList.Outer	       = GetTexture(skinElements, "CategoryList.Outer");
			Textures.CategoryList.Inner.Header = GetTexture(skinElements, "CategoryList.Inner.Header");
			Textures.CategoryList.Inner.Client = GetTexture(skinElements, "CategoryList.Inner.Client");
			Textures.CategoryList.Header       = GetTexture(skinElements, "CategoryList.Header");
		}
		#endregion

		#region UI elements
		public override void DrawButton(Control.ControlBase control, bool depressed, bool hovered, bool disabled)
		{
			if (disabled)
			{
				Textures.Input.Button.Disabled.Draw(Renderer, control.RenderBounds);
				return;
			}
			if (depressed)
			{
				Textures.Input.Button.Pressed.Draw(Renderer, control.RenderBounds);
				return;
			}
			if (hovered)
			{
				Textures.Input.Button.Hovered.Draw(Renderer, control.RenderBounds);
				return;
			}
			Textures.Input.Button.Normal.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawMenuRightArrow(Control.ControlBase control)
		{
			Textures.Menu.RightArrow.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawMenuItem(Control.ControlBase control, bool submenuOpen, bool isChecked)
		{
			if (submenuOpen || control.IsHovered)
				Textures.Menu.Hover.Draw(Renderer, control.RenderBounds);

			if (isChecked)
				Textures.Menu.Check.Draw(Renderer, new Rectangle(control.RenderBounds.X + 4, control.RenderBounds.Y + 3, 15, 15));
		}

		public override void DrawMenuStrip(Control.ControlBase control)
		{
			Textures.Menu.Strip.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawMenu(Control.ControlBase control, bool paddingDisabled)
		{
			if (!paddingDisabled)
			{
				Textures.Menu.BackgroundWithMargin.Draw(Renderer, control.RenderBounds);
				return;
			}

			Textures.Menu.Background.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawShadow(Control.ControlBase control)
		{
			Rectangle r = control.RenderBounds;
			r.X -= 4;
			r.Y -= 4;
			r.Width += 10;
			r.Height += 10;
			Textures.Shadow.Draw(Renderer, r);
		}

		public override void DrawRadioButton(Control.ControlBase control, bool selected, bool depressed)
		{
			if (selected)
			{
				if (control.IsDisabled)
					Textures.RadioButton.Disabled.Checked.Draw(Renderer, control.RenderBounds);
				else
					Textures.RadioButton.Active.Checked.Draw(Renderer, control.RenderBounds);
			}
			else
			{
				if (control.IsDisabled)
					Textures.RadioButton.Disabled.Normal.Draw(Renderer, control.RenderBounds);
				else
					Textures.RadioButton.Active.Normal.Draw(Renderer, control.RenderBounds);
			}
		}

		public override void DrawCheckBox(Control.ControlBase control, bool selected, bool depressed)
		{
			if (selected)
			{
				if (control.IsDisabled)
					Textures.CheckBox.Disabled.Checked.Draw(Renderer, control.RenderBounds);
				else
					Textures.CheckBox.Active.Checked.Draw(Renderer, control.RenderBounds);
			}
			else
			{
				if (control.IsDisabled)
					Textures.CheckBox.Disabled.Normal.Draw(Renderer, control.RenderBounds);
				else
					Textures.CheckBox.Active.Normal.Draw(Renderer, control.RenderBounds);
			}
		}

		public override void DrawGroupBox(Control.ControlBase control, int textStart, int textHeight, int textWidth)
		{
			Rectangle rect = control.RenderBounds;

			rect.Y += (int)(textHeight * 0.5f);
			rect.Height -= (int)(textHeight * 0.5f);

			Renderer.DrawColor = Colors.GroupBox.Light;

			Renderer.DrawFilledRect(new Rectangle(rect.X + 1, rect.Y + 1, textStart - 3, 1));
			Renderer.DrawFilledRect(new Rectangle(rect.X + 1 + textStart + textWidth, rect.Y + 1, rect.Width - textStart + textWidth - 2, 1));
			Renderer.DrawFilledRect(new Rectangle(rect.X + 1, (rect.Y + rect.Height) - 2, rect.X + rect.Width - 2, 1));

			Renderer.DrawFilledRect(new Rectangle(rect.X + 1, rect.Y + 1, 1, rect.Height));
			Renderer.DrawFilledRect(new Rectangle((rect.X + rect.Width) - 2, rect.Y + 1, 1, rect.Height - 1));

			Renderer.DrawColor = Colors.GroupBox.Dark;

			Renderer.DrawFilledRect(new Rectangle(rect.X + 1, rect.Y, textStart - 3, 1));
			Renderer.DrawFilledRect(new Rectangle(rect.X + 1 + textStart + textWidth, rect.Y, rect.Width - textStart - textWidth - 2, 1));
			Renderer.DrawFilledRect(new Rectangle(rect.X + 1, (rect.Y + rect.Height) - 1, rect.X + rect.Width - 2, 1));

			Renderer.DrawFilledRect(new Rectangle(rect.X, rect.Y + 1, 1, rect.Height - 1));
			Renderer.DrawFilledRect(new Rectangle((rect.X + rect.Width) - 1, rect.Y + 1, 1, rect.Height - 1));
		}

		public override void DrawTextBox(Control.ControlBase control)
		{
			if (control.IsDisabled)
			{
				Textures.TextBox.Disabled.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (control.HasFocus)
				Textures.TextBox.Focus.Draw(Renderer, control.RenderBounds);
			else
				Textures.TextBox.Normal.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawTabButton(Control.ControlBase control, bool active, Dock dir)
		{
			if (active)
			{
				DrawActiveTabButton(control, dir);
				return;
			}

			if (dir == Dock.Top)
			{
				Textures.Tab.Top.Inactive.Draw(Renderer, control.RenderBounds);
				return;
			}
			if (dir == Dock.Left)
			{
				Textures.Tab.Left.Inactive.Draw(Renderer, control.RenderBounds);
				return;
			}
			if (dir == Dock.Bottom)
			{
				Textures.Tab.Bottom.Inactive.Draw(Renderer, control.RenderBounds);
				return;
			}
			if (dir == Dock.Right)
			{
				Textures.Tab.Right.Inactive.Draw(Renderer, control.RenderBounds);
				return;
			}
		}

		private void DrawActiveTabButton(Control.ControlBase control, Dock dir)
		{
			if (dir == Dock.Top)
			{
				Textures.Tab.Top.Active.Draw(Renderer, control.RenderBounds.Add(new Rectangle(0, 0, 0, 8)));
				return;
			}
			if (dir == Dock.Left)
			{
				Textures.Tab.Left.Active.Draw(Renderer, control.RenderBounds.Add(new Rectangle(0, 0, 8, 0)));
				return;
			}
			if (dir == Dock.Bottom)
			{
				Textures.Tab.Bottom.Active.Draw(Renderer, control.RenderBounds.Add(new Rectangle(0, -8, 0, 8)));
				return;
			}
			if (dir == Dock.Right)
			{
				Textures.Tab.Right.Active.Draw(Renderer, control.RenderBounds.Add(new Rectangle(-8, 0, 8, 0)));
				return;
			}
		}

		public override void DrawTabControl(Control.ControlBase control)
		{
			Textures.Tab.Control.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawTabTitleBar(Control.ControlBase control)
		{
			Textures.Tab.HeaderBar.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawWindow(Control.ControlBase control, int topHeight, bool inFocus)
		{
			Rectangle rect = control.RenderBounds;
			Rectangle titleRect = rect;
			titleRect.Height = topHeight;
			Rectangle clientRect = rect;
			clientRect.Y += topHeight;
			clientRect.Height -= topHeight;

			if (inFocus)
			{
				Textures.Window.Normal.TitleBar.Draw(Renderer, titleRect);
				Textures.Window.Normal.Client.Draw(Renderer, clientRect);
			}
			else
			{
				Textures.Window.Inactive.TitleBar.Draw(Renderer, titleRect);
				Textures.Window.Inactive.Client.Draw(Renderer, clientRect);
			}
		}

		public override void DrawWindowCloseButton(Control.ControlBase control, bool depressed, bool hovered, bool disabled)
		{
			if (disabled)
			{
				Textures.Window.Close_Disabled.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (depressed)
			{
				Textures.Window.Close_Down.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (hovered)
			{
				Textures.Window.Close_Hover.Draw(Renderer, control.RenderBounds);
				return;
			}

			Textures.Window.Close.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawToolWindow(Control.ControlBase control, bool vertical, int dragSize)
		{
			if (vertical)
			{
				Rectangle rect = control.RenderBounds;
				Rectangle dragRect = rect;
				dragRect.Height = dragSize;
				Rectangle clientRect = rect;
				clientRect.Y += dragSize;
				clientRect.Height -= dragSize;

				Textures.ToolWindow.V.DragBar.Draw(Renderer, dragRect);
				Textures.ToolWindow.V.Client.Draw(Renderer, clientRect);
			}
			else
			{
				Rectangle rect = control.RenderBounds;
				Rectangle dragRect = rect;
				dragRect.Width = dragSize;
				Rectangle clientRect = rect;
				clientRect.X += dragSize;
				clientRect.Width -= dragSize;

				Textures.ToolWindow.H.DragBar.Draw(Renderer, dragRect);
				Textures.ToolWindow.H.Client.Draw(Renderer, clientRect);
			}
		}

		public override void DrawHighlight(Control.ControlBase control)
		{
			Rectangle rect = control.RenderBounds;
			Renderer.DrawColor = new Color(255, 255, 100, 255);
			Renderer.DrawFilledRect(rect);
		}

		public override void DrawScrollBar(Control.ControlBase control, bool horizontal, bool depressed)
		{
			if (horizontal)
				Textures.Scroller.TrackH.Draw(Renderer, control.RenderBounds);
			else
				Textures.Scroller.TrackV.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawScrollBarBar(Control.ControlBase control, bool depressed, bool hovered, bool horizontal)
		{
			if (!horizontal)
			{
				if (control.IsDisabled)
				{
					Textures.Scroller.ButtonV_Disabled.Draw(Renderer, control.RenderBounds);
					return;
				}

				if (depressed)
				{
					Textures.Scroller.ButtonV_Down.Draw(Renderer, control.RenderBounds);
					return;
				}

				if (hovered)
				{
					Textures.Scroller.ButtonV_Hover.Draw(Renderer, control.RenderBounds);
					return;
				}

				Textures.Scroller.ButtonV_Normal.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (control.IsDisabled)
			{
				Textures.Scroller.ButtonH_Disabled.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (depressed)
			{
				Textures.Scroller.ButtonH_Down.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (hovered)
			{
				Textures.Scroller.ButtonH_Hover.Draw(Renderer, control.RenderBounds);
				return;
			}

			Textures.Scroller.ButtonH_Normal.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawProgressBar(Control.ControlBase control, bool horizontal, float progress)
		{
			Rectangle rect = control.RenderBounds;

			if (horizontal)
			{
				Textures.ProgressBar.Back.Draw(Renderer, rect);
				if (progress > 0)
				{
					rect.Width = (int)(rect.Width * progress);
					if (rect.Width >= 5.0f)
						Textures.ProgressBar.Front.Draw(Renderer, rect);
				}
			}
			else
			{
				Textures.ProgressBar.Back.Draw(Renderer, rect);
				if (progress > 0)
				{
					rect.Y = (int)(rect.Y + rect.Height * (1 - progress));
					rect.Height = (int)(rect.Height * progress);
					if (rect.Height >= 5.0f)
						Textures.ProgressBar.Front.Draw(Renderer, rect);
				}
			}
		}

		public override void DrawListBox(Control.ControlBase control)
		{
			Textures.Input.ListBox.Background.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawListBoxLine(Control.ControlBase control, bool selected, bool even)
		{
			if (selected)
			{
				if (even)
				{
					Textures.Input.ListBox.EvenLineSelected.Draw(Renderer, control.RenderBounds);
					return;
				}
				Textures.Input.ListBox.OddLineSelected.Draw(Renderer, control.RenderBounds);
				return;
			}
			
			if (control.IsHovered)
			{
				Textures.Input.ListBox.Hovered.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (even)
			{
				Textures.Input.ListBox.EvenLine.Draw(Renderer, control.RenderBounds);
				return;
			}

			Textures.Input.ListBox.OddLine.Draw(Renderer, control.RenderBounds);
		}

		public void DrawSliderNotchesH(Rectangle rect, int numNotches, float dist)
		{
			if (numNotches == 0) return;

			float iSpacing = rect.Width / (float)numNotches;
			for (int i = 0; i < numNotches + 1; i++)
				Renderer.DrawFilledRect(Util.FloatRect(rect.X + iSpacing * i, rect.Y + dist - 2, 1, 5));
		}

		public void DrawSliderNotchesV(Rectangle rect, int numNotches, float dist)
		{
			if (numNotches == 0) return;

			float iSpacing = rect.Height / (float)numNotches;
			for (int i = 0; i < numNotches + 1; i++)
				Renderer.DrawFilledRect(Util.FloatRect(rect.X + dist - 2, rect.Y + iSpacing * i, 5, 1));
		}

		public override void DrawSlider(Control.ControlBase control, bool horizontal, int numNotches, int barSize)
		{
			Rectangle rect = control.RenderBounds;
			Renderer.DrawColor = new Color(100, 0, 0, 0);

			if (horizontal)
			{
				rect.X += (int) (barSize*0.5);
				rect.Width -= barSize;
				rect.Y += (int)(rect.Height * 0.5 - 1);
				rect.Height = 1;
				DrawSliderNotchesH(rect, numNotches, barSize*0.5f);
				Renderer.DrawFilledRect(rect);
				return;
			}

			rect.Y += (int)(barSize * 0.5);
			rect.Height -= barSize;
			rect.X += (int)(rect.Width * 0.5 - 1);
			rect.Width = 1;
			DrawSliderNotchesV(rect, numNotches, barSize * 0.4f);
			Renderer.DrawFilledRect(rect);
		}

		public override void DrawComboBox(Control.ControlBase control, bool down, bool open)
		{
			if (control.IsDisabled)
			{
				Textures.Input.ComboBox.Disabled.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (down || open)
			{
				Textures.Input.ComboBox.Down.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (control.IsHovered)
			{
				Textures.Input.ComboBox.Down.Draw(Renderer, control.RenderBounds);
				return;
			}

			Textures.Input.ComboBox.Normal.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawKeyboardHighlight(Control.ControlBase control, Rectangle r, int offset)
		{
			Rectangle rect = r;

			rect.X += offset;
			rect.Y += offset;
			rect.Width -= offset * 2;
			rect.Height -= offset * 2;

			//draw the top and bottom
			bool skip = true;
			for (int i = 0; i < rect.Width * 0.5; i++)
			{
				m_Renderer.DrawColor = Color.Black;
				if (!skip)
				{
					Renderer.DrawPixel(rect.X + (i * 2), rect.Y);
					Renderer.DrawPixel(rect.X + (i * 2), rect.Y + rect.Height - 1);
				}
				else
					skip = false;
			}

			for (int i = 0; i < rect.Height * 0.5; i++)
			{
				Renderer.DrawColor = Color.Black;
				Renderer.DrawPixel(rect.X, rect.Y + i * 2);
				Renderer.DrawPixel(rect.X + rect.Width - 1, rect.Y + i * 2);
			}
		}

		public override void DrawToolTip(Control.ControlBase control)
		{
			Textures.Tooltip.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawScrollButton(Control.ControlBase control, Control.Internal.ScrollBarButtonDirection direction, bool depressed, bool hovered, bool disabled)
		{
			int i = 0;
			if (direction == Control.Internal.ScrollBarButtonDirection.Top) i = 1;
			if (direction == Control.Internal.ScrollBarButtonDirection.Right) i = 2;
			if (direction == Control.Internal.ScrollBarButtonDirection.Bottom) i = 3;

			if (disabled)
			{
				Textures.Scroller.Button.Disabled[i].Draw(Renderer, control.RenderBounds);
				return;
			}

			if (depressed)
			{
				Textures.Scroller.Button.Down[i].Draw(Renderer, control.RenderBounds);
				return;
			}

			if (hovered)
			{
				Textures.Scroller.Button.Hover[i].Draw(Renderer, control.RenderBounds);
				return;
			}

			Textures.Scroller.Button.Normal[i].Draw(Renderer, control.RenderBounds);
		}

		public override void DrawComboBoxArrow(Control.ControlBase control, bool hovered, bool down, bool open, bool disabled)
		{
			if (disabled)
			{
				Textures.Input.ComboBox.Button.Disabled.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (down || open)
			{
				Textures.Input.ComboBox.Button.Down.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (hovered)
			{
				Textures.Input.ComboBox.Button.Hover.Draw(Renderer, control.RenderBounds);
				return;
			}

			Textures.Input.ComboBox.Button.Normal.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawNumericUpDownButton(Control.ControlBase control, bool depressed, bool up)
		{
			if (up)
			{
				if (control.IsDisabled)
				{
					Textures.Input.UpDown.Up.Disabled.Draw(Renderer, control.RenderBounds);
					return;
				}

				if (depressed)
				{
					Textures.Input.UpDown.Up.Down.Draw(Renderer, control.RenderBounds);
					return;
				}

				if (control.IsHovered)
				{
					Textures.Input.UpDown.Up.Hover.Draw(Renderer, control.RenderBounds);
					return;
				}

				Textures.Input.UpDown.Up.Normal.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (control.IsDisabled)
			{
				Textures.Input.UpDown.Down.Disabled.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (depressed)
			{
				Textures.Input.UpDown.Down.Down.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (control.IsHovered)
			{
				Textures.Input.UpDown.Down.Hover.Draw(Renderer, control.RenderBounds);
				return;
			}

			Textures.Input.UpDown.Down.Normal.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawStatusBar(Control.ControlBase control)
		{
			Textures.StatusBar.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawTreeButton(Control.ControlBase control, bool open)
		{
			Rectangle rect = control.RenderBounds;

			if (open)
				Textures.Tree.Minus.Draw(Renderer, rect);
			else
				Textures.Tree.Plus.Draw(Renderer, rect);
		}

		public override void DrawTreeControl(Control.ControlBase control)
		{
			Textures.Tree.Background.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawTreeNode(Control.ControlBase ctrl, bool open, bool selected, int labelHeight, int labelWidth, int halfWay, int lastBranch, bool isRoot, int indent)
		{
			if (selected)
			{
				Textures.Selection.Draw(Renderer, new Rectangle(indent, 0, labelWidth, labelHeight));
			}

			base.DrawTreeNode(ctrl, open, selected, labelHeight, labelWidth, halfWay, lastBranch, isRoot, indent);
		}

		public override void DrawColorDisplay(Control.ControlBase control, Color color)
		{
			Rectangle rect = control.RenderBounds;

			if (color.A != 255)
			{
				Renderer.DrawColor = new Color(255, 255, 255, 255);
				Renderer.DrawFilledRect(rect);

				Renderer.DrawColor = new Color(128, 128, 128, 128);

				Renderer.DrawFilledRect(Util.FloatRect(0, 0, rect.Width * 0.5f, rect.Height * 0.5f));
				Renderer.DrawFilledRect(Util.FloatRect(rect.Width * 0.5f, rect.Height * 0.5f, rect.Width * 0.5f, rect.Height * 0.5f));
			}

			Renderer.DrawColor = color;
			Renderer.DrawFilledRect(rect);

			Renderer.DrawColor = Color.Black;
			Renderer.DrawLinedRect(rect);
		}

		public override void DrawModalControl(Control.ControlBase control, Color? backgroundColor)
		{
			if (!control.ShouldDrawBackground)
				return;
			Rectangle rect = control.RenderBounds;
			if (backgroundColor == null)
				Renderer.DrawColor = Colors.ModalBackground;
			else
				Renderer.DrawColor = (Color)backgroundColor;
			Renderer.DrawFilledRect(rect);
		}

		public override void DrawMenuDivider(Control.ControlBase control)
		{
			Rectangle rect = control.RenderBounds;
			Renderer.DrawColor = new Color(100, 0, 0, 0);
			Renderer.DrawFilledRect(rect);
		}

		public override void DrawSliderButton(Control.ControlBase control, bool depressed, bool horizontal)
		{
			if (!horizontal)
			{
				if (control.IsDisabled)
				{
					Textures.Input.Slider.V.Disabled.Draw(Renderer, control.RenderBounds);
					return;
				}
				
				if (depressed)
				{
					Textures.Input.Slider.V.Down.Draw(Renderer, control.RenderBounds);
					return;
				}
				
				if (control.IsHovered)
				{
					Textures.Input.Slider.V.Hover.Draw(Renderer, control.RenderBounds);
					return;
				}

				Textures.Input.Slider.V.Normal.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (control.IsDisabled)
			{
				Textures.Input.Slider.H.Disabled.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (depressed)
			{
				Textures.Input.Slider.H.Down.Draw(Renderer, control.RenderBounds);
				return;
			}

			if (control.IsHovered)
			{
				Textures.Input.Slider.H.Hover.Draw(Renderer, control.RenderBounds);
				return;
			}

			Textures.Input.Slider.H.Normal.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawCategoryHolder(Control.ControlBase control)
		{
			Textures.CategoryList.Outer.Draw(Renderer, control.RenderBounds);
		}

		public override void DrawCategoryInner(Control.ControlBase control, int headerHeight, bool collapsed)
		{
			if (collapsed)
			{
				Textures.CategoryList.Header.Draw(Renderer, control.RenderBounds);
			}
			else
			{
				Rectangle rect = control.RenderBounds;
				Rectangle headerRect = rect;
				headerRect.Height = headerHeight;
				Rectangle clientRect = rect;
				clientRect.Y += headerHeight;
				clientRect.Height -= headerHeight;
				Textures.CategoryList.Inner.Header.Draw(Renderer, headerRect);
				Textures.CategoryList.Inner.Client.Draw(Renderer, clientRect);
			}
		}

		public override void DrawBorder(Control.ControlBase control, BorderType borderType)
		{
			switch (borderType)
			{
				case BorderType.ToolTip:
					Textures.Tooltip.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.StatusBar:
					Textures.StatusBar.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.MenuStrip:
					Textures.Menu.Strip.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.Selection:
					Textures.Selection.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.PanelNormal:
					Textures.Panel.Normal.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.PanelBright:
					Textures.Panel.Bright.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.PanelDark:
					Textures.Panel.Dark.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.PanelHighlight:
					Textures.Panel.Highlight.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.ListBox:
					Textures.Input.ListBox.Background.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.TreeControl:
					Textures.Tree.Background.Draw(Renderer, control.RenderBounds);
					break;
				case BorderType.CategoryList:
					Textures.CategoryList.Outer.Draw(Renderer, control.RenderBounds);
					break;
			}
		}
		#endregion
	}
}
