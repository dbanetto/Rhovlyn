using NUnit.Framework;
using Rhovlyn.Engine.Input;
using Rhovlyn.Engine.Util;
using SDL2;

namespace Rhovlyn.Test.Engine.Input
{
	[TestFixture()]
	public class KeyboardProviderTest
	{
		[Test()]
		public void LoadKeys()
		{
			if (!Parser.Exists<KeyCondition>())
				Parser.Add<KeyCondition>(KeyBoardProvider.ParseKeyBinding);
			var cond = new KeyCondition();
			Assert.IsTrue(Parser.TryParse<KeyCondition>("W", ref cond));
			Assert.AreEqual(SDL.SDL_Scancode.SDL_SCANCODE_W, cond.Keys[0], "Invalid Key Parse for \'W\' test");


			cond = new KeyCondition();
			Assert.IsTrue(Parser.TryParse<KeyCondition>("W+Q", ref cond));
			Assert.AreEqual(new []{ SDL.SDL_Scancode.SDL_SCANCODE_W, SDL.SDL_Scancode.SDL_SCANCODE_Q }, cond.Keys, "Invalid Key Parse for \'W+Q\' test");


			Assert.IsFalse(Parser.TryParse<KeyCondition>("Waaa+91", ref cond), "Invalid Key Parse for \'Waa+91\'");

			Assert.IsFalse(Parser.TryParse<KeyCondition>("W+13)&*%^", ref cond));

		}
	}
}

