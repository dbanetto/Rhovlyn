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
		public WorldState()
		{
		}

		public void Initialize()
		{

		}

		public void LoadContent(ContentManager content)
		{
			this.content = content;
			content.Maps.Add( "test" ,  "Content/map.txt" );
			content.Sprites.Add( "player" , new AnimatedSprite(Vector2.Zero , content.Textures["player"] ));

			var playersprite = (AnimatedSprite)content.Sprites["player"];
			playersprite.Add("up"    , new Animation( new List<int> { 3 } , new List<double> { 0.0 } ));
			playersprite.Add("down"  , new Animation( new List<int> { 2 } , new List<double> { 0.0 } ));

			var right = new Animation(new List<int> { 0 }, new List<double> { 0.0 });
			right.AnimationStarted += (AnimatedSprite sprite) => ( sprite.Effect = SpriteEffects.FlipHorizontally );
			right.AnimationEnded += (AnimatedSprite sprite) => ( sprite.Effect = SpriteEffects.None );
			playersprite.Add("right" , right );

			playersprite.Add("left"  , new Animation( new List<int> { 0 } , new List<double> { 0.0 } ));

			player = new LocalController(playersprite);
			player.Initialize();
			player.LoadContent(content);

			var key = new KeyBoardProvider();
			key.Load("Content/input.ini");
			key.ParseSettings();
			content.Input.Add("keyboard", key); 
		}

		public void UnLoadContent(ContentManager content)
		{
			player.UnLoadContent(content);
		}

		public void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera)
		{
			content.CurrnetMap.Draw(gameTime , spriteBatch , camera);
			content.Sprites.Draw(gameTime , spriteBatch , camera);
		}
		public void Update (GameTime gameTime)
		{
			player.Update(gameTime);
		}
	}
}

