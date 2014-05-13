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

		Camera camera;
		ContentManager content;

		public GameWindow()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			content = new ContentManager("Content/settings.ini");

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			ApplySettings();
			IsMouseVisible = true;
			//HACK : Borderless Window cannot be set on Linux
			//Window.IsBorderless = false;

			content.Init(graphics.GraphicsDevice);
			camera = new Camera(Vector2.Zero , this.Window.ClientBounds);


			//Keep the camera up to date with the Client Size
			this.Window.ClientSizeChanged += (object sender, EventArgs e) => { camera.UpdateBounds(this.Window.ClientBounds); };
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			content.GameStates.Add("world" , new WorldState() );
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
				Exit();
			}
			if (Keyboard.GetState().IsKeyDown(Keys.W)) {
				this.camera.Position = new Vector2(this.camera.Position.X, this.camera.Position.Y - 5);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.S)) {
				this.camera.Position = new Vector2(this.camera.Position.X, this.camera.Position.Y + 5);
			}

			content.CurrentState.Update(gameTime);

			this.Window.Title = this.camera.Bounds.ToString();
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			this.content.CurrentState.Draw(gameTime, spriteBatch , camera);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		public void ApplySettings()
		{
			//Texture & Web Resourses
			bool webresource = false;
			content.Settings.GetBool("Textures" , "AllowWebResources" , ref webresource);
			IO.Path.AllowWebResouces = webresource;

			bool webcache = webresource;
			content.Settings.GetBool("Textures" , "AllowWebResourcesCache" , ref webcache);
			IO.Path.AllowWebResoucesCaching = webcache;

			string cachepath = IO.Path.WebResoucesCachePath;
			content.Settings.Get("Textures" , "CachePath" , ref cachepath);
			IO.Path.WebResoucesCachePath = cachepath;

			int cachetimeout = IO.Path.WebResoucesCacheTimeOut;
			content.Settings.GetInt("Textures" , "CacheTimeout" , ref cachetimeout);
			IO.Path.WebResoucesCacheTimeOut = cachetimeout;

			LoadGraphicsSettings();
		}

		public void LoadGraphicsSettings()
		{
			bool resizable = false;
			content.Settings.GetBool("window", "resizable", ref resizable);
			Window.AllowUserResizing = resizable;

			bool fullscreen = false;
			content.Settings.GetBool("window", "fullscreen", ref fullscreen);
			graphics.IsFullScreen = fullscreen;
		}
	}
}

