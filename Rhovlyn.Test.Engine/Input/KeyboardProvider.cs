using NUnit.Framework;
using Rhovlyn.Engine.Input;
using Rhovlyn.Engine.Util;

namespace Rhovlyn.Test.Engine.Input
{
	[TestFixture()]
	public class KeyboardProviderTest
	{
		[Test()]
		public void LoadKeys()
		{/*
			if (!Parser.Exists<KeyCondition>())
				Parser.Add<KeyCondition>(KeyBoardProvider.ParseKeyBinding);
			var cond = new KeyCondition();
			Assert.IsTrue(Parser.TryParse<KeyCondition>("W", ref cond));
			Assert.AreEqual(Keys.W, cond.Keys[0], "Invalid Key Parse for \'W\' test");


			cond = new KeyCondition();
			Assert.IsTrue(Parser.TryParse<KeyCondition>("W+Q", ref cond));
			Assert.AreEqual(new []{ Keys.W, Keys.Q }, cond.Keys, "Invalid Key Parse for \'W+Q\' test");

			cond = new KeyCondition();
			Assert.IsTrue(Parser.TryParse<KeyCondition>("W+91", ref cond));
			Assert.AreEqual(new []{ Keys.W, (Keys)(91) }, cond.Keys, "Invalid Key Parse for \'W+91\'");

			Assert.IsFalse(Parser.TryParse<KeyCondition>("Waaa+91", ref cond), "Invalid Key Parse for \'Waa+91\'");

			Assert.IsFalse(Parser.TryParse<KeyCondition>("W+13)&*%^", ref cond));
			*/
		}
	}
}

