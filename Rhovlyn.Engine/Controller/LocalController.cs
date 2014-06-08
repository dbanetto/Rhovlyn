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
		private string last_dir = "";

		public LocalController(AnimatedSprite target)
		{
			Target = target;
			target.Controller = this;
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
			}

			if (content.Input["player.down"])
			{
				delta.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			if (content.Input["player.left"])
			{
				delta.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			if (content.Input["player.right"])
			{
				delta.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			var diff = Target.Position - delta;
			if (diff.X < 0)
				last_dir = "right";
			if (diff.X > 0)
				last_dir = "left";
			if (diff.Y < 0)
				last_dir = "down";
			if (diff.Y > 0)
				last_dir = "up";

			if (delta != Target.Position && content.CurrnetMap.IsOnMap(new Rectangle((int)delta.X, (int)delta.Y,
				Target.Area.Width, Target.Area.Height)))
			{
				Target.SetAnimation("move_" + last_dir);
				Target.Position = delta;
			}
			else
			{
				Target.SetAnimation("look_" + last_dir);
			}

		}
	}
}

