using System;
using NUnit.Framework;
using System.IO;

namespace Rhovlyn.Test.Engine.Util
{
	[TestFixture()]
	public class Parser
	{
		[Test()]
		public void BasicParsingTest()
		{
			Assert.AreEqual("Hello World", Rhovlyn.Engine.Util.Parser.Parse<string>("Hello World"));
			Assert.AreEqual(10, Rhovlyn.Engine.Util.Parser.Parse<int>("10"));
			Assert.AreEqual(10.10, Rhovlyn.Engine.Util.Parser.Parse<double>("10.10"));
			Assert.AreEqual(10.10f, Rhovlyn.Engine.Util.Parser.Parse<float>("10.10"));
			Assert.AreEqual(true, Rhovlyn.Engine.Util.Parser.Parse<bool>("true"));
			Assert.AreEqual(255, Rhovlyn.Engine.Util.Parser.Parse<byte>("255"));
		}

		[Test()]
		public void TryParsingTest()
		{
			var vals = "";
			Assert.IsTrue(Rhovlyn.Engine.Util.Parser.TryParse<string>("Hello World", ref vals));
			Assert.AreEqual("Hello World", vals);

			var vali = 0;
			Assert.IsTrue(Rhovlyn.Engine.Util.Parser.TryParse<int>("10", ref vali));
			Assert.AreEqual(10, vali);

			var vald = 0.0;
			Assert.IsTrue(Rhovlyn.Engine.Util.Parser.TryParse<double>("10.10", ref vald));
			Assert.AreEqual(10.10, vald);

			var valf = 0f;
			Assert.IsTrue(Rhovlyn.Engine.Util.Parser.TryParse<float>("10.10", ref valf));
			Assert.AreEqual(10.10f, valf);

			var valbo = false;
			Assert.IsTrue(Rhovlyn.Engine.Util.Parser.TryParse<bool>("true", ref valbo));
			Assert.AreEqual(true, valbo);

			var valbt = byte.MaxValue;
			Assert.IsTrue(Rhovlyn.Engine.Util.Parser.TryParse<byte>("128", ref valbt));
			Assert.AreEqual(128, valbt);

			vali = 0;
			Assert.IsFalse(Rhovlyn.Engine.Util.Parser.TryParse<int>("error", ref vali));
			Assert.AreEqual(0, vali);

			vald = 0.0;
			Assert.IsFalse(Rhovlyn.Engine.Util.Parser.TryParse<double>("error", ref vald));
			Assert.AreEqual(0.0, vald);

			valf = 0f;
			Assert.IsFalse(Rhovlyn.Engine.Util.Parser.TryParse<float>("error", ref valf));
			Assert.AreEqual(0f, valf);

			valbo = false;
			Assert.IsFalse(Rhovlyn.Engine.Util.Parser.TryParse<bool>("error", ref valbo));
			Assert.AreEqual(false, valbo);

			valbt = byte.MaxValue;
			Assert.IsFalse(Rhovlyn.Engine.Util.Parser.TryParse<byte>("error", ref valbt));
			Assert.AreEqual(byte.MaxValue, valbt);
		}
	}
}

