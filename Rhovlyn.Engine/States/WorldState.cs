using System;
using Rhovlyn.Engine.Util;
using Rhovlyn.Engine.Managers;
using Rhovlyn.Engine.Graphics;
using Rhovlyn.Engine.Input;
using Rhovlyn.Engine.Controller;
using Rhovlyn.Engine.Maps;
using SharpDL.Graphics;
using SharpDL;


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

			//this.content.Audio.Add("sfx", "Content/sfx.wav");

			content.Textures.Load("Content/textures.json");
			//content.Maps.Add("test", "gen-map.map");
			//this.content.CurrnetMap.Save("sdkljhgskl.map");
			//content.Maps.Add("loadSaved", "sdkljhgskl.map");
			//content.Maps.Current = "loadSaved";

			content.Sprites.Add("player", new AnimatedSprite(Vector.Zero, content.Textures["player-male"]));

			var playersprite = (AnimatedSprite)content.Sprites["player"];

			//this.content.Audio.ListenerObject = playersprite;
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

		public void Draw(GameTime gameTime, Renderer renderer, Camera camera)
		{
			cameracontroll.Update(new GameTime());
			if (content.CurrnetMap != null)
				content.CurrnetMap.Draw(gameTime, renderer, camera);
			content.Sprites.Draw(gameTime, renderer, camera);
		}

		public void Update(GameTime gameTime)
		{
			if (content.CurrnetMap != null)
				content.CurrnetMap.Update(gameTime);

			//content.Sprites.Update(gameTime);
			cameracontroll.Update(gameTime);
		}
	}
}

