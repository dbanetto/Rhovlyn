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
			if (Rhovlyn.Engine.Util.Parser.Parse<string>("Hello World") != "Hello World")
			{
				throw new InvalidDataException();
			}

			if (Rhovlyn.Engine.Util.Parser.Parse<int>("10") != 10)
			{
				throw new InvalidDataException();
			}

			if (Rhovlyn.Engine.Util.Parser.Parse<double>("10.10") != 10.10)
			{
				throw new InvalidDataException();
			}

			if (Rhovlyn.Engine.Util.Parser.Parse<float>("10.10") != 10.10f)
			{
				throw new InvalidDataException();
			}

			if (!Rhovlyn.Engine.Util.Parser.Parse<bool>("true"))
			{
				throw new InvalidDataException();
			}

			if (Rhovlyn.Engine.Util.Parser.Parse<byte>("255") != 255)
			{
				throw new InvalidDataException();
			}
		}

		[Test()]
		public void TryParsingTest()
		{
			var vals = "";
			if (!Rhovlyn.Engine.Util.Parser.TryParse<string>("Hello World", ref vals))
			{
				throw new InvalidDataException();
			}

			var vali = 0;
			if (!Rhovlyn.Engine.Util.Parser.TryParse<int>("10", ref vali))
			{
				throw new InvalidDataException();
			}

			var vald = 0.0;
			if (!Rhovlyn.Engine.Util.Parser.TryParse<double>("10.10", ref vald))
			{
				throw new InvalidDataException();
			}

			var valf = 0f;
			if (!Rhovlyn.Engine.Util.Parser.TryParse<float>("10.10", ref valf))
			{
				throw new InvalidDataException();
			}

			var valbo = false;
			if (!Rhovlyn.Engine.Util.Parser.TryParse<bool>("true", ref valbo))
			{
				throw new InvalidDataException();
			}

			var valbt = byte.MaxValue;
			if (!Rhovlyn.Engine.Util.Parser.TryParse<byte>("128", ref valbt))
			{
				throw new InvalidDataException();
			}

			vali = 0;
			if (Rhovlyn.Engine.Util.Parser.TryParse<int>("error", ref vali))
			{
				throw new InvalidDataException();
			}

			vald = 0.0;
			if (Rhovlyn.Engine.Util.Parser.TryParse<double>("error", ref vald))
			{
				throw new InvalidDataException();
			}

			valf = 0f;
			if (Rhovlyn.Engine.Util.Parser.TryParse<float>("error", ref valf))
			{
				throw new InvalidDataException();
			}

			valbo = false;
			if (Rhovlyn.Engine.Util.Parser.TryParse<bool>("error", ref valbo))
			{
				throw new InvalidDataException();
			}

			valbt = byte.MaxValue;
			if (Rhovlyn.Engine.Util.Parser.TryParse<byte>("error", ref valbt))
			{
				throw new InvalidDataException();
			}
		}
	}
}

