using System;
using Rhovlyn.Engine.Graphics;
using Rhovlyn.Engine.Managers;
using Microsoft.Xna.Framework;
using Rhovlyn.Engine.Maps;
using Rhovlyn.Engine.Util;
using System.ComponentModel;
using System.Collections.Generic;

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

		public void Update(GameTime gameTime)
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


			var target = Target.Area;
			var goods = new List<Rectangle>();
			foreach (var p in content.CurrnetMap.TilesInArea(target))
			{
				goods.Add(p.Area);
			}
			RectangleUtil.PushBack(ref target, goods.ToArray());
			var newpos = new Vector2(target.X, target.Y);
			if (newpos != Target.Position)
			{
				Target.Position = newpos;
			}
			if (content.CurrnetMap.IsOnMap(new Rectangle((int)delta.X, (int)delta.Y,
				    Target.Area.Width, Target.Area.Height)))
			{
				if (delta != Target.Position)
				{
					Target.Position = delta;
					Target.SetAnimation("move_" + last_dir);
				}
			}
			else
			{
				Target.SetAnimation("look_" + last_dir);
			}
		}
	}
}

