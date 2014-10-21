using System;
using System.Collections.Generic;
using SharpDL.Graphics;

namespace Rhovlyn.Engine.Util
{
	public static class RectangleUtil
	{
		public static bool CoversRect(Rectangle rect, Rectangle[] covers)
		{
			return SubtractArea(rect, covers).Length == 0;
		}

		public static void PushBack(ref Rectangle sprite, Rectangle[] good)
		{
			//Find the Total area we are working in
			var totalArea = sprite;
			foreach (var r in good) {
				totalArea = Rectangle.Union(totalArea, r);
			}

			//Get all the "bad" area
			var negitive = SubtractArea(totalArea, good);

			int minx = int.MaxValue;
			int miny = int.MaxValue;
			foreach (var neg in negitive) {
				var inter = Rectangle.Intersect(sprite, neg);
				if (inter.IsEmpty)
					continue;

				if (inter.Width < Math.Abs(minx) && inter.Width != sprite.Width && inter.Width != 0) {
					minx = inter.Width;
					if (inter.X != sprite.X) {
						minx *= -1;
					}
				}
				if (inter.Height < Math.Abs(miny) && inter.Height != sprite.Height && inter.Height != 0) {
					miny = inter.Height;
					if (inter.Y != sprite.Y) {
						miny *= -1;
					}
				}
			}
			if (minx == int.MaxValue && miny == int.MaxValue)
				return;

			//Apply the smallest
			if (Math.Abs(minx) < Math.Abs(miny)) {
				sprite.X += minx;
				//If that is not enough apply the next smallest
				if (!CoversRect(sprite, good)) {
					sprite.Y += miny;
				}
			} else {
				sprite.Y += miny;
				if (!CoversRect(sprite, good)) {
					sprite.X += minx;
				}
			}
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

		public static Rectangle[] SubtractArea(Rectangle baseArea, Rectangle[] toRemove)
		{
			Queue<Rectangle> areaQueue = new Queue<Rectangle>();
			areaQueue.Enqueue(baseArea);
			foreach (var rm in toRemove) {
				Queue<Rectangle> tmpQ = new Queue<Rectangle>();
				while (areaQueue.Count != 0) {
					var rect = areaQueue.Dequeue();
					foreach (var r in SubtractArea(rect, rm))
						tmpQ.Enqueue(r);
				}
				areaQueue = tmpQ;
				if (areaQueue.Count == 0)
					break;
			}
			return areaQueue.ToArray();
		}
	}
}

