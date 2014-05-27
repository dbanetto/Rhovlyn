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
            if (content.Input["player.up"])
            {
				Target.Position = Vector2.Add(Target.Position, new Vector2( 0 , -speed * (float)gameTime.ElapsedGameTime.TotalSeconds ));
				Target.SetAnimation("up");
            }

            if (content.Input["player.down"])
            {
				Target.Position = Vector2.Add(Target.Position, new Vector2( 0 , speed * (float)gameTime.ElapsedGameTime.TotalSeconds ));
				Target.SetAnimation("down");
            }

            if (content.Input["player.left"])
            {
				Target.Position = Vector2.Add(Target.Position, new Vector2( -speed * (float)gameTime.ElapsedGameTime.TotalSeconds , 0 ));
				Target.SetAnimation("left");
            }

            if (content.Input["player.right"])
            {
				Target.Position = Vector2.Add(Target.Position, new Vector2( speed * (float)gameTime.ElapsedGameTime.TotalSeconds , 0 ));
				Target.SetAnimation("right");
            }
		}
    }
}

