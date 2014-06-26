using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rhovlyn.Engine.Graphics
{
	public class SpriteMap
	{
		public Texture2D Texture { get; private set; }

		public List<Rectangle> Frames { get; private set; }

		public string TextureName { get { return this.Texture.Name; } private set { this.Texture.Name = value; } }

		public SpriteMap(Texture2D texture, string name)
		{
			this.Texture = texture;
			this.TextureName = name;
			this.Frames = new List<Rectangle>();
			this.Frames.Add(new Rectangle(0, 0, this.Texture.Width, this.Texture.Height));
		}

		public SpriteMap(Texture2D texture, string name, List<Rectangle> frames)
		{
			this.Texture = texture;
			this.TextureName = name;
			this.Frames = frames;
		}

		public SpriteMap(Texture2D texture, string name, int rows, int coloumns)
		{
			this.Texture = texture;
			this.TextureName = name;
			int w = Texture.Width / rows;
			int h = Texture.Height / coloumns;

			Frames = new List<Rectangle>();
			for (int y = 0; y < Texture.Height; y += h)
			{
				for (int x = 0; x < Texture.Width; x += w)
				{
					Frames.Add(Util.TextureUtil.EncapsulateTexture(texture, new Rectangle(x, y, w, h)));
				}
			}
		}
	}
}

