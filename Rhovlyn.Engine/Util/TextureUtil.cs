using SharpDL.Graphics;
using SDL2;

namespace Rhovlyn.Engine.Util
{
	public static class TextureUtil
	{
		/// <summary>
		/// The .
		/// </summary>
		public static Rectangle EncapsulateTexture(Texture texture, Rectangle? inArea, byte alpha = 250)
		{

			if (inArea == null) {
				inArea = new Rectangle(0, 0, texture.Width, texture.Height);
			}
			var output = (Rectangle)inArea;
			return output; //HACK: While there is no good way to query the contents of a texture
			var pixels = texture.QueryPixels();


			int top = output.Height, bot = 0, left = output.Width, right = 0;

			//texture.GetData<Color>(0, output, pixels, 0, pixels.Length);
			bool found = false;
			//Check in horizontal slices
			for (int y = 0; y < output.Height; y++) {
				for (int x = 0; x < output.Width; x++) {
					var pixel = pixels[y * output.Width + x];
					if (pixel.A > alpha) {
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
			for (int y = output.Height - 1; y > top; y--) {
				for (int x = output.Width - 1; x > left; x--) {
					var pixel = pixels[y * output.Width + x];
					if (pixel.A > alpha) {
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
			for (int x = 0; x < left; x++) {
				//Shorten search to between top and bot
				for (int y = top; y < bot; y++) {
					var pixel = pixels[y * output.Width + x];
					if (pixel.A > alpha) {
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
			for (int x = output.Width - 1; x > right; x--) {
				for (int y = top; y < bot; y++) {
					var pixel = pixels[y * output.Width + x];
					if (pixel.A > alpha) {
						//Found the closest pixel to the right
						right = x;
						found = true;
						break;
					}
				}
				if (found)
					break;
			} 
			// +1's are to adjust the width/height to include the edges 
			return new Rectangle(output.X + left, output.Y + top, right - left + 1, bot - top + 1);
			//return (Rectangle)inArea; // HACK FIXME: Fix comment out code above
		}


	}
}

