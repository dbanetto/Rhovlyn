#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Security.Policy;
using Rhovlyn.Engine.Graphics;
using Rhovlyn.Engine.Util;
using Rhovlyn.Engine.Maps;
using Rhovlyn.Engine.States;
using Rhovlyn.Engine.Managers;

#endregion
namespace Rhovlyn.Engine
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class GameWindow : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		ContentManager content;
		Graph draw_time;
		Graph update_time;
		int frames_past = 0;
		double frames_timer = 0;
		double draw_graph_timer = 0;
		double update_graph_timer = 0;

		public double FPS { get; private set; }

		public GameWindow()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			content = new ContentManager("Content/settings.ini");

			ApplySettings();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			IsMouseVisible = true;

			draw_time = new Graph(new Vector2(10, 10), 200, 50, true, 100, MaxType.Auto, Color.LightGray);
			update_time = new Graph(new Vector2(220, 10), 200, 50, true, 100, MaxType.Auto, Color.LightGray);

			//Danger Lines
			draw_time.Lines.Add(new Line(64, Color.Red));
			draw_time.Lines.Add(new Line(32, Color.Orange));
			draw_time.Lines.Add(new Line(16, Color.Green));
			draw_time.Lines.Add(new Line(8, Color.CornflowerBlue));
			draw_time.Lines.Add(new Line(1, Color.DarkViolet));
			draw_time.Lines.Add(new Line(0.5, Color.Purple));
			draw_time.Lines.Add(new Line(0.01, Color.White));

			update_time.Lines.Add(new Line(64, Color.Red));
			update_time.Lines.Add(new Line(32, Color.Orange));
			update_time.Lines.Add(new Line(16, Color.Green));
			update_time.Lines.Add(new Line(8, Color.CornflowerBlue));
			update_time.Lines.Add(new Line(1, Color.DarkViolet));
			update_time.Lines.Add(new Line(0.5, Color.Purple));
			update_time.Lines.Add(new Line(0.01, Color.White));

			content.Camera = new Camera(Vector2.Zero, this.Window.ClientBounds);
			content.Init(graphics.GraphicsDevice);

			this.graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;

			//Keep the camera up to date with the Client Size
			this.Window.ClientSizeChanged += (object sender, EventArgs e) =>
			{
				content.Camera.UpdateBounds(this.Window.ClientBounds);
			};
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			content.GameStates.Add("world", new WorldState());

			ApplyGraphicsSettings();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			var then = DateTime.Now;
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}
			content.CurrentState.Update(gameTime);
			content.Audio.Update();

			this.Window.Title = "FPS:" + (int)FPS + " " + this.content.Camera.Bounds.ToString();

			frames_timer += gameTime.ElapsedGameTime.TotalSeconds;
			if (frames_timer > 1)
			{
				FPS = frames_past / frames_timer;
				frames_past = 0;
				frames_timer = 0;
			}

			draw_time.Update(gameTime);
			update_time.Update(gameTime);

			update_graph_timer += gameTime.ElapsedGameTime.TotalSeconds;
			if (update_graph_timer > 0.1)
			{
				update_time.Data.Add((DateTime.Now - then).TotalMilliseconds);
				update_graph_timer = 0;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			var then = DateTime.Now;

			if (this.content.CurrnetMap != null)
				graphics.GraphicsDevice.Clear(this.content.CurrnetMap.Background);
			else
				graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
				
			spriteBatch.Begin();

			this.content.CurrentState.Draw(gameTime, spriteBatch, content.Camera);
			draw_time.Draw(gameTime, spriteBatch, content.Camera);
			update_time.Draw(gameTime, spriteBatch, content.Camera);

			spriteBatch.End();

			frames_past++;

			draw_graph_timer += gameTime.ElapsedGameTime.TotalSeconds;
			if (draw_graph_timer > 0.1)
			{
				draw_time.Data.Add((DateTime.Now - then).TotalMilliseconds);
				draw_graph_timer = 0;
			}
			base.Draw(gameTime);
		}

		public void ApplySettings()
		{
			//Texture & Web Resourses
			bool webresource = false;
			content.Settings.Get<bool>("Textures", "AllowWebResources", ref webresource);
			IO.Path.AllowWebResouces = webresource;

			bool webcache = webresource;
			content.Settings.Get<bool>("Textures", "AllowWebResourcesCache", ref webcache);
			IO.Path.AllowWebResoucesCaching = webcache;

			string cachepath = IO.Path.WebResoucesCachePath;
			content.Settings.Get("Textures", "CachePath", ref cachepath);
			IO.Path.WebResoucesCachePath = cachepath;

			int cachetimeout = IO.Path.WebResoucesCacheTimeOut;
			content.Settings.Get<int>("Textures", "CacheTimeout", ref cachetimeout);
			IO.Path.WebResoucesCacheTimeOut = cachetimeout;
		}

		public void ApplyGraphicsSettings()
		{
			bool resizable = false;
			content.Settings.Get<bool>("window", "resizable", ref resizable);
			Window.AllowUserResizing = resizable;

			bool fullscreen = false;
			content.Settings.Get<bool>("window", "fullscreen", ref fullscreen);
			graphics.IsFullScreen = fullscreen;

			bool borderless = false;
			content.Settings.Get<bool>("window", "borderless", ref borderless);
			Window.IsBorderless = borderless;

			bool vsync = true;
			content.Settings.Get<bool>("window", "vsync", ref vsync);
			graphics.SynchronizeWithVerticalRetrace = vsync;
			this.IsFixedTimeStep = vsync;

			int width = 800;
			if (!content.Settings.Get<int>("window", "width", ref width))
			{
				var str = "";
				content.Settings.Get<String>("window", "width", ref str);
				if (str == "auto")
				{
					width = GraphicsDevice.DisplayMode.Width;
				}
			}

			int height = 600;
			if (!content.Settings.Get<int>("window", "height", ref height))
			{
				var str = "";
				content.Settings.Get<String>("window", "height", ref str);
				if (str == "auto")
				{
					height = GraphicsDevice.DisplayMode.Height;
				}
			}

			this.graphics.PreferredBackBufferWidth = width;
			this.graphics.PreferredBackBufferHeight = height;
			this.graphics.ApplyChanges();
			this.content.Camera.UpdateBounds(new Rectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight));
		}
	}
}

