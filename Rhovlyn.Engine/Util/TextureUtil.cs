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
		public static Rectangle EncapsulateTexture(Texture2D texture, Rectangle? inArea, byte alpha = 250)
		{

			if (inArea == null)
			{
				inArea = new Rectangle(0, 0, texture.Width, texture.Height);
			}
			var output = (Rectangle)inArea;
			var pixels = new Color[output.Width * output.Height];
			texture.GetData<Color>(0, output, pixels, 0, pixels.Length);

			int top = output.Height, bot = 0, left = output.Width, right = 0;

			bool found = false;
			//Check in horizontal slices
			for (int y = 0; y < output.Height; y++)
			{
				for (int x = 0; x < output.Width; x++)
				{
					var pixel = pixels[y * output.Width + x];
					if (pixel.A > alpha)
					{
						//Found the closest pixel to the top
						top = y;
						//Best guess for 1st pixel to the left
						left = x;

						found = true;
						break;
					}
				}
				if (found)
					break;
			}

			found = false;
			//Check in horizontal slices
			for (int y = output.Height - 1; y > top; y--)
			{
				for (int x = output.Width - 1; x > left; x--)
				{
					var pixel = pixels[y * output.Width + x];
					if (pixel.A > alpha)
					{
						//Found the closest pixel to the bottom
						bot = y;
						//Best guess for last pixel to the right
						right = x;
						found = true;
						break;
					}
				}
				if (found)
					break;
			}

			found = false;
			//Search between 0 and the best guess
			//Check in vertical slices
			for (int x = 0; x < left; x++)
			{
				//Shorten search to between top and bot
				for (int y = top; y < bot; y++)
				{
					var pixel = pixels[y * output.Width + x];
					if (pixel.A > alpha)
					{
						//Found the closest pixel to the left
						left = x;
						found = true;
						break;
					}
				}
				if (found)
					break;
			}

			found = false;
			//Check in vertical slices
			for (int x = output.Width - 1; x > right; x--)
			{
				for (int y = top; y < bot; y++)
				{
					var pixel = pixels[y * output.Width + x];
					if (pixel.A > alpha)
					{
						//Found the closest pixel to the right
						right = x;
						found = true;
						break;
					}
				}
				if (found)
					break;
			}

			output = new Rectangle(output.X + left, output.Y + top, right - left + 1, bot - top + 1);
			Console.WriteLine(output);

			return output;
		}


	}
}

