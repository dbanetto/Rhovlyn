using NUnit.Framework;
using System;
using Rhovlyn.Engine.IO;
using System.IO;

namespace Rhovlyn.Test.Engine.IO
{
	[TestFixture()]
    public class Settings
    {
		[Test]
		[Category("Settings")]
		public void SettingsLoad ()
		{
			var s = new Rhovlyn.Engine.IO.Settings();
			if (s.Load("Content/settings.ini") == false)
			{
				throw new IOException("Settings file could not be loaded");
			}
		}

		[Test]
		[Category("Settings")]
		public void SettingsGetString ()
		{
			var s = new Rhovlyn.Engine.IO.Settings();
			if (s.Load("Content/settings.ini") == false)
			{
				throw new IOException("Settings file could not be loaded");
			}

			var result = "";
			if (s.Get("", "root", ref result))
			{
				if (!result.Equals("node"))
				{
					throw new IOException("Settings could not read the root node's value properly");
				}
			}
			else
			{
				throw new  IOException("Settings could not read the root node");
			}

			if (s.Get("test", "string", ref result))
			{
				if (!result.Equals("A string"))
				{
					throw new IOException("Settings could not read the test:string node's value properly");
				}
			}
			else
			{
				throw new  IOException("Settings could not read the test:string node");
			}
		}

		[Test]
		[Category("Settings")]
		public void SettingsGetValues ()
		{
			var s = new Rhovlyn.Engine.IO.Settings();
			if (s.Load("Content/settings.ini") == false)
			{
				throw new IOException("Settings file could not be loaded");
			}
			var bout = false;
			if (s.GetBool("test", "bool", ref bout))
			{
				if (!bout.Equals(true))
				{
					throw new IOException("Settings could not read the test:bool node's value properly");
				}
			}
			else
			{
				throw new  IOException("Settings could not read the test:bool node");
			}

			int iout = 0;
			if (s.GetInt("test", "int", ref iout))
			{
				if (!iout.Equals(1))
				{
					throw new IOException("Settings could not read the test:int node's value properly");
				}
			}
			else
			{
				throw new  IOException("Settings could not read the test:int node");
			}

			if (s.GetInt("test", "neg", ref iout))
			{
				if (!iout.Equals(-1))
				{
					throw new IOException("Settings could not read the test:neg node's value properly");
				}
			}
			else
			{
				throw new  IOException("Settings could not read the test:neg node");
			}
		}

		[Test]
		[Category("Settings")]
		public void SettingsGetBadValues ()
		{
			var s = new Rhovlyn.Engine.IO.Settings();
			if (s.Load("Content/settings.ini") == false)
			{
				throw new IOException("Settings file could not be loaded");
			}

			var bout = false;
			if (s.GetBool("test", "badbool", ref bout))
			{
				throw new  IOException("Settings failed to report a bad get for test:badbool");
			}

			int iout = 0;
			if (s.GetInt("test", "badint", ref iout))
			{
				throw new  IOException("Settings failed to report a bad get for test:badint");
			}
		}
    }
}

