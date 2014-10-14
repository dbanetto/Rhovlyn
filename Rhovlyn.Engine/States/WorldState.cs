using System;
using Rhovlyn.Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Managers;
using Rhovlyn.Engine.Graphics;
using Rhovlyn.Engine.Input;
using Rhovlyn.Engine.Controller;
using Rhovlyn.Engine.Maps;
using Rhovlyn.Engine.IO.JSON;
using System.IO;
using Newtonsoft.Json.Linq;


namespace Rhovlyn.Engine.States
{
	public class WorldState : IGameState
	{
		private ContentManager content;
		private CameraController cameracontroll;

		public WorldState()
		{
		}

		public void Initialize()
		{
		}

		public void LoadContent(ContentManager content)
		{
			this.content = content;

			MapGenerator.GenerateDungeonMap("gen-map.map", DateTime.Now.GetHashCode(), new Rectangle(-50000, -50000, 100000, 100000));

			this.content.Audio.Add("sfx", "Content/sfx.wav");

			content.Textures.Load("Content/textures.json");
			content.Maps.Add("test", "gen-map.map");
			this.content.CurrnetMap.Save("sdkljhgskl.map");
			content.Maps.Add("loadSaved", "sdkljhgskl.map");
			content.Maps.Current = "loadSaved";

			content.Sprites.Add("player", new AnimatedSprite(Vector2.Zero, content.Textures["player-male"]));

			var playersprite = (AnimatedSprite)content.Sprites["player"];

			this.content.Audio.ListenerObject = playersprite;
			var player = new LocalController(playersprite);
			player.Initialize();
			player.LoadContent(content);

			var key = new KeyBoardProvider();
			key.Load("Content/input.ini");
			key.ParseSettings();
			content.Input.Add("keyboard", key);

			cameracontroll = new CameraController(content.Camera);
			cameracontroll.FocusOn(content.Sprites["player"]);
			cameracontroll.Update(new GameTime());
		}

		public void UnLoadContent(ContentManager content)
		{
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
		{
			cameracontroll.Update(new GameTime());
			content.CurrnetMap.Draw(gameTime, spriteBatch, camera);
			content.Sprites.Draw(gameTime, spriteBatch, camera);
		}

		public void Update(GameTime gameTime)
		{
			content.CurrnetMap.Update(gameTime);
			content.Sprites.Update(gameTime);
			cameracontroll.Update(gameTime);
		}
	}
}

