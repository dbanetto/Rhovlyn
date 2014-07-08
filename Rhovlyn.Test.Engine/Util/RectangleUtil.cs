using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

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
			sprite = new Rectangle(49, 45, 10, 10);
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
	}
}

