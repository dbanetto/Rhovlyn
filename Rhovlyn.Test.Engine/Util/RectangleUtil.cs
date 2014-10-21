using NUnit.Framework;
using SharpDL.Graphics;

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

			Assert.IsTrue(Rhovlyn.Engine.Util.RectangleUtil.CoversRect(sprite, bounds), "Failure in bounds check. Failed to check simple in bounds");

			bounds = new [] { new Rectangle(0, 0, 50, 50) };
			sprite = new Rectangle(60, 60, 10, 10);

			Assert.IsFalse(Rhovlyn.Engine.Util.RectangleUtil.CoversRect(sprite, bounds), "Failure in bounds check. Failed to detect out of bounds");

			bounds = new [] { new Rectangle(0, 0, 50, 50), new Rectangle(50, 0, 50, 50) };
			sprite = new Rectangle(45, 0, 10, 10);

			Assert.IsTrue(Rhovlyn.Engine.Util.RectangleUtil.CoversRect(sprite, bounds), "Failure in bounds check. Failed to check overlapping");

			bounds = new [] { new Rectangle(0, 0, 50, 50), new Rectangle(50, 0, 50, 50) };
			sprite = new Rectangle(45, 50, 10, 10);

			Assert.IsFalse(Rhovlyn.Engine.Util.RectangleUtil.CoversRect(sprite, bounds), "Failure in bounds check. Failed to check out of bounds overlapping");
		}

		[Test()]
		public void PushBackTest()
		{
			var bounds = new [] { new Rectangle(0, 0, 50, 50) };
			var sprite = new Rectangle(10, 10, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			Assert.AreEqual(new Rectangle(10, 10, 10, 10), sprite, "Invalid in push back check. Failed to not push");

			bounds = new [] { new Rectangle(0, 0, 50, 50) };
			sprite = new Rectangle(10, 45, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			Assert.AreEqual(new Rectangle(10, 40, 10, 10), sprite, "Invalid in push back check. Failed to push");

			bounds = new [] { new Rectangle(0, 0, 50, 50), new Rectangle(50, 0, 50, 50) };
			sprite = new Rectangle(49, 45, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			Assert.AreEqual(new Rectangle(49, 40, 10, 10), sprite, "Invalid in push back check. Failed to push on multi");

			bounds = new [] { new Rectangle(0, 0, 50, 50) };
			sprite = new Rectangle(46, 45, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			Assert.AreEqual(new Rectangle(40, 40, 10, 10), sprite, "Invalid in push back check. Failed to push on double out");

			bounds = new [] { new Rectangle(0, 0, 50, 50), new Rectangle(50, 50, 50, 50) };
			sprite = new Rectangle(49, 49, 10, 10);
			Rhovlyn.Engine.Util.RectangleUtil.PushBack(ref sprite, bounds);
			Assert.AreEqual(new Rectangle(50, 50, 10, 10), sprite, "Invalid in push back check. Failed to push on double out and diagonal");
		}

		[Test()]
		public void RectSubtractTest()
		{
			var area = new Rectangle(0, 0, 100, 100);
			var split = Rhovlyn.Engine.Util.RectangleUtil.SubtractArea(area, new Rectangle(25, 25, 25, 25));

			Assert.IsTrue(ArraysEqual<Rectangle>(split, new [] { 
				new Rectangle(0, 0, 100, 25)
				, new Rectangle(0, 50, 100, 50)
				, new Rectangle(0, 25, 25, 25)
				, new Rectangle(50, 25, 50, 25)
			}), "Invalid Area Subtraction, center subtraction");

			area = new Rectangle(0, 0, 100, 100);
			Assert.AreEqual(0, Rhovlyn.Engine.Util.RectangleUtil.SubtractArea(area, area).Length, "Invalid Area Subtraction, complete overlap");


			split = Rhovlyn.Engine.Util.RectangleUtil.SubtractArea(area, Rectangle.Empty);
			Assert.IsTrue(ArraysEqual<Rectangle>(split, new []  { area }), "Invalid Area Subtraction, no overlap");
		}

		[Test()]
		public void RectSubtractsTest()
		{
			var area = new Rectangle(0, 0, 100, 100);
			var split = Rhovlyn.Engine.Util.RectangleUtil.SubtractArea(area, new [] {
				new Rectangle(25, 25, 25, 25),
				new Rectangle(25, 25, 25, 25)
			});
			Assert.IsTrue(ArraysEqual<Rectangle>(split, new [] { 
				new Rectangle(0, 0, 100, 25)
				, new Rectangle(0, 50, 100, 50)
				, new Rectangle(0, 25, 25, 25)
				, new Rectangle(50, 25, 50, 25)
			}), "Invalid Multiple Area Subtractions, double center subtraction");

			area = new Rectangle(0, 0, 100, 100);
			split = Rhovlyn.Engine.Util.RectangleUtil.SubtractArea(area, new [] {
				new Rectangle(25, 25, 12, 25),
				new Rectangle(37, 25, 13, 25)
			});
			Assert.IsTrue(ArraysEqual<Rectangle>(split, new [] { 
				new Rectangle(0, 0, 100, 25)
				, new Rectangle(0, 50, 100, 50)
				, new Rectangle(0, 25, 25, 25)
				, new Rectangle(50, 25, 50, 25)
			}), "Invalid Multiple Area Subtractions, two center subtraction");
		}

		private static bool ArraysEqual<T>(T[] a1, T[] a2)
		{
			if (ReferenceEquals(a1, a2))
				return true;

			Assert.AreNotEqual(null, a1);
			Assert.AreNotEqual(null, a2);

			Assert.AreEqual(a1.Length, a2.Length);

			for (int i = 0; i < a1.Length; i++) {
				Assert.AreEqual(a1[i], a2[i]);
			}
			return true;
		}
	}
}

