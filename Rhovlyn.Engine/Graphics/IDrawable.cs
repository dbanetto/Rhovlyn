using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Util;

namespace Rhovlyn.Engine.Graphics
{
	public interface IDrawable
	{
		void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera);
		void Update (GameTime gameTime);
	}
}

