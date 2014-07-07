using System;
using Microsoft.Xna.Framework;

namespace Rhovlyn.Engine.Util
{
	public static class RectangleUtil
	{
		public static bool InBounds(Rectangle sprite, Rectangle[] bounds)
		{
			var inbounds = false;
			var contained = bounds[0];
			for (int i = 0; i < bounds.Length; i++)
			{
				if (bounds[i].Contains(sprite))
				{
					inbounds = true;
					break;
				}
				contained = Rectangle.Union(contained, Rectangle.Intersect(bounds[i], sprite));
			}
			return inbounds || contained == sprite;
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

			while (!InBounds(sprite, good))
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
			}
		}
	}
}

