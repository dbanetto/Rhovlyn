#region Using Statements
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rhovlyn.Engine.Util;
using Rhovlyn.Engine.States;
using Rhovlyn.Engine.Managers;
using System.IO;
using System.Globalization;
using Microsoft.Xna.Framework;

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
		Graph drawTime;
		Graph updateTime;
		int framesPast = 0;
		double framesTimer = 0;
		double drawGraphTimer = 0;
		double updateGraphTimer = 0;
		bool lockedGraphToggle = false;


		public double FPS { get; private set; }

		public bool RenderGraphs { get; set; }

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
			AddParsers();

			IsMouseVisible = true;

			drawTime = new Graph(new Vector2(10, 10), 200, 50, true, 100, MaxType.Auto, Color.LightGray);
			updateTime = new Graph(new Vector2(220, 10), 200, 50, true, 100, MaxType.Auto, Color.LightGray);

			//Danger Lines
			drawTime.Lines.Add(new Line(64, Color.Red));
			drawTime.Lines.Add(new Line(32, Color.Orange));
			drawTime.Lines.Add(new Line(16, Color.Green));
			drawTime.Lines.Add(new Line(8, Color.CornflowerBlue));
			drawTime.Lines.Add(new Line(1, Color.DarkViolet));
			drawTime.Lines.Add(new Line(0.5, Color.Purple));
			drawTime.Lines.Add(new Line(0.01, Color.White));

			updateTime.Lines.Add(new Line(64, Color.Red));
			updateTime.Lines.Add(new Line(32, Color.Orange));
			updateTime.Lines.Add(new Line(16, Color.Green));
			updateTime.Lines.Add(new Line(8, Color.CornflowerBlue));
			updateTime.Lines.Add(new Line(1, Color.DarkViolet));
			updateTime.Lines.Add(new Line(0.5, Color.Purple));
			updateTime.Lines.Add(new Line(0.01, Color.White));

			content.Camera = new Camera(Vector2.Zero, Window.ClientBounds);
			content.Init(graphics.GraphicsDevice);

			graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;

			//Keep the camera up to date with the Client Size
			Window.ClientSizeChanged += (sender, e) => {
				content.Camera.UpdateBounds(Window.ClientBounds);
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
			if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
				Exit();
			}
			content.CurrentState.Update(gameTime);
			content.Audio.Update();

			Window.Title = "FPS:" + (int)FPS + " " + content.Camera.Bounds;

			framesTimer += gameTime.ElapsedGameTime.TotalSeconds;
			if (framesTimer > 1) {
				FPS = framesPast / framesTimer;
				framesPast = 0;
				framesTimer = 0;
			}

			if (RenderGraphs) {
				drawTime.Update(gameTime);
				updateTime.Update(gameTime);

				updateGraphTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (updateGraphTimer > 0.1) {
					updateTime.Data.Add((DateTime.Now - then).TotalMilliseconds);
					updateGraphTimer = 0;
				}
			}

			if (content.Input["core.togglegraphs"] && !lockedGraphToggle) {
				RenderGraphs = !RenderGraphs;
				lockedGraphToggle = true;

				drawTime.Clear();
				updateTime.Clear();
			} else {
				lockedGraphToggle = content.Input["core.togglegraphs"];
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

			Render(gameTime, spriteBatch, content.Camera);

			if (content.Input["core.screenshot"]) {
				var backup = GraphicsDevice.GetRenderTargets();
				var target = new RenderTarget2D(GraphicsDevice, content.Camera.Bounds.Width, content.Camera.Bounds.Height);
				GraphicsDevice.SetRenderTarget(target);

				Render(gameTime, spriteBatch, content.Camera);

				using (var file = new FileStream(String.Format("screenshot_{0}.png", DateTime.Now.ToString("%h-%m_%dd-%mm-%yy")), FileMode.Create)) {
					target.SaveAsPng(file, content.Camera.Bounds.Width, content.Camera.Bounds.Height);
				}
				target.Dispose();
				GraphicsDevice.SetRenderTargets(backup);
			}

			framesPast++;
			if (RenderGraphs) {
				drawGraphTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (drawGraphTimer > 0.1) {
					drawTime.Data.Add((DateTime.Now - then).TotalMilliseconds);
					drawGraphTimer = 0;
				}
			}
			base.Draw(gameTime);
		}

		public void Render(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
		{
			if (content.CurrnetMap != null)
				graphics.GraphicsDevice.Clear(content.CurrnetMap.Background);
			else
				graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			content.CurrentState.Draw(gameTime, spriteBatch, camera);
			if (RenderGraphs) {
				drawTime.Draw(gameTime, spriteBatch, camera);
				updateTime.Draw(gameTime, spriteBatch, camera);
			}
			spriteBatch.End();
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

			bool drawgraphs = false;
			content.Settings.Get<bool>("Window", "RenderGraphs", ref drawgraphs);
			this.RenderGraphs = drawgraphs;
		}

		public void ApplyGraphicsSettings()
		{
			bool resizable = false;
			content.Settings.Get<bool>("window", "resizable", ref resizable);
			Window.AllowUserResizing = resizable;

			bool fullscreen = false;
			content.Settings.Get<bool>("window", "fullscreen", ref fullscreen);
			graphics.IsFullScreen = fullscreen;

			// FIXME : IsBorderlessEXT is not a member of Window
//			bool borderless = false;
//			content.Settings.Get<bool>("window", "borderless", ref borderless);
//			Window.IsBorderlessEXT = borderless;

			bool vsync = true;
			content.Settings.Get<bool>("window", "vsync", ref vsync);
			graphics.SynchronizeWithVerticalRetrace = vsync;
			IsFixedTimeStep = vsync;

			int width = 800;
			if (!content.Settings.Get<int>("window", "width", ref width)) {
				var str = "";
				content.Settings.Get<String>("window", "width", ref str);
				if (str == "auto") {
					width = GraphicsDevice.DisplayMode.Width;
				}
			}

			int height = 600;
			if (!content.Settings.Get<int>("window", "height", ref height)) {
				var str = "";
				content.Settings.Get<String>("window", "height", ref str);
				if (str == "auto") {
					height = GraphicsDevice.DisplayMode.Height;
				}
			}

			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;
			graphics.ApplyChanges();
			content.Camera.UpdateBounds(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
		}

		private static void AddParsers()
		{
			Parser.Add<Color>(o => {
				if (o.StartsWith("0x") && (o.Length == 8 || o.Length == 5)) {
					o = o.Substring(2);
					if (o.Length == 3) {
						var r = int.Parse(o.Substring(0, 1), NumberStyles.AllowHexSpecifier);
						var g = int.Parse(o.Substring(1, 1), NumberStyles.AllowHexSpecifier);
						var b = int.Parse(o.Substring(2, 1), NumberStyles.AllowHexSpecifier);
						return new Color(r * 16, g * 16, b * 16);
					} else {
						var r = int.Parse(o.Substring(0, 2), NumberStyles.HexNumber);
						var g = int.Parse(o.Substring(2, 2), NumberStyles.AllowHexSpecifier);
						var b = int.Parse(o.Substring(4, 2), NumberStyles.AllowHexSpecifier);
						return new Color(r, g, b);
					}
				} else {
					int r, g, b;
					var rgb = o.Split(',');

					//Parse a RGB comma sperated string
					if (rgb.Length != 3)
						throw new InvalidDataException("Expected a comma sperated RGB value");
					r = byte.Parse(rgb[0]);
					g = byte.Parse(rgb[1]);
					b = byte.Parse(rgb[2]);

					return new Color(r, g, b);
				}
			});
		}
	}

}

