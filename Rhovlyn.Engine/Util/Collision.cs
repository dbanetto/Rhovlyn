using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Graphics;

namespace Rhovlyn.Engine.Util
{
	public static class Collision
	{
		/// <summary>
		/// Determines if the sprites A and B are touching.
		/// </summary>
		/// <returns><c>true</c> if sprites A B are touching; otherwise, <c>false</c>.</returns>
		/// <remark>
		/// Checks if touhcing rectangle first then pixel by pixel overlap
		/// </remark>
		/// <param name = "a">Sprite A</param>
		/// <param name = "b">Sprite B</param>
		public static bool IsTouching(Sprite a, Sprite b)
		{
			return a.Area.Intersects(b.Area) && IsTextureOverlap(a, b);
		}

		/// <summary>
		/// Determines if the Textures ofthe two sprites overlap.
		/// </summary>
		/// <returns><c>true</c> if Texture overlap ; otherwise, <c>false</c>.</returns>
		/// <param name="a">Sprite A</param>
		/// <param name="b">Sprite B</param>
		/// <remarks>Does not handle rotated textures</remarks>
		public static bool IsTextureOverlap(Sprite a, Sprite b)
		{
			var intercept = Rectangle.Intersect(a.Area, b.Area);
			//If the intercept contains an area, there is no chance for the textures to NOT collide
			if (intercept.Contains(a.Area) || intercept.Contains(b.Area))
				return true;

			var tex_region = new Rectangle(a.SpriteMap.Frames[a.Frameindex].X, a.SpriteMap.Frames[a.Frameindex].Y, intercept.Width, intercept.Height);
			var A_data = new Color[tex_region.Width * tex_region.Height];
			a.SpriteMap.Texture.GetData<Color>(0, tex_region, A_data, 0, A_data.Length);

			tex_region = new Rectangle(b.SpriteMap.Frames[b.Frameindex].X, b.SpriteMap.Frames[b.Frameindex].Y, intercept.Width, intercept.Height);
			var B_data = new Color[tex_region.Width * tex_region.Height];
			b.SpriteMap.Texture.GetData<Color>(0, tex_region, B_data, 0, B_data.Length);

			for (int i = 0; i < A_data.Length; i++) {
				if (A_data[i].A != 0 && B_data[i].A != 0)
					return true;
			}
			return false;
		}

	}
}