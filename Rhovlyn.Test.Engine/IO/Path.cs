using NUnit.Framework;
using System;

using Rhovlyn.Engine.IO;
using System.IO;

namespace Rhovlyn.Test.Engine.IO
{
	[TestFixture()]
	public class Path
	{
		[Test]
		[ExpectedException(typeof(IOException))]
		public void PathWebResoucesDisabled()
		{
			Rhovlyn.Engine.IO.Path.AllowWebResouces = false;
			Rhovlyn.Engine.IO.Path.ResolvePath("http://i.imgur.com/Vf0An8J.png");
		}

		[Test]
		[Category("Web Content")]
		public void PathCacheDisabled()
		{
			Rhovlyn.Engine.IO.Path.WebResoucesCachePath = "cache/";
			Rhovlyn.Engine.IO.Path.AllowWebResouces = true;
			Rhovlyn.Engine.IO.Path.AllowWebResoucesCaching = false;
			Console.WriteLine("Downloading file");
			Rhovlyn.Engine.IO.Path.ResolvePath("http://i.imgur.com/Vf0An8J.png");

			Console.WriteLine("Downloading file");
			Assert.IsNotInstanceOfType(typeof(FileStream),
				Rhovlyn.Engine.IO.Path.ResolvePath("http://i.imgur.com/Vf0An8J.png"),
				"A File Stream from cache should not be returned");
		}

		[Test]
		[Category("Web Content")]
		public void PathCacheClear()
		{
			Rhovlyn.Engine.IO.Path.WebResoucesCachePath = "cache/";
			Rhovlyn.Engine.IO.Path.WebResoucesCacheTimeOut = 10;
			Rhovlyn.Engine.IO.Path.AllowWebResouces = true;
			Rhovlyn.Engine.IO.Path.AllowWebResoucesCaching = true;

			Console.WriteLine("Downloading file");
			Rhovlyn.Engine.IO.Path.ResolvePath("http://i.imgur.com/Vf0An8J.png");

			//Clear cache
			Console.WriteLine("Cleaning cache");
			Rhovlyn.Engine.IO.Path.WebResoucesCacheTimeOut = 0;

			Console.WriteLine("Downloading file");
			Assert.IsInstanceOfType(typeof(FileStream) 
				, Rhovlyn.Engine.IO.Path.ResolvePath("http://i.imgur.com/Vf0An8J.png")
				, "A File Stream from cache should be returned");
		}

	}
}

