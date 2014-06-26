using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.Security.AccessControl;
using OpenTK.Graphics.OpenGL;

namespace Rhovlyn.Engine.Util
{
	public static class TextureUtil
	{
		/// <summary>
		/// The .
		/// </summary>
		public static Rectangle EncapsulateTexture(Texture2D Texture, Rectangle? InArea)
		{

			if (InArea == null)
			{
				InArea = new Rectangle(0, 0, Texture.Width, Texture.Height);
			}
			var output = (Rectangle)InArea;
			var pixels = new Color[output.Width * output.Height];
			Texture.GetData<Color>(0, output, pixels, 0, pixels.Length);

			int top = output.Height, bot = 0, left = output.Width, right = 0;

			for (int y = 0; y < output.Height; y++)
			{
				for (int x = 0; x < output.Width; x++)
				{
					var pixel = pixels[y * output.Width + x];
					if (pixel.A > 128)
					{
						if (x < left)
						{
							left = x;
						}
						if (x > right)
						{
							right = x;
						}

						if (y < top)
						{
							top = y;
						}
						if (y > bot)
						{
							bot = y;
						}
					}
				}
			}
			output = new Rectangle(output.X + left, output.Y + top, right - left + 1, bot - top + 1);

			Console.WriteLine(output);
			return output;
		}


	}
}

