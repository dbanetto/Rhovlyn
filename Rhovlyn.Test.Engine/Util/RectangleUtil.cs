using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Rhovlyn.Test.Engine.Util
{
	[TestFixture()]
	public class RectangleUtil
	{
		[Test()]
		public void CollisionDetectionTest()
		{
			var bounds = new [] { new Rectangle(0, 0, 50, 50) };
			var sprite = new Rectangle(10, 10, 10, 10);

			if (!Rhovlyn.Engine.Util.RectangleUtil.InBounds(sprite, bounds))
			{
				throw new Exception("Invaild in bounds check. Failed to check simple in bounds");
			}

			bounds = new [] { new Rectangle(0, 0, 50, 50) };
			sprite = new Rectangle(60, 60, 10, 10);

			if (Rhovlyn.Engine.Util.RectangleUtil.InBounds(sprite, bounds))
			{
				throw new Exception("Invaild in bounds check. Failed to detect out of bounds");
			}

			bounds = new [] { new Rectangle(0, 0, 50, 50), new Rectangle(50, 0, 50, 50) };
			sprite = new Rectangle(45, 0, 10, 10);

			if (!Rhovlyn.Engine.Util.RectangleUtil.InBounds(sprite, bounds))
			{
				throw new Exception("Invaild in bounds check. Failed to check overlapping");
			}

			bounds = new [] { new Rectangle(0, 0, 50, 50), new Rectangle(50, 0, 50, 50) };
			sprite = new Rectangle(45, 50, 10, 10);

			if (Rhovlyn.Engine.Util.RectangleUtil.InBounds(sprite, bounds))
			{
				throw new Exception("Invaild in bounds check. Failed to check out of bounds overlapping");
			}
		}

		[Test()]
		public void PushBackTest()
		{
			var bounds = new [] { new Rectangle(0, 0, 50, 50) };
			var sprite = new Rectangle(10, 10, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			if (sprite != new Rectangle(10, 10, 10, 10))
			{
				throw new Exception("Invaild in push back check. Failed to not push");
			}

			bounds = new [] { new Rectangle(0, 0, 50, 50) };
			sprite = new Rectangle(10, 45, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			if (sprite != new Rectangle(10, 39, 10, 10))
			{
				throw new Exception("Invaild in push back check. Failed to push");
			}

			bounds = new [] { new Rectangle(0, 0, 50, 50), new Rectangle(50, 0, 50, 50) };
			sprite = new Rectangle(49, 45, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			if (sprite != new Rectangle(49, 39, 10, 10))
			{
				throw new Exception("Invaild in push back check. Failed to push on multi");
			}

			bounds = new [] { new Rectangle(0, 0, 50, 50) };
			sprite = new Rectangle(46, 45, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			if (sprite != new Rectangle(39, 39, 10, 10))
			{
				throw new Exception("Invaild in push back check. Failed to push on double out");
			}

			bounds = new [] { new Rectangle(0, 0, 50, 50), new Rectangle(50, 50, 50, 50) };
			sprite = new Rectangle(49, 45, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			if (sprite != new Rectangle(51, 51, 10, 10))
			{
				throw new Exception("Invaild in push back check. Failed to push on double out and diagonal");
			}
		}

		[Test()]
		public void RectSubtractTest()
		{
			var area = new Rectangle(0, 0, 100, 100);
			var split = Rhovlyn.Engine.Util.RectangleUtil.SubtractArea(area, new Rectangle(25, 25, 25, 25));
			if (!ArraysEqual<Rectangle>(split, new []
			{ 
				new Rectangle(0, 0, 100, 25)
				, new Rectangle(0, 50, 100, 50)
				, new Rectangle(0, 25, 25, 25)
				, new Rectangle(50, 25, 50, 25)
			}))
			{
				throw new Exception("Invaild Area Subtraction, center subtraction");
			}

			area = new Rectangle(0, 0, 100, 100);
			if (Rhovlyn.Engine.Util.RectangleUtil.SubtractArea(area, area).Length != 0)
				throw new Exception("Invaild Area Subtraction, complete overlap");


			split = Rhovlyn.Engine.Util.RectangleUtil.SubtractArea(area, Rectangle.Empty);
			if (!ArraysEqual<Rectangle>(split, new []  { area }))
				throw new Exception("Invaild Area Subtraction, no overlap");
		}

		private static bool ArraysEqual<T>(T[] a1, T[] a2)
		{
			if (ReferenceEquals(a1, a2))
				return true;

			if (a1 == null || a2 == null)
				return false;

			if (a1.Length != a2.Length)
				return false;

			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < a1.Length; i++)
			{
				if (!comparer.Equals(a1[i], a2[i]))
					return false;
			}
			return true;
		}
	}
}

