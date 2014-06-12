using System;
using NUnit;
using NUnit.Framework;

using Microsoft.Xna.Framework;

namespace Rhovlyn.Test.Engine
{
	[TestFixture()]
	public class RectangleTest
	{
		public RectangleTest()
		{
		}

		[Test()]
		public void Contains ()
		{
			Rectangle a = new Rectangle(0, 0, 100, 100);
			Rectangle b = new Rectangle(0,0, 50, 50);
			if (!a.Contains(b))
				throw new Exception("Not expected behavior");

			if (b.Contains(a))
				throw new Exception("Not expected behavior");

			a = new Rectangle(0, 0, 100, 100);
			b = new Rectangle(0 ,-25,  50,  50);
			if (a.Contains(b))
				throw new Exception("Not expected behavior");

			a = new Rectangle(0, 0, 100, 100);
			b = new Rectangle(-50 ,-50,  1, 1);
			if (a.Contains(b))
				throw new Exception("Not expected behavior");
		}

		[Test()]
		public void Intercepts()
		{
			Rectangle a = new Rectangle(0, 0, 100, 100);
			Rectangle b = new Rectangle(0,0, 50, 50);
			if (!a.Intersects(b))
				throw new Exception("Not expected behavior");

			if (!b.Intersects(a))
				throw new Exception("Not expected behavior");

			a = new Rectangle(0, 0, 100, 100);
			b = new Rectangle(0 ,-25,  50,  50);
			if (!a.Intersects(b))
				throw new Exception("Not expected behavior");

			if (!b.Intersects(a))
				throw new Exception("Not expected behavior");

			a = new Rectangle(0, 0, 100, 100);
			b = new Rectangle(-50 ,-50,  1, 1);
			if (a.Intersects(b))
				throw new Exception("Not expected behavior");

			a = new Rectangle(int.MinValue,0, 100, 100);
			b = new Rectangle(0,0, 50, 50);
			if (a.Intersects(b))
				throw new Exception("Not expected behavior");

			if (b.Intersects(a))
				throw new Exception("Not expected behavior");
		}


	}
}

