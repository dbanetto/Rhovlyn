using System;
using Rhovlyn.Engine.Graphics;
using Rhovlyn.Engine.Managers;
using Microsoft.Xna.Framework;

namespace Rhovlyn.Engine.Controller
{
	public class LocalController : IController
	{
		public AnimatedSprite Target { get; private set; }
		private ContentManager content;
		public LocalController(AnimatedSprite target)
		{
			Target = target;
		}

		public void Initialize()
		{

		}

		public void LoadContent(ContentManager content)
		{
			this.content = content;
		}

		public void UnLoadContent(ContentManager content)
		{

		}

		public void Update (GameTime gameTime)
		{
			int speed = 128;
			var delta = Target.Position;
			if (content.Input["player.up"])
			{
				delta.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
				Target.SetAnimation("up");
			}

			if (content.Input["player.down"])
			{
				delta.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
				Target.SetAnimation("down");
			}

			if (content.Input["player.left"])
			{
				delta.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
				Target.SetAnimation("left");
			}

			if (content.Input["player.right"])
			{
				delta.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
				Target.SetAnimation("right");
			}

			if (delta != Target.Position && content.CurrnetMap.IsOnMap( new Rectangle( (int)delta.X , (int)delta.Y,
											 Target.Area.Width , Target.Area.Height)))
			{
				Target.Position = delta;
			}

		}
	}
}

