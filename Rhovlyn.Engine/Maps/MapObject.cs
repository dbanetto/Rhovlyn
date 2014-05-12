using System;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Rhovlyn.Engine.Maps
{
	public class MapObject : Sprite
    {
		public MapObject(Vector2 positon , SpriteMap spritemap)
			: base(positon , spritemap)
        {

        }
    }
}

