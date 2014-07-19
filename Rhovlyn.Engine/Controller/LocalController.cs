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
		private string lastDir = "";

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
			var searchArea = Target.Area;
			var target = Target.Area;
			searchArea.Inflate(target.Width, target.Height);
			var goods = new List<Rectangle>();
			foreach (var p in content.CurrnetMap.TilesInArea(searchArea))
			{
				goods.Add(p.Area);
			}
			RectangleUtil.PushBack(ref target, goods.ToArray());
			var newpos = new Vector2(target.X, target.Y);
			if (newpos != new Vector2((int)Target.Position.X, (int)Target.Position.Y))
			{
				Target.Position = newpos;
			}

			int speed = 128;
			var delta = Target.Position;
			if (content.Input["player.sprint"])
			{
				speed *= 5;
				Target.AnimationSpeed = 2.0;
			}
			else
			{
				Target.AnimationSpeed = 1.0;
			}

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
				lastDir = "right";
			if (diff.X > 0)
				lastDir = "left";
			if (diff.Y < 0)
				lastDir = "down";
			if (diff.Y > 0)
				lastDir = "up";

			if (content.CurrnetMap.IsOnMap(new Rectangle((int)delta.X, (int)delta.Y,
				    Target.Area.Width, Target.Area.Height)))
			{
				if (delta != Target.Position)
				{
					Target.Position = delta;
					Target.SetAnimation("move_" + lastDir);
				}
				else
				{
					Target.SetAnimation("look_" + lastDir);
				}
			}
			else
			{
				Target.SetAnimation("look_" + lastDir);
			}
		}
	}
}

