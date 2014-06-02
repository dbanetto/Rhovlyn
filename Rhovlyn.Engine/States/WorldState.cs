using System;
using Rhovlyn.Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Managers;
using Rhovlyn.Engine.Graphics;
using Rhovlyn.Engine.Input;
using Rhovlyn.Engine.Controller;
using System.Collections.Generic;

namespace Rhovlyn.Engine.States
{
	public class WorldState : IGameState
	{
		private ContentManager content;
		private LocalController player;
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

			Rhovlyn.Engine.Maps.MapGenerator.GenerateDungeonMap( "gen-map.map" , DateTime.Now.GetHashCode() );


			this.content.Audio.Add("sfx", "Content/sfx.wav");



			content.Textures.Load("Content/textures.txt");
			content.Maps.Add( "test" ,  "gen-map.map" );
			content.Sprites.Add( "player" , new AnimatedSprite(Vector2.Zero , content.Textures["player"] ));

			var playersprite = (AnimatedSprite)content.Sprites["player"];
			playersprite.AddAnimation("up"    , new Animation( new List<int> { 3 } , new List<double> { 0.0 } ));
			playersprite.AddAnimation("down"  , new Animation( new List<int> { 2 } , new List<double> { 0.0 } ));
			this.content.Audio.ListenerObject = playersprite;
			var right = new Animation(new List<int> { 0 }, new List<double> { 0.0 });
			right.AnimationStarted += (AnimatedSprite sprite) => ( sprite.Effect = SpriteEffects.FlipHorizontally );
			right.AnimationEnded += (AnimatedSprite sprite) => ( sprite.Effect = SpriteEffects.None );
			playersprite.AddAnimation("right" , right );

			playersprite.AddAnimation("left"  , new Animation( new List<int> { 0 } , new List<double> { 0.0 } ));

			player = new LocalController(playersprite);
			player.Initialize();
			player.LoadContent(content);

			var key = new KeyBoardProvider();
			key.Load("Content/input.ini");
			key.ParseSettings();
			content.Input.Add("keyboard", key);

			cameracontroll = new CameraController(content.Camera);
			cameracontroll.FocusOn(content.Sprites["player"]);
		}

		public void UnLoadContent(ContentManager content)
		{
			player.UnLoadContent(content);
		}

		public void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera)
		{
			cameracontroll.Update(gameTime);
			content.CurrnetMap.Draw(gameTime , spriteBatch , camera);
			content.Sprites.Draw(gameTime , spriteBatch , camera);
		}
		public void Update (GameTime gameTime)
		{
			if (this.content.Input["player.sound"])
			{
				this.content.Audio.Play("sfx");
			}
			player.Update(gameTime);
			cameracontroll.Update(gameTime);
		}
	}
}

