using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Rhovlyn.Engine.Util
{
	public static class RectangleUtil
	{
		public static bool InBounds(Rectangle sprite, Rectangle[] bounds)
		{
			var inbounds = false;
			var intercepts = new List<Rectangle>();
			for (int i = 0; i < bounds.Length; i++)
			{
				if (bounds[i].Contains(sprite))
				{
					inbounds = true;
					break;
				}
				var inter = Rectangle.Intersect(sprite, bounds[i]);
				if (inter != Rectangle.Empty)
					intercepts.Add(inter);

				if (i > 0 && bounds[i].Height == bounds[i - 1].Height &&
				    bounds[i].Width == bounds[i - 1].Width)
				{
					if (bounds[i].X == bounds[i - 1].X)
					{
						foreach (var y in new [] { bounds[i].Height, -bounds[i].Height })
						{
							if (bounds[i].Y + y == bounds[i - 1].Y)
							{
								bounds[i] = Rectangle.Union(bounds[i], bounds[i - 1]);
								i--;
								continue;
							}
						}
					}
					else if (bounds[i].Y == bounds[i - 1].Y)
					{
						foreach (var x in new [] { bounds[i].Width, -bounds[i].Width })
						{
							if (bounds[i].X + x == bounds[i - 1].X)
							{
								bounds[i] = Rectangle.Union(bounds[i], bounds[i - 1]);
								i--;
								continue;
							}
						}
					}
				}
			}
			if (inbounds)
				return inbounds;
			if (intercepts.Count > 0)
				return CoversRect(sprite, intercepts.ToArray());
			else
				return false;
			//throw new Exception("I dunno how to deal with this");
		}

		public static void PushBack(ref Rectangle sprite, Rectangle[] good)
		{
			//This is not the BEST way to knock back into bounds
			//It assumes all the good space is a regular shape and the sprite is not caught in a nock
			var blob = good[0];
			foreach (var r in good)
			{
				blob = Rectangle.Union(blob, r);
			}

			do
			{
				int minx = int.MaxValue;
				int miny = int.MaxValue;

				var inter = Rectangle.Intersect(sprite, blob);
				if (inter != Rectangle.Empty)
				{
					if (inter.Width < Math.Abs(minx) && inter.Width != sprite.Width && inter.Width != 0)
					{
						minx = sprite.Width - inter.Width + 1;
						if (inter.X == sprite.X)
						{
							minx *= -1;
						}
					}

					if (inter.Height < Math.Abs(miny) && inter.Height != sprite.Height && inter.Height != 0)
					{
						miny = sprite.Height - inter.Height + 1;
						if (inter.Y == sprite.Y)
						{
							miny *= -1;
						}
					}

				}
				if (minx == int.MaxValue && miny == int.MaxValue)
					break;

				if (Math.Abs(minx) < Math.Abs(miny))
				{
					sprite.X += minx;
					Console.WriteLine("Psuhed X back " + minx);
				}
				else
				{
					sprite.Y += miny;
					Console.WriteLine("Psuhed Y back " + miny);
				}
				Console.WriteLine(String.Format("Psuhed back {0},{1}", minx, miny));
			} while (!InBounds(sprite, good))
				;
		}

		public static bool CoversRect(Rectangle rect, Rectangle[] covers)
		{
			for (int x = rect.Left; x > rect.Right; x++)
			{
				for (int y = rect.Top; y > rect.Bottom; y++)
				{
					bool hasit = false;
					foreach (var r in covers)
					{
						if (r.Contains(new Point(x, y)))
						{
							hasit = true;
							break;
						}
					}
					if (!hasit)
						return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Subtracts the given area, toRemove, from the rectangle baseArea
		/// </summary>
		/// <returns>
		/// An array of rectangles with the combined area of Area, but with out toRemove.
		/// If toRemove completely overlaps baseArea an empty area is returned.
		/// </returns>
		/// <param name="baseArea">Base area</param>
		/// <param name="toRemove">Area to remove.</param>
		/// <remarks></remarks>
		public static Rectangle[] SubtractArea(Rectangle baseArea, Rectangle toRemove)
		{
			if (toRemove.Contains(baseArea))
				return new Rectangle[0];

			var overlap = Rectangle.Intersect(baseArea, toRemove);

			//Does not subtract any area from baseArea
			if (overlap.IsEmpty)
				return new Rectangle[1] { baseArea };

			var area = new List<Rectangle>();

			if (overlap.Top != baseArea.Top)
				area.Add(new Rectangle(baseArea.X, baseArea.Y, baseArea.Width, overlap.Y - baseArea.Y));

			if (overlap.Bottom != baseArea.Bottom)
				area.Add(new Rectangle(baseArea.Left, overlap.Bottom, baseArea.Width, baseArea.Bottom - overlap.Bottom));

			if (overlap.Left != baseArea.Left)
				area.Add(new Rectangle(baseArea.Left, overlap.Top, overlap.Left - baseArea.Left, overlap.Height));

			if (overlap.Right != baseArea.Right)
				area.Add(new Rectangle(overlap.Right, overlap.Top, baseArea.Right - overlap.Right, overlap.Height));

			return area.ToArray();
		}
	}
}

