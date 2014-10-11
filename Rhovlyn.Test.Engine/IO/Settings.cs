using NUnit.Framework;
using System;
using Rhovlyn.Engine.IO;
using System.IO;
using Rhovlyn.Engine.Util;

namespace Rhovlyn.Test.Engine.IO
{
	[TestFixture()]
	public class Settings
	{
		[Test]
		[Category("Settings")]
		public void SettingsLoad()
		{
			var s = new Rhovlyn.Engine.IO.Settings();
			Assert.IsTrue(s.Load("Content/settings.ini"));
		}

		[Test]
		[Category("Settings")]
		public void SettingsGetString()
		{
			var s = new Rhovlyn.Engine.IO.Settings();
			Assert.IsTrue(s.Load("Content/settings.ini"));

			var result = "";
			Assert.IsTrue(s.Get("", "root", ref result));
			Assert.IsTrue(result.Equals("node"));

			Assert.IsTrue(s.Get("test", "string", ref result));
			Assert.IsTrue(result.Equals("A string"));
		}

		[Test]
		[Category("Settings")]
		public void SettingsGetValues()
		{
			var s = new Rhovlyn.Engine.IO.Settings();
			Parser.Init();
			Assert.IsTrue(s.Load("Content/settings.ini"));

			var bout = false;
			Assert.IsTrue(s.Get<bool>("test", "bool", ref bout));
			Assert.IsTrue(bout.Equals(true));

			int iout = 0;
			Assert.IsTrue(s.Get<int>("test", "int", ref iout));
			Assert.IsTrue(iout.Equals(1));

			Assert.IsTrue(s.Get<int>("test", "neg", ref iout));
			Assert.IsTrue(iout.Equals(-1));
		}

		[Test]
		[Category("Settings")]
		public void SettingsGetBadValues()
		{
			var s = new Rhovlyn.Engine.IO.Settings();
			Assert.IsTrue(s.Load("Content/settings.ini"));

			var bout = false;
			Assert.IsFalse(s.Get<bool>("test", "badbool", ref bout));

			int iout = 0;
			Assert.IsFalse(s.Get<int>("test", "badint", ref iout));
		}
	}
}

