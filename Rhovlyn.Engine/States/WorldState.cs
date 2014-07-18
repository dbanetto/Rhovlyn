using System;
using Rhovlyn.Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Managers;
using Rhovlyn.Engine.Graphics;
using Rhovlyn.Engine.Input;
using Rhovlyn.Engine.Controller;
using System.Collections.Generic;
using Rhovlyn.Engine.Maps;

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

			content.Textures.Load("Content/textures.txt");
			content.Maps.Add("test", "gen-map.map");
			this.content.CurrnetMap.Save("sdkljhgskl.map");
			content.Maps.Add("loadSaved", "sdkljhgskl.map");
			content.Maps.Current = "loadSaved";

			content.Sprites.Add("player", new AnimatedSprite(Vector2.Zero, content.Textures["male"]));

			var playersprite = (AnimatedSprite)content.Sprites["player"];
			//Needs to be moved to an external file
			playersprite.AddAnimation("move_up", new Animation(new List<int> { 0, 1, 2, 1 }, new List<double> { 0.1, 0.1, 0.1, 0.1 }));
			playersprite.AddAnimation("move_down", new Animation(new List<int> { 6, 7, 8, 7 }, new List<double> { 0.1, 0.1, 0.1, 0.1 }));
			playersprite.AddAnimation("move_right", new Animation(new List<int> { 3, 4, 5, 4 }, new List<double> { 0.1, 0.1, 0.1, 0.1 }));
			playersprite.AddAnimation("move_left", new Animation(new List<int> { 9, 10, 11, 10 }, new List<double> { 0.1, 0.1, 0.1, 0.1 }));

			playersprite.AddAnimation("look_up", new Animation(new List<int> { 1 }, new List<double> { 0 }));
			playersprite.AddAnimation("look_down", new Animation(new List<int> { 7 }, new List<double> { 0 }));
			playersprite.AddAnimation("look_right", new Animation(new List<int> { 4 }, new List<double> { 0 }));
			playersprite.AddAnimation("look_left", new Animation(new List<int> { 10 }, new List<double> { 0 }));

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
			this.content.CurrnetMap.Update(gameTime);
			this.content.Sprites.Update(gameTime);
			cameracontroll.Update(gameTime);
		}
	}
}

