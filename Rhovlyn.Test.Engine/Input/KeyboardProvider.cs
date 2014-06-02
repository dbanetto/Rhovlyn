using NUnit.Framework;
using System;
using Rhovlyn.Engine.Input;
using Microsoft.Xna.Framework.Input;

namespace Rhovlyn.Test.Engine.Input
{
	[TestFixture()]
	public class KeyboardProviderTest
	{
		[Test()]
		public void LoadKeys()
		{
			KeyCondition cond = new KeyCondition();
			if (KeyBoardProvider.ParseKeyBinding("W", ref cond))
			{
				if (cond.Keys[0] != Keys.W)
				{
					throw new Exception("Invalid Key Parse for \'W\' test");
				}
			}

			cond = new KeyCondition();
			if (KeyBoardProvider.ParseKeyBinding("W+Q", ref cond))
			{
				if (cond.Keys[0] != Keys.W || cond.Keys[1] != Keys.Q)
				{
					throw new Exception("Invalid Key Parse for \'W+Q\' test");
				}
			}

			cond = new KeyCondition();
			if (KeyBoardProvider.ParseKeyBinding("W+91", ref cond))
			{
				if (cond.Keys[0] != Keys.W || (int)cond.Keys[1] != 91)
				{
					throw new Exception("Invalid Key Parse for \'W+91\'" );
				}
			}

			if (KeyBoardProvider.ParseKeyBinding("Waaa+91", ref cond))
			{
				throw new Exception("Invalid Key Parse for \'Waa+91\'" );
			}

			if (KeyBoardProvider.ParseKeyBinding("W+13)&*%^", ref cond))
			{
				throw new Exception("Invalid Key Parse for \'Waa+91\'" );
			}
		}
	}
}

