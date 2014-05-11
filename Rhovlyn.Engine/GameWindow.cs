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
		TextureMananger textures;
		SpriteManager sprites;
		Camera camera;
		Map map;
		Rhovlyn.Engine.IO.Settings settings;

		public GameWindow()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";


			settings = new Rhovlyn.Engine.IO.Settings("Content/settings.ini");
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

			textures = new TextureMananger(this.graphics.GraphicsDevice);
			sprites = new SpriteManager();
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
			textures.Add("player" , "Content/sprites/sheep.png");

			sprites.Add( "player" , new Sprite(Vector2.Zero , textures["player"] ));
			map = new Map( "Content/map.txt" , this.textures);
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



			this.sprites.Update(gameTime);
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
				this.sprites.Draw(gameTime , spriteBatch, this.camera);
				this.map.Draw(gameTime , spriteBatch, this.camera);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		public void ApplySettings()
		{
			//Texture & Web Resourses
			bool webresource = false;
			settings.GetBool("Textures" , "AllowWebResources" , ref webresource);
			IO.Path.AllowWebResouces = webresource;

			bool webcache = webresource;
			settings.GetBool("Textures" , "AllowWebResources" , ref webcache);
			IO.Path.AllowWebResoucesCaching = webcache;

			string cachepath = IO.Path.WebResoucesCachePath;
			settings.Get("Textures" , "AllowWebResources" , ref cachepath);
			IO.Path.WebResoucesCachePath = cachepath;

			LoadGraphicsSettings();
		}

		public void LoadGraphicsSettings()
		{
			bool resizable = false;
			this.settings.GetBool("window", "resizable", ref resizable);
			Window.AllowUserResizing = resizable;

			bool fullscreen = false;
			this.settings.GetBool("window", "fullscreen", ref fullscreen);
			graphics.IsFullScreen = fullscreen;
		}
	}
}

