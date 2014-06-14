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
		/// <param name="A">Sprite A</param>
		/// <param name="B">Sprite B</param>
		public static bool IsTouching(Sprite A, Sprite B)
		{
			if (A.Area.Intersects(B.Area))
			{
				return IsTextureOverlap(A, B);
			}
			return false;
		}

		/// <summary>
		/// Determines if the Textures ofthe two sprites overlap.
		/// </summary>
		/// <returns><c>true</c> if Texture overlap ; otherwise, <c>false</c>.</returns>
		/// <param name="A">Sprite A</param>
		/// <param name="B">Sprite B</param>
		public static bool IsTextureOverlap(Sprite A, Sprite B)
		{
			var intercept = Rectangle.Intersect(A.Area, B.Area);
			//If the intercept contains an area, there is no chance for the textures to NOT collide
			if (intercept.Contains(A.Area) || intercept.Contains(B.Area))
				return true;

			var tex_region = new Rectangle(A.SpriteMap.Frames[A.Frameindex].X, A.SpriteMap.Frames[A.Frameindex].Y, intercept.Width, intercept.Height);
			var A_data = new Color[tex_region.Width * tex_region.Height];
			A.SpriteMap.Texture.GetData<Color>(0, tex_region, A_data, 0, A_data.Length);

			tex_region = new Rectangle(B.SpriteMap.Frames[B.Frameindex].X, B.SpriteMap.Frames[B.Frameindex].Y, intercept.Width, intercept.Height);
			var B_data = new Color[tex_region.Width * tex_region.Height];
			B.SpriteMap.Texture.GetData<Color>(0, tex_region, B_data, 0, B_data.Length);

			for (int i = 0; i < A_data.Length; i++)
			{
				if (A_data[i].A != 0 && B_data[i].A != 0)
					return true;
			}
			return false;
		}

	}
}