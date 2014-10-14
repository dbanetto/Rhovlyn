using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Rhovlyn.Test.Engine
{
	[TestFixture()]
	public class RectangleTest
	{

		[Test()]
		public void Contains()
		{

			var a = new Rectangle(0, 0, 100, 100);
			var b = new Rectangle(0, 0, 50, 50);
			Assert.IsTrue(a.Contains(b));

			Assert.IsFalse(b.Contains(a));

			a = new Rectangle(0, 0, 100, 100);
			b = new Rectangle(0, -25, 50, 50);
			Assert.IsFalse(a.Contains(b));

			a = new Rectangle(0, 0, 100, 100);
			b = new Rectangle(-50, -50, 1, 1);
			Assert.IsFalse(a.Contains(b));
		}

		[Test()]
		public void Intercepts()
		{
			var a = new Rectangle(0, 0, 100, 100);
			var b = new Rectangle(0, 0, 50, 50);
			Assert.IsTrue(a.Intersects(b));

			Assert.IsTrue(b.Intersects(a));

			a = new Rectangle(0, 0, 100, 100);
			b = new Rectangle(0, -25, 50, 50);
			Assert.IsTrue(a.Intersects(b));

			Assert.IsTrue(b.Intersects(a));

			a = new Rectangle(0, 0, 100, 100);
			b = new Rectangle(-50, -50, 1, 1);
			Assert.IsFalse(a.Intersects(b));

			a = new Rectangle(int.MinValue, 0, 100, 100);
			b = new Rectangle(0, 0, 50, 50);
			Assert.IsFalse(a.Intersects(b));

			Assert.IsFalse(b.Intersects(a));
		}


	}
}

