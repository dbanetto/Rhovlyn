using System;
using System.Collections.Generic;

namespace Rhovlyn.Engine.Util
{
	/// <summary>
	/// Parser for an object
	/// MUST return null on failure
	/// </summary>
	public delegate object ObjectParser(string input);
	public static class Parser
	{
		private static Dictionary< Type , ObjectParser > parsers = new Dictionary<Type, ObjectParser>();

		public static void init()
		{
			parsers.Add(typeof(String), (i) => i);
			parsers.Add(typeof(int), (i) =>
			{ 
				int s;
				if (int.TryParse(i, out s))
				{ 
					return s;
				}
				return null;
			}
			);
			parsers.Add(typeof(float), (i) =>
			{ 
				float s;
				if (float.TryParse(i, out s))
				{ 
					return s; 
				}
				return null;
			}
			);
			parsers.Add(typeof(double), (i) =>
			{ 
				double s;
				if (double.TryParse(i, out s))
				{ 
					return s; 
				}
				return null;
			}
			);
			parsers.Add(typeof(bool), (i) =>
			{ 
				bool s;
				if (bool.TryParse(i, out s))
				{ 
					return s; 
				}
				return null;
			}
			);
			parsers.Add(typeof(byte), (i) =>
			{ 
				byte s;
				if (byte.TryParse(i, out s))
				{ 
					return s; 
				}
				return null;
			}
			);
		}

		public static bool TryParse<T>(string obj, ref T result)
		{
			if (parsers.ContainsKey(typeof(T)))
			{
				var parsed = parsers[typeof(T)](obj);
				if (parsed != null)
				{
					result = (T)(parsed);
					return true;
				}
				return false;
			}
			throw new NotImplementedException(String.Format("Cannot parse unknown type: {0}", typeof(T)));
		}

		public static T Parse<T>(string obj)
		{
			if (parsers.ContainsKey(typeof(T)))
			{
				return (T)(parsers[typeof(T)](obj));
			}
			throw new NotImplementedException(String.Format("Cannot parse unknown type: {0}", typeof(T)));
		}

		public static bool Add<T>(ObjectParser parser, bool overrides = false)
		{
			if (parsers.ContainsKey(typeof(T)))
			{
				if (overrides)
				{
					parsers[typeof(T)] = parser;
					Console.WriteLine(String.Format("The Parser for {0} is overriden", typeof(T)));
					return true;
				}
				throw new Exception(String.Format("The Parser for {0} already exists", typeof(T)));
			}
			parsers.Add(typeof(T), parser);
			return true;
		}


		public static bool Exists<T>()
		{
			return parsers.ContainsKey(typeof(T));
		}
	}
}

