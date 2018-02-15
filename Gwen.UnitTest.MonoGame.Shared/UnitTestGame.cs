using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Gwen.Control;

namespace Gwen.UnitTest.MonoGame
{
    public class UnitTestGame : Game
    {
		GraphicsDeviceManager m_Graphics;

		bool m_ChangeGraphicsSettings;

		private Gwen.Renderer.MonoGame.Input.MonoGame m_Input;
		private Gwen.Renderer.MonoGame.MonoGame m_Renderer;
		private Gwen.Skin.SkinBase m_Skin;
		private Gwen.Control.Canvas m_Canvas;
		private Gwen.UnitTest.UnitTest m_UnitTest;

		const int FpsFrames = 50;
		private readonly Queue<int> m_Ftime;
		private int m_Time;

		public UnitTestGame()
		{
			m_Graphics = new GraphicsDeviceManager(this);
			m_Graphics.PreferredBackBufferWidth = 1024;
			m_Graphics.PreferredBackBufferHeight = 768;
			m_Graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(SetToPreserve‌​);

#if ANDROID
			m_Graphics.IsFullScreen = true;
#endif

			m_Graphics.SynchronizeWithVerticalRetrace = false;
			this.IsFixedTimeStep = false;

			Content.RootDirectory = "Content";

			m_ChangeGraphicsSettings = false;

			m_Ftime = new Queue<int>(FpsFrames);
		}

		private void SetToPreserve(object sender, PreparingDeviceSettingsEventArgs eventargs)
		{
			eventargs.GraphicsDeviceInformation.PresentationParameters.R‌​enderTargetUsage = RenderTargetUsage.PreserveContents;
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			base.OnExiting(sender, args);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			this.Window.AllowUserResizing = true;
			this.Window.ClientSizeChanged += new EventHandler<EventArgs>(OnClientSizeChanged);

			IsMouseVisible = true;
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			Platform.Platform.Init(new Gwen.Platform.MonoGame.MonoGamePlatform());
			Loader.LoaderBase.Init(new Gwen.Loader.MonoGame.MonoGameAssetLoader(Content));

			m_Renderer = new Gwen.Renderer.MonoGame.MonoGame(GraphicsDevice, Content, Content.Load<Effect>("GwenEffect"));
			m_Renderer.Resize(m_Graphics.PreferredBackBufferWidth, m_Graphics.PreferredBackBufferHeight);

			m_Skin = new Gwen.Skin.TexturedBase(m_Renderer, "Skins/DefaultSkin", "Skins/DefaultSkinDefinition");
			m_Skin.DefaultFont = new Font(m_Renderer, "Arial", 11);
			m_Canvas = new Canvas(m_Skin);
			m_Input = new Gwen.Renderer.MonoGame.Input.MonoGame(this);
			m_Input.Initialize(m_Canvas);

			m_Canvas.SetSize(m_Graphics.PreferredBackBufferWidth, m_Graphics.PreferredBackBufferHeight);
			m_Canvas.ShouldDrawBackground = true;
			m_Canvas.BackgroundColor = new Color(255, 150, 170, 170);

			m_UnitTest = new Gwen.UnitTest.UnitTest(m_Canvas);

			m_Time = 0;
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			if (m_Canvas != null)
			{
				m_Canvas.Dispose();
				m_Canvas = null;
			}
			if (m_Skin != null)
			{
				m_Skin.Dispose();
				m_Skin = null;
			}
			if (m_Renderer != null)
			{
				m_Renderer.Dispose();
				m_Renderer = null;
			}
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (m_ChangeGraphicsSettings)
			{
				m_Graphics.ApplyChanges();
				m_ChangeGraphicsSettings = false;
			}

			m_Time += gameTime.ElapsedGameTime.Milliseconds;

			if (m_Time > 1000)
			{
				//Debug.WriteLine (String.Format ("String Cache size: {0} Draw Calls: {1} Vertex Count: {2}", renderer.TextCacheSize, renderer.DrawCallCount, renderer.VertexCount));
				m_UnitTest.Note = String.Format("String Cache size: {0} Draw Calls: {1} Vertex Count: {2}", m_Renderer.TextCacheSize, m_Renderer.DrawCallCount, m_Renderer.VertexCount);
				m_UnitTest.Fps = 1000f * m_Ftime.Count / m_Ftime.Sum(i => i);

				m_Time = 0;

				if (m_Renderer.TextCacheSize > 1000) // each cached string is an allocated texture, flush the cache once in a while in your real project
					m_Renderer.FlushTextCache();
			}

			m_Input.ProcessMouseState();
			m_Input.ProcessKeyboardState();
			m_Input.ProcessTouchState();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			if (m_Ftime.Count == FpsFrames)
				m_Ftime.Dequeue();

			m_Ftime.Enqueue(gameTime.ElapsedGameTime.Milliseconds);

			GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.MidnightBlue);

			m_Canvas.RenderCanvas();

			base.Draw(gameTime);
		}

		private void OnClientSizeChanged(object sender, EventArgs e)
		{
			m_Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
			m_Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

			m_ChangeGraphicsSettings = true;

			m_Renderer.Resize(m_Graphics.PreferredBackBufferWidth, m_Graphics.PreferredBackBufferHeight);
			m_Canvas.SetSize(m_Graphics.PreferredBackBufferWidth, m_Graphics.PreferredBackBufferHeight);
		}
	}
}
